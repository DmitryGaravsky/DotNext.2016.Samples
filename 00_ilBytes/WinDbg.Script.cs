// P01 Break at clrjit loading
sxe ld:clrjit;g

// P02 Load SOS-tool
.loadby sos clr

// P03 [0,2] Find Program.Main method
!name2ee * Program.Main
Not JITTED yet. Use !bpmd -md 00664d14 to break on run.

// P04 [0,11] Dump IL
0:000> !dumpil 00664e2c
ilAddr = 003e2150
IL_0000: ldstr "Hello, {0}.{1}!"
IL_0005: stloc.0 
IL_0006: ldloc.0 
IL_0007: ldarg.1 
IL_0008: ldarg.2 
IL_0009: box System.Int32
IL_000e: call System.String::Format 
IL_0013: call System.Console::WriteLine 
IL_0018: ret 

// P04 [0,4] Find method by Name
!name2ee * Foo.SayHello
0:000> !bpmd -md 00664e2c
MethodDesc = 00664e2c
Adding pending breakpoints...

// P05 [0,10] Dump assembler code
0:000> !u -n 00664e2c
Normal JIT generated code
DotNext.Samples.Foo.SayHello(System.String, Int32)
Begin 007c0848, size 73
008b0848 55              push    ebp
008b0849 8bec            mov     ebp,esp
008b084b 57              push    edi
...
008b08b7 5d              pop     ebp
008b08b8 c20400          ret     4