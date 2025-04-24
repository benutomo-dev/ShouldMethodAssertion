namespace ShouldMethodAssertion.ShouldExtensions;

internal class OverloadResolutionPriorities
{
    public const int Default = 0;
    public const int LowPriority = -1;
    public const int GenericEnum = -2;
    public const int GenericStruct = -3;
    public const int Object = -4;
}
