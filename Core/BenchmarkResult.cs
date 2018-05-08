namespace UnityBenchmarkHarness {
	class BenchmarkResult {
		public double Time    { get; }
		public long   Memory  { get; }
		public int    GCCount { get; }

		public BenchmarkResult(double time, long memory, int gcCount) {
			Time    = time;
			Memory  = memory;
			GCCount = gcCount;
		}
	}
}
