/*************************************************
   Copyright (c) 2021 Undersoft

   System.Extract.Stock.StockArrayHandle.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System.Extract.Stock
{
    using System;
    using System.Extract;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="StockArrayHandle" />.
    /// </summary>
    public class StockArrayHandle
    {
        #region Fields

        public int sizeStruct = 0;
        public Type typeStruct = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StockArrayHandle"/> class.
        /// </summary>
        /// <param name="t">The t<see cref="Type"/>.</param>
        public StockArrayHandle(Type t)
        {
            typeStruct = t;
            sizeStruct = Marshal.SizeOf(t);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The GetPtr.
        /// </summary>
        /// <param name="structure">The structure<see cref="object[]"/>.</param>
        /// <returns>The <see cref="void*"/>.</returns>
        public unsafe void* GetPtr(object[] structure)
        {
            Type gg = structure.GetType();
            GCHandle pinn = GCHandle.Alloc(structure, GCHandleType.Pinned);
            IntPtr address = Marshal.UnsafeAddrOfPinnedArrayElement(structure, 0);
            return address.ToPointer();
        }

        /// <summary>
        /// The PtrToStructure.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public object PtrToStructure(IntPtr pointer)
        {
            return Extractor.PointerToStructure(pointer, typeStruct, 0);
        }

        /// <summary>
        /// The PtrToStructure.
        /// </summary>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        /// <param name="structure">The structure<see cref="object"/>.</param>
        public void PtrToStructure(IntPtr pointer, object structure)
        {
            if (structure != null)
                Extractor.PointerToStructure(pointer, structure);
            else
                structure = Extractor.PointerToStructure(pointer, typeStruct, 0);
        }

        /// <summary>
        /// The ReadArray.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="object[]"/>.</param>
        /// <param name="source">The source<see cref="byte*"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        public unsafe void ReadArray(ref object[] buffer, byte* source, int index, int count)
        {
            if (index < 0) index = 0;
            int bufferIndex = index;
            if (buffer == null) { buffer = new object[count]; bufferIndex = 0; }
            else if (buffer.Length - index < count) { count = buffer.Length - index; }
            if (count < 0) throw new ArgumentOutOfRangeException("count");

            int offset = index * sizeStruct;
            for (int i = 0; i < count; i++)
            {
                int arrayoffset = i + bufferIndex;
                if (buffer[arrayoffset] != null)
                    Extractor.PointerToStructure(source + ((i * sizeStruct) + offset), buffer[arrayoffset]);
                else
                    buffer[arrayoffset] = Extractor.PointerToStructure(source, typeStruct, ((i * sizeStruct) + offset));
            }
        }

        /// <summary>
        /// The ReadArray.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="object[]"/>.</param>
        /// <param name="destIndex">The destIndex<see cref="int"/>.</param>
        /// <param name="source">The source<see cref="byte*"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        public unsafe void ReadArray(ref object[] buffer, int destIndex, byte* source, int index, int count)
        {
            if (index < 0) index = 0;
            if (buffer == null) { buffer = new object[count]; destIndex = 0; }
            else if (buffer.Length - destIndex - index < count)
                count = buffer.Length - destIndex - index;
            if (count < 0) throw new ArgumentOutOfRangeException("count");

            int offset = index * sizeStruct;

            for (int i = 0; i < count; i++)
            {
                int arrayoffset = i + destIndex;
                if (buffer[arrayoffset] != null)
                    Extractor.PointerToStructure(source + ((i * sizeStruct) + offset), buffer[arrayoffset]);
                else
                    buffer[arrayoffset] = Extractor.PointerToStructure(source, typeStruct, ((i * sizeStruct) + offset));
            }
        }

        /// <summary>
        /// The ReadArray.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="object[]"/>.</param>
        /// <param name="source">The source<see cref="IntPtr"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        public unsafe void ReadArray(ref object[] buffer, IntPtr source, int index, int count)
        {
            if (index < 0) index = 0;
            int bufferIndex = index;
            if (buffer == null) { buffer = new object[count]; bufferIndex = 0; }
            else if (buffer.Length - index < count) { count = buffer.Length - index; }
            if (count < 0) throw new ArgumentOutOfRangeException("count");

            int offset = index * sizeStruct;

            for (int i = 0; i < count; i++)
            {
                int arrayoffset = i + bufferIndex;
                if (buffer[arrayoffset] != null)
                    Extractor.PointerToStructure(source + ((i * sizeStruct) + offset), buffer[arrayoffset]);
                else
                    buffer[arrayoffset] = Extractor.PointerToStructure(source, typeStruct, ((i * sizeStruct) + offset));
            }
        }

        /// <summary>
        /// The SizeOf.
        /// </summary>
        /// <param name="t">The t<see cref="object"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int SizeOf(object t)
        {
            return sizeStruct;
        }

        /// <summary>
        /// The StructureToPtr.
        /// </summary>
        /// <param name="s">The s<see cref="object"/>.</param>
        /// <param name="pointer">The pointer<see cref="IntPtr"/>.</param>
        public unsafe void StructureToPtr(object s, IntPtr pointer)
        {
            s.StructureTo((byte*)pointer);
        }

        /// <summary>
        /// The WriteArray.
        /// </summary>
        /// <param name="destination">The destination<see cref="byte*"/>.</param>
        /// <param name="srcIndex">The srcIndex<see cref="int"/>.</param>
        /// <param name="buffer">The buffer<see cref="object[]"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        public unsafe void WriteArray(byte* destination, int srcIndex, ref object[] buffer, int index, int count)
        {
            if (index < 0) index = 0;
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (count < 0) throw new ArgumentOutOfRangeException("count");
            if (buffer.Length - index < count) count = buffer.Length - index;

            int offset = index * sizeStruct;

            for (int i = 0; i < count; i++)
            {
                int arrayoffset = i + srcIndex;
                Extractor.StructureToPointer(buffer[arrayoffset], destination + ((i * sizeStruct) + offset));
            }
        }

        /// <summary>
        /// The WriteArray.
        /// </summary>
        /// <param name="destination">The destination<see cref="byte*"/>.</param>
        /// <param name="buffer">The buffer<see cref="object[]"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        public unsafe void WriteArray(byte* destination, ref object[] buffer, int index, int count)
        {

            if (index < 0) index = 0;
            int bufferIndex = index;
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (count < 0) throw new ArgumentOutOfRangeException("count");
            if (buffer.Length - index < count) count = buffer.Length - index;

            int offset = index * sizeStruct;

            for (int i = 0; i < count; i++)
            {
                int arrayoffset = i + bufferIndex;
                Extractor.StructureToPointer(buffer[arrayoffset], destination + ((i * sizeStruct) + offset));
            }
        }

        /// <summary>
        /// The WriteArray.
        /// </summary>
        /// <param name="destination">The destination<see cref="IntPtr"/>.</param>
        /// <param name="buffer">The buffer<see cref="object[]"/>.</param>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <param name="count">The count<see cref="int"/>.</param>
        public unsafe void WriteArray(IntPtr destination, ref object[] buffer, int index, int count)
        {
            if (index < 0) index = 0;
            int bufferIndex = index;
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (count < 0) throw new ArgumentOutOfRangeException("count");
            if (buffer.Length - index < count) count = buffer.Length - index;

            int offset = index * sizeStruct;

            for (int i = 0; i < count; i++)
            {
                int arrayoffset = i + bufferIndex;
                Extractor.StructureToPointer(buffer[arrayoffset], destination + ((i * sizeStruct) + offset));
            }
        }

        #endregion
    }
}
