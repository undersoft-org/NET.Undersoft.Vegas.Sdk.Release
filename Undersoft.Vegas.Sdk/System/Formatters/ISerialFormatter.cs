/*************************************************
   Copyright (c) 2021 Undersoft

   System.ISerialFormatter.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    using System.IO;

    /// <summary>
    /// Defines the <see cref="ISerialFormatter" />.
    /// </summary>
    public interface ISerialFormatter
    {
        #region Properties

        /// <summary>
        /// Gets or sets the DeserialCount.
        /// </summary>
        int DeserialCount { get; set; }

        /// <summary>
        /// Gets the ItemsCount.
        /// </summary>
        int ItemsCount { get; }

        /// <summary>
        /// Gets or sets the ProgressCount.
        /// </summary>
        int ProgressCount { get; set; }

        /// <summary>
        /// Gets or sets the SerialCount.
        /// </summary>
        int SerialCount { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The Deserialize.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="ISerialBuffer"/>.</param>
        /// <param name="serialFormat">The serialFormat<see cref="SerialFormat"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        object Deserialize(ISerialBuffer buffer, SerialFormat serialFormat = SerialFormat.Binary);

        /// <summary>
        /// The Deserialize.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="serialFormat">The serialFormat<see cref="SerialFormat"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        object Deserialize(Stream stream, SerialFormat serialFormat = SerialFormat.Binary);

        /// <summary>
        /// The GetHeader.
        /// </summary>
        /// <returns>The <see cref="object"/>.</returns>
        object GetHeader();

        /// <summary>
        /// The GetMessage.
        /// </summary>
        /// <returns>The <see cref="object[]"/>.</returns>
        object[] GetMessage();

        /// <summary>
        /// The Serialize.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="ISerialBuffer"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <param name="serialFormat">The serialFormat<see cref="SerialFormat"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        int Serialize(ISerialBuffer buffer, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary);

        /// <summary>
        /// The Serialize.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="offset">The offset<see cref="int"/>.</param>
        /// <param name="batchSize">The batchSize<see cref="int"/>.</param>
        /// <param name="serialFormat">The serialFormat<see cref="SerialFormat"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        int Serialize(Stream stream, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary);

        #endregion
    }
}
