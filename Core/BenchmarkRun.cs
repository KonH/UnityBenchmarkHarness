using System.Diagnostics;

namespace UnityBenchmarkHarness {
	class BenchmarkRun {
		public double TotalMilliseconds => _sw.Elapsed.TotalMilliseconds;

		Stopwatch _sw = new Stopwatch();

		public void Start() {
			_sw.Start();
		}

		public void Stop() {
			_sw.Stop();
		}
	}
}