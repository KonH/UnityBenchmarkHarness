using System.Collections.Generic;
using UnityEngine;

namespace UnityBenchmarkHarness {
	public abstract class UnityBenchmark : MonoBehaviour {
		public abstract List<BenchmarkRunnerBase> CreateRunners();
	}
}
