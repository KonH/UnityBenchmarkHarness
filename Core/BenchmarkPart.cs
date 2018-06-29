using System.Collections.Generic;
using ByteSizeLib;
// temp
using UnityBenchmarkHarness.Editor;
using UnityEditorInternal.Profiling;

namespace UnityBenchmarkHarness {
	public class BenchmarkPart {
		public string                               Argument  { get; }
		public List<string>                         FuncNames { get; } = new List<string>();
		public Dictionary<string, BenchmarkResult>  Results   { get; } = new Dictionary<string, BenchmarkResult> ();

		public BenchmarkPart(string argument) {
			Argument = argument;
		}

		public void AddProfilerResult(string funcName, FunctionData func) {
			var totalTimeStr = func.GetValue(ProfilerColumn.TotalTime);
			var gcMemoryStr = func.GetValue(ProfilerColumn.GCMemory);
			double totalTime;
			ByteSize gcMemory;
			if ( double.TryParse(totalTimeStr, out totalTime) && ByteSize.TryParse(gcMemoryStr, out gcMemory) ) {
				Results.Add(funcName, new BenchmarkResult(totalTime, (int)gcMemory.Bytes));
			}
		}
	}
}
