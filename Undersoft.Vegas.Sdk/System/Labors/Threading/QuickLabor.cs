/*************************************************
   Copyright (c) 2021 Undersoft

   QuickLabor.cs
              
   @author: Dariusz Hanc                                                  
   @date: (28.05.2021)                                            
   @licence MIT                                       
 *************************************************/

namespace System.Labors
{
    using System.Instant;
    using System.Linq;
    using System.Sets;

    /// <summary>
    /// Defines the <see cref="QuickLabor" />.
    /// </summary>
    public class QuickLabor
    {
        #region Fields

        public Laboratory Lab;
        public Laborer Laborer;
        public Subject Subject;
        public Laborator Visor;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickLabor"/> class.
        /// </summary>
        /// <param name="safeClose">The safeClose<see cref="bool"/>.</param>
        /// <param name="className">The className<see cref="string"/>.</param>
        /// <param name="methodName">The methodName<see cref="string"/>.</param>
        /// <param name="result">The result<see cref="object"/>.</param>
        /// <param name="input">The input<see cref="object[]"/>.</param>
        public QuickLabor(bool safeClose, string className, string methodName, out object result, params object[] input)
            : this(1, safeClose, Summon.New(className), methodName, input)
        {
            result = Laborer.Output;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="QuickLabor"/> class.
        /// </summary>
        /// <param name="laborersCount">The laborersCount<see cref="int"/>.</param>
        /// <param name="safeClose">The safeClose<see cref="bool"/>.</param>
        /// <param name="_methods">The _methods<see cref="IDeck{IDeputy}"/>.</param>
        public QuickLabor(int laborersCount, bool safeClose, IDeck<IDeputy> _methods)
        {
            LaborMethods _ant = new LaborMethods();
            foreach (var am in _methods)
                _ant.Put(am);
            Lab = new Laboratory(_ant);
            Lab.Scope["Primary"].LaborersCount = laborersCount;
            Lab.RunLaborators();
            Subject = Lab.Scope["Primary"];
            Visor = Subject.Visor;
            foreach (var am in _methods)
                Lab.Elaborate(am.Info.Name, am.ParameterValues);
            Subject.Visor.Close(safeClose);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="QuickLabor"/> class.
        /// </summary>
        /// <param name="laborersCount">The laborersCount<see cref="int"/>.</param>
        /// <param name="safeClose">The safeClose<see cref="bool"/>.</param>
        /// <param name="classObject">The classObject<see cref="object"/>.</param>
        /// <param name="methodName">The methodName<see cref="string"/>.</param>
        /// <param name="input">The input<see cref="object[]"/>.</param>
        public QuickLabor(int laborersCount, bool safeClose, object classObject, string methodName, params object[] input)
        {
            IDeputy am = new Deputy(classObject, methodName);
            LaborMethods _ant = new LaborMethods();
            _ant.Put(am);
            Lab = new Laboratory(_ant);
            Lab.Scope["Primary"].LaborersCount = laborersCount;
            Lab.RunLaborators();
            Subject = Lab.Scope["Primary"];
            Visor = Subject.Visor;
            Laborer = Subject.Labors.AsValues().ElementAt(0).Laborer;
            Lab.Elaborate(am.Info.Name, input);
            Subject.Visor.Close(safeClose);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="QuickLabor"/> class.
        /// </summary>
        /// <param name="laborersCount">The laborersCount<see cref="int"/>.</param>
        /// <param name="evokerCount">The evokerCount<see cref="int"/>.</param>
        /// <param name="safeClose">The safeClose<see cref="bool"/>.</param>
        /// <param name="method">The method<see cref="IDeputy"/>.</param>
        /// <param name="evoker">The evoker<see cref="IDeputy"/>.</param>
        public QuickLabor(int laborersCount, int evokerCount, bool safeClose, IDeputy method, IDeputy evoker)
        {
            LaborMethods _ant = new LaborMethods();
            _ant.Put(method);
            _ant.Put(evoker);
            Lab = new Laboratory(_ant);
            Lab.Scope["Primary"].LaborersCount = laborersCount;
            Lab.RunLaborators();
            Subject = Lab.Scope["Primary"];
            Visor = Subject.Visor;
            Laborer = Subject.Labors.AsValues().ElementAt(0).Laborer;
            Laborer.AddEvoker(Subject.Labors.AsCards().Skip(1).First().Value);
            Lab.Elaborate(method.Info.Name, method.ParameterValues);
            Subject.Visor.Close(safeClose);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="QuickLabor"/> class.
        /// </summary>
        /// <param name="classObject">The classObject<see cref="object"/>.</param>
        /// <param name="methodName">The methodName<see cref="string"/>.</param>
        /// <param name="result">The result<see cref="object"/>.</param>
        /// <param name="input">The input<see cref="object[]"/>.</param>
        public QuickLabor(object classObject, string methodName, out object result, params object[] input)
            : this(1, false, classObject, methodName, input)
        {
            result = Laborer.Input;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="QuickLabor"/> class.
        /// </summary>
        /// <param name="className">The className<see cref="string"/>.</param>
        /// <param name="methodName">The methodName<see cref="string"/>.</param>
        /// <param name="input">The input<see cref="object[]"/>.</param>
        public QuickLabor(string className, string methodName, params object[] input)
            : this(1, false, Summon.New(className), methodName, input)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Close.
        /// </summary>
        /// <param name="safeClose">The safeClose<see cref="bool"/>.</param>
        public void Close(bool safeClose = false)
        {
            Subject.Visor.Close(safeClose);
        }

        /// <summary>
        /// The Elaborate.
        /// </summary>
        public void Elaborate()
        {
            Visor.Elaborate(this.Laborer);
        }

        /// <summary>
        /// The Elaborate.
        /// </summary>
        /// <param name="input">The input<see cref="object[]"/>.</param>
        public void Elaborate(params object[] input)
        {
            this.Laborer.Input = input;
            Visor.Elaborate(this.Laborer);
        }

        #endregion
    }
}
