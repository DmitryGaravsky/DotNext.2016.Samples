namespace DotNext.Samples {
    public class Benchmarks_Parrots {
        //
        class Parrot {
            public int f = 42;
            public int P {
                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
                get { return f; }
            }
        }
        //
        static readonly Parrot parrotInstance = new Parrot();
        static readonly System.Func<Parrot, int> parrotFunc_Field = p => p.f;
        static readonly System.Func<Parrot, int> parrotFunc_Property = p => p.P;
        //
        [BenchmarkDotNet.Attributes.Benchmark]
        public int GetParrot_Field() {
            return parrotFunc_Field(parrotInstance);
        }
        [BenchmarkDotNet.Attributes.Benchmark]
        public int GetParrot_Property() {
            return parrotFunc_Property(parrotInstance);
        }
    }
}