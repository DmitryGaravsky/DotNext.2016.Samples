namespace DotNext.Samples {
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    using MA = System.Reflection.MethodAttributes;

    class Program {
        static void Main(string[] args) {
            DynamicMethod dm = new DynamicMethod("DoSomething", null, Type.EmptyTypes);
            var dm_ILGen = dm.GetILGenerator();
            CheckRestrictions(dm_ILGen);
            //
            var dynAsmName = new AssemblyName("Dynamic." + Guid.NewGuid().ToString());
            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(dynAsmName, AssemblyBuilderAccess.Run);
            var moduleBuilder = asmBuilder.DefineDynamicModule(dynAsmName.FullName);
            //
            var typeBuilder = moduleBuilder.DefineType("Foo", TypeAttributes.Public);
            var methodBuilder = typeBuilder.DefineMethod("DoSomething", MA.Public);
            var mb_ILGen = methodBuilder.GetILGenerator();
            CheckRestrictions(mb_ILGen);
        }
        static void CheckRestrictions(ILGenerator ILGen) {
            var ilGenType = ILGen.GetType();
            var methods = ilGenType.GetMethods();
            bool foundAny = false;
            var pattern = ILReader.Analyzer.NotSupported.Instance;
            for(int i = 0; i < methods.Length; i++) {
                var method = methods[i];
                var reader = CreateReader(method);
                if(pattern.Match(reader) && pattern.StartIndex == 0) {
                    if(!foundAny) {
                        Console.WriteLine(ilGenType.ToString() + ":");
                        foundAny = true;
                    }
                    Console.WriteLine(" - " + method.Name);
                }
            }
        }
        //
        static ILReader.Readers.IILReader CreateReader(MethodInfo method) {
            var cfg = ILReader.Configuration.Resolve(method);
            return cfg.GetReader(method);
        }
    }
}