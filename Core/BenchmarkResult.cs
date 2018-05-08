namespace UnityBenchmarkHarness {
	class BenchmarkResult {
		public double Time   { get; }
		public long   Memory { get; }

		public BenchmarkResult(double time, long memory) {
			Time   = time;
			Memory = memory;
		}
	}
}
