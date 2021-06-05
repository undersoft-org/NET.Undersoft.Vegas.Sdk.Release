/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.SortTerms.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
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

    /// <summary>
    /// Defines the <see cref="SortTerm" />.
    /// </summary>
    [Serializable]
    public class SortTerm : ISortTerm
    {
        #region Fields

        public string dataTypeName;
        [NonSerialized] private Type dataType;
        [NonSerialized] private IFigures figures;
        private string rubricName;
        private MemberRubric sortedRubric;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SortTerm"/> class.
        /// </summary>
        public SortTerm()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SortTerm"/> class.
        /// </summary>
        /// <param name="table">The table<see cref="IFigures"/>.</param>
        public SortTerm(IFigures table)
        {
            Figures = table;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SortTerm"/> class.
        /// </summary>
        /// <param name="sortedRubric">The sortedRubric<see cref="MemberRubric"/>.</param>
        /// <param name="direction">The direction<see cref="SortDirection"/>.</param>
        /// <param name="ordinal">The ordinal<see cref="int"/>.</param>
        public SortTerm(MemberRubric sortedRubric, SortDirection direction = SortDirection.ASC, int ordinal = 0)
        {
            Direction = direction;
            SortedRubric = sortedRubric;
            RubricId = ordinal;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SortTerm"/> class.
        /// </summary>
        /// <param name="rubricName">The rubricName<see cref="string"/>.</param>
        /// <param name="direction">The direction<see cref="string"/>.</param>
        /// <param name="ordinal">The ordinal<see cref="int"/>.</param>
        public SortTerm(string rubricName, string direction = "ASC", int ordinal = 0)
        {
            RubricName = rubricName;
            SortDirection sortDirection;
            Enum.TryParse(direction, true, out sortDirection);
            Direction = sortDirection;
            RubricId = ordinal;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Direction.
        /// </summary>
        public SortDirection Direction { get; set; }

        /// <summary>
        /// Gets or sets the Figures.
        /// </summary>
        public IFigures Figures
        {
            get { return figures; }
            set
            {
                if (value != null)
                {
                    figures = value;
                    if (rubricName != null)
                        if (value.Rubrics.ContainsKey(rubricName))
                        {
                            MemberRubric pyl = value.Rubrics.AsValues().Where(c => c.RubricName == rubricName).First();
                            if (pyl != null)
                            {
                                if (sortedRubric == null)
                                    sortedRubric = pyl;
                                if (RubricType == null)
                                    RubricType = pyl.RubricType;
                                if (TypeString == null)
                                    TypeString = GetTypeString(pyl.RubricType);
                            }
                        }
                }
            }
        }

        /// <summary>
        /// Gets or sets the Index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the RubricId.
        /// </summary>
        public int RubricId { get; set; }

        /// <summary>
        /// Gets or sets the RubricName.
        /// </summary>
        public string RubricName
        {
            get
            {
                return rubricName;
            }
            set
            {
                rubricName = value;
                if (Figures != null)
                {
                    if (Figures.Rubrics.ContainsKey(rubricName))
                    {
                        if (sortedRubric == null)
                            SortedRubric = Figures.Rubrics.AsValues().Where(c => c.RubricName == RubricName).First();
                        if (RubricType == null)
                            RubricType = SortedRubric.RubricType;
                        if (TypeString == null)
                            TypeString = GetTypeString(RubricType);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the RubricType.
        /// </summary>
        public Type RubricType
        {
            get
            {
                if (dataType == null && dataTypeName != null)
                    dataType = Type.GetType(dataTypeName);
                return dataType;
            }
            set
            {
                dataType = value;
                dataTypeName = value.FullName;
            }
        }

        /// <summary>
        /// Gets or sets the SortedRubric.
        /// </summary>
        public MemberRubric SortedRubric
        {
            get { return sortedRubric; }
            set
            {
                if (value != null)
                {
                    sortedRubric = value;
                    rubricName = sortedRubric.RubricName;
                    RubricType = sortedRubric.RubricType;
                    TypeString = GetTypeString(RubricType);
                }
            }
        }

        /// <summary>
        /// Gets or sets the TypeString.
        /// </summary>
        public string TypeString { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The Compare.
        /// </summary>
        /// <param name="term">The term<see cref="SortTerm"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Compare(SortTerm term)
        {
            if (RubricName != term.RubricName || Direction != term.Direction)
                return false;

            return true;
        }

        /// <summary>
        /// The GetTypeString.
        /// </summary>
        /// <param name="column">The column<see cref="MemberRubric"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string GetTypeString(MemberRubric column)
        {
            Type dataType = column.RubricType;
            string type = "string";
            if (dataType == typeof(string))
                type = "string";
            else if (dataType == typeof(int))
                type = "int";
            else if (dataType == typeof(decimal))
                type = "decimal";
            else if (dataType == typeof(DateTime))
                type = "DateTime";
            else if (dataType == typeof(Single))
                type = "Single";
            else if (dataType == typeof(float))
                type = "float";
            else
                type = "string";
            return type;
        }

        /// <summary>
        /// The GetTypeString.
        /// </summary>
        /// <param name="RubricType">The RubricType<see cref="Type"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string GetTypeString(Type RubricType)
        {
            Type dataType = RubricType;
            string type = "string";
            if (dataType == typeof(string))
                type = "string";
            else if (dataType == typeof(int))
                type = "int";
            else if (dataType == typeof(decimal))
                type = "decimal";
            else if (dataType == typeof(DateTime))
                type = "DateTime";
            else if (dataType == typeof(Single))
                type = "Single";
            else if (dataType == typeof(float))
                type = "float";
            else
                type = "string";
            return type;
        }

        #endregion
    }

    /// <summary>
    /// Defines the <see cref="SortTerms" />.
    /// </summary>
    [Serializable]
    public class SortTerms : Collection<SortTerm>, ICollection
    {
        #region Fields

        [NonSerialized]
        private IFigures figures;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SortTerms"/> class.
        /// </summary>
        public SortTerms()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SortTerms"/> class.
        /// </summary>
        /// <param name="figures">The figures<see cref="IFigures"/>.</param>
        public SortTerms(IFigures figures)
        {
            this.figures = figures;
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
        /// <param name="terms">The terms<see cref="ICollection{SortTerm}"/>.</param>
        public void Add(ICollection<SortTerm> terms)
        {
            foreach (SortTerm term in terms)
            {
                term.Figures = Figures;
                term.Index = ((IList)this).Add(term);
            }
        }

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="item">The item<see cref="ISortTerm"/>.</param>
        public void Add(ISortTerm item)
        {
            Add(new SortTerm(item.RubricName, item.Direction.ToString(), item.RubricId));
        }

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="value">The value<see cref="SortTerm"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public new int Add(SortTerm value)
        {
            value.Figures = Figures;
            value.Index = ((IList)this).Add(value);
            return value.Index;
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
        /// <returns>The <see cref="SortTerms"/>.</returns>
        public SortTerms Clone()
        {
            SortTerms mx = (SortTerms)this.MemberwiseClone();
            return mx;
        }

        /// <summary>
        /// The Contains.
        /// </summary>
        /// <param name="item">The item<see cref="ISortTerm"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Contains(ISortTerm item)
        {
            return Contains(new SortTerm(item.RubricName, item.Direction.ToString(), item.RubricId));
        }

        /// <summary>
        /// The CopyTo.
        /// </summary>
        /// <param name="array">The array<see cref="ISortTerm[]"/>.</param>
        /// <param name="arrayIndex">The arrayIndex<see cref="int"/>.</param>
        public void CopyTo(ISortTerm[] array, int arrayIndex)
        {
            Array.Copy(this.Cast<ISortTerm>().ToArray(), array, Count);
        }

        /// <summary>
        /// The Find.
        /// </summary>
        /// <param name="data">The data<see cref="SortTerm"/>.</param>
        /// <returns>The <see cref="SortTerm"/>.</returns>
        public SortTerm Find(SortTerm data)
        {
            foreach (SortTerm lDetailValue in this)
                if (lDetailValue == data)    // Found it
                    return lDetailValue;
            return null;    // Not found
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <returns>The <see cref="List{SortTerm}"/>.</returns>
        public List<SortTerm> Get()
        {
            return this.AsEnumerable().Select(c => c).ToList();
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="RubricNames">The RubricNames<see cref="List{string}"/>.</param>
        /// <returns>The <see cref="List{SortTerm}"/>.</returns>
        public List<SortTerm> Get(List<string> RubricNames)
        {
            return this.AsEnumerable().Where(c => RubricNames.Contains(c.RubricName)).ToList();
        }

        /// <summary>
        /// The GetTerms.
        /// </summary>
        /// <param name="RubricName">The RubricName<see cref="string"/>.</param>
        /// <returns>The <see cref="SortTerm[]"/>.</returns>
        public SortTerm[] GetTerms(string RubricName)
        {
            return this.AsEnumerable().Where(c => c.RubricName == RubricName).ToArray();
        }

        /// <summary>
        /// The Have.
        /// </summary>
        /// <param name="RubricName">The RubricName<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Have(string RubricName)
        {
            return this.AsEnumerable().Where(c => c.RubricName == RubricName).Any();
        }

        /// <summary>
        /// The IndexOf.
        /// </summary>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int IndexOf(object value)
        {
            for (int i = 0; i < Count; i++)
                if (this[i] == value)    // Found it
                    return i;
            return -1;
        }

        /// <summary>
        /// The Remove.
        /// </summary>
        /// <param name="value">The value<see cref="ICollection{SortTerm}"/>.</param>
        public void Remove(ICollection<SortTerm> value)
        {
            foreach (SortTerm term in value)
                Remove(term);
        }

        /// <summary>
        /// The Remove.
        /// </summary>
        /// <param name="item">The item<see cref="ISortTerm"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Remove(ISortTerm item)
        {
            return Remove(new SortTerm(item.RubricName, item.Direction.ToString(), item.RubricId));
        }

        /// <summary>
        /// The Renew.
        /// </summary>
        /// <param name="terms">The terms<see cref="ICollection{SortTerm}"/>.</param>
        public void Renew(ICollection<SortTerm> terms)
        {
            bool diffs = false;
            if (Count != terms.Count)
            {
                diffs = true;
            }
            else
            {
                foreach (SortTerm term in terms)
                {
                    if (Have(term.RubricName))
                    {
                        int same = 0;
                        foreach (SortTerm myterm in GetTerms(term.RubricName))
                        {
                            if (myterm.Compare(term))
                                same++;
                        }
                        if (same == 0)
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
                foreach (SortTerm term in terms)
                    term.Index = ((IList)this).Add(term);
            }
        }

        /// <summary>
        /// The SetRange.
        /// </summary>
        /// <param name="data">The data<see cref="SortTerm[]"/>.</param>
        public void SetRange(SortTerm[] data)
        {
            for (int i = 0; i < data.Length; i++)
                this[i] = data[i];
        }

        #endregion
    }
}
