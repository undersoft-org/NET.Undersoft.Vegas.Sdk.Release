using System.Runtime.InteropServices;
using System.Extract;
using System.Uniques;
using System.Sets;
using System.Instant.Linking;
using System.Linq;
using System.Reflection;
using System.IO;

/******************************************************************
    Copyright (c) 2020 Undersoft

    @name System.Instant.FigureCard                
    
    @project NET.Undersoft.Sdk
    @author Darius Hanc                                                                               
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                       
 
 ******************************************************************/
namespace System.Instant
{     
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class FigureCard : BaseCard<IFigure>, IFigure, IEquatable<IFigure>, IComparable<IFigure>
    {
        private IDeck<object> presets;

        public FigureCard(IFigures figures)
        {
            Figures = figures;
        }
        public FigureCard(object key, IFigure value, IFigures figures) : base(key, value)
        {
            Figures = figures;
        }
        public FigureCard(ulong key, IFigure value, IFigures figures) : base(key, value)
        {
            Figures = figures;
        }
        public FigureCard(IFigure value, IFigures figures) : base(value)
        {            
            Figures = figures;
            UniquesAsKey();            
        }
        public FigureCard(ICard<IFigure> value, IFigures figures) : base(value)
        {
            Figures = figures;
            UniquesAsKey();
        }

        public object this[int fieldId]
        {
            get => GetPreset(fieldId);
            set => SetPreset(fieldId, value);
        }
        public object this[string propertyName]
        {
            get => GetPreset(propertyName);
            set => SetPreset(propertyName, value);
        }

        public override void Set(object key, IFigure value)
        {
            this.value = value;
            this.value.UniqueKey = key.UniqueKey();
        }
        public override void Set(IFigure value)
        {
            this.value = value;
        }
        public override void Set(ICard<IFigure> card)
        {
            this.value = card.Value;        
        }

        public override bool Equals(ulong key)
        {
            return Key == key;
        }
        public override bool Equals(object y)
        {
            return Key.Equals(y.UniqueKey());
        }
        public          bool Equals(IFigure other)
        {
            return Key == other.UniqueKey;
        }

        public override int GetHashCode()
        {
            return Value.GetUniqueBytes().BitAggregate64to32().ToInt32();
        }

        public override int CompareTo(object other)
        {
            return (int)(Key - other.UniqueKey64());
        }
        public override int CompareTo(ulong key)
        {
            return (int)(Key - key);
        }
        public override int CompareTo(ICard<IFigure> other)
        {
            return (int)(Key - other.Key);
        }
        public          int CompareTo(IFigure other)
        {
            return (int)(Key - other.UniqueKey);
        }

        public override byte[] GetBytes()
        {
            if (!Figures.Prime && presets != null)
            {

                IFigure f = Figures.NewFigure();
                f.ValueArray = ValueArray;
                f.SerialCode = value.SerialCode;
                byte[] ba = f.GetBytes();
                f = null;
                return ba;
            }
            else
                return value.GetBytes();
        }

        public unsafe override byte[] GetUniqueBytes()
        {
            return value.GetUniqueBytes();
        }

        public override int[] UniqueOrdinals()
        {
            return Figures.KeyRubrics.Ordinals;
        }

        public override object[] UniqueValues()
        {
            int[] ordinals = UniqueOrdinals();
            if (ordinals != null)
                return ordinals.Select(x => value[x]).ToArray();
            return null;
        }

        public override ulong UniquesAsKey()
        {
            ulong key = value.UniqueKey;
            if (key == 0)
            {
                IRubrics r = Figures.KeyRubrics;
                key = r.Ordinals.Select(x => value[x]).ToArray().UniqueKey64(r.BinarySizes, r.BinarySize);
                value.UniqueKey = key;
            }
            return key;
        }

        public override ulong Key
        {
            get => value.UniqueKey;                                                   
            set => this.value.UniqueKey = value;            
        }

        public override ulong UniqueKey
        {
            get => value.UniqueKey;
            set => this.value.UniqueKey = value;
        }

        public object[] ValueArray
        {
            get
            {
                if (Figures.Prime || presets == null)
                    return value.ValueArray;
                object[] valarr = value.ValueArray;
                presets.AsCards().Select(x => valarr[x.Key] = x.Value).ToArray();
                return valarr;
            }
            set
            {
                int l = value.Length;
                for (int i = 0; i < l; i++)
                    SetPreset(i, value[i]);
            }
        }

        public     Ussn SerialCode
        {
            get => value.SerialCode;
            set => this.value.SerialCode = value;
        }
       
        public IFigures Figures { get; set; }

        public object GetPreset(int fieldId)
        {           
            if (presets != null && !Figures.Prime)
            {
                object val = presets.Get(fieldId);
                if (val != null) 
                    return val;                
            }
            return value[fieldId];
        }
        public object GetPreset(string propertyName)
        {
            if (presets != null && !Figures.Prime)
            {
                MemberRubric rubric = Figures.Rubrics[propertyName.UniqueKey()];
                if (rubric != null)
                {
                    object val = presets.Get(rubric.FieldId);
                    if (val != null)
                        return val;
                }
                else
                    throw new IndexOutOfRangeException("Field doesn't exist");
            }
            return value[propertyName];
        }

        public ICard<object>[] GetPresets()
        {
            return presets.AsCards().ToArray();
        }

        public void SetPreset(int fieldId, object value)
        {           
            if (GetPreset(fieldId).Equals(value))           
                return;
            if (!Figures.Prime)
            {
                if (presets == null)
                    presets = new Deck<object>(9);
                presets.Put(fieldId, value);
            }
            else
                this.value[fieldId] = value;            
        }
        public void SetPreset(string propertyName, object value)
        {
            MemberRubric rubric = Figures.Rubrics[propertyName.UniqueKey()];
            if (rubric != null)
                SetPreset(rubric.FieldId, value);
            else
                throw new IndexOutOfRangeException("Field doesn't exist");
        }

        public void WritePresets()
        {
            foreach (var c in presets.AsCards())
                value[(int)c.Key] = c.Value;
            presets = null;
        }

        public bool HavePresets
          => presets != null ?
          true :
          false;
    }
}
