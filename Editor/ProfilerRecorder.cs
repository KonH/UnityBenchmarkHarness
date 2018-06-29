using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityBenchmarkHarness.Editor {
	public class ProfilerRecorder {
		int _lastFrameIndex;

		public Dictionary<string, FunctionData> Results { get; } = new Dictionary<string, FunctionData>();

		List<string> _methods = new List<string>();

		public ProfilerRecorder(List<string> methods) {
			_methods = new List<string>(methods);
			foreach ( var m in methods ) {
				Results.Add(m, null);
			}
		}

		public bool Update() {
			if ( ProfilerDriver.lastFrameIndex > _lastFrameIndex ) {
				var data = new ProfilerData(_lastFrameIndex, ProfilerDriver.lastFrameIndex);
				_lastFrameIndex = ProfilerDriver.lastFrameIndex;
				foreach ( var frame in data.Frames ) {
					foreach ( var func in frame.Functions ) {
						var keys = _methods;
						foreach ( var key in keys ) {
							if ( func.FunctionPath.EndsWith(key, System.StringComparison.Ordinal) ) {
								Results[key] = func;
							}
						}
					}
				}
			}
			return Results.All(p => p.Value != null);
		}

		/* todo
		// temp
		void DrawResults() {
			var report = new BenchmarkReport();
			report.Name = "BenchmarkName";
			report.Summaries = new List<BenchmarkSummary>();
			foreach ( var runner in Runners ) {
				foreach ( var part in runner.Parts ) {
					var results = part.Results.Values;
					var summaryName = $"{runner.SelfName}";
					if ( runner.Parts.Count > 1 ) {
						summaryName += $"_{part.Argument}";
					}
					var summary = new BenchmarkSummary(summaryName, results);
					report.Summaries.Add(summary);
				}
			}
			report.ToJson();
		}*/
	}
}