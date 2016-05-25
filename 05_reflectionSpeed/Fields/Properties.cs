namespace DotNext.Samples {
    using System;
    using System.Reflection;
    using BenchmarkDotNet.Attributes;
    using BF = System.Reflection.BindingFlags;

    public class Benchmarks_GetProperty_OneProperty {
        static Type type_public = typeof(Foo);
        static Type type_private = typeof(pFoo);
        //
        [Benchmark(Description = "1.1. GetProperty(Instance,Public)")]
        public PropertyInfo GetProperty_Instance_Public() {
            return type_public.GetProperty("P0", BF.Instance | BF.Public);
        }
        [Benchmark(Description = "1.2. GetProperty(Instance,Private)")]
        public PropertyInfo GetProperty_Instance_Private() {
            return type_private.GetProperty("P0", BF.Instance | BF.NonPublic);
        }
        [Benchmark(Description = "1.3. GetProperty(Static,Public)")]
        public PropertyInfo GetProperty_Static_Public() {
            return type_public.GetProperty("S0", BF.Static | BF.Public);
        }
        [Benchmark(Description = "1.4. GetProperty(Static,Private)")]
        public PropertyInfo GetProperty_Static_Private() {
            return type_private.GetProperty("S0", BF.Static | BF.NonPublic);
        }
        [Benchmark(Description = "2.1. GetProperties(Instance,Public)")]
        public PropertyInfo[] GetProperties_Instance_Public() {
            return type_public.GetProperties(BF.Instance | BF.Public);
        }
        [Benchmark(Description = "2.2. GetProperties(Instance,Private)")]
        public PropertyInfo[] GetProperties_Instance_Private() {
            return type_private.GetProperties(BF.Instance | BF.NonPublic);
        }
        [Benchmark(Description = "2.3. GetProperties(Static,Public)")]
        public PropertyInfo[] GetProperties_Static_Public() {
            return type_public.GetProperties(BF.Static | BF.Public);
        }
        [Benchmark(Description = "2.4. GetProperties(Static,Private)")]
        public PropertyInfo[] GetProperties_Static_Private() {
            return type_private.GetProperties(BF.Static | BF.NonPublic);
        }
        [Benchmark(Description = "3.1. GetProperties(All, Public)")]
        public PropertyInfo[] GetProperties_PublicAll() {
            return type_public.GetProperties(BF.Static | BF.Instance | BF.NonPublic | BF.Public);
        }
        [Benchmark(Description = "3.2. GetProperties(All, Private)")]
        public PropertyInfo[] GetProperties_PrivateAll() {
            return type_private.GetProperties(BF.Static | BF.Instance | BF.NonPublic | BF.Public);
        }
    }
    public class Benchmarks_GetProperty_TenProperties {
        static Type type_public = typeof(Bar);
        static Type type_private = typeof(pBar);
        //
        [Benchmark(Description = "1.1. GetProperty(Instance,Public)")]
        public PropertyInfo GetProperty_Instance_Public() {
            return type_public.GetProperty("P0", BF.Instance | BF.Public);
        }
        [Benchmark(Description = "1.2. GetProperty(Instance,Private)")]
        public PropertyInfo GetProperty_Instance_Private() {
            return type_private.GetProperty("P0", BF.Instance | BF.NonPublic);
        }
        [Benchmark(Description = "1.3. GetProperty(Instance,Public,Last)")]
        public PropertyInfo GetProperty_Instance_Public_Last() {
            return type_public.GetProperty("P9", BF.Instance | BF.Public);
        }
        [Benchmark(Description = "1.4. GetProperty(Static,Public)")]
        public PropertyInfo GetField_Static_Public() {
            return type_public.GetProperty("S0", BF.Static | BF.Public);
        }
        [Benchmark(Description = "1.5. GetProperty(Static,Private)")]
        public PropertyInfo GetField_Static_Private() {
            return type_private.GetProperty("S0", BF.Static | BF.NonPublic);
        }
        [Benchmark(Description = "1.6. GetProperty(Static,Public,Last)")]
        public PropertyInfo GetField_Static_Public_Last() {
            return type_public.GetProperty("S9", BF.Static | BF.Public);
        }
        [Benchmark(Description = "2.1. GetProperties(Instance,Public)")]
        public PropertyInfo[] GetFields_Instance_Public() {
            return type_public.GetProperties(BF.Instance | BF.Public);
        }
        [Benchmark(Description = "2.2. GetProperties(Instance,Private)")]
        public PropertyInfo[] GetFields_Instance_Private() {
            return type_private.GetProperties(BF.Instance | BF.NonPublic);
        }
        [Benchmark(Description = "2.3. GetProperties(Static,Public)")]
        public PropertyInfo[] GetFields_Static_Public() {
            return type_public.GetProperties(BF.Static | BF.Public);
        }
        [Benchmark(Description = "2.4. GetProperties(Static,Private)")]
        public PropertyInfo[] GetFields_Static_Private() {
            return type_private.GetProperties(BF.Static | BF.NonPublic);
        }
        [Benchmark(Description = "3.1. GetProperties(All, Public)")]
        public PropertyInfo[] GetFields_PublicAll() {
            return type_public.GetProperties(BF.Static | BF.Instance | BF.NonPublic | BF.Public);
        }
        [Benchmark(Description = "3.2. GetProperties(All, Private)")]
        public PropertyInfo[] GetFields_PrivateAll() {
            return type_private.GetProperties(BF.Static | BF.Instance | BF.NonPublic | BF.Public);
        }
    }
    //
    public class Benchmarks_GetPropertyValue_Struct {
        static structFooBar fb = new structFooBar();
        static structFooBar_R fb_r = new structFooBar_R();
        static structFooBar_P fb_p = new structFooBar_P();
        static structFooBar_PR fb_pr = new structFooBar_PR();
        static PropertyInfo X_private = typeof(structFooBar).GetProperty("X", BF.Instance | BF.NonPublic);
        static PropertyInfo X_private_readonly = typeof(structFooBar_R).GetProperty("X", BF.Instance | BF.NonPublic);
        static PropertyInfo X_public = typeof(structFooBar_P).GetProperty("X", BF.Instance | BF.Public);
        static PropertyInfo X_public_readonly = typeof(structFooBar_PR).GetProperty("X", BF.Instance | BF.Public);
        static PropertyInfo Y_private = typeof(structFooBar).GetProperty("Y", BF.Instance | BF.NonPublic);
        static PropertyInfo Y_private_readonly = typeof(structFooBar_R).GetProperty("Y", BF.Instance | BF.NonPublic);
        static PropertyInfo Y_public = typeof(structFooBar_P).GetProperty("Y", BF.Instance | BF.Public);
        static PropertyInfo Y_public_readonly = typeof(structFooBar_PR).GetProperty("Y", BF.Instance | BF.Public);
        //
        [Benchmark(Description = "1.1. GetPropertyValue(Struct,Public,ValueType)")]
        public object GetPropertyValue_Instance_Public_ValueType() {
            return X_public.GetValue(fb_p, null);
        }
        [Benchmark(Description = "1.2. GetPropertyValue(Struct,Private,ValueType)")]
        public object GetPropertyValue_Instance_Private_ValueType() {
            return X_private.GetValue(fb, null);
        }
        [Benchmark(Description = "1.3. GetPropertyValue(Struct,Public,Readonly,ValueType)")]
        public object GetPropertyValue_Instance_Public_Readonly_ValueType() {
            return X_public_readonly.GetValue(fb_pr, null);
        }
        [Benchmark(Description = "1.4. GetPropertyValue(Struct,Private,Readonly,ValueType)")]
        public object GetPropertyValue_Instance_Private_Readonly_ValueType() {
            return X_private_readonly.GetValue(fb_r, null);
        }
        [Benchmark(Description = "2.1. GetPropertyValue(Struct,Public,RefType)")]
        public object GetPropertyValue_Instance_Public_RefType() {
            return Y_public.GetValue(fb_p, null);
        }
        [Benchmark(Description = "2.2. GetPropertyValue(Struct,Private,RefType)")]
        public object GetPropertyValue_Instance_Private_RefType() {
            return Y_private.GetValue(fb, null);
        }
        [Benchmark(Description = "2.3. GetPropertyValue(Struct,Public,Readonly,RefType)")]
        public object GetPropertyValue_Instance_Public_Readonly_RefType() {
            return Y_public_readonly.GetValue(fb_pr, null);
        }
        [Benchmark(Description = "2.4. GetPropertyValue(Struct,Private,Readonly,RefType)")]
        public object GetPropertyValue_Instance_Private_Readonly_RefType() {
            return Y_private_readonly.GetValue(fb_r, null);
        }
    }
    public class Benchmarks_SetPropertyValue_Struct {
        static structFooBar fb = new structFooBar();
        static structFooBar_P fb_p = new structFooBar_P();
        static PropertyInfo X_private = typeof(structFooBar).GetProperty("X", BF.Instance | BF.NonPublic);
        static PropertyInfo X_public = typeof(structFooBar_P).GetProperty("X", BF.Instance | BF.Public);
        static PropertyInfo Y_private = typeof(structFooBar).GetProperty("Y", BF.Instance | BF.NonPublic);
        static PropertyInfo Y_public = typeof(structFooBar_P).GetProperty("Y", BF.Instance | BF.Public);
        //
        [Benchmark(Description = "1.1. SetPropertyValue(Struct,Public,ValueType)")]
        public void SetPropertyValue_Instance_Public_ValueType() {
            X_public.SetValue(fb_p, 42, null);
        }
        [Benchmark(Description = "1.2. SetPropertyValue(Struct,Private,ValueType)")]
        public void SetPropertyValue_Instance_Private_ValueType() {
            X_private.SetValue(fb, 42, null);
        }
        [Benchmark(Description = "2.1. SetPropertyValue(Struct,Public,RefType)")]
        public void SetPropertyValue_Instance_Public_RefType() {
            Y_public.SetValue(fb_p, null, null);
        }
        [Benchmark(Description = "2.2. SetPropertyValue(Struct,Private,RefType)")]
        public void SetPropertyValue_Instance_Private_RefType() {
            Y_private.SetValue(fb, null, null);
        }
    }
    //
    public class Benchmarks_GetPropertyValue_Class {
        static clsFooBar fb = new clsFooBar();
        static clsFooBar_R fb_r = new clsFooBar_R();
        static clsFooBar_P fb_p = new clsFooBar_P();
        static clsFooBar_PR fb_pr = new clsFooBar_PR();
        //
        static PropertyInfo X_private = typeof(clsFooBar).GetProperty("X", BF.Instance | BF.NonPublic);
        static PropertyInfo X_private_readonly = typeof(clsFooBar_R).GetProperty("X", BF.Instance | BF.NonPublic);
        static PropertyInfo X_public = typeof(clsFooBar_P).GetProperty("X", BF.Instance | BF.Public);
        static PropertyInfo X_public_readonly = typeof(clsFooBar_PR).GetProperty("X", BF.Instance | BF.Public);
        static PropertyInfo Y_private = typeof(clsFooBar).GetProperty("Y", BF.Instance | BF.NonPublic);
        static PropertyInfo Y_private_readonly = typeof(clsFooBar_R).GetProperty("Y", BF.Instance | BF.NonPublic);
        static PropertyInfo Y_public = typeof(clsFooBar_P).GetProperty("Y", BF.Instance | BF.Public);
        static PropertyInfo Y_public_readonly = typeof(clsFooBar_PR).GetProperty("Y", BF.Instance | BF.Public);
        //
        static PropertyInfo SX_private = typeof(clsFooBar_S).GetProperty("X", BF.Static | BF.NonPublic);
        static PropertyInfo SX_private_readonly = typeof(clsFooBar_SR).GetProperty("X", BF.Static | BF.NonPublic);
        static PropertyInfo SX_public = typeof(clsFooBar_SP).GetProperty("X", BF.Static | BF.Public);
        static PropertyInfo SX_public_readonly = typeof(clsFooBar_SPR).GetProperty("X", BF.Static | BF.Public);
        static PropertyInfo SY_private = typeof(clsFooBar_S).GetProperty("Y", BF.Static | BF.NonPublic);
        static PropertyInfo SY_private_readonly = typeof(clsFooBar_SR).GetProperty("Y", BF.Static | BF.NonPublic);
        static PropertyInfo SY_public = typeof(clsFooBar_SP).GetProperty("Y", BF.Static | BF.Public);
        static PropertyInfo SY_public_readonly = typeof(clsFooBar_SPR).GetProperty("Y", BF.Static | BF.Public);
        //
        [Benchmark(Description = "1.1. GetPropertyValue(Class,Public,ValueType)")]
        public object GetPropertyValue_Instance_Public_ValueType() {
            return X_public.GetValue(fb_p, null);
        }
        [Benchmark(Description = "1.2. GetPropertyValue(Class,Private,ValueType)")]
        public object GetPropertyValue_Instance_Private_ValueType() {
            return X_private.GetValue(fb, null);
        }
        [Benchmark(Description = "1.3. GetPropertyValue(Class,Public,Readonly,ValueType)")]
        public object GetPropertyValue_Instance_Public_Readonly_ValueType() {
            return X_public_readonly.GetValue(fb_pr, null);
        }
        [Benchmark(Description = "1.4. GetPropertyValue(Class,Private,Readonly,ValueType)")]
        public object GetPropertyValue_Instance_Private_Readonly_ValueType() {
            return X_private_readonly.GetValue(fb_r, null);
        }
        [Benchmark(Description = "2.1. GetPropertyValue(Class,Public,RefType)")]
        public object GetPropertyValue_Instance_Public_RefType() {
            return Y_public.GetValue(fb_p, null);
        }
        [Benchmark(Description = "2.2. GetPropertyValue(Class,Private,RefType)")]
        public object GetPropertyValue_Instance_Private_RefType() {
            return Y_private.GetValue(fb, null);
        }
        [Benchmark(Description = "2.3. GetPropertyValue(Class,Public,Readonly,RefType)")]
        public object GetPropertyValue_Instance_Public_Readonly_RefType() {
            return Y_public_readonly.GetValue(fb_pr, null);
        }
        [Benchmark(Description = "2.4. GetPropertyValue(Class,Private,Readonly,RefType)")]
        public object GetPropertyValue_Instance_Private_Readonly_RefType() {
            return Y_private_readonly.GetValue(fb_r, null);
        }
        //
        [Benchmark(Description = "3.1. GetStaticPropertyValue(Class,Public,ValueType)")]
        public object GetPropertyValue_Static_Public_ValueType() {
            return SX_public.GetValue(null, null);
        }
        [Benchmark(Description = "3.2. GetStaticPropertyValue(Class,Private,ValueType)")]
        public object GetPropertyValue_Static_Private_ValueType() {
            return SX_private.GetValue(null, null);
        }
        [Benchmark(Description = "3.3. GetStaticPropertyValue(Class,Public,Readonly,ValueType)")]
        public object GetPropertyValue_Static_Public_Readonly_ValueType() {
            return SX_public_readonly.GetValue(null, null);
        }
        [Benchmark(Description = "3.4. GetStaticPropertyValue(Class,Private,Readonly,ValueType)")]
        public object GetPropertyValue_Static_Private_Readonly_ValueType() {
            return SX_private_readonly.GetValue(null, null);
        }
        [Benchmark(Description = "4.1. GetStaticPropertyValue(Class,Public,RefType)")]
        public object GetPropertyValue_Static_Public_RefType() {
            return SY_public.GetValue(null, null);
        }
        [Benchmark(Description = "4.2. GetStaticPropertyValue(Class,Private,RefType)")]
        public object GetPropertyValue_Static_Private_RefType() {
            return SY_private.GetValue(null, null);
        }
        [Benchmark(Description = "4.3. GetStaticPropertyValue(Class,Public,Readonly,RefType)")]
        public object GetPropertyValue_Static_Public_Readonly_RefType() {
            return SY_public_readonly.GetValue(null, null);
        }
        [Benchmark(Description = "4.4. GetStaticPropertyValue(Class,Private,Readonly,RefType)")]
        public object GetPropertyValue_Static_Private_Readonly_RefType() {
            return SY_private_readonly.GetValue(null, null);
        }
    }
    public class Benchmarks_SetPropertyValue_Class {
        static clsFooBar fb = new clsFooBar();
        static clsFooBar_P fb_p = new clsFooBar_P();
        static PropertyInfo X_private = typeof(clsFooBar).GetProperty("X", BF.Instance | BF.NonPublic);
        static PropertyInfo X_public = typeof(clsFooBar_P).GetProperty("X", BF.Instance | BF.Public);
        static PropertyInfo Y_private = typeof(clsFooBar).GetProperty("Y", BF.Instance | BF.NonPublic);
        static PropertyInfo Y_public = typeof(clsFooBar_P).GetProperty("Y", BF.Instance | BF.Public);
        static PropertyInfo SX_private = typeof(clsFooBar_S).GetProperty("X", BF.Static | BF.NonPublic);
        static PropertyInfo SX_public = typeof(clsFooBar_SP).GetProperty("X", BF.Static | BF.Public);
        static PropertyInfo SY_private = typeof(clsFooBar_S).GetProperty("Y", BF.Static | BF.NonPublic);
        static PropertyInfo SY_public = typeof(clsFooBar_SP).GetProperty("Y", BF.Static | BF.Public);
        //
        [Benchmark(Description = "1.1. SetPropertyValue(Class,Public,ValueType)")]
        public void SetPropertyValue_Instance_Public_ValueType() {
            X_public.SetValue(fb_p, 42, null);
        }
        [Benchmark(Description = "1.2. SetPropertyValue(Class,Private,ValueType)")]
        public void SetPropertyValue_Instance_Private_ValueType() {
            X_private.SetValue(fb, 42, null);
        }
        [Benchmark(Description = "2.1. SetPropertyValue(Class,Public,RefType)")]
        public void SetPropertyValue_Instance_Public_RefType() {
            Y_public.SetValue(fb_p, null, null);
        }
        [Benchmark(Description = "2.2. SetPropertyValue(Class,Private,RefType)")]
        public void SetPropertyValue_Instance_Private_RefType() {
            Y_private.SetValue(fb, null, null);
        }
        [Benchmark(Description = "3.1. SetStaticPropertyValue(Class,Public,ValueType)")]
        public void SetPropertyValue_Static_Public_ValueType() {
            SX_public.SetValue(null, 42, null);
        }
        [Benchmark(Description = "3.2. SetStaticPropertyValue(Class,Private,ValueType)")]
        public void SetPropertyValue_Static_Private_ValueType() {
            SX_private.SetValue(null, 42, null);
        }
        [Benchmark(Description = "4.1. SetStaticPropertyValue(Class,Public,RefType)")]
        public void SetPropertyValue_Static_Public_RefType() {
            SY_public.SetValue(null, null, null);
        }
        [Benchmark(Description = "4.2. SetStaticPropertyValue(Class,Private,RefType)")]
        public void SetPropertyValue_Static_Private_RefType() {
            SY_private.SetValue(null, null, null);
        }
    }
}