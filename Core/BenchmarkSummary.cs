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
				CreateTimeMeause     (results),
				CreateUsedHeapMeasure(results),
				CreateMonoHeapMeasure(results),
				CreateMonoMemMeasure (results),
				CreateAllocMemMeasure(results),
				CreateGcMemoryMeasure(results),
				CreateGcCountMeasure (results),
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

		static BenchmarkMeasure CreateUsedHeapMeasure(ICollection<BenchmarkResult> results) {
			return CreateMeausure("Memory:UsedHeap", r => r.UsedHeap, results);
		}

		static BenchmarkMeasure CreateMonoHeapMeasure(ICollection<BenchmarkResult> results) {
			return CreateMeausure("Memory:MonoHeapSize", r => r.MonoHeap, results);
		}

		static BenchmarkMeasure CreateMonoMemMeasure(ICollection<BenchmarkResult> results) {
			return CreateMeausure("Memory:MonoUsedSize", r => r.MonoMemory, results);
		}

		static BenchmarkMeasure CreateAllocMemMeasure(ICollection<BenchmarkResult> results) {
			return CreateMeausure("Memory:TotalAllocatedMemory", r => r.AllocMemory, results);
		}

		static BenchmarkMeasure CreateGcMemoryMeasure(ICollection<BenchmarkResult> results) {
			return CreateMeausure("Memory:GC_TotalMemory", r => r.GCCount, results);
		}

		static BenchmarkMeasure CreateGcCountMeasure(ICollection<BenchmarkResult> results) {
			return CreateMeausure("Memory:GC_Count", r => r.GCCount, results);
		}
	}
}