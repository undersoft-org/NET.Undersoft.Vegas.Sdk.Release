/*************************************************
   Copyright (c) 2021 Undersoft

   System.Extractor.Extraction.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Extract
{
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Defines the <see cref="Extraction" />.
    /// </summary>
    public static partial class Extraction
    {
        #region Fields

        internal static readonly IExtractor _extract;
        private static AssemblyBuilder _asmBuilder;
        private static AssemblyName _asmName = new AssemblyName() { Name = "Extract" };
        private static ModuleBuilder _modBuilder;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="Extraction"/> class.
        /// </summary>
        static Extraction()
        {
            _asmBuilder = AssemblyBuilder.DefineDynamicAssembly(_asmName, AssemblyBuilderAccess.RunAndCollect);
            _modBuilder = _asmBuilder.DefineDynamicModule(_asmName.Name + ".dll");

            var typeBuilder = _modBuilder.DefineType("Extract",
                       TypeAttributes.Public
                       | TypeAttributes.AutoClass
                       | TypeAttributes.AnsiClass
                       | TypeAttributes.Class
                       | TypeAttributes.Serializable
                       | TypeAttributes.BeforeFieldInit);

            typeBuilder.AddInterfaceImplementation(typeof(IExtractor));

            CompileCopyByteArrayBlockUInt32(typeBuilder);
            CompileCopyPointerBlockUInt32(typeBuilder);

            CompileCopyByteArrayBlockUInt64(typeBuilder);
            CompileCopyPointerBlockUInt64(typeBuilder);

            CompileValueObjectToPointer(typeBuilder);
            CompileValueObjectToByteArrayRef(typeBuilder);

            CompileValueObjectToNewByteArray(typeBuilder);
            CompileValueTypeToNewByteArray(typeBuilder);
            CompileValueObjectToNewPointer(typeBuilder);

            CompilePointerToValueObject(typeBuilder);
            CompilePointerToValueType(typeBuilder);

            CompileByteArrayToValueObject(typeBuilder);
            CompileByteArrayToValueType(typeBuilder);

            TypeInfo _extractType = typeBuilder.CreateTypeInfo();
            _extract = (IExtractor)Activator.CreateInstance(_extractType);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The BytesToValueStructure.
        /// </summary>
        /// <param name="array">The array<see cref="byte[]"/>.</param>
        /// <param name="structure">The structure<see cref="ValueType"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ValueType"/>.</returns>
        public unsafe static ValueType BytesToValueStructure(byte[] array, ValueType structure, ulong offset)
        {
            _extract.BytesToValueStructure(array, ref structure, offset);
            return structure;
        }

        /// <summary>
        /// The BytesToValueStructure.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="byte[]"/>.</param>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public unsafe static object BytesToValueStructure(byte[] ptr, object structure, ulong offset)
        {
            _extract.BytesToValueStructure(ptr, ref structure, offset);
            return structure;
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="src">The src<see cref="byte*"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(byte* dest, uint destOffset, byte* src, uint srcOffset, uint count)
        {
            _extract.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte*"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="src">The src<see cref="byte*"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(byte* dest, ulong destOffset, byte* src, ulong srcOffset, ulong count)
        {
            _extract.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="uint"/>.</param>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="uint"/>.</param>
        /// <param name="count">The count<see cref="uint"/>.</param>
        public static unsafe void CopyBlock(byte[] dest, uint destOffset, byte[] src, uint srcOffset, uint count)
        {
            _extract.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        /// <summary>
        /// The CopyBlock.
        /// </summary>
        /// <param name="dest">The dest<see cref="byte[]"/>.</param>
        /// <param name="destOffset">The destOffset<see cref="ulong"/>.</param>
        /// <param name="src">The src<see cref="byte[]"/>.</param>
        /// <param name="srcOffset">The srcOffset<see cref="ulong"/>.</param>
        /// <param name="count">The count<see cref="ulong"/>.</param>
        public static unsafe void CopyBlock(byte[] dest, ulong destOffset, byte[] src, ulong srcOffset, ulong count)
        {
            _extract.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        /// <summary>
        /// The GetExtractor.
        /// </summary>
        /// <returns>The <see cref="IExtractor"/>.</returns>
        public static IExtractor GetExtractor()
        {
            return _extract;
        }

        /// <summary>
        /// The PointerToValueStructure.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="byte*"/>.</param>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public unsafe static object PointerToValueStructure(byte* ptr, object structure, ulong offset)
        {
            _extract.PointerToValueStructure(ptr, ref structure, offset);
            return structure;
        }

        /// <summary>
        /// The PointerToValueStructure.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="byte*"/>.</param>
        /// <param name="structure">The structure<see cref="ValueType"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ValueType"/>.</returns>
        public unsafe static ValueType PointerToValueStructure(byte* ptr, ValueType structure, ulong offset)
        {
            _extract.PointerToValueStructure(ptr, ref structure, offset);
            return structure;
        }

        /// <summary>
        /// The PointerToValueStructure.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="IntPtr"/>.</param>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public unsafe static object PointerToValueStructure(IntPtr ptr, object structure, ulong offset)
        {
            _extract.PointerToValueStructure((byte*)ptr.ToPointer(), ref structure, offset);
            return structure;
        }

        /// <summary>
        /// The PointerToValueStructure.
        /// </summary>
        /// <param name="ptr">The ptr<see cref="IntPtr"/>.</param>
        /// <param name="structure">The structure<see cref="ValueType"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        /// <returns>The <see cref="ValueType"/>.</returns>
        public unsafe static ValueType PointerToValueStructure(IntPtr ptr, ValueType structure, ulong offset)
        {
            _extract.PointerToValueStructure((byte*)ptr.ToPointer(), ref structure, offset);
            return structure;
        }

        /// <summary>
        /// The ValueStructureToBytes.
        /// </summary>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] ValueStructureToBytes(object structure)
        {
            return _extract.ValueStructureToBytes(structure);
        }

        /// <summary>
        /// The ValueStructureToBytes.
        /// </summary>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <param name="ptr">The ptr<see cref="byte[]"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        public unsafe static void ValueStructureToBytes(object structure, ref byte[] ptr, ulong offset)
        {
            _extract.ValueStructureToBytes(structure, ref ptr, offset);
        }

        /// <summary>
        /// The ValueStructureToBytes.
        /// </summary>
        /// <param name="structure">The structure<see cref="ValueType"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] ValueStructureToBytes(ValueType structure)
        {
            return _extract.ValueStructureToBytes(structure);
        }

        /// <summary>
        /// The ValueStructureToIntPtr.
        /// </summary>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <returns>The <see cref="IntPtr"/>.</returns>
        public static unsafe IntPtr ValueStructureToIntPtr(object structure)
        {
            return new IntPtr(_extract.ValueStructureToPointer(structure));
        }

        /// <summary>
        /// The ValueStructureToPointer.
        /// </summary>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <returns>The <see cref="byte*"/>.</returns>
        public static unsafe byte* ValueStructureToPointer(object structure)
        {
            return _extract.ValueStructureToPointer(structure);
        }

        /// <summary>
        /// The ValueStructureToPointer.
        /// </summary>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        /// <param name="ptr">The ptr<see cref="byte*"/>.</param>
        /// <param name="offset">The offset<see cref="ulong"/>.</param>
        public unsafe static void ValueStructureToPointer(object structure, byte* ptr, ulong offset)
        {
            _extract.ValueStructureToPointer(structure, ptr, offset);
        }

        #endregion
    }
}
