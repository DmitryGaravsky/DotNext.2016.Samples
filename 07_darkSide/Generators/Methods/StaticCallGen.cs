namespace DotNext.Samples {
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    static class StaticCallGenerator {
        static readonly MethodInfo mInfo_SayHello = typeof(Bar)
            .GetMethod("SayHello", Type.EmptyTypes);
        internal static void Proceed(MethodBuilder method, ILGenerator ILGen) {
            ILGen.Emit(OpCodes.Ldarg_1);  // bar
            ILGen.Emit(OpCodes.Call, mInfo_SayHello);
            ILGen.Emit(OpCodes.Ret);
        }
    }
}