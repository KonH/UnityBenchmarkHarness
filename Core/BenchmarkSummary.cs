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

		public static List<BenchmarkSummary> TestWrapper<T>(string name, int repeats, Action<T, int> payload, params int[] iterations) where T : new() {
			var summaries = new List<BenchmarkSummary>();
			for ( var step = 0; step < iterations.Length; step++ ) {
				var curIters = iterations[step];
				var fullName = $"{name} (repeats: {repeats}, iterations: {curIters})";
				var results = new List<BenchmarkResult>(repeats);
				for ( var r = 0; r < repeats; r++ ) {
					var list = new T();
					results.Add(BenchmarkRunner.Perform(() => payload(list, curIters)));
				}
				summaries.Add(CombineResults(fullName, results));
			}
			return summaries;
		}

		static BenchmarkSummary CombineResults(string name, ICollection<BenchmarkResult> results) {
			var measures = new List<BenchmarkMeasure>() {
				CreateTimeMeause(results),
				CreateMemMeasure(results)
			};
			return new BenchmarkSummary(name, measures);
		}

		static BenchmarkMeasure CreateMeause(string name, Func<BenchmarkResult, double> getter, ICollection<BenchmarkResult> results) {
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
			return CreateMeause("Time", r => r.Time, results);
		}

		static BenchmarkMeasure CreateMemMeasure(ICollection<BenchmarkResult> results) {
			return CreateMeause("Memory", r => r.Memory, results);
		}
	}
}