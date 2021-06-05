/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.FilterTerms.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (29.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Treatments
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    #region Enums

    [Serializable]
    public enum OperandType
    {
        Equal,
        EqualOrMore,
        EqualOrLess,
        More,
        Less,
        Like,
        NotLike,
        Contains,
        None
    }
    [Serializable]
    public enum LogicType
    {
        And,
        Or
    }
    [Serializable]
    public enum FilterStage
    {
        None,
        First,
        Second,
        Third
    }

    #endregion

    /// <summary>
    /// Defines the <see cref="FilterOperand" />.
    /// </summary>
    public static class FilterOperand
    {
        #region Methods

        /// <summary>
        /// The ParseOperand.
        /// </summary>
        /// <param name="operandString">The operandString<see cref="string"/>.</param>
        /// <returns>The <see cref="OperandType"/>.</returns>
        public static OperandType ParseOperand(string operandString)
        {
            OperandType _operand = OperandType.None;
            switch (operandString)
            {
                case "=":
                    _operand = OperandType.Equal;
                    break;
                case ">=":
                    _operand = OperandType.EqualOrMore;
                    break;
                case ">":
                    _operand = OperandType.More;
                    break;
                case "<=":
                    _operand = OperandType.EqualOrLess;
                    break;
                case "<":
                    _operand = OperandType.Less;
                    break;
                case "like":
                    _operand = OperandType.Like;
                    break;
                case "!like":
                    _operand = OperandType.NotLike;
                    break;
                default:
                    _operand = OperandType.None;
                    break;
            }
            return _operand;
        }

        /// <summary>
        /// The StringOperand.
        /// </summary>
        /// <param name="operand">The operand<see cref="OperandType"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string StringOperand(OperandType operand)
        {
            string operandString = "";
            switch (operand)
            {
                case OperandType.Equal:
                    operandString = "=";
                    break;
                case OperandType.EqualOrMore:
                    operandString = ">=";
                    break;
                case OperandType.More:
                    operandString = ">";
                    break;
                case OperandType.EqualOrLess:
                    operandString = "<=";
                    break;
                case OperandType.Less:
                    operandString = "<";
                    break;
                case OperandType.Like:
                    operandString = "like";
                    break;
                case OperandType.NotLike:
                    operandString = "!like";
                    break;
                default:
                    operandString = "=";
                    break;
            }
            return operandString;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="FilterTerm" />.
    /// </summary>
    [Serializable]
    public class FilterTerm : ICloneable, IFilterTerm
    {
        #region Fields

        public string valueTypeName;
        [NonSerialized] private IFigures figures;
        [NonSerialized] private Type valueType;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTerm"/> class.
        /// </summary>
        public FilterTerm()
        {
            Stage = FilterStage.First;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTerm"/> class.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        public FilterTerm(IFigures figures)
        {
            Stage = FilterStage.First;
            this.figures = figures;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTerm"/> class.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        /// <param name="filterColumn">The filterColumn<see cref="string"/>.</param>
        /// <param name="operand">The operand<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <param name="logic">The logic<see cref="string"/>.</param>
        /// <param name="stage">The stage<see cref="int"/>.</param>
        public FilterTerm(IFigures figures, string filterColumn, string operand, object value, string logic = "And", int stage = 1)
        {
            RubricName = filterColumn;
            OperandType tempOperand1;
            Enum.TryParse(operand, true, out tempOperand1);
            Operand = tempOperand1;
            Value = value;
            LogicType tempLogic;
            Enum.TryParse(logic, true, out tempLogic);
            Logic = tempLogic;
            this.figures = figures;
            if (figures != null)
            {
                MemberRubric[] filterRubrics = this.figures.Rubrics.AsValues().Where(c => c.RubricName == RubricName).ToArray();
                if (filterRubrics.Length > 0)
                {
                    FilterRubric = filterRubrics[0]; ValueType = FilterRubric.RubricType;
                }
            }
            Stage = (FilterStage)Enum.ToObject(typeof(FilterStage), stage);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTerm"/> class.
        /// </summary>
        /// <param name="filterColumn">The filterColumn<see cref="MemberRubric"/>.</param>
        /// <param name="operand">The operand<see cref="OperandType"/>.</param>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <param name="logic">The logic<see cref="LogicType"/>.</param>
        /// <param name="stage">The stage<see cref="FilterStage"/>.</param>
        public FilterTerm(MemberRubric filterColumn, OperandType operand, object value, LogicType logic = LogicType.And, FilterStage stage = FilterStage.First)
        {
            Operand = operand;
            Value = value;
            Logic = logic;
            ValueType = filterColumn.RubricType;
            RubricName = filterColumn.RubricName;
            FilterRubric = filterColumn;
            Stage = stage;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTerm"/> class.
        /// </summary>
        /// <param name="filterColumn">The filterColumn<see cref="string"/>.</param>
        /// <param name="operand">The operand<see cref="OperandType"/>.</param>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <param name="logic">The logic<see cref="LogicType"/>.</param>
        /// <param name="stage">The stage<see cref="FilterStage"/>.</param>
        public FilterTerm(string filterColumn, OperandType operand, object value, LogicType logic = LogicType.And, FilterStage stage = FilterStage.First)
        {
            RubricName = filterColumn;
            Operand = operand;
            Value = value;
            Logic = logic;
            Stage = stage;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTerm"/> class.
        /// </summary>
        /// <param name="filterColumn">The filterColumn<see cref="string"/>.</param>
        /// <param name="operand">The operand<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <param name="logic">The logic<see cref="string"/>.</param>
        /// <param name="stage">The stage<see cref="int"/>.</param>
        public FilterTerm(string filterColumn, string operand, object value, string logic = "And", int stage = 1)
        {
            RubricName = filterColumn;
            OperandType tempOperand1;
            Enum.TryParse(operand, true, out tempOperand1);
            Operand = tempOperand1;
            Value = value;
            LogicType tempLogic;
            Enum.TryParse(logic, true, out tempLogic);
            Logic = tempLogic;
            Stage = (FilterStage)Enum.ToObject(typeof(FilterStage), stage);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Figures.
        /// </summary>
        public IFigures Figures
        {
            get
            {
                return figures;
            }
            set
            {
                figures = value;
                if (FilterRubric == null && value != null)
                {
                    MemberRubric[] filterRubrics = figures.Rubrics.AsValues()
                             .Where(c => c.RubricName == RubricName).ToArray();
                    if (filterRubrics.Length > 0)
                    {
                        FilterRubric = filterRubrics[0];
                        ValueType = FilterRubric.RubricType;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the FilterRubric.
        /// </summary>
        public MemberRubric FilterRubric { get; set; }

        /// <summary>
        /// Gets or sets the Index.
        /// </summary>
        [DisplayName("Pos")]
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the Logic.
        /// </summary>
        public LogicType Logic { get; set; }

        /// <summary>
        /// Gets or sets the Operand.
        /// </summary>
        public OperandType Operand { get; set; }

        /// <summary>
        /// Gets or sets the RubricName.
        /// </summary>
        public string RubricName { get; set; }

        /// <summary>
        /// Gets or sets the Stage.
        /// </summary>
        public FilterStage Stage { get; set; } = FilterStage.First;

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the ValueType.
        /// </summary>
        public Type ValueType
        {
            get
            {
                if (valueType == null && valueTypeName != null)
                    valueType = Type.GetType(valueTypeName);
                return valueType;
            }
            set
            {
                valueType = value;
                valueTypeName = value.FullName;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Clone.
        /// </summary>
        /// <returns>The <see cref="object"/>.</returns>
        public object Clone()
        {
            FilterTerm clone = (FilterTerm)this.MemberwiseClone();
            clone.FilterRubric = FilterRubric;
            return clone;
        }

        /// <summary>
        /// The Clone.
        /// </summary>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <returns>The <see cref="FilterTerm"/>.</returns>
        public FilterTerm Clone(object value)
        {
            FilterTerm clone = (FilterTerm)this.MemberwiseClone();
            clone.FilterRubric = FilterRubric;
            clone.Value = value;
            return clone;
        }

        /// <summary>
        /// The Compare.
        /// </summary>
        /// <param name="term">The term<see cref="FilterTerm"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Compare(FilterTerm term)
        {
            if (RubricName != term.RubricName)
                return false;
            if (!Value.Equals(term.Value))
                return false;
            if (!Operand.Equals(term.Operand))
                return false;
            if (!Stage.Equals(term.Stage))
                return false;
            if (!Logic.Equals(term.Logic))
                return false;

            return true;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="FilterTerms" />.
    /// </summary>
    [Serializable]
    public class FilterTerms : Collection<FilterTerm>, ICollection
    {
        #region Fields

        [NonSerialized] private IFigures figures;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTerms"/> class.
        /// </summary>
        public FilterTerms()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTerms"/> class.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        public FilterTerms(IFigures figures)
        {
            this.Figures = figures;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Figures.
        /// </summary>
        public IFigures Figures
        {
            get { return figures; }
            set { figures = value; }
        }

        /// <summary>
        /// Gets a value indicating whether IsReadOnly.
        /// </summary>
        public bool IsReadOnly => throw new NotImplementedException();

        #endregion

        #region Methods

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="value">The value<see cref="FilterTerm"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public new int Add(FilterTerm value)
        {
            value.Figures = figures;
            value.Index = ((IList)this).Add(value);
            return value.Index;
        }

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="terms">The terms<see cref="ICollection{FilterTerm}"/>.</param>
        public void Add(ICollection<FilterTerm> terms)
        {
            foreach (FilterTerm term in terms)
            {
                term.Figures = Figures;
                term.Index = Add(term);
            }
        }

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="item">The item<see cref="IFilterTerm"/>.</param>
        public void Add(IFilterTerm item)
        {
            Add(new FilterTerm(item.RubricName, item.Operand, item.Value, item.Logic, item.Stage));
        }

        /// <summary>
        /// The AddNew.
        /// </summary>
        /// <returns>The <see cref="object"/>.</returns>
        public object AddNew()
        {
            return (object)((IBindingList)this).AddNew();
        }

        /// <summary>
        /// The Clone.
        /// </summary>
        /// <returns>The <see cref="FilterTerms"/>.</returns>
        public FilterTerms Clone()
        {
            FilterTerms ft = new FilterTerms();
            foreach (FilterTerm t in this)
            {
                FilterTerm _t = new FilterTerm(t.RubricName, t.Operand, t.Value, t.Logic, t.Stage);
                ft.Add(_t);
            }
            return ft;
        }

        /// <summary>
        /// The Contains.
        /// </summary>
        /// <param name="item">The item<see cref="IFilterTerm"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Contains(IFilterTerm item)
        {
            return Contains(new FilterTerm(item.RubricName, item.Operand, item.Value, item.Logic, item.Stage));
        }

        /// <summary>
        /// The Contains.
        /// </summary>
        /// <param name="RubricName">The RubricName<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Contains(string RubricName)
        {
            return this.AsEnumerable().Where(c => c.RubricName == RubricName).Any();
        }

        /// <summary>
        /// The CopyTo.
        /// </summary>
        /// <param name="array">The array<see cref="IFilterTerm[]"/>.</param>
        /// <param name="arrayIndex">The arrayIndex<see cref="int"/>.</param>
        public void CopyTo(IFilterTerm[] array, int arrayIndex)
        {
            Array.Copy(this.AsQueryable().Cast<IFilterTerm>().ToArray(), array, Count);
        }

        /// <summary>
        /// The Find.
        /// </summary>
        /// <param name="data">The data<see cref="FilterTerm"/>.</param>
        /// <returns>The <see cref="FilterTerm"/>.</returns>
        public FilterTerm Find(FilterTerm data)
        {
            foreach (FilterTerm lDetailValue in this)
                if (lDetailValue == data)    // Found it
                    return lDetailValue;
            return null;    // Not found
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="stage">The stage<see cref="int"/>.</param>
        /// <returns>The <see cref="List{FilterTerm}"/>.</returns>
        public List<FilterTerm> Get(int stage)
        {
            FilterStage filterStage = (FilterStage)Enum.ToObject(typeof(FilterStage), stage);
            return this.AsEnumerable().Where(c => filterStage.Equals(c.Stage)).ToList();
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="RubricNames">The RubricNames<see cref="List{string}"/>.</param>
        /// <returns>The <see cref="List{FilterTerm}"/>.</returns>
        public List<FilterTerm> Get(List<string> RubricNames)
        {
            return this.AsEnumerable().Where(c => RubricNames.Contains(c.FilterRubric.RubricName)).ToList();
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="RubricName">The RubricName<see cref="string"/>.</param>
        /// <returns>The <see cref="FilterTerm[]"/>.</returns>
        public FilterTerm[] Get(string RubricName)
        {
            return this.AsEnumerable().Where(c => c.RubricName == RubricName).ToArray();
        }

        /// <summary>
        /// The IndexOf.
        /// </summary>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int IndexOf(object value)
        {
            for (int i = 0; i < Count; i++)
                if (ReferenceEquals(this[i], value))    // Found it
                    return i;
            return -1;
        }

        /// <summary>
        /// The Remove.
        /// </summary>
        /// <param name="value">The value<see cref="ICollection{FilterTerm}"/>.</param>
        public void Remove(ICollection<FilterTerm> value)
        {
            foreach (FilterTerm term in value)
                Remove(term);
        }

        /// <summary>
        /// The Remove.
        /// </summary>
        /// <param name="item">The item<see cref="IFilterTerm"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Remove(IFilterTerm item)
        {
            return Remove(new FilterTerm(item.RubricName, item.Operand, item.Value, item.Logic, item.Stage));
        }

        /// <summary>
        /// The Renew.
        /// </summary>
        /// <param name="terms">The terms<see cref="ICollection{FilterTerm}"/>.</param>
        public void Renew(ICollection<FilterTerm> terms)
        {
            bool diffs = false;
            if (Count != terms.Count)
            {
                diffs = true;
            }
            else
            {
                foreach (FilterTerm term in terms)
                {
                    if (Contains(term.RubricName))
                    {
                        int same = 0;
                        foreach (FilterTerm myterm in Get(term.RubricName))
                        {
                            if (!myterm.Compare(term))
                                same++;
                        }
                        if (same != 0)
                        {
                            diffs = true;
                            break;
                        }
                    }
                    else
                    {
                        diffs = true;
                        break;
                    }
                }
            }

            if (diffs)
            {
                Clear();
                foreach (FilterTerm term in terms)
                    Add(term);
            }
        }

        /// <summary>
        /// The Reset.
        /// </summary>
        public void Reset()
        {
            this.Clear();
        }

        /// <summary>
        /// The SetRange.
        /// </summary>
        /// <param name="data">The data<see cref="FilterTerm[]"/>.</param>
        public void SetRange(FilterTerm[] data)
        {
            for (int i = 0; i < data.Length; i++)
                this[i] = data[i];
        }

        #endregion
    }
}
