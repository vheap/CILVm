namespace TinyVm;

// VmValue is a struct that represents a value in the virtual machine. It can hold any object, but provides a method to retrieve it as a string.
public readonly struct VmValue
{
    private readonly object? _value;

    public VmValue(object? value)
    {
        _value = value;
    }

    public string AsString() =>
        _value as string
        ?? throw new InvalidOperationException("VM value is not a string.");

    public override string ToString() => _value?.ToString() ?? "null";
}
