﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace System.Instant.Treatments
{
    [Serializable]
    public class SortTerms : Collection<SortTerm>, ICollection
    {
        public SortTerms()
        {
 
        }
        public SortTerms(IFigures figures)
        {          
            this.figures = figures;

        }

        [NonSerialized]
        private IFigures figures;
        public IFigures Figures
        { get { return figures; } set { figures = value; } }

        public bool IsReadOnly => throw new NotImplementedException();

        public List<SortTerm> Get(List<string> RubricNames)
        {
            return this.AsEnumerable().Where(c => RubricNames.Contains(c.RubricName)).ToList();
        }
        public List<SortTerm> Get()
        {
            return this.AsEnumerable().Select(c => c).ToList();
        }
        public bool Have(string RubricName)
        {
            return this.AsEnumerable().Where(c => c.RubricName == RubricName).Any();
        }
        public SortTerm[] GetTerms(string RubricName)
        {
            return this.AsEnumerable().Where(c => c.RubricName == RubricName).ToArray();
        }

        public SortTerms Clone()
        {
            SortTerms mx = (SortTerms)this.MemberwiseClone();
            return mx;
        }

        #region ICollection
        public new int Add(SortTerm value)
        {
            value.Figures = Figures;
            value.Index = ((IList)this).Add(value);
            return value.Index;
        }
        public void Add(ICollection<SortTerm> terms)
        {
            foreach (SortTerm term in terms)
            {
                term.Figures = Figures;
                term.Index = ((IList)this).Add(term);
            }
        }
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
        public void Remove(ICollection<SortTerm> value)
        {
            foreach (SortTerm term in value)
                Remove(term);
        }
        public object AddNew()
        {
            return (object)((IBindingList)this).AddNew();
        }
      
        public void SetRange(SortTerm[] data)
        {
            for (int i = 0; i < data.Length; i++)
                this[i] = data[i];
        }
        public int IndexOf(object value)
        {
            for (int i = 0; i < Count; i++)
                if (this[i] == value)    // Found it
                    return i;
            return -1;
        }
     
        public SortTerm Find(SortTerm data)
        {
            foreach (SortTerm lDetailValue in this)
                if (lDetailValue == data)    // Found it
                    return lDetailValue;
            return null;    // Not found
        }

        public void Add(ISortTerm item)
        {
            Add(new SortTerm(item.RubricName, item.Direction.ToString(), item.RubricId));
        }

        public bool Contains(ISortTerm item)
        {
            return Contains(new SortTerm(item.RubricName, item.Direction.ToString(), item.RubricId));
        }

        public void CopyTo(ISortTerm[] array, int arrayIndex)
        {
            Array.Copy(this.Cast<ISortTerm>().ToArray(), array, Count);
        }

        public bool Remove(ISortTerm item)
        {
           return Remove(new SortTerm(item.RubricName, item.Direction.ToString(), item.RubricId)); 
        }       

        #endregion

    }

    [Serializable]
    public class SortTerm : ISortTerm
    {
        public string dataTypeName;
        [NonSerialized] private Type dataType;
        [NonSerialized] private IFigures figures;

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

        public SortTerm()
        {
        }
        public SortTerm(IFigures table)
        {
            Figures = table;
        }
        public SortTerm(string rubricName, string direction = "ASC", int ordinal = 0)
        {
                RubricName = rubricName;            
                SortDirection sortDirection;
                Enum.TryParse(direction, true, out sortDirection);
                Direction = sortDirection;
                RubricId = ordinal;
        }

        public SortTerm(MemberRubric sortedRubric, SortDirection direction = SortDirection.ASC, int ordinal = 0)
        {
            Direction = direction;
            SortedRubric = sortedRubric;
            RubricId = ordinal;
        }

        public SortDirection Direction { get; set; }
        private MemberRubric sortedRubric;
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
        public string TypeString { get; set; }
        public int RubricId { get; set; }
        private string rubricName;
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
        public int Index { get; set; }
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

        public bool Compare(SortTerm term)
        {
            if (RubricName != term.RubricName || Direction != term.Direction)
                return false;

            return true;
        }

    }
}