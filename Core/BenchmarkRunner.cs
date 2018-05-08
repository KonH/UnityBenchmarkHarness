using System;
using System.Diagnostics;

namespace UnityBenchmarkHarness {
	class BenchmarkRunner {
		Stopwatch _timer;
		long      _startMem;

		public static BenchmarkRunner Start() {
			GC.Collect();
			return new BenchmarkRunner() {
				_startMem = GC.GetTotalMemory(false),
				_timer    = Stopwatch.StartNew()
			};
		}

		public static BenchmarkResult Perform(Action action) {
			var b = Start();
			action();
			return b.Finish();
		}

		public BenchmarkResult Finish() {
			_timer.Stop();
			var memUsed = GC.GetTotalMemory(false) - _startMem;
			return new BenchmarkResult(_timer.Elapsed.TotalMilliseconds, memUsed);
		}
	}
}