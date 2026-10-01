namespace TinyVm;


    /* ldstr "Hello World" is an example of a VmInstruction where:
     
    ldstr is the OpCode telling it to load a string

    "Hello World" is the Operand, the string to be loaded. */

public sealed class VmInstruction
{
    //OpCode is the operation code telling what type of operation the instruction represents, such as LoadString, Call, or Return.
    public required VmOpCode OpCode { get; init; }
   
    //Operand tells what the operation will be performed on, such as a string to load or a method to call.
    public object? Operand { get; init; }

    public override string ToString() =>
        Operand is null ? OpCode.ToString() : $"{OpCode} {Operand}";
}
