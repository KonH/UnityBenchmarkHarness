using System;
using System.Collections.Generic;

namespace UnityBenchmarkHarness {
	public class BenchmarkSummary {
		public string                 Name     { get; }
		public List<BenchmarkMeasure> Measures { get; }

		public BenchmarkSummary(string name, List<BenchmarkMeasure> measures) {
			Name     = name;
			Measures = measures;
		}

		internal static BenchmarkSummary CombineResults(string name, ICollection<BenchmarkResult> results) {
			var measures = new List<BenchmarkMeasure>() {
				CreateTimeMeause(results),
				CreateMemMeasure(results)
			};
			return new BenchmarkSummary(name, measures);
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

		static BenchmarkMeasure CreateTimeMeause(ICollection<BenchmarkResult> results) {
			return CreateMeausure("Time", r => r.Time, results);
		}

		static BenchmarkMeasure CreateMemMeasure(ICollection<BenchmarkResult> results) {
			return CreateMeausure("Memory", r => r.Memory, results);
		}
	}
}