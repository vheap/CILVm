namespace GuestProgram;

// Dummy guest application to be emulated in the virtual machine providing a body presenting ldstr, call and ret instructions.
// The bigger the guest application, the more complex the CIL instruction, leading to a more complex virtual machine implementation to translate those CIL instructions.
public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello World");
    }
}
