using System;
using System.Collections.Generic;

namespace UnityBenchmarkHarness {
	[Serializable]
	public class BenchmarkReport {
		public string                 Name      = string.Empty;
		public List<BenchmarkSummary> Summaries = new List<BenchmarkSummary>();
	}
}
