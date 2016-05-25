namespace DotNext.Samples {
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    static class TryFaultGenerator {
        static readonly MethodInfo mInfo_WriteLine = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) });
        static readonly ConstructorInfo cInfo_Exception = typeof(Exception).GetConstructor(Type.EmptyTypes);
        //
        internal static void Proceed(MethodBuilder method, ILGenerator ILGen) {
            var @ret = ILGen.DefineLabel();
            var @endTry = ILGen.DefineLabel();
            //.try
            var @try = ILGen.BeginExceptionBlock();
            ILGen.Emit(OpCodes.Ldstr, "Body");
            ILGen.Emit(OpCodes.Call, mInfo_WriteLine);
            ILGen.Emit(OpCodes.Ldarg_1);
            ILGen.Emit(OpCodes.Brfalse_S, @endTry);
            ILGen.Emit(OpCodes.Newobj, cInfo_Exception);
            ILGen.Emit(OpCodes.Throw);
            ILGen.MarkLabel(@endTry);
            ILGen.Emit(OpCodes.Leave_S, @ret);
            //.end try

            //.fault
            ILGen.BeginFaultBlock();
            ILGen.Emit(OpCodes.Ldstr, "Fault");
            ILGen.Emit(OpCodes.Call, mInfo_WriteLine);
            ILGen.EndExceptionBlock();
            //.end fault
            ILGen.MarkLabel(@ret);
            ILGen.Emit(OpCodes.Ret);
        }
    }
}