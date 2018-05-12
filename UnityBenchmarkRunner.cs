using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// temp
using UnityBenchmarkHarness.Editor;
using UnityEditorInternal;

namespace UnityBenchmarkHarness {
	public class UnityBenchmarkRunner : MonoBehaviour {
		public float StartPause = 3.0f;

		protected List<BenchmarkRunner> Runners = new List<BenchmarkRunner>();

		int          _lastFrameIndex = 0;
		List<string> _foundNames     = new List<string>();

		IEnumerator Start() {
			yield return new WaitForSeconds(StartPause);
			Run();
			yield return WaitForProfilerData();
		}

		void Run() {
			bool result = false;
			result = RunInternal();
			Debug.Log(result);
		}

		// temp
		protected virtual bool RunInternal() {
			return true;
		}

		IEnumerator WaitForProfilerData() {
			while ( true ) {
				if ( ProfilerDriver.lastFrameIndex > _lastFrameIndex ) {
					var data = new ProfilerData(_lastFrameIndex, ProfilerDriver.lastFrameIndex);
					_lastFrameIndex = ProfilerDriver.lastFrameIndex;
					foreach ( var frame in data.Frames ) {
						foreach ( var func in frame.Functions ) {
							_foundNames.Clear();
							var allComplete = true;
							foreach ( var runner in Runners ) {
								if ( runner.IsComplete ) {
									continue;
								}
								allComplete = false;
								foreach ( var part in runner.Parts ) {
									foreach ( var wantedFuncName in part.FuncNames ) {
										if ( func.FunctionPath.EndsWith(wantedFuncName) ) {
											part.AddProfilerResult(wantedFuncName, func);
											_foundNames.Add(wantedFuncName);
										}
									}
								}
							}
							if ( allComplete ) {
								DrawResults();
								yield break;
							}
						}
					}
					yield return null;
				}
			}
		}

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
		}
	}
}
