using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using MA = System.Reflection.MethodAttributes;

namespace DotNext.Samples.Dynamic {
    static class Generator {
        public static Type Generate(Type type, Action<ILGenerator> gen) {
            var dynAsmName = new AssemblyName("Bar.Dynamic." + Guid.NewGuid().ToString());
            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(dynAsmName, AssemblyBuilderAccess.Run);
            var moduleBuilder = asmBuilder.DefineDynamicModule(dynAsmName.FullName);
            //
            var typeBuilder = moduleBuilder.DefineType("Darth" + type.Name, TypeAttributes.Public, type);
            DoMethodOverride(typeBuilder, "SayHello", gen);
            return typeBuilder.CreateType();
        }
        static void DoMethodOverride(TypeBuilder typebuilder, string methodName, Action<ILGenerator> gen) {
            var method = typebuilder.BaseType.GetMethod(methodName);
            var m = typebuilder.DefineMethod(method.Name,
                method.Attributes & ~MA.Abstract,
                method.CallingConvention, method.ReturnType,
                method.GetParameters().Select(p => p.ParameterType).ToArray());
            gen(m.GetILGenerator());
            typebuilder.DefineMethodOverride(m, method);
        }
    }
}