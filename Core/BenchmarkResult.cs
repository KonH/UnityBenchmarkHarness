namespace UnityBenchmarkHarness {
	public class BenchmarkResult {
		public double TotalTime { get; }
		public int    GCMemory  { get; }

		public BenchmarkResult(double totalTime, int gcMemory) {
			TotalTime = totalTime;
			GCMemory  = gcMemory;
		}
	}
}
