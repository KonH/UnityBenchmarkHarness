﻿using System;
using System.IO;
using UnityEngine;

namespace UnityBenchmarkHarness {
	public static class BenchmarkSummaryExtensions {
		public static void WriteToConsole(this BenchmarkReport report) {
			Debug.Log(report.Name);
			foreach ( var s in report.Summaries ) {
				var str = $"{s.Name}\n";
				foreach ( var m in s.Measures ) {
					if ( AboutEqual(m.Min, m.Max) ) {
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

		public static bool AboutEqual(double x, double y) {
    		var epsilon = Math.Max(Math.Abs(x), Math.Abs(y)) * 1E-15;
    		return Math.Abs(x - y) <= epsilon;
		}


		// temp
		public static void ToJson(this BenchmarkReport report) {
			var str = JsonUtility.ToJson(report);
			Debug.Log(str);

			var text = File.ReadAllText(Path.Combine("Assets", "UnityBenchmarkHarness", "Templates", "index.html"));
			var resultText = text.Replace("{DATA}", str);

			File.WriteAllText($"{report.Name}.html", resultText);
		}
	}
}
