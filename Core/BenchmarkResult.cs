namespace UnityBenchmarkHarness {
	class BenchmarkResult {
		public double Time        { get; }
		public long   UsedHeap    { get; }
		public long   MonoHeap    { get; }
		public long   MonoMemory  { get; }
		public long   AllocMemory { get; }
		public long   GCMemory    { get; }
		public int    GCCount     { get; }

		public BenchmarkResult(double time, long usedHeap, long monoHeap, long monoMemory, long allocMemory, long gcMemory, int gcCount) {
			Time        = time;
			UsedHeap    = usedHeap;
			MonoHeap    = monoHeap;
			MonoMemory  = monoMemory;
			AllocMemory = allocMemory;
			GCMemory    = gcMemory;
			GCCount     = gcCount;
		}
	}
}
