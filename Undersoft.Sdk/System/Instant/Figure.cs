using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Instant
{
    public class Figure : IInstant
    {
        private Type compiledType;
        private MemberRubric[] members;
        private FigureMode mode { get; set; }

        public Figure(Type figureModelType, FigureMode modeType = FigureMode.Reference) : this(figureModelType,null,  modeType) { }
        public Figure(Type figureModelType, string figureTypeName, FigureMode modeType = FigureMode.Reference) :
            this(figureModelType.GetMembers().Select(m => m.MemberType == MemberTypes.Field ? m :
                                               m.MemberType == MemberTypes.Property ? m :
                                               null).Where(p => p != null).ToArray(),
                                               figureTypeName == null ? figureModelType.Name : figureTypeName, modeType){}

        public Figure(IList<MemberInfo> figureMembers, FigureMode modeType = FigureMode.Reference) : this(figureMembers.ToArray(), null, modeType) {}
        public Figure(IList<MemberInfo> figureMembers, string figureTypeName, FigureMode modeType = FigureMode.Reference)
        {
            Name = (figureTypeName != null && figureTypeName != "") ? figureTypeName : DateTime.Now.ToBinary().ToString();
            mode = modeType;
            
            members = CreateMemberRurics(figureMembers);

            Rubrics = new MemberRubrics(members);
            Rubrics.KeyRubrics = new MemberRubrics();         
        }

        public Figure(MemberRubrics figureRubrics, string figureTypeName, FigureMode modeType = FigureMode.Reference) : this(figureRubrics.ToArray(), figureTypeName, modeType)
        {          
        }

        public IFigure Generate()
        {
            if (this.Type == null)
            {
                if (mode == FigureMode.Reference)
                {
                    var ifcref = new FigureCompilerReference(this);
                    compiledType = ifcref.CompileFigureType(Name);
                }
                else
                {
                    var ifcvt = new FigureCompilerValueType(this);
                    compiledType = ifcvt.CompileFigureType(Name);
                }

                this.Type = compiledType.New().GetType();
                Size = Marshal.SizeOf(this.Type);

                if (!members.Where(m => m.Name == "SystemSerialCode").Any())
                    members = new MemberRubric[] { new MemberRubric(this.Type.GetProperty("SystemSerialCode")) }.Concat(members).ToArray();

                members.Select((m, y) => m.RubricId = y).ToArray();

                members.Select(m => m.FigureField = this.Type.GetField("_" + m.RubricName, BindingFlags.NonPublic | BindingFlags.Instance)).ToArray();

                members.Where(m => m.FigureField != null)
                    .Select((f, y) => new object[]
                    {
                    f.FigureFieldId = y - 1,
                    f.RubricOffset = (int)Marshal.OffsetOf(this.Type, "_" + f.RubricName)
                    }).ToArray();

                Rubrics.Insert(0, members[0]);

                Rubrics.Update();
            }

            return newFigure();
        }

        public string Name { get; set; }
        public Type Type { get; set; }
        public int Size { get; set; }
        public IRubrics Rubrics
        { get; set; }

        public object New()
        {
            return this.Type.New();
        }

        private IFigure newFigure()
        {
            return (IFigure)this.Type.New();
        }

        private MemberRubric[] CreateMemberRurics(IList<MemberInfo> membersInfo)
        {
            return membersInfo.Select(m => !(m is MemberRubric) ? m.MemberType == MemberTypes.Field ? new MemberRubric((FieldInfo)m) :
                                                                  m.MemberType == MemberTypes.Property ? new MemberRubric((PropertyInfo)m) :
                                                                  null : (MemberRubric)m).Where(p => p != null).ToArray();

        }       
    }
}