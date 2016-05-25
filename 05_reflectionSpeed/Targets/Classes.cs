namespace DotNext.Samples {
    // One Member
    class Foo {
        public static int s0;
        public int f0;
        //
        public int F0 { get { return f0; } set { f0 = value; } }
        public static int S0 { get { return s0; } set { s0 = value; } }
    }
    class pFoo {
        static int s0;
        int f0;
        //
        static int S0 { get { return s0; } set { s0 = value; } }
        int F0 { get { return f0; } set { f0 = value; } }
    }
    // Ten Members
    class Bar {
        public static int s0, s1, s2, s3, s4, s5, s6, s7, s8, s9;
        public int f0, f1, f2, f3, f4, f5, f6, f7, f8, f9;
        //
        public int F0 { get { return f0; } set { f0 = value; } }
        public int F1 { get { return f1; } set { f1 = value; } }
        public int F2 { get { return f2; } set { f2 = value; } }
        public int F3 { get { return f3; } set { f3 = value; } }
        public int F4 { get { return f4; } set { f4 = value; } }
        public int F5 { get { return f5; } set { f5 = value; } }
        public int F6 { get { return f6; } set { f6 = value; } }
        public int F7 { get { return f7; } set { f7 = value; } }
        public int F8 { get { return f8; } set { f8 = value; } }
        public int F9 { get { return f9; } set { f9 = value; } }

        public static int S0 { get { return s0; } set { s0 = value; } }
        public static int S1 { get { return s1; } set { s1 = value; } }
        public static int S2 { get { return s2; } set { s2 = value; } }
        public static int S3 { get { return s3; } set { s3 = value; } }
        public static int S4 { get { return s4; } set { s4 = value; } }
        public static int S5 { get { return s5; } set { s5 = value; } }
        public static int S6 { get { return s6; } set { s6 = value; } }
        public static int S7 { get { return s7; } set { s7 = value; } }
        public static int S8 { get { return s8; } set { s8 = value; } }
        public static int S9 { get { return s9; } set { s9 = value; } }
    }
    class pBar {
        static int s0, s1, s2, s3, s4, s5, s6, s7, s8, s9;
        int f0, f1, f2, f3, f4, f5, f6, f7, f8, f9;
        //
        int F0 { get { return f0; } set { f0 = value; } }
        int F1 { get { return f1; } set { f1 = value; } }
        int F2 { get { return f2; } set { f2 = value; } }
        int F3 { get { return f3; } set { f3 = value; } }
        int F4 { get { return f4; } set { f4 = value; } }
        int F5 { get { return f5; } set { f5 = value; } }
        int F6 { get { return f6; } set { f6 = value; } }
        int F7 { get { return f7; } set { f7 = value; } }
        int F8 { get { return f8; } set { f8 = value; } }
        int F9 { get { return f9; } set { f9 = value; } }

        static int S0 { get { return s0; } set { s0 = value; } }
        static int S1 { get { return s1; } set { s1 = value; } }
        static int S2 { get { return s2; } set { s2 = value; } }
        static int S3 { get { return s3; } set { s3 = value; } }
        static int S4 { get { return s4; } set { s4 = value; } }
        static int S5 { get { return s5; } set { s5 = value; } }
        static int S6 { get { return s6; } set { s6 = value; } }
        static int S7 { get { return s7; } set { s7 = value; } }
        static int S8 { get { return s8; } set { s8 = value; } }
        static int S9 { get { return s9; } set { s9 = value; } }
    }
    // Structs
    struct structFooBar {
        int x;
        int X { get { return x; } set { x = value; } }
        object y;
        object Y { get { return y; } set { y = value; } }
    }
    struct structFooBar_P {
        public int x;
        public int X { get { return x; } set { x = value; } }
        public object y;
        public object Y { get { return y; } set { y = value; } }
    }
    struct structFooBar_R {
        structFooBar_R(int x, object y) { this.x = x; this.y = y; }
        readonly int x;
        int X { get { return x; } }
        readonly object y;
        object Y { get { return y; } }
    }
    struct structFooBar_PR {
        structFooBar_PR(int x, object y) { this.x = x; this.y = y; }
        public readonly int x;
        public int X { get { return x; } }
        public readonly object y;
        public object Y { get { return y; } }
    }
    // Classes
    class clsFooBar {
        int x;
        int X { get { return x; } set { x = value; } }
        object y;
        object Y { get { return y; } set { y = value; } }
    }
    class clsFooBar_P {
        public int x;
        public int X { get { return x; } set { x = value; } }
        public object y;
        public object Y { get { return y; } set { y = value; } }
    }
    class clsFooBar_R {
        readonly int x = 42;
        readonly object y = new object();
        int X { get { return x; } }
        object Y { get { return y; } }
    }
    class clsFooBar_PR {
        public readonly int x = 42;
        public readonly object y = new object();
        public int X { get { return x; } }
        public object Y { get { return y; } }
    }
    //
    class clsFooBar_S {
        static int x;
        static int X { get { return x; } set { x = value; } }
        static object y;
        static object Y { get { return y; } set { y = value; } }
    }
    class clsFooBar_SP {
        public static int x;
        public static int X { get { return x; } set { x = value; } }
        public static object y;
        public static object Y { get { return y; } set { y = value; } }
    }
    class clsFooBar_SR {
        static readonly int x = 42;
        static readonly object y = new object();
        static int X { get { return x; } }
        static object Y { get { return y; } }
    }
    class clsFooBar_SPR {
        public static readonly int x = 42;
        public static readonly object y = new object();
        public static int X { get { return x; } }
        public static object Y { get { return y; } }
    }
}