namespace DotNext.Samples {
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    static class CatchWhenGenerator {
        static readonly MethodInfo mInfo_WriteLine = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) });
        static readonly ConstructorInfo cInfo_Exception = typeof(Exception).GetConstructor(Type.EmptyTypes);
        //
        internal static void Proceed(MethodBuilder method, ILGenerator ILGen) {
            var @ret = ILGen.DefineLabel();
            var @endTry = ILGen.DefineLabel();
            var @endFilter = ILGen.DefineLabel();
            //.try
            var @try = ILGen.BeginExceptionBlock();
            ILGen.Emit(OpCodes.Ldstr, "Throw");
            ILGen.Emit(OpCodes.Call, mInfo_WriteLine);
            ILGen.Emit(OpCodes.Newobj, cInfo_Exception);
            ILGen.Emit(OpCodes.Throw);
            ILGen.MarkLabel(@endTry);
            //.end try
            //.filter
            //ILGen.BeginExceptFilterBlock();
            //ILGen.Emit(OpCodes.Isinst, typeof(Exception));
            //ILGen.Emit(OpCodes.Brfalse_S, endFilter);
            //ILGen.Emit(OpCodes.Ldarg_1);
            //ILGen.Emit(OpCodes.Brfalse_S, endFilter);
            //ILGen.MarkLabel(endFilter);
            //.end filter
            //.catch
            ILGen.BeginCatchBlock(typeof(Exception));
            //ILGen.BeginCatchBlock(null);
            ILGen.Emit(OpCodes.Ldstr, "Swallow");
            ILGen.Emit(OpCodes.Call, mInfo_WriteLine);
            ILGen.Emit(OpCodes.Leave_S, @ret);
            ILGen.EndExceptionBlock();
            //.end catch
            ILGen.MarkLabel(@ret);
            ILGen.Emit(OpCodes.Ret);
        }
    }
}