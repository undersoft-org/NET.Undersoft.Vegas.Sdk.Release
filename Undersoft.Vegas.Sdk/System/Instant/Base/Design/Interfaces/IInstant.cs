namespace System.Instant
{
    public interface IInstant
    {
        Type   BaseType { get; set; }
        Type   Type { get; set; }
        string Name { get; set; }
        int    Size { get; }

        IRubrics Rubrics { get; }

        object New();
    }
}