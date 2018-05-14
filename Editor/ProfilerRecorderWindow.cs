using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityBenchmarkHarness.Editor {
	public class ProfilerRecorderWindow : EditorWindow {
		public UnityBenchmarkRunner Runner;

		ProfilerRecorder _recorder;
		List<string> _methods = new List<string>();

		[MenuItem("Tools/UnityBenchmarkHarness/ProfilerRecorded")]
		static void Initialize() {
			var window = GetWindow<ProfilerRecorderWindow>();
			window.Show();
		}

		void OnGUI() {
			if ( _methods.Count == 0 ) {
				GUILayout.Label("Select runner:");
				Runner = EditorGUILayout.ObjectField(Runner, typeof(UnityBenchmarkRunner), true) as UnityBenchmarkRunner;
				if ( Runner ) {
					_methods = Runner.CreateRunners().SelectMany(r => r.GetAllMethodNames()).ToList();
				}
			} else {
				GUILayout.Label($"Loaded {_methods.Count} methods");
				if ( _recorder == null ) {
					if ( GUILayout.Button("Record") ) {
						StartRecording();
					}
				} else {
					if ( GUILayout.Button("Stop") ) {
						_recorder = null;
					}
				}
				if ( GUILayout.Button("Clear") ) {
					Runner = null;
					_methods.Clear();
				}
			}
		}

		public void StartRecording() {
			_recorder = new ProfilerRecorder(_methods);
		}

		void Update() {
			if ( _recorder != null ) {
				var done = _recorder.Update();
				if ( done ) {
					Debug.LogFormat("Recordered {0} results ", _recorder.Results.Count);
					_recorder = null;
				}
			}
		}

	}
}
