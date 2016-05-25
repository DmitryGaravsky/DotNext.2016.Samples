using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Columns;

namespace DotNext.Samples {
    class Program {
        static void Main(string[] args) {
            BenchmarkDotNet.Running.BenchmarkRunner.Run<Benchmarks>();
        }
    }
    public class Foo { }
    public class Command { }
    // localizer
    public enum FooStringId { MessageAddCommandConstructorError }
    static class FooLocalizer {
        public static string GetString(FooStringId id) {
            return null;
        }
    }
    // commands
    public enum FooCommandId {
        SomeCommand
    };
    public class FooCommand : Command {
        public FooCommand(Foo foo) { }
    }
    //
    public class FooCommandRepositoryBase_v00 {
        readonly Foo foo;
        public FooCommandRepositoryBase_v00(Foo foo) {
            this.foo = foo;
        }
        protected void AddCommandConstructor(FooCommandId commandId, Type commandType) {
            ConstructorInfo ci = commandType.GetConstructor(new Type[] { foo.GetType() });
            if(ci == null)
                throw new Exception(string.Format(FooLocalizer.GetString(FooStringId.MessageAddCommandConstructorError), commandType));
            commands.Add(commandId, ci);
        }
        readonly Dictionary<FooCommandId, ConstructorInfo> commands = new Dictionary<FooCommandId, ConstructorInfo>();
        public Command CreateCommand(FooCommandId id) {
            ConstructorInfo ci;
            return (commands.TryGetValue(id, out ci) && ci != null) ? ci.Invoke(new object[] { foo }) as Command : null;
        }
    }
    public class FooCommandRepositoryBase_v01 {
        // constant elements (type-level)
        static readonly Type[] ctorParameterTypes = new Type[] { typeof(Foo) };
        static readonly string errorFormat = FooLocalizer.GetString(FooStringId.MessageAddCommandConstructorError);
        //
        readonly object[] ctorParams;
        public FooCommandRepositoryBase_v01(Foo foo) {
            // constant elements (instance-level)
            this.ctorParams = new object[] { foo };
        }
        protected void AddCommandConstructor(FooCommandId commandId, Type commandType) {
            ConstructorInfo ci = commandType.GetConstructor(ctorParameterTypes);
            if(ci == null)
                throw new Exception(string.Format(errorFormat, commandType.ToString())); // avoid boxing
            commands.Add(commandId, ci);
        }
        Dictionary<FooCommandId, ConstructorInfo> commands = new Dictionary<FooCommandId, ConstructorInfo>();
        public Command CreateCommand(FooCommandId id) {
            ConstructorInfo ci;
            return commands.TryGetValue(id, out ci) ? ci.Invoke(ctorParams) as Command : null; // un-needed null-check
        }
    }
    public class FooCommandRepositoryBase_v02 {
        // constant elements (type-level)
        static readonly Type[] ctorParameterTypes = new Type[] { typeof(Foo) };
        static readonly string errorFormat = FooLocalizer.GetString(FooStringId.MessageAddCommandConstructorError);
        //
        readonly ConstantExpression ctorParameter;
        public FooCommandRepositoryBase_v02(Foo foo) {
            // constant elements (instance-level)
            this.ctorParameter = Expression.Constant(foo, typeof(Foo));
        }
        protected void AddCommandConstructor(FooCommandId commandId, Type commandType) {
            ConstructorInfo ci = commandType.GetConstructor(ctorParameterTypes);
            if(ci == null)
                throw new Exception(string.Format(errorFormat, commandType.ToString())); // avoid boxing
            var ctorExp = Expression.New(ci, ctorParameter);
            initializers.Add(commandId, Expression.Lambda<Func<Command>>(ctorExp).Compile()); // cache initializer
        }
        Dictionary<FooCommandId, Func<Command>> initializers = new Dictionary<FooCommandId, Func<Command>>();
        public Command CreateCommand(FooCommandId id) {
            Func<Command> initializer;
            return initializers.TryGetValue(id, out initializer) ? initializer() : null;
        }
    }
    public class FooCommandRepositoryBase_v03 {
        // constant elements (type-level)
        static readonly Type[] ctorParameterTypes = new Type[] { typeof(Foo) };
        static readonly string errorFormat = 
            FooLocalizer.GetString(FooStringId.MessageAddCommandConstructorError);
        //
        readonly ConstantExpression ctorParameter;
        readonly Func<Command>[] initializers;
        public FooCommandRepositoryBase_v03(Foo foo) {
            // constant elements (instance-level)
            this.ctorParameter = Expression.Constant(foo, typeof(Foo));
            this.initializers = new Func<Command>[Enum.GetValues(typeof(FooCommandId)).Length];
        }
        protected void AddCommandConstructor(FooCommandId commandId, Type commandType) {
            ConstructorInfo ci = commandType.GetConstructor(ctorParameterTypes);
            if(ci == null)
                throw new Exception(string.Format(errorFormat, commandType.ToString()));
            var ctorExp = Expression.New(ci, ctorParameter);
            initializers[(int)commandId] = Expression.Lambda<Func<Command>>(ctorExp).Compile();
        }
        public Command CreateCommand(FooCommandId id) {
            return initializers[(int)id].Invoke();
        }
    }
    public class FooCommandRepositoryBase_v04 {
        readonly Foo foo;
        public FooCommandRepositoryBase_v04(Foo foo) {
            this.foo = foo;
        }
        public Command CreateCommand(FooCommandId id) {
            switch(id) {
                case FooCommandId.SomeCommand:
                    return new FooCommand(foo);
                default:
                    return null;
            }
        }
    }
    //
    [ConfigSource]
    public class Benchmarks {
        #region Config
        class ConfigSourceAttribute : Attribute, IConfigSource {
            public IConfig Config { get; private set; }
            public ConfigSourceAttribute() {
                Config = ManualConfig.CreateEmpty()
                    .With(new IColumn[] { StatisticColumn.OperationsPerSecond });
            }
        }
        #endregion Config
        #region classes
        class FooCommandRepositoryv00 : FooCommandRepositoryBase_v00 {
            public FooCommandRepositoryv00(Foo foo)
                : base(foo) {
                AddCommandConstructor(FooCommandId.SomeCommand, typeof(FooCommand));
            }
        }
        class FooCommandRepositoryv01 : FooCommandRepositoryBase_v01 {
            public FooCommandRepositoryv01(Foo foo)
                : base(foo) {
                AddCommandConstructor(FooCommandId.SomeCommand, typeof(FooCommand));
            }
        }
        class FooCommandRepositoryv02 : FooCommandRepositoryBase_v02 {
            public FooCommandRepositoryv02(Foo foo)
                : base(foo) {
                AddCommandConstructor(FooCommandId.SomeCommand, typeof(FooCommand));
            }
        }
        class FooCommandRepositoryv03 : FooCommandRepositoryBase_v03 {
            public FooCommandRepositoryv03(Foo foo)
                : base(foo) {
                AddCommandConstructor(FooCommandId.SomeCommand, typeof(FooCommand));
            }
        }
        class FooCommandRepositoryv04 : FooCommandRepositoryBase_v04 {
            public FooCommandRepositoryv04(Foo foo)
                : base(foo) {
            }
        }
        #endregion
        Foo foo;
        FooCommandRepositoryv00 repository0;
        FooCommandRepositoryv01 repository1;
        FooCommandRepositoryv02 repository2;
        FooCommandRepositoryv03 repository3;
        FooCommandRepositoryv04 repository4;
        [Setup]
        public void Setup() {
            foo = new Foo();
            repository0 = new FooCommandRepositoryv00(foo);
            repository1 = new FooCommandRepositoryv01(foo);
            repository2 = new FooCommandRepositoryv02(foo);
            repository3 = new FooCommandRepositoryv03(foo);
            repository4 = new FooCommandRepositoryv04(foo);
        }
        [Benchmark]
        public Command Original_Dirty() {
            return repository0.CreateCommand(FooCommandId.SomeCommand);
        }
        [Benchmark]
        public Command Original_Clean() {
            return repository1.CreateCommand(FooCommandId.SomeCommand);
        }
        [Benchmark]
        public Command InitializersCache() {
            return repository2.CreateCommand(FooCommandId.SomeCommand);
        }
        [Benchmark]
        public Command InitializersArray() {
            return repository3.CreateCommand(FooCommandId.SomeCommand);
        }
        [Benchmark(Baseline = true)]
        public Command DirectConstruct() {
            return repository4.CreateCommand(FooCommandId.SomeCommand);
        }
    }
}