using System;

namespace UnityBenchmarkHarness {
	[Serializable]
	public class BenchmarkMeasure {
		public string Name = string.Empty;
		public double Min  = 0.0;
		public double Max  = 0.0;
		public double Avg  = 0.0;

		public BenchmarkMeasure(string name, double min, double max, double avg) {
			Name = name;
			Min  = min;
			Max  = max;
			Avg  = avg;
		}
	}
}
