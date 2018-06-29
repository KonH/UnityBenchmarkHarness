using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Profiling;
using UnityEngine;

namespace UnityBenchmarkHarness {
	public abstract class BenchmarkRuntimeTaskBase {
		public List<string> IterationNames { get; } = new List<string>();

		public BenchmarkRuntimeTaskBase(string methodName, string argumentName, int iterations) {
			for ( var i = 0; i < iterations; i++ ) {
				IterationNames.Add($"{methodName}_[{argumentName}]_{i}");
			}
		}

		public abstract bool Run();

		protected void PreRunGC() {
			GC.Collect(0, GCCollectionMode.Forced, true);
		}

		protected bool Perform(string name, Func<bool> action) {
			Debug.LogFormat("Perform: {0}", name);
			PreRunGC();
			Profiler.BeginSample(name);
			var result = action();
			Profiler.EndSample();
			return result;
		}
	}

	public class BenchmarkRuntimeTask<T>:BenchmarkRuntimeTaskBase {
		Func<T, bool> _payload;
		T _argument;

		public BenchmarkRuntimeTask(string methodName, int iterations, Func<T, bool> payload, T argument)
			:base(methodName, argument.ToString(), iterations) {
			_payload = payload;
			_argument = argument;
		}

		public override bool Run() {
			var result = false;
			foreach ( var iterName in IterationNames ) {
				result = Perform(iterName, () => _payload(_argument));
			}
			return result;
		}
	}

	public abstract class BenchmarkRunnerBase {
		protected List<BenchmarkRuntimeTaskBase> _tasks = new List<BenchmarkRuntimeTaskBase>();

		public abstract bool Run();

		public List<string> GetAllMethodNames() => _tasks.SelectMany(t => t.IterationNames).ToList();
	}

	public class BenchmarkRunner<T>:BenchmarkRunnerBase {
		
		public BenchmarkRunner(string name, int iterations, Func<T, bool> payload, params T[] arguments) {
			Init(name, iterations, payload, arguments);
		}

		public void Init(string name, int iterations, Func<T, bool> payload, params T[] arguments) {
			for ( var step = 0; step < arguments.Length; step++ ) {
				var curArgument = arguments[step];
				_tasks.Add(new BenchmarkRuntimeTask<T>(name, iterations, payload, curArgument));
			}
		}

		public override bool Run() {
			var result = false;
			foreach ( var task in _tasks ) {
				result = task.Run();
			}
			return result;
		}
	}
}