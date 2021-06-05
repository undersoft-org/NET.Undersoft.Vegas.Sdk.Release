using System.Runtime.InteropServices;
using System.Extract;
using System.Uniques;
using System.Multemic;
using System.Instant.Linkmap;
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
    public class FigureCard : Card<IFigure>, IFigure, IEquatable<IFigure>, IComparable<IFigure>
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
        public FigureCard(long key, IFigure value, IFigures figures) : base(key, value)
        {
            Figures = figures;
        }
        public FigureCard(IFigure value, IFigures figures) : base(value)
        {            
            Figures = figures;
            IdentitiesToKey();
            
        }
        public FigureCard(ICard<IFigure> value, IFigures figures) : base(value)
        {
            Figures = figures;
            IdentitiesToKey();
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
            this.value.KeyBlock = key.GetHashKey();
        }
        public override void Set(IFigure value)
        {
            this.value = value;
        }
        public override void Set(ICard<IFigure> card)
        {
            this.value = card.Value;        
        }

        public override bool Equals(long key)
        {
            return Key == key;
        }
        public override bool Equals(object y)
        {
            return Key.Equals(y.GetHashKey());
        }
        public          bool Equals(IFigure other)
        {
            return Key == other.KeyBlock;
        }

        public override int GetHashCode()
        {
            return Value.GetKeyBytes().BitAggregate64to32().ToInt32();
        }

        public override int CompareTo(object other)
        {
            return (int)(Key - other.GetHashKey64());
        }
        public override int CompareTo(long key)
        {
            return (int)(Key - key);
        }
        public override int CompareTo(ICard<IFigure> other)
        {
            return (int)(Key - other.Key);
        }
        public          int CompareTo(IFigure other)
        {
            return (int)(Key - other.KeyBlock);
        }

        public override byte[] GetBytes()
        {
            if (Figures.Prime && presets != null)
            {

                IFigure f = Figures.NewFigure();
                f.ValueArray = ValueArray;
                f.SystemSerialCode = value.SystemSerialCode;
                byte[] ba = f.GetBytes();
                f = null;
                return ba;
            }
            else
                return value.GetBytes();
        }

        public unsafe override byte[] GetKeyBytes()
        {
            return value.GetKeyBytes();
        }

        public override    int[] IdentityIndexes()
        {
            if(Figures.KeyRubrics != null)
                return Figures.KeyRubrics.Ordinals;
            return null;
        }
        public override object[] IdentityValues()
        {
            int[] ordinals = IdentityIndexes();
            if (ordinals != null)
               return ordinals.Select(x => value[x]).ToArray();
            return null;      
        }
        public override     long IdentitiesToKey()
        {
            long key = value.KeyBlock;
            if (key == 0)
            {
                key = Figures.KeyRubrics.Ordinals.Select(x => value[x]).ToArray().GetHashKey();
                value.KeyBlock = key;
            }
            return key;
        }  

        public override long Key
        {
            get => value.KeyBlock;                                                   
            set => this.value.KeyBlock = value;            
        }

        public override long KeyBlock
        {
            get => value.KeyBlock;
            set => this.value.KeyBlock = value;
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

        public     Ussn SystemSerialCode
        {
            get => value.SystemSerialCode;
            set => this.value.SystemSerialCode = value;
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
                MemberRubric rubric = Figures.Rubrics[propertyName.GetHashKey()];
                if (rubric != null)
                {
                    object val = presets.Get(rubric.FigureFieldId);
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
                    presets = new Deck<object>(5);
                presets.Put(fieldId, value);
            }
            else
                this.value[fieldId] = value;            
        }
        public void SetPreset(string propertyName, object value)
        {
            MemberRubric rubric = Figures.Rubrics[propertyName.GetHashKey()];
            if (rubric != null)
                SetPreset(rubric.FigureFieldId, value);
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
