namespace DotNext.Samples {
    using System;
    using System.Reflection;
    using BenchmarkDotNet.Attributes;
    using BF = System.Reflection.BindingFlags;

    public class Benchmarks_GetField_OneField {
        static Type type_public = typeof(Foo);
        static Type type_private = typeof(pFoo);
        //
        [Benchmark(Description = "1.1. GetField(Instance,Public)")]
        public FieldInfo GetField_Instance_Public() {
            return type_public.GetField("f0", BF.Instance | BF.Public);
        }
        [Benchmark(Description = "1.2. GetField(Instance,Private)")]
        public FieldInfo GetField_Instance_Private() {
            return type_private.GetField("f0", BF.Instance | BF.NonPublic);
        }
        [Benchmark(Description = "1.3. GetField(Static,Public)")]
        public FieldInfo GetField_Static_Public() {
            return type_public.GetField("s0", BF.Static | BF.Public);
        }
        [Benchmark(Description = "1.4. GetField(Static,Private)")]
        public FieldInfo GetField_Static_Private() {
            return type_private.GetField("s0", BF.Static | BF.NonPublic);
        }
        [Benchmark(Description = "2.1. GetFields(Instance,Public)")]
        public FieldInfo[] GetFields_Instance_Public() {
            return type_public.GetFields(BF.Instance | BF.Public);
        }
        [Benchmark(Description = "2.2. GetFields(Instance,Private)")]
        public FieldInfo[] GetFields_Instance_Private() {
            return type_private.GetFields(BF.Instance | BF.NonPublic);
        }
        [Benchmark(Description = "2.3. GetFields(Static,Public)")]
        public FieldInfo[] GetFields_Static_Public() {
            return type_public.GetFields(BF.Static | BF.Public);
        }
        [Benchmark(Description = "2.4. GetFields(Static,Private)")]
        public FieldInfo[] GetFields_Static_Private() {
            return type_private.GetFields(BF.Static | BF.NonPublic);
        }
        [Benchmark(Description = "3.1. GetFields(All, Public)")]
        public FieldInfo[] GetFields_PublicAll() {
            return type_public.GetFields(BF.Static | BF.Instance | BF.NonPublic | BF.Public);
        }
        [Benchmark(Description = "3.2. GetFields(All, Private)")]
        public FieldInfo[] GetFields_PrivateAll() {
            return type_private.GetFields(BF.Static | BF.Instance | BF.NonPublic | BF.Public);
        }
    }
    public class Benchmarks_GetField_TenField {
        static Type type_public = typeof(Bar);
        static Type type_private = typeof(pBar);
        //
        [Benchmark(Description = "1.1. GetField(Instance,Public)")]
        public FieldInfo GetField_Instance_Public() {
            return type_public.GetField("f0", BF.Instance | BF.Public);
        }
        [Benchmark(Description = "1.2. GetField(Instance,Private)")]
        public FieldInfo GetField_Instance_Private() {
            return type_private.GetField("f0", BF.Instance | BF.NonPublic);
        }
        [Benchmark(Description = "1.3. GetField(Instance,Public,Last)")]
        public FieldInfo GetField_Instance_Public_Last() {
            return type_public.GetField("f9", BF.Instance | BF.Public);
        }
        [Benchmark(Description = "1.4. GetField(Static,Public)")]
        public FieldInfo GetField_Static_Public() {
            return type_public.GetField("s0", BF.Static | BF.Public);
        }
        [Benchmark(Description = "1.5. GetField(Static,Private)")]
        public FieldInfo GetField_Static_Private() {
            return type_private.GetField("s0", BF.Static | BF.NonPublic);
        }
        [Benchmark(Description = "1.6. GetField(Static,Public,Last)")]
        public FieldInfo GetField_Static_Public_Last() {
            return type_public.GetField("s9", BF.Static | BF.Public);
        }
        [Benchmark(Description = "2.1. GetFields(Instance,Public)")]
        public FieldInfo[] GetFields_Instance_Public() {
            return type_public.GetFields(BF.Instance | BF.Public);
        }
        [Benchmark(Description = "2.2. GetFields(Instance,Private)")]
        public FieldInfo[] GetFields_Instance_Private() {
            return type_private.GetFields(BF.Instance | BF.NonPublic);
        }
        [Benchmark(Description = "2.3. GetFields(Static,Public)")]
        public FieldInfo[] GetFields_Static_Public() {
            return type_public.GetFields(BF.Static | BF.Public);
        }
        [Benchmark(Description = "2.4. GetFields(Static,Private)")]
        public FieldInfo[] GetFields_Static_Private() {
            return type_private.GetFields(BF.Static | BF.NonPublic);
        }
        [Benchmark(Description = "3.1. GetFields(All, Public)")]
        public FieldInfo[] GetFields_PublicAll() {
            return type_public.GetFields(BF.Static | BF.Instance | BF.NonPublic | BF.Public);
        }
        [Benchmark(Description = "3.2. GetFields(All, Private)")]
        public FieldInfo[] GetFields_PrivateAll() {
            return type_private.GetFields(BF.Static | BF.Instance | BF.NonPublic | BF.Public);
        }
    }
    //
    public class Benchmarks_GetFieldValue_Struct {
        static structFooBar fb = new structFooBar();
        static structFooBar_R fb_r = new structFooBar_R();
        static structFooBar_P fb_p = new structFooBar_P();
        static structFooBar_PR fb_pr = new structFooBar_PR();
        static FieldInfo x_private = typeof(structFooBar).GetField("x", BF.Instance | BF.NonPublic);
        static FieldInfo x_private_readonly = typeof(structFooBar_R).GetField("x", BF.Instance | BF.NonPublic);
        static FieldInfo x_public = typeof(structFooBar_P).GetField("x", BF.Instance | BF.Public);
        static FieldInfo x_public_readonly = typeof(structFooBar_PR).GetField("x", BF.Instance | BF.Public);
        static FieldInfo y_private = typeof(structFooBar).GetField("y", BF.Instance | BF.NonPublic);
        static FieldInfo y_private_readonly = typeof(structFooBar_R).GetField("y", BF.Instance | BF.NonPublic);
        static FieldInfo y_public = typeof(structFooBar_P).GetField("y", BF.Instance | BF.Public);
        static FieldInfo y_public_readonly = typeof(structFooBar_PR).GetField("y", BF.Instance | BF.Public);
        //
        [Benchmark(Description = "1.1. GetFieldValue(Struct,Public,ValueType)")]
        public object GetFieldValue_Instance_Public_ValueType() {
            return x_public.GetValue(fb_p);
        }
        [Benchmark(Description = "1.2. GetFieldValue(Struct,Private,ValueType)")]
        public object GetFieldValue_Instance_Private_ValueType() {
            return x_private.GetValue(fb);
        }
        [Benchmark(Description = "1.3. GetFieldValue(Struct,Public,Readonly,ValueType)")]
        public object GetFieldValue_Instance_Public_Readonly_ValueType() {
            return x_public_readonly.GetValue(fb_pr);
        }
        [Benchmark(Description = "1.4. GetFieldValue(Struct,Private,Readonly,ValueType)")]
        public object GetFieldValue_Instance_Private_Readonly_ValueType() {
            return x_private_readonly.GetValue(fb_r);
        }
        [Benchmark(Description = "2.1. GetFieldValue(Struct,Public,RefType)")]
        public object GetFieldValue_Instance_Public_RefType() {
            return y_public.GetValue(fb_p);
        }
        [Benchmark(Description = "2.2. GetFieldValue(Struct,Private,RefType)")]
        public object GetFieldValue_Instance_Private_RefType() {
            return y_private.GetValue(fb);
        }
        [Benchmark(Description = "2.3. GetFieldValue(Struct,Public,Readonly,RefType)")]
        public object GetFieldValue_Instance_Public_Readonly_RefType() {
            return y_public_readonly.GetValue(fb_pr);
        }
        [Benchmark(Description = "2.4. GetFieldValue(Struct,Private,Readonly,RefType)")]
        public object GetFieldValue_Instance_Private_Readonly_RefType() {
            return y_private_readonly.GetValue(fb_r);
        }
    }
    public class Benchmarks_SetFieldValue_Struct {
        static structFooBar fb = new structFooBar();
        static structFooBar_P fb_p = new structFooBar_P();
        static FieldInfo x_private = typeof(structFooBar).GetField("x", BF.Instance | BF.NonPublic);
        static FieldInfo x_public = typeof(structFooBar_P).GetField("x", BF.Instance | BF.Public);
        static FieldInfo y_private = typeof(structFooBar).GetField("y", BF.Instance | BF.NonPublic);
        static FieldInfo y_public = typeof(structFooBar_P).GetField("y", BF.Instance | BF.Public);
        //
        [Benchmark(Description = "1.1. SetFieldValue(Struct,Public,ValueType)")]
        public void SetFieldValue_Instance_Public_ValueType() {
            x_public.SetValue(fb_p, 42);
        }
        [Benchmark(Description = "1.2. SetFieldValue(Struct,Private,ValueType)")]
        public void SetFieldValue_Instance_Private_ValueType() {
            x_private.SetValue(fb, 42);
        }
        [Benchmark(Description = "2.1. SetFieldValue(Struct,Public,RefType)")]
        public void SetFieldValue_Instance_Public_RefType() {
            y_public.SetValue(fb_p, null);
        }
        [Benchmark(Description = "2.2. SetFieldValue(Struct,Private,RefType)")]
        public void SetFieldValue_Instance_Private_RefType() {
            y_private.SetValue(fb, null);
        }
    }
    //
    public class Benchmarks_GetFieldValue_Class {
        static clsFooBar fb = new clsFooBar();
        static clsFooBar_R fb_r = new clsFooBar_R();
        static clsFooBar_P fb_p = new clsFooBar_P();
        static clsFooBar_PR fb_pr = new clsFooBar_PR();
        //
        static FieldInfo x_private = typeof(clsFooBar).GetField("x", BF.Instance | BF.NonPublic);
        static FieldInfo x_private_readonly = typeof(clsFooBar_R).GetField("x", BF.Instance | BF.NonPublic);
        static FieldInfo x_public = typeof(clsFooBar_P).GetField("x", BF.Instance | BF.Public);
        static FieldInfo x_public_readonly = typeof(clsFooBar_PR).GetField("x", BF.Instance | BF.Public);
        static FieldInfo y_private = typeof(clsFooBar).GetField("y", BF.Instance | BF.NonPublic);
        static FieldInfo y_private_readonly = typeof(clsFooBar_R).GetField("y", BF.Instance | BF.NonPublic);
        static FieldInfo y_public = typeof(clsFooBar_P).GetField("y", BF.Instance | BF.Public);
        static FieldInfo y_public_readonly = typeof(clsFooBar_PR).GetField("y", BF.Instance | BF.Public);
        //
        static FieldInfo sx_private = typeof(clsFooBar_S).GetField("x", BF.Static | BF.NonPublic);
        static FieldInfo sx_private_readonly = typeof(clsFooBar_SR).GetField("x", BF.Static | BF.NonPublic);
        static FieldInfo sx_public = typeof(clsFooBar_SP).GetField("x", BF.Static | BF.Public);
        static FieldInfo sx_public_readonly = typeof(clsFooBar_SPR).GetField("x", BF.Static | BF.Public);
        static FieldInfo sy_private = typeof(clsFooBar_S).GetField("y", BF.Static | BF.NonPublic);
        static FieldInfo sy_private_readonly = typeof(clsFooBar_SR).GetField("y", BF.Static | BF.NonPublic);
        static FieldInfo sy_public = typeof(clsFooBar_SP).GetField("y", BF.Static | BF.Public);
        static FieldInfo sy_public_readonly = typeof(clsFooBar_SPR).GetField("y", BF.Static | BF.Public);
        //
        [Benchmark(Description = "1.1. GetFieldValue(Class,Public,ValueType)")]
        public object GetFieldValue_Instance_Public_ValueType() {
            return x_public.GetValue(fb_p);
        }
        [Benchmark(Description = "1.2. GetFieldValue(Class,Private,ValueType)")]
        public object GetFieldValue_Instance_Private_ValueType() {
            return x_private.GetValue(fb);
        }
        [Benchmark(Description = "1.3. GetFieldValue(Class,Public,Readonly,ValueType)")]
        public object GetFieldValue_Instance_Public_Readonly_ValueType() {
            return x_public_readonly.GetValue(fb_pr);
        }
        [Benchmark(Description = "1.4. GetFieldValue(Class,Private,Readonly,ValueType)")]
        public object GetFieldValue_Instance_Private_Readonly_ValueType() {
            return x_private_readonly.GetValue(fb_r);
        }
        [Benchmark(Description = "2.1. GetFieldValue(Class,Public,RefType)")]
        public object GetFieldValue_Instance_Public_RefType() {
            return y_public.GetValue(fb_p);
        }
        [Benchmark(Description = "2.2. GetFieldValue(Class,Private,RefType)")]
        public object GetFieldValue_Instance_Private_RefType() {
            return y_private.GetValue(fb);
        }
        [Benchmark(Description = "2.3. GetFieldValue(Class,Public,Readonly,RefType)")]
        public object GetFieldValue_Instance_Public_Readonly_RefType() {
            return y_public_readonly.GetValue(fb_pr);
        }
        [Benchmark(Description = "2.4. GetFieldValue(Class,Private,Readonly,RefType)")]
        public object GetFieldValue_Instance_Private_Readonly_RefType() {
            return y_private_readonly.GetValue(fb_r);
        }
        //
        [Benchmark(Description = "3.1. GetStaticFieldValue(Class,Public,ValueType)")]
        public object GetFieldValue_Static_Public_ValueType() {
            return sx_public.GetValue(null);
        }
        [Benchmark(Description = "3.2. GetStaticFieldValue(Class,Private,ValueType)")]
        public object GetFieldValue_Static_Private_ValueType() {
            return sx_private.GetValue(null);
        }
        [Benchmark(Description = "3.3. GetStaticFieldValue(Class,Public,Readonly,ValueType)")]
        public object GetFieldValue_Static_Public_Readonly_ValueType() {
            return sx_public_readonly.GetValue(null);
        }
        [Benchmark(Description = "3.4. GetStaticFieldValue(Class,Private,Readonly,ValueType)")]
        public object GetFieldValue_Static_Private_Readonly_ValueType() {
            return sx_private_readonly.GetValue(null);
        }
        [Benchmark(Description = "4.1. GetStaticFieldValue(Class,Public,RefType)")]
        public object GetFieldValue_Static_Public_RefType() {
            return sy_public.GetValue(null);
        }
        [Benchmark(Description = "4.2. GetStaticFieldValue(Class,Private,RefType)")]
        public object GetFieldValue_Static_Private_RefType() {
            return sy_private.GetValue(null);
        }
        [Benchmark(Description = "4.3. GetStaticFieldValue(Class,Public,Readonly,RefType)")]
        public object GetFieldValue_Static_Public_Readonly_RefType() {
            return sy_public_readonly.GetValue(null);
        }
        [Benchmark(Description = "4.4. GetStaticFieldValue(Class,Private,Readonly,RefType)")]
        public object GetFieldValue_Static_Private_Readonly_RefType() {
            return sy_private_readonly.GetValue(null);
        }
    }
    public class Benchmarks_SetFieldValue_Class {
        static clsFooBar fb = new clsFooBar();
        static clsFooBar_P fb_p = new clsFooBar_P();
        static FieldInfo x_private = typeof(clsFooBar).GetField("x", BF.Instance | BF.NonPublic);
        static FieldInfo x_public = typeof(clsFooBar_P).GetField("x", BF.Instance | BF.Public);
        static FieldInfo y_private = typeof(clsFooBar).GetField("y", BF.Instance | BF.NonPublic);
        static FieldInfo y_public = typeof(clsFooBar_P).GetField("y", BF.Instance | BF.Public);
        static FieldInfo sx_private = typeof(clsFooBar_S).GetField("x", BF.Static | BF.NonPublic);
        static FieldInfo sx_public = typeof(clsFooBar_SP).GetField("x", BF.Static | BF.Public);
        static FieldInfo sy_private = typeof(clsFooBar_S).GetField("y", BF.Static | BF.NonPublic);
        static FieldInfo sy_public = typeof(clsFooBar_SP).GetField("y", BF.Static | BF.Public);
        //
        [Benchmark(Description = "1.1. SetFieldValue(Class,Public,ValueType)")]
        public void SetFieldValue_Instance_Public_ValueType() {
            x_public.SetValue(fb_p, 42);
        }
        [Benchmark(Description = "1.2. SetFieldValue(Class,Private,ValueType)")]
        public void SetFieldValue_Instance_Private_ValueType() {
            x_private.SetValue(fb, 42);
        }
        [Benchmark(Description = "2.1. SetFieldValue(Class,Public,RefType)")]
        public void SetFieldValue_Instance_Public_RefType() {
            y_public.SetValue(fb_p, null);
        }
        [Benchmark(Description = "2.2. SetFieldValue(Class,Private,RefType)")]
        public void SetFieldValue_Instance_Private_RefType() {
            y_private.SetValue(fb, null);
        }
        [Benchmark(Description = "3.1. SetStaticFieldValue(Class,Public,ValueType)")]
        public void SetFieldValue_Static_Public_ValueType() {
            sx_public.SetValue(null, 42);
        }
        [Benchmark(Description = "3.2. SetStaticFieldValue(Class,Private,ValueType)")]
        public void SetFieldValue_Static_Private_ValueType() {
            sx_private.SetValue(null, 42);
        }
        [Benchmark(Description = "4.1. SetStaticFieldValue(Class,Public,RefType)")]
        public void SetFieldValue_Static_Public_RefType() {
            sy_public.SetValue(null, null);
        }
        [Benchmark(Description = "4.2. SetStaticFieldValue(Class,Private,RefType)")]
        public void SetFieldValue_Static_Private_RefType() {
            sy_private.SetValue(null, null);
        }
    }
}