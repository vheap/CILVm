This project is meant for educational purposes only. Use lawfully and responsibly.

[Full study on emulation using C#, code obfuscation, security concerns and limitations can be found here.](https://vastheap.com/c-malware-emulation)

# What is CIL VM?

It is a sample project creating a virtual machine using C#, although miniaturized for our purposes, and showing the potential of emulating .NET assemblies suspected of suspicious behavior by acting as a runtime layer between the guest code of the operating system instead of .NET runtime, becoming another valuable tool to malware analysts.

The virtual machine exposes the ability to create a virtualized stack and translating CIL instructions into VM-appropriate ones, which are then logged instead of being executed.

It uses dnlib to read the assembly's CIL and metadata, translating the former into custom instructions compatible with the VM.

It supports the following instructions:
- ldstr
- call
- ret

Included with the project is a Hello World sample program that is created to let the VM read its instructions and display how values can be retrieved within emulation without running the code. For all intents and purposes, it is a virtual machine, albeit the concept of one.

<img width="1592" height="893" alt="aenOWveZ0e" src="https://github.com/user-attachments/assets/427094f3-25b8-4886-a9b3-a62196430e9e" />

Turns C# code from
```cs
Console.WriteLine("Hello, World!");
```

into CIL

```
ldstr "Hello, World!"
call void System.Console::WriteLine(string)
ret
```

That is later emulated via the VM rather than .NET runtime executing it.
