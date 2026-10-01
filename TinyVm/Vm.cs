using dnlib.DotNet;

namespace TinyVm;

public sealed class Vm
{
    public Stack<VmValue> Stack { get; } = new();

    public void Execute(VmMethod method)
    {
        Console.WriteLine($"Executing: {method.Name}");
        Console.WriteLine();

        //Instruction pointer (IP) to track the current instruction index, used to iterate through the instructions in the method.
        int ip = 0;

        while (ip < method.Instructions.Count)
        {
            VmInstruction instruction = method.Instructions[ip];

            Console.WriteLine($"\nIP {ip}: {instruction.OpCode}");

            switch (instruction.OpCode)
            {
                case VmOpCode.LoadString:
                    ExecuteLoadString(instruction);
                    break;

                case VmOpCode.Call:
                    ExecuteCall(instruction);
                    break;

                case VmOpCode.Return:
                    Console.WriteLine("  return from guest method");
                    return;

                default:
                    throw new NotSupportedException(
                        $"Unsupported VM opcode: {instruction.OpCode}");
            }

            ip++;
        }
    }

    /// <summary>
    /// ExecuteLoadString retrieves the string value from the instruction's operand and pushes it onto the VM's stack.
    /// </summary>
    /// <param name="instruction"></param>
    private void ExecuteLoadString(VmInstruction instruction)
    {
        string value = (string)instruction.Operand!;
        Stack.Push(new VmValue(value));

        Console.WriteLine($"  push \"{value}\"");
        DumpStack();
    }

    /// <summary>
    /// ExecuteCall handles method calls within the VM. It checks if the target method is a call and pops (removes) the string value from the stack.
    /// </summary>
    /// <param name="instruction"></param>
    private void ExecuteCall(VmInstruction instruction)
    {
        var target = (IMethod)instruction.Operand!;

        // Check if the target method is a call to System.Console.WriteLine with a single string parameter, e.g. Console.WriteLine("Hello, World!");
        if (target.DeclaringType.FullName == "System.Console" &&
            target.Name == "WriteLine" &&
            target.MethodSig is { Params.Count: 1 } sig &&
            sig.Params[0].FullName == "System.String")
        {
            // Pop the string value from the stack and simulate the call.
            string value = Stack.Pop().AsString();

            Console.WriteLine($"  intercepted: {target.FullName}");
            Console.WriteLine($"  removed from stack: \"{value}\"");
            Console.WriteLine($"  output: {value}");
            DumpStack();
            return;
        }

        throw new NotSupportedException(
            $"The tiny VM does not implement call target: {target.FullName}");
    }

    /// <summary>
    /// DumpStack prints the current contents of the VM's stack to simulate the idea of dumping memory, only this time it is virtualized.
    /// </summary>
    private void DumpStack()
    {
        string contents = Stack.Count == 0
            ? "stack is empty"
            : string.Join(", ", Stack.Reverse().Select(v => v.ToString()));

        Console.WriteLine($"  stack: [{contents}]");
    }
}
