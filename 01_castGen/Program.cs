using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CLR;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using MA = System.Reflection.MethodAttributes;

namespace DotNext.Samples {
    class Program {
        static void Main(string[] args) {
            // P0 [0,3] Cast Reference
            object j = new J();
            A a1 = CastDelegate<A>.Default(j);
            A a2 = Caster<A>.Default.As(j);

            // P1 [0,2] Cast Reference (Evil)
            string evil = Caster<string>.Default.As(j);
            Console.WriteLine(evil.GetType().ToString());


            // P2 [0,3] How to use (For Lists)
            List<A> aList = new List<A>() { new B() };
            // Impossible
            // List<B> bList = (List<B>)(object)aList;

            // P3 [0,2] Cast Lists
            List<B> bList = Caster<List<B>>.Default.As(aList);
            Console.WriteLine(bList.Count.ToString());
            
          
            // BenchmarkDotNet.Running.BenchmarkRunner.Run<Benchmarks>();
        }
    }
    public abstract class Caster<T> {
        public static readonly Caster<T> Default;
        static Caster() {
            var dynAsmName = new AssemblyName("CastGen.Dynamic." + Guid.NewGuid().ToString());
            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(dynAsmName, AssemblyBuilderAccess.Run);
            var moduleBuilder = asmBuilder.DefineDynamicModule(dynAsmName.FullName);

            Type type = typeof(Caster<T>);
            var typeBuilder = moduleBuilder.DefineType("Darth" + type.Name, TypeAttributes.Public | TypeAttributes.Sealed, type);
            DoMethodOverride(typeBuilder, "As", GenerateCast);
            Default = Activator.CreateInstance(typeBuilder.CreateType()) as Caster<T>;
        }
        static void DoMethodOverride(TypeBuilder typebuilder, string methodName, Action<ILGenerator> generate) {
            var method = typebuilder.BaseType.GetMethod(methodName);
            var m = typebuilder.DefineMethod(method.Name,
                (method.Attributes & ~MA.Abstract) | MA.Final,
                method.CallingConvention, method.ReturnType,
                method.GetParameters().Select(p => p.ParameterType).ToArray());
            generate(m.GetILGenerator());
            typebuilder.DefineMethodOverride(m, method);
        }
        static void GenerateCast(ILGenerator ilGen) {
            ilGen.Emit(OpCodes.Ldarg_1);
            ilGen.Emit(OpCodes.Ret);
        }
        //
        public abstract T As(object instance);
    }
    static class CastDelegate<T> {
        public static readonly Func<object, T> Default;
        //
        static CastDelegate() {
            var ma = MethodAttributes.Public | MethodAttributes.Static;
            DynamicMethod dm = new DynamicMethod("_cast", ma, CallingConventions.Standard, typeof(T), new Type[] { typeof(object) }, typeof(CastDelegate<T>), true);
            GenerateCast(dm.GetILGenerator());
            Default = dm.CreateDelegate(typeof(Func<object, T>)) as Func<object, T>;
        }
        static void GenerateCast(ILGenerator ilGen) {
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ret);
        }
    }

    #region TestClasses
    public class A { }
    public class B : A { }
    public class C : B { }
    public class D : C { }
    public class E : D { }
    public class F : E { }
    public class G : F { }
    public class H : G { }
    public class I : H { }
    public class J : I { }
    #endregion TestClasses

    public class Benchmarks {
        static object instance = new J();
        static Caster<A> caster;
        static Func<object, A> castDelegate;
        [Setup]
        public void SetUp() {
            castDelegate = CastDelegate<A>.Default;
            caster = Caster<A>.Default;
        }
        [Benchmark, MethodImpl(MethodImplOptions.NoInlining)]
        public A Test_Cast() {
            return (A)instance;
        }
        [Benchmark, MethodImpl(MethodImplOptions.NoInlining)]
        public A Test_As() {
            return instance as A;
        }
        [Benchmark, MethodImpl(MethodImplOptions.NoInlining)]
        public A Test_EntityPtr() {
            return EntityPtr.CastRef<A>(instance);
        }
        [Benchmark, MethodImpl(MethodImplOptions.NoInlining)]
        public A Test_CastDelegate() {
            return castDelegate(instance);
        }
        [Benchmark, MethodImpl(MethodImplOptions.NoInlining)]
        public A Test_Caster() {
            return caster.As(instance);
        }
    }
}


// P03 [1,7] Benchmarks
/*
            Method |    Median |    StdDev |
------------------ |---------- |---------- |
           Test_As | 5.2365 ns | 0.2410 ns |
         Test_Cast | 5.2857 ns | 0.1746 ns |
 Test_CastDelegate | 2.7783 ns | 0.0912 ns |
       Test_Caster | 1.7604 ns | 0.0581 ns |
    Test_EntityPtr | 0.0011 ns | 0.0216 ns |
*/