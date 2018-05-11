using System.Collections.Generic;
using ByteSizeLib;
// temp
using UnityBenchmarkHarness.Editor;
using UnityEditorInternal.Profiling;

namespace UnityBenchmarkHarness {
	public class BenchmarkPart {
		public string                                      Argument        { get; }
		public List<string>                                FuncNames       { get; } = new List<string>();
		public Dictionary<string, RuntimeBenchmarkResult>  RuntimeResults  { get; } = new Dictionary<string, RuntimeBenchmarkResult> ();
		public Dictionary<string, ProfilerBenchmarkResult> ProfilerResults { get; } = new Dictionary<string, ProfilerBenchmarkResult>();

		public BenchmarkPart(string argument) {
			Argument = argument;
		}

		public void AddProfilerResult(string funcName, FunctionData func) {
			var totalTimeStr = func.GetValue(ProfilerColumn.TotalTime);
			var gcMemoryStr = func.GetValue(ProfilerColumn.GCMemory);
			double totalTime;
			ByteSize gcMemory;
			if ( double.TryParse(totalTimeStr, out totalTime) && ByteSize.TryParse(gcMemoryStr, out gcMemory) ) {
				ProfilerResults.Add(funcName, new ProfilerBenchmarkResult(totalTime, (int)gcMemory.Bytes));
			}
		}

		internal List<BenchmarkResult> GetResults() {
			var results = new List<BenchmarkResult>();
			foreach ( var name in FuncNames ) {
				results.Add(new BenchmarkResult(RuntimeResults[name], ProfilerResults[name]));
			}
			return results;
		}
	}
}
