/*************************************************
   Copyright (c) 2021 Undersoft

   Laborer.cs
              
   @author: Dariusz Hanc                                                  
   @date: (28.05.2021)                                            
   @licence MIT                                       
 *************************************************/

namespace System.Labors
{
    using System.Collections.Generic;
    using System.Instant;
    using System.Sets;
    using System.Uniques;

    /// <summary>
    /// Defines the <see cref="Laborer" />.
    /// </summary>
    public class Laborer : IUnique
    {
        #region Fields

        private Board<object> input;
        private Board<object> output;
        private Ussc SerialCode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Laborer"/> class.
        /// </summary>
        public Laborer()
        {
            input = new Board<object>();
            output = new Board<object>();
            EvokersIn = new NoteEvokers();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Laborer"/> class.
        /// </summary>
        /// <param name="Name">The Name<see cref="string"/>.</param>
        /// <param name="Method">The Method<see cref="IDeputy"/>.</param>
        public Laborer(string Name, IDeputy Method) : this()
        {
            Work = Method;
            LaborerName = Name;

            SerialCode = new Ussc(($"{Work.UniqueKey}.{DateTime.Now.ToBinary()}").UniqueKey());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Empty.
        /// </summary>
        public IUnique Empty => new Ussc();

        /// <summary>
        /// Gets or sets the EvokersIn.
        /// </summary>
        public NoteEvokers EvokersIn { get; set; }

        /// <summary>
        /// Gets or sets the Input.
        /// </summary>
        public object Input
        {

            get
            {
                object _entry = null;
                input.TryDequeue(out _entry);
                return _entry;

            }
            set
            {
                input.Enqueue(value);
            }
        }

        /// <summary>
        /// Gets or sets the Labor.
        /// </summary>
        public Labor Labor { get; set; }

        /// <summary>
        /// Gets or sets the LaborerName.
        /// </summary>
        public string LaborerName { get; set; }

        /// <summary>
        /// Gets or sets the Output.
        /// </summary>
        public object Output
        {
            get
            {
                object _result = null;
                if (output.TryPick(0, out _result))
                    return _result;
                return null;
            }
            set
            {
                output.Enqueue(value);
            }
        }

        /// <summary>
        /// Gets or sets the UniqueKey.
        /// </summary>
        public ulong UniqueKey { get => SerialCode.UniqueKey; set => SerialCode.UniqueKey = value; }

        /// <summary>
        /// Gets or sets the UniqueSeed.
        /// </summary>
        public ulong UniqueSeed { get => ((IUnique)SerialCode).UniqueSeed; set => ((IUnique)SerialCode).UniqueSeed = value; }

        /// <summary>
        /// Gets or sets the Work.
        /// </summary>
        public IDeputy Work { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The AddEvoker.
        /// </summary>
        /// <param name="Recipient">The Recipient<see cref="Labor"/>.</param>
        public void AddEvoker(Labor Recipient)
        {
            EvokersIn.Add(new NoteEvoker(Labor, Recipient, new List<Labor>() { Labor }));
        }

        /// <summary>
        /// The AddEvoker.
        /// </summary>
        /// <param name="Recipient">The Recipient<see cref="Labor"/>.</param>
        /// <param name="RelationLabors">The RelationLabors<see cref="List{Labor}"/>.</param>
        public void AddEvoker(Labor Recipient, List<Labor> RelationLabors)
        {
            EvokersIn.Add(new NoteEvoker(Labor, Recipient, RelationLabors));
        }

        /// <summary>
        /// The AddEvoker.
        /// </summary>
        /// <param name="RecipientName">The RecipientName<see cref="string"/>.</param>
        public void AddEvoker(string RecipientName)
        {
            EvokersIn.Add(new NoteEvoker(Labor, RecipientName, new List<string>() { LaborerName }));
        }

        /// <summary>
        /// The AddEvoker.
        /// </summary>
        /// <param name="RecipientName">The RecipientName<see cref="string"/>.</param>
        /// <param name="RelationNames">The RelationNames<see cref="List{string}"/>.</param>
        public void AddEvoker(string RecipientName, List<string> RelationNames)
        {
            EvokersIn.Add(new NoteEvoker(Labor, RecipientName, RelationNames));
        }

        /// <summary>
        /// The CompareTo.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int CompareTo(IUnique other)
        {
            return SerialCode.CompareTo(other);
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="other">The other<see cref="IUnique"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(IUnique other)
        {
            return SerialCode.Equals(other);
        }

        /// <summary>
        /// The GetBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public byte[] GetBytes()
        {
            return SerialCode.GetBytes();
        }

        /// <summary>
        /// The GetUniqueBytes.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public byte[] GetUniqueBytes()
        {
            return SerialCode.GetUniqueBytes();
        }

        #endregion
    }
}
