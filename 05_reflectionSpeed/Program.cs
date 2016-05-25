namespace DotNext.Samples {

    class Program {
        static void Main(string[] args) {
            //Fields
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Benchmarks_GetField_OneField));
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Benchmarks_GetField_TenField));
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Benchmarks_GetFieldValue_Struct));
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Benchmarks_SetFieldValue_Struct));
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Benchmarks_GetFieldValue_Class));
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Benchmarks_SetFieldValue_Class));
            // Properties
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Benchmarks_GetProperty_OneProperty));
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Benchmarks_GetProperty_TenProperties));
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Benchmarks_GetPropertyValue_Struct));
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Benchmarks_SetPropertyValue_Struct));
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Benchmarks_GetPropertyValue_Class));
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Benchmarks_SetPropertyValue_Class));
            // Parrots
            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Benchmarks_Parrots));
        }
    }
}