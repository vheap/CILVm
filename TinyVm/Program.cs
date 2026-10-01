using dnlib.DotNet;
using TinyVm;

string guestPath = args.Length > 0
    ? Path.GetFullPath(args[0])
    : Path.Combine(AppContext.BaseDirectory, "GuestProgram.dll");

if (!File.Exists(guestPath))
{
    Console.Error.WriteLine($"Guest assembly not found: {guestPath}");
    Console.Error.WriteLine("Build the solution or pass a DLL path as the first argument.");
    return 1;
}

Console.WriteLine($"Reading guest assembly as data: {guestPath}");
Console.WriteLine();

// Load the guest assembly using dnlib to read its metadata and CIL instructions.
using ModuleDefMD module = ModuleDefMD.Load(guestPath);

// Find the Program type and Main method in the guest assembly.
// By default, th entry point of the app is expected to be Program -> Main()
TypeDef programType = module.Types.FirstOrDefault(
    t => t.Namespace == "GuestProgram" && t.Name == "Program")
    ?? throw new InvalidOperationException("GuestProgram.Program was not found.");

MethodDef main = programType.Methods.FirstOrDefault(
    m => m.Name == "Main")
    ?? throw new InvalidOperationException("GuestProgram.Program.Main was not found.");

Console.WriteLine("Real program CIL extracted:");
foreach (var instruction in main.Body.Instructions)
    Console.WriteLine($"  {instruction.OpCode,-10} {instruction.Operand}");

Console.WriteLine();

// Translate the CIL instructions of the Main method into our custom virtual machine instructions.
VmMethod vmMethod = CilTranslator.Translate(main);
var vm = new Vm();
// Execute the translated virtual machine method, which will simulate the execution of our guest program.
vm.Execute(vmMethod);

Console.ReadLine();
return 0;
