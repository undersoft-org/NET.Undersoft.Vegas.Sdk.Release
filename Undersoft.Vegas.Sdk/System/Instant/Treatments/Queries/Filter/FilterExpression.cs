/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.FilterExpression.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Treatments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Uniques;

    public class QueryExpression
    {
        private System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();

        private Expression<Func<IFigure, bool>> Expression
        { get; set; }

        public Expression<Func<IFigure, bool>> Query
        { get { return CreateExpression(Stage); } }
        public FilterTerms Conditions;
        public int Stage
        { get; set; }

        public QueryExpression()
        {
            Conditions = new FilterTerms();
            nfi.NumberDecimalSeparator = ".";
            Stage = 1;
        }

        public Expression<Func<IFigure, bool>> this[int stage]
        {
            get
            {
                return CreateExpression(stage);
            }
        }

        public Expression<Func<IFigure, bool>> CreateExpression(int stage = 1)
        {
            Expression<Func<IFigure, bool>> exps = null;
            List<FilterTerm> fcs = Conditions.Get(stage);
            Expression = null;
            LogicType previousLogic = LogicType.And;
            foreach (FilterTerm fc in fcs)
            {
                exps = null;
                if (fc.Operand != OperandType.Contains)
                {
                    if (Expression != null)
                        if (previousLogic != LogicType.Or)
                            Expression = Expression.And(CaseConditioner(fc, exps));
                        else
                            Expression = Expression.Or(CaseConditioner(fc, exps));
                    else
                        Expression = CaseConditioner(fc, exps);
                    previousLogic = fc.Logic;
                }
                else
                {
                    HashSet<int> list = new HashSet<int>((fc.Value.GetType() == typeof(string)) ? fc.Value.ToString().Split(';')
                                                          .Select(p => Convert.ChangeType(p, fc.FilterRubric.RubricType).GetHashCode()) :
                                                         (fc.Value.GetType() == typeof(List<object>)) ? ((List<object>)fc.Value)
                                                          .Select(p => Convert.ChangeType(p, fc.FilterRubric.RubricType).GetHashCode()) : null);

                    if (list != null && list.Count > 0)
                        exps = (r => list.Contains(r[fc.FilterRubric.RubricName].GetHashCode()));

                    if (Expression != null)
                        if (previousLogic != LogicType.Or)
                            Expression = Expression.And(exps);
                        else
                            Expression = Expression.Or(exps);
                    else
                        Expression = exps;
                    previousLogic = fc.Logic;
                }
            }
            return Expression;
        }
        private Expression<Func<IFigure, bool>> CaseConditioner(FilterTerm fc, Expression<Func<IFigure, bool>> ex)
        {
            if (fc.Value != null)
            {
                object Value = fc.Value;
                OperandType Operand = fc.Operand;
                if (Operand != OperandType.Like && Operand != OperandType.NotLike)
                {
                    switch (Operand)
                    {
                        case OperandType.Equal:

                            ex = (r => r[fc.FilterRubric.FieldId] != null ?
                            fc.FilterRubric.RubricType == typeof(IUnique) ||
                            fc.FilterRubric.RubricType == typeof(string) ||
                            fc.FilterRubric.RubricType == typeof(DateTime) ?
                            r[fc.FilterRubric.FieldId].ComparableUInt64(fc.FilterRubric.RubricType)
                                .Equals(Value.ComparableUInt64(fc.FilterRubric.RubricType)) :
                            r[fc.FilterRubric.FieldId].ComparableDouble(fc.FilterRubric.RubricType)
                                 .Equals(Value.ComparableDouble(fc.FilterRubric.RubricType)) :
                              false);
                            break;

                        case OperandType.EqualOrMore:

                            ex = (r => r[fc.FilterRubric.FieldId] != null ?
                             fc.FilterRubric.RubricType == typeof(IUnique) ||
                             fc.FilterRubric.RubricType == typeof(string) ||
                             fc.FilterRubric.RubricType == typeof(DateTime) ?
                              r[fc.FilterRubric.FieldId].ComparableUInt64(fc.FilterRubric.RubricType)
                                                      >=
                              (Value.ComparableUInt64(fc.FilterRubric.RubricType)) :
                            r[fc.FilterRubric.FieldId].ComparableDouble(fc.FilterRubric.RubricType)
                                                      >=
                            (Value.ComparableDouble(fc.FilterRubric.RubricType)) : false);
                            break;

                        case OperandType.More:

                            ex = (r => r[fc.FilterRubric.FieldId] != null ?
                            fc.FilterRubric.RubricType == typeof(IUnique) ||
                            fc.FilterRubric.RubricType == typeof(string) ||
                            fc.FilterRubric.RubricType == typeof(DateTime) ?
                             r[fc.FilterRubric.FieldId].ComparableUInt64(fc.FilterRubric.RubricType)
                                                     >
                             (Value.ComparableUInt64(fc.FilterRubric.RubricType)) :
                           r[fc.FilterRubric.FieldId].ComparableDouble(fc.FilterRubric.RubricType)
                                                     >
                           (Value.ComparableDouble(fc.FilterRubric.RubricType)) : false);
                            break;

                        case OperandType.EqualOrLess:

                            ex = (r => r[fc.FilterRubric.FieldId] != null ?
                            fc.FilterRubric.RubricType == typeof(IUnique) ||
                            fc.FilterRubric.RubricType == typeof(string) ||
                            fc.FilterRubric.RubricType == typeof(DateTime) ?
                            r[fc.FilterRubric.FieldId].ComparableUInt64(fc.FilterRubric.RubricType)
                                                    <=
                            (Value.ComparableUInt64(fc.FilterRubric.RubricType)) :
                            r[fc.FilterRubric.FieldId].ComparableDouble(fc.FilterRubric.RubricType)
                                                    <=
                            (Value.ComparableDouble(fc.FilterRubric.RubricType)) : false);
                            break;

                        case OperandType.Less:

                            ex = (r => r[fc.FilterRubric.FieldId] != null ?
                             fc.FilterRubric.RubricType == typeof(IUnique) ||
                             fc.FilterRubric.RubricType == typeof(string) ||
                             fc.FilterRubric.RubricType == typeof(DateTime) ?
                             r[fc.FilterRubric.FieldId].ComparableUInt64(fc.FilterRubric.RubricType)
                                                    <
                             (Value.ComparableUInt64(fc.FilterRubric.RubricType)) :
                             r[fc.FilterRubric.FieldId].ComparableDouble(fc.FilterRubric.RubricType)
                                                    <
                             (Value.ComparableDouble(fc.FilterRubric.RubricType)) : false);
                            break;
                        default:
                            break;
                    }
                }
                else if (Operand != OperandType.NotLike)

                    ex = (r => r[fc.FilterRubric.FieldId] != null ?
                    Convert.ChangeType(r[fc.FilterRubric.FieldId], fc.FilterRubric.RubricType).ToString()
                        .Contains(Convert.ChangeType(Value, fc.FilterRubric.RubricType).ToString()) :
                            false);
                else
                    ex = (r => r[fc.FilterRubric.FieldId] != null ?
                    !Convert.ChangeType(r[fc.FilterRubric.FieldId], fc.FilterRubric.RubricType).ToString()
                        .Contains(Convert.ChangeType(Value, fc.FilterRubric.RubricType).ToString()) :
                            false);
            }
            return ex;
        }
    }
}
