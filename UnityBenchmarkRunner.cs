using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityBenchmarkHarness {
	public class UnityBenchmarkRunner : MonoBehaviour {
		public float StartPause = 3.0f;

		public List<UnityBenchmark> Benchmarks = new List<UnityBenchmark>();

		IEnumerator Start() {
			var runners = CreateRunners();
			yield return new WaitForSeconds(StartPause);
			Run(runners);
		}

		public List<BenchmarkRunnerBase> CreateRunners() {
			return Benchmarks.SelectMany(b => b.CreateRunners()).ToList();
		}

		public void Run(List<BenchmarkRunnerBase> runners) {
			var result = false;
			foreach ( var runner in runners ) {
				result = runner.Run();
			}
			Debug.LogFormat("Benchmarks ended with side-effect result: {0}", result);
		}
	}
}
