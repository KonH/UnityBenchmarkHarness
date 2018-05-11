namespace UnityBenchmarkHarness {
	public class ProfilerBenchmarkResult {
		public double TotalTime { get; }
		public int    GCMemory  { get; }

		public ProfilerBenchmarkResult(double totalTime, int gcMemory) {
			TotalTime = totalTime;
			GCMemory  = gcMemory;
		}
	}
}
