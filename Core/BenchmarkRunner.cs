using System;
using System.Collections.Generic;
using UnityEngine.Profiling;

namespace UnityBenchmarkHarness {
	public class BenchmarkRunner {
		public string              SelfName { get; }
		public List<BenchmarkPart> Parts    { get; } = new List<BenchmarkPart>();

		public bool IsComplete {
			get {
				foreach ( var p in Parts ) {
					foreach ( var name in p.FuncNames ) {
						if ( !p.ProfilerResults.ContainsKey(name) ) {
							return false;
						}
					}
				}
				return true;
			}
		}

		public BenchmarkRunner(string name) {
			SelfName = name;
		}

		public void Run<TArgument>(
			int iterations, Action<TArgument> payload, params TArgument[] arguments
		) {
			Parts.Clear();
			for ( var step = 0; step < arguments.Length; step++ ) {
				var curArgument = arguments[step];
				var part = new BenchmarkPart(curArgument.ToString());
				for ( var r = 0; r < iterations; r++ ) {
					var localName = $"{SelfName}_a[{curArgument}]_{r}";
					part.FuncNames.Add(localName);
					part.RuntimeResults.Add(localName, Perform(localName, () => payload(curArgument)));
				}
				Parts.Add(part);
			}
		}

		static void PreRunGC() {
			GC.Collect(0, GCCollectionMode.Forced, true);
		}

		static RuntimeBenchmarkResult Perform(string name, Action action) {
			var br = new BenchmarkRun();
			PreRunGC();
			br.Start();
			Profiler.BeginSample(name);
			action();
			Profiler.EndSample();
			br.Stop();
			return new RuntimeBenchmarkResult(br.TotalMilliseconds);
		}
	}
}