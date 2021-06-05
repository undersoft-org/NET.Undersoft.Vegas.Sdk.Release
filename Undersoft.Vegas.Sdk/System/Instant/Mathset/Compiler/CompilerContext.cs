/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Mathset.CompilerContext.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant.Mathset
{
    using System;
    using System.Reflection.Emit;

    #region Delegates

    /// <summary>
    /// The Evaluator.
    /// </summary>
    public delegate void Evaluator();

    #endregion

    /// <summary>
    /// Defines the <see cref="CompilerContext" />.
    /// </summary>
    [Serializable]
    public class CompilerContext
    {
        #region Fields

        [NonSerialized] internal int indexVariableCount;
        [NonSerialized] internal int[] indexVariables;
        [NonSerialized] internal int paramCount;
        [NonSerialized] internal IFigures[] paramTables = new IFigures[10];
        [NonSerialized] internal int pass = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilerContext"/> class.
        /// </summary>
        public CompilerContext()
        {
            indexVariableCount = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Count.
        /// </summary>
        public int Count
        {
            get { return paramCount; }
        }

        /// <summary>
        /// Gets the ParamCards.
        /// </summary>
        public IFigures[] ParamCards
        {
            get { return paramTables; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The GenLocalLoad.
        /// </summary>
        /// <param name="g">The g<see cref="ILGenerator"/>.</param>
        /// <param name="a">The a<see cref="int"/>.</param>
        public static void GenLocalLoad(ILGenerator g, int a)
        {
            switch (a)
            {
                case 0: g.Emit(OpCodes.Ldloc_0); break;
                case 1: g.Emit(OpCodes.Ldloc_1); break;
                case 2: g.Emit(OpCodes.Ldloc_2); break;
                case 3: g.Emit(OpCodes.Ldloc_3); break;
                default:
                    g.Emit(OpCodes.Ldloc, a);
                    break;
            }
        }

        /// <summary>
        /// The GenLocalStore.
        /// </summary>
        /// <param name="g">The g<see cref="ILGenerator"/>.</param>
        /// <param name="a">The a<see cref="int"/>.</param>
        public static void GenLocalStore(ILGenerator g, int a)
        {
            switch (a)
            {
                case 0: g.Emit(OpCodes.Stloc_0); break;
                case 1: g.Emit(OpCodes.Stloc_1); break;
                case 2: g.Emit(OpCodes.Stloc_2); break;
                case 3: g.Emit(OpCodes.Stloc_3); break;
                default:
                    g.Emit(OpCodes.Stloc, a);
                    break;
            }
        }

        /// <summary>
        /// The Add.
        /// </summary>
        /// <param name="v">The v<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int Add(IFigures v)
        {
            int index = GetIndexOf(v);
            if (index < 0)
            {
                paramTables[paramCount] = v;
                return indexVariableCount + paramCount++;
            }
            return index;
        }

        /// <summary>
        /// The AllocIndexVariable.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public int AllocIndexVariable()
        {
            return indexVariableCount++;
        }

        /// <summary>
        /// The GenerateLocalInit.
        /// </summary>
        /// <param name="g">The g<see cref="ILGenerator"/>.</param>
        public void GenerateLocalInit(ILGenerator g)
        {
            // declare indexes
            for (int i = 0; i < indexVariableCount; i++)
                g.DeclareLocal(typeof(int));

            // declare parameters
            string paramFieldName = "DataParameters";

            for (int i = 0; i < paramCount; i++)
                g.DeclareLocal(typeof(IFigures));

            for (int i = 0; i < paramCount; i++)
                g.DeclareLocal(typeof(IFigure));

            g.DeclareLocal(typeof(double));

            // load the parameters from parameters array
            for (int i = 0; i < paramCount; i++)
            {
                // simple this.paramTables[i]
                g.Emit(OpCodes.Ldarg_0); //this
                g.Emit(OpCodes.Ldfld, typeof(CombinedMathset).GetField(paramFieldName));
                g.Emit(OpCodes.Ldc_I4, i);
                g.Emit(OpCodes.Ldelem_Ref);
                g.Emit(OpCodes.Stloc, indexVariableCount + i);
            }
        }

        /// <summary>
        /// The GetBufforIndexOf.
        /// </summary>
        /// <param name="v">The v<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetBufforIndexOf(IFigures v)
        {
            for (int i = 0; i < paramCount; i++)
                if (paramTables[i] == v) return indexVariableCount + i + paramCount + 1;
            return -1;
        }

        /// <summary>
        /// The GetIndexOf.
        /// </summary>
        /// <param name="v">The v<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetIndexOf(IFigures v)
        {
            for (int i = 0; i < paramCount; i++)
                if (paramTables[i] == v) return indexVariableCount + i;
            return -1;
        }

        // index access by variable number
        /// <summary>
        /// The GetIndexVariable.
        /// </summary>
        /// <param name="number">The number<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetIndexVariable(int number)
        {
            return indexVariables[number];
        }

        /// <summary>
        /// The GetSubIndexOf.
        /// </summary>
        /// <param name="v">The v<see cref="IFigures"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetSubIndexOf(IFigures v)
        {
            for (int i = 0; i < paramCount; i++)
                if (paramTables[i] == v) return indexVariableCount + i + paramCount;
            return -1;
        }

        /// <summary>
        /// The IsFirstPass.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsFirstPass()
        {
            return pass == 0;
        }

        /// <summary>
        /// The NextPass.
        /// </summary>
        public void NextPass()
        {
            pass++;
            // local variables array
            indexVariables = new int[indexVariableCount];
            for (int i = 0; i < indexVariableCount; i++)
                indexVariables[i] = i;
        }

        /// <summary>
        /// The SetIndexVariable.
        /// </summary>
        /// <param name="number">The number<see cref="int"/>.</param>
        /// <param name="value">The value<see cref="int"/>.</param>
        public void SetIndexVariable(int number, int value)
        {
            indexVariables[number] = value;
        }

        #endregion
    }
}
