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

    /// <summary>
    /// Defines the <see cref="Link" />.
    /// </summary>
    [JsonObject]
    [Serializable]
    public class Link : IUnique
    {
        #region Fields

        private Ussc serialcode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class.
        /// </summary>
        public Link()
        {
            Name = Unique.NewKey.ToString() + "_L";
            UniqueKey = Name.UniqueKey64();
            Origin = new LinkMember(this, LinkSite.Origin);
            Target = new LinkMember(this, LinkSite.Target);
            Linker.Map.Links.Put(this);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class.
        /// </summary>
        /// <param name="origin">The origin<see cref="IFigures"/>.</param>
        /// <param name="target">The target<see cref="IFigures"/>.</param>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class.
        /// </summary>
        /// <param name="origin">The origin<see cref="IFigures"/>.</param>
        /// <param name="target">The target<see cref="IFigures"/>.</param>
        /// <param name="keyRubric">The keyRubric<see cref="IRubric"/>.</param>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class.
        /// </summary>
        /// <param name="origin">The origin<see cref="IFigures"/>.</param>
        /// <param name="target">The target<see cref="IFigures"/>.</param>
        /// <param name="keyRubrics">The keyRubrics<see cref="IRubrics"/>.</param>
        public Link(IFigures origin, IFigures target, IRubrics keyRubrics) : this(origin, target)
        {
            foreach (IUnique rubric in keyRubrics)
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
        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class.
        /// </summary>
        /// <param name="origin">The origin<see cref="IFigures"/>.</param>
        /// <param name="target">The target<see cref="IFigures"/>.</param>
        /// <param name="keyRubricNames">The keyRubricNames<see cref="string[]"/>.</param>
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

        /// <summary>
        /// Gets the Empty.
        /// </summary>
        public IUnique Empty => Ussn.Empty;

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Origin.
        /// </summary>
        public LinkMember Origin { get; set; }

        /// <summary>
        /// Gets or sets the OriginKeys.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the OriginName.
        /// </summary>
        public string OriginName
        {
            get { return Origin.Name; }
            set
            {
                Origin.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the OriginRubrics.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the SerialCode.
        /// </summary>
        public Ussc SerialCode { get => serialcode; set => serialcode = value; }

        /// <summary>
        /// Gets or sets the Target.
        /// </summary>
        public LinkMember Target { get; set; }

        /// <summary>
        /// Gets or sets the TargetKeys.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the TargetName.
        /// </summary>
        public string TargetName
        {
            get { return Target.Name; }
            set
            {
                Target.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the TargetRubrics.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the UniqueKey.
        /// </summary>
        public ulong UniqueKey { get => serialcode.UniqueKey; set => serialcode.UniqueKey = value; }

        /// <summary>
        /// Gets or sets the UniqueSeed.
        /// </summary>
        public ulong UniqueSeed { get => serialcode.UniqueSeed; set => serialcode.UniqueSeed = value; }

        #endregion

        #region Methods

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int CompareTo(IUnique other)
        {
            return serialcode.CompareTo(other);
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(IUnique other)
        {
            return serialcode.Equals(other);
        }

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public byte[] GetBytes()
        {
            return serialcode.GetBytes();
        }

        /// <summary>
        /// The GetUniqueBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public byte[] GetUniqueBytes()
        {
            return serialcode.GetUniqueBytes();
        }

        #endregion
    }
}
