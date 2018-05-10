using System;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace UnityBenchmarkHarness {
	public class BenchmarkRunner {
		Stopwatch _timer;
		long      _startUsedHeap;
		long      _startHeapSize;
		long      _startMonoMem;
		long      _startAllocMem;
		long      _startGcMem;
		int       _startGcCount;

		public static List<BenchmarkSummary> Run<TArgument>(
			string name, int iterations, Action<TArgument> payload, params TArgument[] arguments
		) {
			var summaries = new List<BenchmarkSummary>();
			for ( var step = 0; step < arguments.Length; step++ ) {
				var curArgument = arguments[step];
				var fullName = $"{name} (iterations: {iterations}, argument: {curArgument})";
				var results = new List<BenchmarkResult>(iterations);
				for ( var r = 0; r < iterations; r++ ) {
					var localName = $"{name}_a[{curArgument}]_{r}";
					results.Add(Perform(localName, () => payload(curArgument)));
				}
				summaries.Add(BenchmarkSummary.CombineResults(fullName, results));
			}
			return summaries;
		}

		static void PreRunGC() {
			GC.Collect(0, GCCollectionMode.Forced, true);
		}

		static BenchmarkRunner Start() {
			var sw = new Stopwatch();
			var br = new BenchmarkRunner();

			PreRunGC();

			br._startUsedHeap = Profiler.usedHeapSizeLong;
			br._startHeapSize = Profiler.GetMonoHeapSizeLong();
			br._startMonoMem  = Profiler.GetMonoUsedSizeLong();
			br._startAllocMem = Profiler.GetTotalAllocatedMemoryLong();
			br._timer         = sw;
			br._startGcMem    = GC.GetTotalMemory(false);
			br._startGcCount  = GC.CollectionCount(0);

			sw.Start();

			return br;
		}

		static BenchmarkResult Perform(string name, Action action) {
			Profiler.BeginSample(name);
			var b = Start();
			action();
			var result = b.Finish();
			Profiler.EndSample();
			return result;
		}

		BenchmarkResult Finish() {
			_timer.Stop();
			var monoHeapUsed = Profiler.GetMonoHeapSizeLong()         - _startHeapSize;
			var heapUsed     = Profiler.usedHeapSizeLong              - _startUsedHeap;
			var monoMemUsed  = Profiler.GetMonoUsedSizeLong()         - _startMonoMem;
			var allocMemUsed = Profiler.GetTotalAllocatedMemoryLong() - _startAllocMem;
			var gcMemUsed    = GC.GetTotalMemory(false)               - _startGcMem;
			var gcCount      = GC.CollectionCount(0)                  - _startGcCount;
			return new BenchmarkResult(_timer.Elapsed.TotalMilliseconds, heapUsed, monoHeapUsed, monoMemUsed, allocMemUsed, gcMemUsed, gcCount);
		}
	}
}