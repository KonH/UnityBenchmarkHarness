namespace UnityBenchmarkHarness {
	public class RuntimeBenchmarkResult {
		public double StopwatchMs { get; }

		public RuntimeBenchmarkResult(double stopwatchMs) {
			StopwatchMs = stopwatchMs;
		}
	}
}
