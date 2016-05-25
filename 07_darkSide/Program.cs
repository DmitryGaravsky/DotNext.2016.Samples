namespace DotNext.Samples {
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using MA = System.Reflection.MethodAttributes;

    public class Bar {
        string name;
        public Bar(string name) {
            this.name = name;
        }
        // P0 [0,11] SayHello
        public void SayHello() {
            if(this == null) {
                Console.WriteLine("Oops, I'm NULL!");
                return;
            }
            object self = this;
            if(!(self is Bar))
                Console.WriteLine("Evil, I'm " + self.GetType() + "!");
            else
                Console.WriteLine("Hello! I'm " + name + "!");
        }
    }
    //
    public abstract class Foo {
        // P1 [1,5] TailCall
        /* 
        public override void TailCall(int seed) {
            if(seed == 0) return;
            Console.WriteLine(seed);
            TailCall(--seed);
        }
        */
        public abstract void TailCall(int seed);
        // P2 [1,3] StaticCall
        /* 
        public override void StaticCall(Bar bar) {
            Bar.SayHello();
        }
        */
        public abstract void StaticCall(Bar bar);
        // P3 [1,3] EvilCall
        /* 
        public override void EvilCall() {
            Evil.SayHello();
        }
        */
        public abstract void EvilCall();
        // P4 [1,7] TryFault
        /*
        public override void TryFault() {
            try {
                Console.WriteLine("Body");
                throw new Exception();
            }
            fault { Console.WriteLine("Fault"); }
        }
        */
        public abstract void TryFault(bool @throw);
        // P5 [1,9] CatchWhen
        /*
        Public Shared Sub CatchWhen(ByVal Swallow As Boolean)
            Try
                Console.WriteLine("Throw")
                Throw New Exception()
         
            Catch ex As Exception When Swallow
                Console.WriteLine("Swallow")
            End Try
        End Sub
        */
        public abstract void CatchWhen(bool swallow);
    }
    //
    class Program {
        static void Main(string[] args) {
            var dynAsmName = new AssemblyName("Foo.Dynamic." + Guid.NewGuid().ToString());
            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(dynAsmName, AssemblyBuilderAccess.Run);
            var moduleBuilder = asmBuilder.DefineDynamicModule(dynAsmName.FullName);
            //
            Type type = typeof(Foo);
            var typeBuilder = moduleBuilder.DefineType("Darth" + type.Name, TypeAttributes.Public, type);
            // Methods
            DoMethodOverride(typeBuilder, "TailCall", TailCallGenerator.Proceed);
            DoMethodOverride(typeBuilder, "StaticCall", StaticCallGenerator.Proceed);
            DoMethodOverride(typeBuilder, "EvilCall", EvilCallGenerator.Proceed);
            // Exceptions
            DoMethodOverride(typeBuilder, "TryFault", TryFaultGenerator.Proceed);
            DoMethodOverride(typeBuilder, "CatchWhen", CatchWhenGenerator.Proceed);
            //
            Type darthType = typeBuilder.CreateType();
            var darthFoo = Activator.CreateInstance(darthType) as Foo;

            //Console.WriteLine("01. Tail Call");
            //Console.ReadLine();
            //const int N = 100000;
            //TailCall(N);
            //darthFoo.TailCall(N);
            //Console.ReadKey();

            //Console.Clear();
            //Console.WriteLine("02. Static Call");
            //darthFoo.StaticCall(new Bar("Bar"));
            //darthFoo.StaticCall(null);
            //Console.ReadLine();

            //Console.Clear();
            //Console.WriteLine("03. Evil Call");
            //darthFoo.EvilCall();
            //Console.ReadLine();

            //Console.Clear();
            //Console.WriteLine("04. Try Fault");
            //darthFoo.TryFault(false);
            //Console.WriteLine();
            //try { darthFoo.TryFault(true); }
            //catch { }
            //Console.ReadLine();

            //Console.Clear();
            //Console.WriteLine("05. Catch When");
            //Console.WriteLine("DO NOT WORK DUE ILGENERATOR RESTRICTIOINS");
            //try { darthFoo.CatchWhen(false); }
            //catch { }
            //Console.WriteLine();
            //darthFoo.CatchWhen(true);
            //Console.ReadLine();

            Console.Clear();
            Console.WriteLine("05. Catch When");
            HelloFromDynamicILInfo();
            Console.ReadLine();
        }
        static void DoMethodOverride(TypeBuilder typebuilder, string methodName, Action<MethodBuilder, ILGenerator> gen) {
            var method = typebuilder.BaseType.GetMethod(methodName);
            var m = typebuilder.DefineMethod(method.Name,
                method.Attributes & ~MA.Abstract,
                method.CallingConvention, method.ReturnType,
                method.GetParameters().Select(p => p.ParameterType).ToArray());
            gen(m, m.GetILGenerator());
            typebuilder.DefineMethodOverride(m, method);
        }
        static void TailCall(int seed) {
            if(seed == 0) return;
            Console.WriteLine(seed);
            TailCall(--seed);
        }
        static void HelloFromDynamicILInfo() {
            DynamicMethod dm = new DynamicMethod("HelloWorld", typeof(void), Type.EmptyTypes, typeof(Foo), false);
            DynamicILInfo il = dm.GetDynamicILInfo();
            SignatureHelper sigHelper = SignatureHelper.GetLocalVarSigHelper();
            il.SetLocalSignature(sigHelper.GetSignature());
            byte[] code = { 
            /* ldstr */ 0x72, 0x00, 0x00, 0x00, 0x00, 
            /* call  */ 0x28, 0x00, 0x00, 0x00, 0x00,
            /* ret   */ 0x2a
            };
            byte[] strTokenBytes = BitConverter.GetBytes(il.GetTokenFor("Hello world!"));
            var writeLine = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) });
            byte[] mTokenBytes = BitConverter.GetBytes(il.GetTokenFor(writeLine.MethodHandle));
            Array.Copy(strTokenBytes, 0, code, 1, strTokenBytes.Length);
            Array.Copy(mTokenBytes, 0, code, 5, mTokenBytes.Length);
            il.SetCode(code, 3);
            var hello = (Action)dm.CreateDelegate(typeof(Action));
        }
    }
}