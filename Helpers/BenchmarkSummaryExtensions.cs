using System.Collections.Generic;
using UnityEngine;

namespace UnityBenchmarkHarness {
	public static class BenchmarkSummaryExtensions {
		public static void WriteToConsole(this List<BenchmarkSummary> summaries) {
			foreach ( var s in summaries ) {
				var str = $"{s.Name}\n";
				foreach ( var m in s.Measures ) {
					if ( m.Min == m.Max ) {
						str += string.Format("{0}: {1:N}\n", m.Name, m.Min);
					} else {
						str += string.Format(
							"{0}: [{1:N}-{2:N}], avg: {3:N}\n",
							m.Name, m.Min, m.Max, m.Avg);
					}
				}
				Debug.Log(str);
			}
		}
	}
}
