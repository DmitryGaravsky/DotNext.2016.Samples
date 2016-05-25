namespace DotNext.Samples {
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    using FastAccessors.Monads;

    public class Foo {
        public void SayHello(string name, int year) {
            string format = "Hello, {0}.{1}!";
            Console.WriteLine(string.Format(format, name, year));
        }
    }
    public abstract class Bar {
        public abstract void SayHello(string name, int year);
    }
    //
    class Program {
        static void Main(string[] args) {
            // P0 [0,3] View IL (WinDbg)
            Foo foo = new Foo();
            foo.SayHello("DotNext", 2016);
            Console.ReadLine();

            // P1 [0,3] Read IL
            Type fooType = typeof(Foo);
            var method = fooType.GetMethod("SayHello");
            DumpMethod(method);

            // P3 View IL - Just set breakpoint here and watch the method
            Console.ReadLine();
            Console.Clear();

            // P4 [5,7] Create Dynamic Type
            var formatMethod = typeof(string).GetMethod("Format", new Type[] { typeof(string), typeof(object), typeof(object) });
            var writeLineMethod = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) });
            var toStringMethod = typeof(object).GetMethod("ToString", Type.EmptyTypes);
            Type barType = Dynamic.Generator.Generate(typeof(Bar), gen =>
            {
                gen.Emit(OpCodes.Ldstr, "Hello from DynamicType, {0}.{1}!");
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_2);
                gen.Emit(OpCodes.Box, typeof(int));
                gen.Emit(OpCodes.Call, formatMethod);
                gen.Emit(OpCodes.Call, writeLineMethod);
                gen.Emit(OpCodes.Ret);
            });
            Bar bar = (Bar)Activator.CreateInstance(barType);
            bar.SayHello("DotNext", 2016);
            Console.ReadLine();

            // P5 [0,2] Dump IL (Dynamic Type) 
            method = barType.GetMethod("SayHello");
            DumpMethod(method);

            // P6 View IL - Just set breakpoint here and watch the method
            Console.ReadLine();
            
            // P7 [2,4] Create DynamicMethod
            var dm = new DynamicMethod("SayHello", null, new Type[] { typeof(string), typeof(int) });
            var ILGen = dm.GetILGenerator();
            ILGen.Emit(OpCodes.Ldstr, "Hello from DynamicMethod, {0}.{1}!");
            ILGen.Emit(OpCodes.Ldarg_0);
            ILGen.Emit(OpCodes.Ldarg_1);
            ILGen.Emit(OpCodes.Box, typeof(int));

            // P8 Dump IL (Dynamic Method before fixup)
            DumpMethod(dm);
            Console.WriteLine("Dynamic Method Status: before fixup");
            Console.ReadLine();
            Console.Clear();

            ILGen.Emit(OpCodes.Call, formatMethod);
            ILGen.Emit(OpCodes.Call, writeLineMethod);
            ILGen.Emit(OpCodes.Ret);


            // P9 [0,3] CreateDelegate
            var sayHello = dm.CreateDelegate(typeof(Action<string, int>)) as Action<string, int>;
            sayHello("DotNext", 2016);
            Console.ReadLine();

            // P10 [0,2] Dump IL (Dynamic Method)
            DumpMethod(dm);
            Console.ReadLine();
        }

        static void DumpMethod(MethodInfo method) {
            Console.Clear();
            Console.WriteLine(method.ToString() + " " + method.GetMethodImplementationFlags().ToString());
            //
            var methodBody = method.GetMethodBody();
            if(methodBody != null) {
                byte[] ilBytes = methodBody.GetILAsByteArray();
                //
                Console.WriteLine(string.Format("// Code size {0} (0x{0:X2}) ", ilBytes.Length));
                Console.WriteLine(".maxstack " + methodBody.MaxStackSize.ToString());
                //
                if(methodBody.LocalVariables.Count > 0) {
                    Console.WriteLine(Environment.NewLine + "// LocalSignature");
                    byte[] localSignature = method.Module.ResolveSignature(methodBody.LocalSignatureMetadataToken);
                    DumpBytes(localSignature, 2, ConsoleColor.Yellow);
                    Console.WriteLine();
                    if(methodBody.InitLocals)
                        Console.WriteLine(".locals init (");
                    else
                        Console.WriteLine(".locals (");
                    for(int i = 0; i < methodBody.LocalVariables.Count; i++) {
                        Console.WriteLine(string.Format("  [{0}] {1}", i, methodBody.LocalVariables[i]));
                    }
                    Console.WriteLine(")");
                }
                //
                Console.WriteLine(".body (");
                DumpBytes(ilBytes);
                Console.WriteLine(Environment.NewLine + ")");
            }
        }
        static void DumpMethod(DynamicMethod method) {
            Console.Clear();
            Console.WriteLine(method.ToString() + " " + method.GetMethodImplementationFlags().ToString());
            //
            ILGenerator ILGen = method.GetILGenerator();
            int maxStackSize = (int)ILGen.@ƒ("m_maxStackSize");
            byte[] ilBytes, localSignature;

            var resolver = method.@ƒ("m_resolver");
            if(resolver != null) {
                Type DynamicResolverType = Type.GetType("System.Reflection.Emit.DynamicResolver");
                ilBytes = (byte[])resolver.@ƒ(DynamicResolverType, "m_code");
                localSignature = (byte[])resolver.@ƒ(DynamicResolverType, "m_localSignature");
            }
            else {
                ilBytes = new byte[ILGen.ILOffset];
                Array.Copy((byte[])ILGen.@ƒ("m_ILStream"), ilBytes, ilBytes.Length);
                localSignature = ((SignatureHelper)ILGen.@ƒ("m_localSignature")).GetSignature();
            }

            Console.WriteLine(string.Format("// Code size {0} (0x{0:X2}) ", ilBytes.Length));
            Console.WriteLine(".maxstack " + maxStackSize.ToString());
            if(localSignature != null && localSignature.Length > 0) {
                Console.WriteLine("// LocalSignature");
                DumpBytes(localSignature, 2, ConsoleColor.Yellow);
                Console.WriteLine();
            }
            //
            Console.WriteLine(".body (");
            DumpBytes(ilBytes);
            Console.WriteLine(Environment.NewLine + ")");
        }
        static void DumpBytes(byte[] bytes, int tab = 2, ConsoleColor foreColor = ConsoleColor.Magenta) {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = foreColor;
            Console.SetCursorPosition(Console.CursorLeft + tab, Console.CursorTop);
            for(int i = 0; i < bytes.Length; i++) {
                string hex = string.Format("{0:X2} ", bytes[i]);
                if((i + 1) % 8 == 0) {
                    Console.WriteLine(hex);
                    Console.SetCursorPosition(Console.CursorLeft + tab, Console.CursorTop);
                }
                else Console.Write(hex);
            }
            Console.ForegroundColor = color;
        }
    }
}