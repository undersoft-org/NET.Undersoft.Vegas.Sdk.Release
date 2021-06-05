/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Link.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (29.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Linking
{
    using System.Uniques;

    [JsonObject]
    [Serializable]
    public class Link : IUnique
    {
        #region Fields

        private Ussc serialcode;

        #endregion

        #region Constructors

        public Link()
        {
            Name = Unique.NewKey.ToString() + "_L";
            UniqueKey = Name.UniqueKey64();
            Origin = new LinkMember(this, LinkSite.Origin);
            Target = new LinkMember(this, LinkSite.Target);
            Linker.Map.Links.Put(this);
        }
        public Link(IFigures origin, IFigures target)
        {
            Name = origin.Type.Name + "_" + target.Type.Name;
            UniqueKey = Name.UniqueKey64();
            Origin = new LinkMember(origin, this, LinkSite.Origin);
            Target = new LinkMember(target, this, LinkSite.Target);            
            origin.Linker.TargetLinks.Put(this);
            target.Linker.OriginLinks.Put(this);
            Linker.Map.Links.Put(this);
        }
        public Link(IFigures origin, IFigures target, IRubric keyRubric) : this(origin, target)
        {
            var originRubric = origin.Rubrics[keyRubric];
            var targetRubric = target.Rubrics[keyRubric];
            if (originRubric != null && targetRubric != null)
            {
                OriginKeys.Add(originRubric);
                TargetKeys.Add(targetRubric);
            }
            else
                throw new IndexOutOfRangeException("Rubric not found");
            OriginKeys.Update();
            TargetKeys.Update();
        }
        public Link(IFigures origin, IFigures target, IRubrics keyRubrics) : this(origin, target)
        {
            foreach(IUnique rubric in keyRubrics)
            {
                var originRubric = origin.Rubrics[rubric];
                var targetRubric = target.Rubrics[rubric];
                if (originRubric != null && targetRubric != null)
                {
                    OriginKeys.Add(originRubric);
                    TargetKeys.Add(targetRubric);
                }
                else
                    throw new IndexOutOfRangeException("Rubric not found");
                OriginKeys.Update();
                TargetKeys.Update();
            }
        }
        public Link(IFigures origin, IFigures target, string[] keyRubricNames) : this(origin, target)
        {
            foreach (var name in keyRubricNames)
            {
                var originRubric = origin.Rubrics[name];
                var targetRubric = target.Rubrics[name];
                if (originRubric != null && targetRubric != null)
                {
                    OriginKeys.Add(originRubric);
                    TargetKeys.Add(targetRubric);
                }
                else
                    throw new IndexOutOfRangeException("Rubric not found");
            }
            OriginKeys.Update();
            TargetKeys.Update();
        }

        #endregion

        #region Properties

        public IUnique Empty => Ussn.Empty;

        public ulong UniqueKey { get => serialcode.UniqueKey; set => serialcode.UniqueKey = value; }

        public string Name { get; set; }

        public LinkMember Origin { get; set; }

        public IRubrics OriginKeys
        {
            get
            {
                return Origin.KeyRubrics;
            }
            set
            {
                Origin.KeyRubrics.Renew(value);
                Origin.KeyRubrics.Update();
            }
        }

        public string OriginName
        {
            get { return Origin.Name; }
            set
            {
                Origin.Name = value;
            }
        }

        public IRubrics OriginRubrics
        {
            get
            {
                return Origin.Rubrics;
            }
            set
            {
                Origin.Rubrics = value;
            }
        }

        public ulong UniqueSeed { get => serialcode.UniqueSeed; set => serialcode.UniqueSeed = value; }

        public Ussc SerialCode
        {
            get => serialcode;
            set => serialcode = value;            
        }

        public LinkMember Target { get; set; }

        public IRubrics TargetKeys
        {
            get
            {
                return Target.KeyRubrics;
            }
            set
            {
                Target.KeyRubrics.Renew(value);
                Target.KeyRubrics.Update();
            }
        }

        public string TargetName
        {
            get { return Target.Name; }
            set
            {
                Target.Name = value;
            }
        }

        public IRubrics TargetRubrics
        {
            get
            {
                return Target.KeyRubrics;
            }
            set
            {
                Target.KeyRubrics = value;
            }
        }

        #endregion

        #region Methods

        public int CompareTo(IUnique other)
        {
            return serialcode.CompareTo(other);
        }

        public bool Equals(IUnique other)
        {
            return serialcode.Equals(other);
        }

        public byte[] GetBytes()
        {
            return serialcode.GetBytes();
        }

        public byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }

        #endregion
    }
}
