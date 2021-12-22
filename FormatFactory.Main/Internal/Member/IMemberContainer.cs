namespace Irvin.FormatFactory.Internal.Member
{
    internal interface IMemberContainer
    {
        string Name { get; }
        object CreateNew();
    }
}