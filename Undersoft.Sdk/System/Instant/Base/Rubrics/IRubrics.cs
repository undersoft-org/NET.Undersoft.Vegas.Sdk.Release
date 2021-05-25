using System.Multemic;

namespace System.Instant
{
    public interface IRubrics : IUnique, IDeck<MemberRubric>
    {
        IFigures Figures { get; set; }
        IRubrics KeyRubrics { get; set; }
        FieldMappings Mappings { get; set; }
        int[] Ordinals { get; }

        void Update();
    }
}