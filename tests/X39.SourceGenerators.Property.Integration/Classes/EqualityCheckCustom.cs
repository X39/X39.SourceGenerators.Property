namespace TestNamespace;

public partial class EqualityCheckCustom
{
    private static bool CustomEqualityCheck(int oldValue, int newValue) => false;
    [EqualityCheck(EEqualityCheckMode.Custom, Custom: nameof(CustomEqualityCheck))]
    private int _field;
}