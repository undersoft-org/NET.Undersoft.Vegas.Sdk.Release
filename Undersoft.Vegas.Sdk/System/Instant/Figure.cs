/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Figure.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class Figure : IInstant
    {
        #region Fields

        private Type compiledType;
        private MemberRubrics fieldRubrics;
        private MemberRubrics propertyRubrics;

        #endregion

        #region Constructors

        public Figure(IList<MemberInfo> figureMembers, FigureMode modeType = FigureMode.Reference)
            : this(figureMembers.ToArray(), null, modeType)
        {
        }
        public Figure(IList<MemberInfo> figureMembers, string figureTypeName, FigureMode modeType = FigureMode.Reference)
        {
            Name = (figureTypeName != null && figureTypeName != "") ? figureTypeName : DateTime.Now.ToBinary().ToString();
            mode = modeType;

            Rubrics = fieldRubrics = new MemberRubrics(createMemberRurics(figureMembers));
            Rubrics.KeyRubrics = new MemberRubrics();
        }
        public Figure(MemberRubrics figureRubrics, string figureTypeName, FigureMode modeType = FigureMode.Reference)
            : this(figureRubrics.ToArray(), figureTypeName, modeType)
        {
        }
        public Figure(Type figureModelType, FigureMode modeType = FigureMode.Reference) : this(figureModelType, null, modeType)
        {
        }
        public Figure(Type figureModelType, string figureTypeName, FigureMode modeType = FigureMode.Reference)
        {
            BaseType = figureModelType;

            if (modeType == FigureMode.Derived)
                IsDerived = true;

            Name = figureTypeName == null ? figureModelType.Name : figureTypeName;
            mode = modeType;

            Rubrics = fieldRubrics = new MemberRubrics(createMemberRurics(figureModelType.GetRuntimeFields().ToArray()));

            propertyRubrics = new MemberRubrics(createMemberRurics(figureModelType.GetRuntimeProperties().ToArray())
                                                                        .Where(r => fieldRubrics.ContainsKey(r)));
            Rubrics.KeyRubrics = new MemberRubrics();
        }

        #endregion

        #region Properties

        public Type BaseType { get; set; }

        public bool IsDerived { get; set; }

        public string Name { get; set; }

        public IRubrics Rubrics { get; set; }

        public int Size { get; set; }

        public Type Type { get; set; }

        private FigureMode mode { get; set; }

        #endregion

        #region Methods

        public IFigure Combine()
        {
            if (this.Type == null)
            {
                try
                {
                    switch (mode)
                    {
                        case FigureMode.Reference:
                            combineDynamicType(new FigureCompilerReference(this, fieldRubrics, propertyRubrics));
                            break;
                        case FigureMode.ValueType:
                            combineDynamicType(new FigureCompilerValueType(this, fieldRubrics, propertyRubrics));
                            break;
                        case FigureMode.Derived:
                            combineDerivedType(new FigureCompilerDerivedType(this, fieldRubrics, propertyRubrics));
                            break;
                        default:
                            break;
                    }

                    Rubrics.Update();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return newFigure();
        }

        public object New()
        {
            if (this.Type == null)
                return Combine();
            return this.Type.New();
        }

        private void combineDerivedType(FigureCompiler compiler)
        {
            var fcdt = compiler;
            compiledType = fcdt.CompileFigureType(Name);
            Rubrics.KeyRubrics.Add(fcdt.Identities.Values);
            this.Type = compiledType.New().GetType();
            Size = Marshal.SizeOf(this.Type);

            if (!Rubrics.AsValues().Where(m => m.Name == "SerialCode").Any())
            {
                var f = this.Type.GetField("serialcode", BindingFlags.NonPublic | BindingFlags.Instance);
                var mr = new MemberRubric(f);
                mr.RubricName = "SerialCode";
                mr.FigureField = f;
                mr.RubricOffset = (int)Marshal.OffsetOf(this.Type, f.Name);
                Rubrics.Insert(0, mr);
            }

            var df = fcdt.derivedFields;
            Rubrics.AsValues().Select((m, y) => m.FigureField = df[y].RubricInfo).ToArray();
            Rubrics.AsValues().Where(m => m.FigureField != null)
                                       .Select((f, y) => new object[] {
                                           f.FieldId = y - 1,
                                           f.RubricId = y - 1 })                                       
                                         .ToArray();

            foreach (var rubric in Rubrics.Skip(1).ToArray())
            {
                try
                {
                    rubric.RubricOffset = (int)Marshal.OffsetOf(BaseType, rubric.FigureField.Name);
                }
                catch (Exception ex)
                {
                    rubric.RubricOffset = -1;
                }
            }
        }

        private void combineDynamicType(FigureCompiler compiler)
        {
            var fcvt = compiler;
            compiledType = fcvt.CompileFigureType(Name);
            Rubrics.KeyRubrics.Add(fcvt.Identities.Values);
            this.Type = compiledType.New().GetType();
            Size = Marshal.SizeOf(this.Type);
            var rf = this.Type.GetRuntimeFields().ToArray();

            if (!Rubrics.AsValues().Where(m => m.Name == "SerialCode").Any())
            {
                var mr = new MemberRubric(rf[0]);
                mr.RubricName = "SerialCode";
                Rubrics.Insert(0, mr);
            }

            Rubrics.AsValues().Select((m, y) => m.FigureField = rf[y]).ToArray();
            Rubrics.AsValues().Where(m => m.FigureField != null)
                                         .Select((f, y) => new object[] {
                                           f.FieldId = y - 1,
                                           f.RubricId = y - 1,
                                           f.RubricOffset = (int)Marshal.OffsetOf(this.Type, f.FigureField.Name) })
                                           .ToArray();
        }

        private MemberRubric[] createMemberRurics(IList<MemberInfo> membersInfo)
        {
            return membersInfo.Select(m => !(m is MemberRubric) ? m.MemberType == MemberTypes.Field ? new MemberRubric((FieldInfo)m) :
                                                                  m.MemberType == MemberTypes.Property ? new MemberRubric((PropertyInfo)m) :
                                                                  null : (MemberRubric)m).Where(p => p != null).ToArray();
        }

        private IFigure newFigure()
        {
            if (this.Type == null)
                return Combine();
            return (IFigure)this.Type.New();
        }

        #endregion
    }
}
