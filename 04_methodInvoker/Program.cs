using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using BenchmarkDotNet.Attributes;
using BF = System.Reflection.BindingFlags;
using MA = System.Reflection.MethodAttributes;

namespace DotNext.Samples {
    #region Test Classes
    class Obj {
        string GetName() {
            return "Obj";
        }
        static string GetStaticName() {
            return "Obj";
        }
        public string GetPublicName() {
            return "Obj";
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
        Obj instance;
        Type instanceType;
        MethodInvoker_Reflection r1;
        MethodInvoker_ReflectionCached r2;
        [Setup]
        public void SetUp() {
            obj = instance = new Obj();
            instanceType = typeof(Obj);
            r1 = new MethodInvoker_Reflection(typeof(Obj));
            r2 = new MethodInvoker_ReflectionCached(typeof(Obj));
        }
        #endregion
        [Benchmark(Description = "1.1. Reflection(Instance)")]
        public object Invoke01_Reflection() {
            return r1.Invoke(obj, "GetName");
        }
        [Benchmark(Description = "1.2. Reflection(Static)")]
        public object Invoke01_Reflection_Static() {
            return r1.Invoke(null, "GetStaticName");
        }
        [Benchmark(Description = "1.3. Reflection(Public)")]
        public object Invoke01_Reflection_Public() {
            return r1.Invoke(obj, "GetPublicName");
        }
        [Benchmark(Description = "2.1. ReflectionCache(Instance)")]
        public object Invoke02_Reflection() {
            return r2.Invoke(obj, "GetName");
        }
        [Benchmark(Description = "2.2. ReflectionCache(Static)")]
        public object Invoke02_Reflection_Static() {
            return r2.Invoke(null, "GetStaticName");
        }
        [Benchmark(Description = "2.3. ReflectionCache(Public)")]
        public object Invoke02_Reflection_Public() {
            return r2.Invoke(obj, "GetPublicName");
        }
    }
    //
    sealed class MethodInvoker_Reflection {
        readonly Type type;
        public MethodInvoker_Reflection(Type type) {
            this.type = type;
        }
        public object Invoke(object instance, string methodName, object[] parameters = null) {
            var method = type.GetMethod(methodName, (instance == null) ?
                (BF.Public | BF.NonPublic | BF.Static) :
                (BF.Public | BF.NonPublic | BF.Instance));
            return (method != null) ? method.Invoke(instance, parameters) : null;
        }
    }
    sealed class MethodInvoker_ReflectionCached {
        readonly Type type;
        public MethodInvoker_ReflectionCached(Type type) {
            this.type = type;
        }
        static IDictionary<string, MethodInfo> mInfos = new Dictionary<string, MethodInfo>();
        public object Invoke(object instance, string methodName, object[] parameters = null) {
            MethodInfo method;
            string key = type.Name + "." + methodName;
            if(!mInfos.TryGetValue(key, out method)) {
                method = type.GetMethod(methodName, (instance == null) ?
                    (BF.Public | BF.NonPublic | BF.Static) :
                    (BF.Public | BF.NonPublic | BF.Instance));
                mInfos.Add(key, method);
            }
            return (method != null) ? method.Invoke(instance, parameters) : null;
        }
    }
    static class @Invoke {
        /// <summary>@Monad(object): Get Field Value</summary>
        [System.Diagnostics.DebuggerStepThrough]
        public static object @ƒ(this object instance, Type type, string fieldName) {
            return FieldAccessor.GetFieldValue(instance, type, fieldName);
        }
        /// <summary>@Monad(Type): Get Static Field Value</summary>
        [System.Diagnostics.DebuggerStepThrough]
        public static object @ƒ(this Type type, string fieldName) {
            return FieldAccessorStatic.GetFieldValue(type, fieldName);
        }
        /// <summary>@Monad(T): Get Field Value</summary>
        [System.Diagnostics.DebuggerStepThrough]
        public static object @ƒ<T>(this T instance, string fieldName) {
            return FieldAccessor<T>.GetFieldValue(instance, fieldName);
        }
        #region Accessors
        static class FieldAccessor {
            static IDictionary<string, Func<object, object>> accessors = new Dictionary<string, Func<object, object>>();
            static Func<object, object> defaultAccessor = _ => null;
            internal static object GetFieldValue(object instance, Type type, string fieldName) {
                Func<object, object> accessor;
                string key = type.Name + "." + fieldName;
                if(!accessors.TryGetValue(key, out accessor)) {
                    var field = type.GetField(fieldName, BF.Public | BF.NonPublic | BF.Instance);
                    accessor = (field != null) ? EmitFieldAccesssor(field, type) : defaultAccessor;
                    accessors.Add(key, accessor);
                }
                return accessor(instance);
            }
            readonly static Type[] accessorArgs = new Type[] { typeof(object) };
            static Func<object, object> EmitFieldAccesssor(System.Reflection.FieldInfo field, Type type) {
                var method = new DynamicMethod("__get_" + field.Name, MA.Static | MA.Public, CallingConventions.Standard,
                    typeof(object), accessorArgs, typeof(FieldAccessor).Module, true);
                var ilGen = method.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(type.IsValueType ? OpCodes.Unbox : OpCodes.Castclass, type);
                ilGen.Emit(OpCodes.Ldfld, field);
                ilGen.Emit(OpCodes.Ret);
                return method.CreateDelegate(typeof(Func<object, object>)) as Func<object, object>;
            }
        }
        static class FieldAccessor<T> {
            static IDictionary<string, Func<T, object>> accessors = new Dictionary<string, Func<T, object>>();
            static Func<T, object> defaultAccessor = _ => null;
            internal static object GetFieldValue(T instance, string fieldName) {
                Func<T, object> accessor;
                if(!accessors.TryGetValue(fieldName, out accessor)) {
                    var field = type.GetField(fieldName, BF.Public | BF.NonPublic | BF.Instance);
                    accessor = (field != null) ? EmitFieldAccesssor(field) : defaultAccessor;
                    accessors.Add(fieldName, accessor);
                }
                return accessor(instance);
            }
            readonly static Type type = typeof(T);
            readonly static Type[] accessorArgs = new Type[] { typeof(T) };
            static Func<T, object> EmitFieldAccesssor(System.Reflection.FieldInfo field) {
                var method = new DynamicMethod("__get_" + field.Name, MA.Static | MA.Public, CallingConventions.Standard,
                    typeof(object), accessorArgs, type, true);
                var ilGen = method.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, field);
                ilGen.Emit(OpCodes.Ret);
                return method.CreateDelegate(typeof(Func<T, object>)) as Func<T, object>;
            }
        }
        static class FieldAccessorStatic {
            static IDictionary<string, Func<object>> accessors = new Dictionary<string, Func<object>>();
            static Func<object> defaultAccessor = () => null;
            internal static object GetFieldValue(Type type, string fieldName) {
                Func<object> accessor;
                string key = type.Name + "." + fieldName;
                if(!accessors.TryGetValue(key, out accessor)) {
                    var field = type.GetField(fieldName, BF.Public | BF.NonPublic | BF.Static);
                    accessor = (field != null) ? EmitFieldAccesssor(field, type) : defaultAccessor;
                    accessors.Add(key, accessor);
                }
                return accessor();
            }
            static Func<object> EmitFieldAccesssor(System.Reflection.FieldInfo field, Type type) {
                var method = new DynamicMethod("__get_" + field.Name, MA.Static | MA.Public, CallingConventions.Standard,
                    typeof(object), null, type, true);
                var ilGen = method.GetILGenerator();
                ilGen.Emit(OpCodes.Ldsfld, field);
                ilGen.Emit(OpCodes.Ret);
                return method.CreateDelegate(typeof(Func<object>)) as Func<object>;
            }
        }
        #endregion
    }
}