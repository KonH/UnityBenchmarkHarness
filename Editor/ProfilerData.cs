using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEditorInternal.Profiling;

namespace UnityBenchmarkHarness.Editor {
	public class ProfilerData {
		public List<FrameData> Frames { get; private set; } = new List<FrameData>();

		public ProfilerData(int firstFrameIndex, int lastFrameIndex) {
			var profilerSortColumn = ProfilerColumn.TotalTime;
			var viewType           = ProfilerViewType.Hierarchy;
			var property           = new ProfilerProperty();

			for ( var index = firstFrameIndex; index <= lastFrameIndex; index++ ) {
				property.SetRoot(index, profilerSortColumn, viewType);
				property.onlyShowGPUSamples = false;

				var frameData = new FrameData();
				while ( property.Next(true) ) {
					var functionData = new FunctionData(property);
					frameData.Functions.Add(functionData);
				}
				property.Cleanup();
				Frames.Add(frameData);
			}
		}
	}

	public class FrameData {
		public List<FunctionData> Functions { get; private set; } = new List<FunctionData>();
	}

	public class FunctionData {
		static ProfilerColumn[] AllColumns = Enum.GetValues(typeof(ProfilerColumn)) as ProfilerColumn[];

		public string              FunctionPath { get; }
		public FunctionDataValue[] Values       { get; }

		public string GetValue(ProfilerColumn column) {
			return FindDataValue(column)?.Value;
		}

		FunctionDataValue FindDataValue(ProfilerColumn column) {
			var length = Values.Length;
			for ( var i = 0; i < length; ++i ) {
				var value = Values[i];
				if ( value.Column == column ) {
					return value;
				}
			}
			return null;
		}

		public FunctionData(ProfilerProperty property) {
			Values = new FunctionDataValue[AllColumns.Length];
			FunctionPath = property.propertyPath;
			for ( int i = 0; i < AllColumns.Length; i++ ) {
				var column = AllColumns[i];
				if ( column == ProfilerColumn.DontSort ) {
					continue;
				} 
				Values[i] = new FunctionDataValue(
					column,
					property.GetColumn(column)
				);
			}
		}
	}

	public class FunctionDataValue {
		public ProfilerColumn Column { get; }
		public string         Value  { get; }

		public FunctionDataValue(ProfilerColumn column, string value) {
			Column = column;
			Value  = value;
		}
	}
}