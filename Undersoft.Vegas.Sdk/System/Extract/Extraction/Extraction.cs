using System.Reflection.Emit;
using System.Reflection;

namespace System.Extract
{
    public static partial class Extraction
    {
        static readonly IExtractor _extract;

        private static AssemblyName _asmName = new AssemblyName() { Name = "Extract" };
        private static ModuleBuilder _modBuilder;
        private static AssemblyBuilder _asmBuilder;

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

        public static IExtractor GetExtractor()
        {
            return _extract;
        }

        public static unsafe void CopyBlock(byte[] dest, uint destOffset, byte[] src, uint srcOffset, uint count)
        {
            _extract.CopyBlock(dest, destOffset, src, srcOffset, count);
        }         
        public static unsafe void CopyBlock(byte* dest, uint destOffset, byte* src, uint srcOffset, uint count)
        {
            _extract.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        public static unsafe void CopyBlock(byte[] dest, ulong destOffset, byte[] src, ulong srcOffset, ulong count)
        {
            _extract.CopyBlock(dest, destOffset, src, srcOffset, count);
        }
        public static unsafe void CopyBlock(byte* dest, ulong destOffset, byte* src, ulong srcOffset, ulong count)
        {
            _extract.CopyBlock(dest, destOffset, src, srcOffset, count);
        }

        public unsafe static void ValueStructureToPointer(object structure, byte* ptr, ulong offset)
        {
            _extract.ValueStructureToPointer(structure, ptr, offset);
        }
        public unsafe static void ValueStructureToBytes(object structure, ref byte[] ptr, ulong offset)
        {
            _extract.ValueStructureToBytes(structure, ref ptr, offset);
        }

        public static byte[] ValueStructureToBytes(ValueType structure)
        {
            return _extract.ValueStructureToBytes(structure);
        }
        public static byte[] ValueStructureToBytes(object structure)
        {
            return _extract.ValueStructureToBytes(structure);
        }
        public static unsafe byte* ValueStructureToPointer(object structure)
        {
            return _extract.ValueStructureToPointer(structure);
        }
        public static unsafe IntPtr ValueStructureToIntPtr(object structure)
        {
            return new IntPtr(_extract.ValueStructureToPointer(structure));
        }

        public unsafe static object PointerToValueStructure(byte* ptr, object structure, ulong offset)
        {
            _extract.PointerToValueStructure(ptr, ref structure, offset);
            return structure;
        }
        public unsafe static ValueType PointerToValueStructure(byte* ptr, ValueType structure, ulong offset)
        {
            _extract.PointerToValueStructure(ptr, ref structure, offset);
            return structure;
        }

        public unsafe static object PointerToValueStructure(IntPtr ptr, object structure, ulong offset)
        {
            _extract.PointerToValueStructure((byte*)ptr.ToPointer(), ref structure, offset);
            return structure;
        }
        public unsafe static ValueType PointerToValueStructure(IntPtr ptr, ValueType structure, ulong offset)
        {
            _extract.PointerToValueStructure((byte*)ptr.ToPointer(), ref structure, offset);
            return structure;
        }

        public unsafe static object BytesToValueStructure(byte[] ptr, object structure, ulong offset)
        {
            _extract.BytesToValueStructure(ptr, ref structure, offset);
            return structure;
        }
        public unsafe static ValueType BytesToValueStructure(byte[] array, ValueType structure, ulong offset)
        {
            _extract.BytesToValueStructure(array, ref structure, offset);
            return structure;
        }


    }
}
