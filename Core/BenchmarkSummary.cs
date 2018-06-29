using System;
using System.Collections.Generic;

namespace UnityBenchmarkHarness {
	[Serializable]
	public class BenchmarkSummary {
		public string                 Name     = string.Empty;
		public List<BenchmarkMeasure> Measures = null;

		internal BenchmarkSummary(string name, ICollection<BenchmarkResult> results) {
			Name = name;
			Measures = new List<BenchmarkMeasure>() {
				CreateProfilerTimeMeasure(results),
				CreateGCMemoryMeasure    (results),
			};
		}

		static BenchmarkMeasure CreateMeausure(string name, Func<BenchmarkResult, double> getter, ICollection<BenchmarkResult> results) {
			double min = double.MaxValue, max = 0, sum = 0;
			foreach ( var r in results ) {
				var cur = getter(r);
				if ( cur < min ) {
					min = cur;
				}
				if ( cur > max ) {
					max = cur;
				}
				sum += cur;
			}
			var avg = sum / results.Count;
			return new BenchmarkMeasure(name, min, max, avg);
		}


		static BenchmarkMeasure CreateProfilerTimeMeasure(ICollection<BenchmarkResult> results) {
			return CreateMeausure("Profiler_TotalTime", r => r.TotalTime, results);
		}

		static BenchmarkMeasure CreateGCMemoryMeasure(ICollection<BenchmarkResult> results) {
			return CreateMeausure("Profiler_GCMemory", r => r.GCMemory, results);
		}
	}
}