/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.IRubrics.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (28.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Sets;

    public interface IRubrics : IUnique, IDeck<MemberRubric>
    {
        #region Properties

        IFigures Figures { get; set; }

        IRubrics KeyRubrics { get; set; }

        FieldMappings Mappings { get; set; }

        int[] Ordinals { get; }

        int[] BinarySizes { get; }

        int BinarySize { get; }

        #endregion

        #region Methods

        byte[] GetBytes(IFigure figure);

        ulong GetUniqueKey(IFigure figure, uint seed = 0);

        byte[] GetUniqueBytes(IFigure figure, uint seed = 0);

        void SetUniqueKey(IFigure figure, uint seed = 0);

        void Update();

        #endregion
    }
}
