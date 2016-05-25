using System;
using System.Collections.Generic;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BF = System.Reflection.BindingFlags;
using FastAccessors;

namespace DotNext.Samples {
    #region Test Classes
    class Foo {
        static string static_Name = "Foo";
        //
        string private_Name;
        readonly string readonly_Name;
        public string public_Name;
        public Foo(string name = "Foo") {
            this.readonly_Name = name + "readonly";
            this.private_Name = name + "private";
            this.public_Name = name + "public";
        }
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)] // Suppress Optimization
        public static string GetName(object obj) {
            return ((Foo)obj).private_Name + ((Foo)obj).public_Name + ((Foo)obj).readonly_Name;
        }
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)] // Suppress Optimization
        public static string GetName() {
            return static_Name;
        }
    }
    #endregion
    class Program {
        static void Main(string[] args) {
            BenchmarkDotNet.Running.BenchmarkRunner.Run<Benchmarks>();
        }
    }
    //
    public class Benchmarks {
        #region SetUp
        object obj;
        Foo instance;
        Type instanceType;
        FieldAccessor_Reflection r1;
        FieldAccessor_ReflectionCached r2;
        static int fld_private_Name_T;
        static int fld_private_Name;
        static int fld_static_Name;
        [Setup]
        public void SetUp() {
            obj = instance = new Foo();
            instanceType = typeof(Foo);
            r1 = new FieldAccessor_Reflection(typeof(Foo));
            r2 = new FieldAccessor_ReflectionCached(typeof(Foo));
            //
            fld_private_Name = "private_Name".ƒRegister(instanceType);
            fld_static_Name = "static_Name".ƒsRegister(instanceType, true);
            fld_private_Name_T = "private_Name".ƒRegister<Foo>(true);
        }
        #endregion
        [Benchmark(Description = "1.1. Reflection(Instance)")]
        public object Accessor01_Reflection() {
            return r1.GetFieldValue(obj, "private_Name");
        }
        [Benchmark(Description = "1.2. Reflection(Static)")]
        public object Accessor01_Reflection_Static() {
            return r1.GetFieldValue(null, "static_Name");
        }
        [Benchmark(Description = "1.3. Reflection(Public)")]
        public object Accessor01_Reflection_Public() {
            return r1.GetFieldValue(obj, "public_Name");
        }
        [Benchmark(Description = "1.4. Reflection(Readonly)")]
        public object Accessor01_Reflection_Readonly() {
            return r1.GetFieldValue(obj, "readonly_Name");
        }
        [Benchmark(Description = "2.1. ReflectionCache(Instance)")]
        public object Accessor02_Reflection() {
            return r2.GetFieldValue(obj, "private_Name");
        }
        [Benchmark(Description = "2.2. ReflectionCache(Static)")]
        public object Accessor02_Reflection_Static() {
            return r2.GetFieldValue(null, "static_Name");
        }
        [Benchmark(Description = "3.1 DynamicMethod(Instance)")]
        public object Accessor03_DynamicMethod() {
            return obj.@ƒ(instanceType, "private_Name");
        }
        [Benchmark(Description = "3.2 DynamicMethod(Static)")]
        public object Accessor03_DynamicMethod_Static() {
            return instanceType.@ƒs("static_Name");
        }
        [Benchmark(Description = "3.3 DynamicMethod(Public)")]
        public object Accessor03_DynamicMethod_Public() {
            return obj.@ƒ(instanceType, "public_Name");
        }
        [Benchmark(Description = "3.4 DynamicMethod(Readonly)")]
        public object Accessor03_DynamicMethod_Readonly() {
            return obj.@ƒ(instanceType, "readonly_Name");
        }
        [Benchmark(Description = "3.5 DynamicMethod(Generic)")]
        public object Accessor03_DynamicMethod_Generic() {
            return instance.@ƒ("private_Name");
        }
        [Benchmark(Description = "4.1 DynamicMethod(Instance,Key)")]
        public object Accessor04_DynamicMethod_Key() {
            return obj.@ƒ(fld_private_Name);
        }
        [Benchmark(Description = "4.2 DynamicMethod(Static,Key)")]
        public object Accessor04_DynamicMethod_Static_Key() {
            return Accessor.@ƒs(fld_static_Name);
        }
        [Benchmark(Description = "4.3 DynamicMethod(Generic,Key)")]
        public object Accessor04_DynamicMethod_Generic_Key() {
            return instance.@ƒ(fld_private_Name_T);
        }
        [Benchmark(Description = "4.4 DynamicMethod(Generic,DefaultField)")]
        public object Accessor04_DynamicMethod_Generic_DefaultField() {
            return instance.@ƒDefault();
        }
        [Benchmark(Description = "4.5 DynamicMethod(Static,DefaultField)")]
        public object Accessor04_DynamicMethod_Static_DefaultField() {
            return Accessor.@ƒsDefault();
        }
    }
    //
    sealed class FieldAccessor_Reflection {
        readonly Type type;
        public FieldAccessor_Reflection(Type type) {
            this.type = type;
        }
        public object GetFieldValue(object instance, string fieldName) {
            var field = type.GetField(fieldName, (instance == null) ?
                (BF.Public | BF.NonPublic | BF.Static) :
                (BF.Public | BF.NonPublic | BF.Instance));
            return (field != null) ? field.GetValue(instance) : null;
        }
    }
    sealed class FieldAccessor_ReflectionCached {
        readonly Type type;
        public FieldAccessor_ReflectionCached(Type type) {
            this.type = type;
        }
        static IDictionary<string, FieldInfo> fInfos = new Dictionary<string, FieldInfo>(StringComparer.Ordinal);
        public object GetFieldValue(object instance, string fieldName) {
            FieldInfo field;
            string key = type.Name + "." + fieldName;
            if(!fInfos.TryGetValue(key, out field)) {
                field = type.GetField(fieldName, (instance == null) ?
                   (BF.Public | BF.NonPublic | BF.Static) :
                   (BF.Public | BF.NonPublic | BF.Instance));
                fInfos.Add(key, field);
            }
            return (field != null) ? field.GetValue(instance) : null;
        }
    }
}