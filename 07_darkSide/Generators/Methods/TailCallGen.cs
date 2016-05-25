namespace DotNext.Samples {
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    static class TailCallGenerator {
        static readonly MethodInfo mInfo_WriteLine = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) });
        internal static void Proceed(MethodBuilder method, ILGenerator ILGen) {
            // P1 [0,10] TailCall
            var @body = ILGen.DefineLabel();
            ILGen.Emit(OpCodes.Ldarg_1);
            ILGen.Emit(OpCodes.Brtrue_S, @body);
            ILGen.Emit(OpCodes.Ret);
            ILGen.MarkLabel(@body);
            ILGen.Emit(OpCodes.Ldarg_0); // this
            ILGen.Emit(OpCodes.Ldarg_1);
            ILGen.Emit(OpCodes.Call, mInfo_WriteLine);
            ILGen.Emit(OpCodes.Ldarg_1);
            ILGen.Emit(OpCodes.Ldc_I4, 1);
            ILGen.Emit(OpCodes.Sub);
            ILGen.Emit(OpCodes.Tailcall);
            ILGen.Emit(OpCodes.Call, method);
            ILGen.Emit(OpCodes.Ret);
        }
    }
}