/*************************************************
   Copyright (c) 2021 Undersoft

   System.ISerialBlock.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

namespace System
{
    /// <summary>
    /// Defines the <see cref="ISerialBuffer" />.
    /// </summary>
    public interface ISerialBuffer : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the BlockOffset.
        /// </summary>
        int BlockOffset { get; set; }

        /// <summary>
        /// Gets or sets the BlockSize.
        /// </summary>
        long BlockSize { get; set; }

        /// <summary>
        /// Gets the DeserialBlock.
        /// </summary>
        byte[] DeserialBlock { get; }

        /// <summary>
        /// Gets or sets the DeserialBlockId.
        /// </summary>
        int DeserialBlockId { get; set; }

        /// <summary>
        /// Gets the DeserialBlockPtr.
        /// </summary>
        IntPtr DeserialBlockPtr { get; }

        /// <summary>
        /// Gets or sets the SerialBlock.
        /// </summary>
        byte[] SerialBlock { get; set; }

        /// <summary>
        /// Gets or sets the SerialBlockId.
        /// </summary>
        int SerialBlockId { get; set; }

        /// <summary>
        /// Gets the SerialBlockPtr.
        /// </summary>
        IntPtr SerialBlockPtr { get; }

        /// <summary>
        /// Gets the Site.
        /// </summary>
        ServiceSite Site { get; }

        #endregion
    }
}
