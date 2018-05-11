namespace UnityBenchmarkHarness {
	class BenchmarkResult {
		public RuntimeBenchmarkResult  Runtime  { get; }
		public ProfilerBenchmarkResult Profiler { get; }

		public BenchmarkResult(RuntimeBenchmarkResult runtime, ProfilerBenchmarkResult profiler) {
			Runtime  = runtime;
			Profiler = profiler;
		}
	}
}
