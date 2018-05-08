namespace UnityBenchmarkHarness {
	public class BenchmarkMeasure {
		public string Name  { get; }
		public double Min   { get; }
		public double Max   { get; }
		public double Avg   { get; }

		public BenchmarkMeasure(string name, double min, double max, double avg) {
			Name = name;
			Min  = min;
			Max  = max;
			Avg  = avg;
		}
	}
}
