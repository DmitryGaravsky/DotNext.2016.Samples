namespace DotNext.Samples {
    using System;
    using System.IO;
    using System.Reflection;
    using System.Reflection.Emit;

    class Program {
        static void Main(string[] args) {
            Assembly fooLib1 = LoadLib(startupPath, "v1");
            Type fooType1 = fooLib1.GetType("FooLib.Foo")
                .Report();
            Assembly fooLib2 = LoadLib(startupPath, "v2");
            Type fooType2 = fooLib2.GetType("FooLib.Foo")
                .Report();
            //
            var dynAsmName = new AssemblyName("FooLib.Dynamic." + Guid.NewGuid().ToString());
            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(dynAsmName, AssemblyBuilderAccess.Run);
            var moduleBuilder = asmBuilder.DefineDynamicModule(dynAsmName.FullName);
            //
            Type deriverType1 = BuildDescendant(fooType1, "x01", moduleBuilder)
                .Check(expectedBaseType: fooType1);
            Type deriverType2 = BuildDescendant(fooType2, "x02", moduleBuilder)
                .Check(expectedBaseType: fooType2);
        }
        static Type BuildDescendant(Type type, string suffix, ModuleBuilder moduleBuilder) {
            return moduleBuilder
                .DefineType(type.Name + suffix, TypeAttributes.Public, type)
                .CreateType();
        }
        static string startupPath = Assembly.GetExecutingAssembly().Location;
        static Assembly LoadLib(string startupPath, string version) {
            string versionPath = Path.Combine(Path.GetDirectoryName(startupPath), @"FooLibs\" + version + @"\FooLib.dll");
            return Assembly.LoadFile(versionPath);
        }
    }
    static class Ext {
        internal static Type Report(this Type type) {
            Console.WriteLine("Type: " + type.TypeNameAndHash());
            return type;
        }
        internal static Type Check(this Type type, Type expectedBaseType) {
            string resolution = (type.BaseType == expectedBaseType) ? "(Success):" : "(Error):";
            Console.WriteLine("Derived Type" + resolution.PadRight(12) + type.Name + ".BaseType==" + type.BaseType.TypeNameAndHash());
            return type;
        }
        static string TypeNameAndHash(this Type type) {
            return type.Name + "[" + type.GetHashCode().ToString("X8") + "]";
        }
    }
}