namespace TinyVm;

public sealed class VmMethod
{
    public required string Name { get; init; }
    public List<VmInstruction> Instructions { get; } = new();
}
