using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace TinyVm;

// CilTranslator translates CIL instructions from MethodDef into a VmMethod to be used by the virtual machine,
// translating specific CIL OpCodes (ldstr, call, ret) into corresponding VM instructions.
public static class CilTranslator
{
    // Translates a MethodDef into a VmMethod, converting its CIL instructions into VM instructions.
    public static VmMethod Translate(MethodDef method)
    {
        if (!method.HasBody)
            throw new InvalidOperationException($"Method has no CIL body: {method.FullName}");

        var vmMethod = new VmMethod
        {
            Name = method.FullName
        };
  
        foreach (Instruction instruction in method.Body.Instructions)
            vmMethod.Instructions.Add(TranslateInstruction(instruction));

        return vmMethod;
    }

    // Translates a single CIL instruction into a corresponding VM instruction.
    private static VmInstruction TranslateInstruction(Instruction instruction)
    {
        if (instruction.OpCode == OpCodes.Ldstr)
        {
            return new VmInstruction
            {
                OpCode = VmOpCode.LoadString,
                Operand = (string)instruction.Operand
            };
        }

        if (instruction.OpCode == OpCodes.Call)
        {
            return new VmInstruction
            {
                OpCode = VmOpCode.Call,
                //IMethod is a dnlib type that represents a method reference in the CIL code and cast as such.
                Operand = (IMethod)instruction.Operand
            };
        }

        if (instruction.OpCode == OpCodes.Ret)
        {
            return new VmInstruction
            {
                OpCode = VmOpCode.Return
            };
        }

        throw new NotSupportedException(
            $"The tiny VM only supports ldstr, call and ret only as examples. The OpCode ({instruction.OpCode}) is not supported");
    }
}
