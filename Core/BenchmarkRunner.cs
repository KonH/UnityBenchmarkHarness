using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace UnityBenchmarkHarness {
	public class BenchmarkRunner {
		Stopwatch _timer;
		long      _startMem;

		public static List<BenchmarkSummary> Run<TArgument>(
			string name, int iterations, Action<TArgument> payload, params TArgument[] arguments
		) {
			var summaries = new List<BenchmarkSummary>();
			for ( var step = 0; step < arguments.Length; step++ ) {
				var curArgument = arguments[step];
				var fullName = $"{name} (iterations: {iterations}, argument: {curArgument})";
				var results = new List<BenchmarkResult>(iterations);
				for ( var r = 0; r < iterations; r++ ) {
					results.Add(Perform(() => payload(curArgument)));
				}
				summaries.Add(BenchmarkSummary.CombineResults(fullName, results));
			}
			return summaries;
		}

		static BenchmarkRunner Start() {
			GC.Collect();
			return new BenchmarkRunner() {
				_startMem = GC.GetTotalMemory(false),
				_timer    = Stopwatch.StartNew()
			};
		}

		static BenchmarkResult Perform(Action action) {
			var b = Start();
			action();
			return b.Finish();
		}

		BenchmarkResult Finish() {
			_timer.Stop();
			var memUsed = GC.GetTotalMemory(false) - _startMem;
			return new BenchmarkResult(_timer.Elapsed.TotalMilliseconds, memUsed);
		}
	}
}