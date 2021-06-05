/*************************************************
   Copyright (c) 2021 Undersoft

   NoteTopic.cs
              
   @author: Dariusz Hanc                                                  
   @date: (28.05.2021)                                            
   @licence MIT                                       
 *************************************************/

namespace System.Labors
{
    using System.Collections.Generic;
    using System.Sets;

    /// <summary>
    /// Defines the <see cref="NoteTopic" />.
    /// </summary>
    public class NoteTopic : Catalog<Note>
    {
        #region Fields

        /// <summary>
        /// Defines the RecipientBox.
        /// </summary>
        public NoteBox RecipientBox;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteTopic"/> class.
        /// </summary>
        /// <param name="senderName">The senderName<see cref="string"/>.</param>
        /// <param name="notelist">The notelist<see cref="IList{Note}"/>.</param>
        /// <param name="recipient">The recipient<see cref="NoteBox"/>.</param>
        public NoteTopic(string senderName, IList<Note> notelist, NoteBox recipient = null)
        {
            if (recipient != null)
            {
                RecipientBox = recipient;
            }
            if (notelist != null && notelist.Count > 0)
            {
                foreach (Note evocation in notelist)
                {
                    evocation.SenderName = SenderName;
                    Notes = evocation;
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteTopic"/> class.
        /// </summary>
        /// <param name="senderName">The senderName<see cref="string"/>.</param>
        /// <param name="note">The note<see cref="Note"/>.</param>
        /// <param name="recipient">The recipient<see cref="NoteBox"/>.</param>
        public NoteTopic(string senderName, Note note, NoteBox recipient = null)
        {
            if (recipient != null)
            {
                RecipientBox = recipient;
            }
            SenderName = senderName;
            Notes = note;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteTopic"/> class.
        /// </summary>
        /// <param name="senderName">The senderName<see cref="string"/>.</param>
        /// <param name="recipient">The recipient<see cref="NoteBox"/>.</param>
        public NoteTopic(string senderName, NoteBox recipient = null)
        {
            if (recipient != null)
                RecipientBox = recipient;
            SenderName = senderName;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteTopic"/> class.
        /// </summary>
        /// <param name="senderName">The senderName<see cref="string"/>.</param>
        /// <param name="recipient">The recipient<see cref="NoteBox"/>.</param>
        /// <param name="parameters">The parameters<see cref="object[]"/>.</param>
        public NoteTopic(string senderName, NoteBox recipient = null, params object[] parameters)
        {
            if (recipient != null)
                RecipientBox = recipient;
            SenderName = senderName;
            if (parameters != null)
            {
                if (parameters[0].GetType() == typeof(Dictionary<string, object>))
                {
                    Note result = new Note(senderName, parameters);
                    Notes = result;
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Notes.
        /// </summary>
        public Note Notes
        {
            get
            {
                Note _result = null;
                TryDequeue(out _result);
                return _result;
            }
            set
            {
                value.SenderName = SenderName;
                Enqueue(DateTime.Now.ToBinary(), value);
                if (RecipientBox != null)
                    RecipientBox.QualifyToEvoke();
            }
        }

        /// <summary>
        /// Gets or sets the SenderName.
        /// </summary>
        public string SenderName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The AddNote.
        /// </summary>
        /// <param name="noteList">The noteList<see cref="IList{Note}"/>.</param>
        public void AddNote(IList<Note> noteList)
        {
            foreach (Note result in noteList)
                Notes = result;
        }

        /// <summary>
        /// The AddNote.
        /// </summary>
        /// <param name="note">The note<see cref="Note"/>.</param>
        public void AddNote(Note note)
        {
            Notes = note;
        }

        /// <summary>
        /// The AddNote.
        /// </summary>
        /// <param name="parameters">The parameters<see cref="object[]"/>.</param>
        public void AddNote(params object[] parameters)
        {
            if (parameters != null)
            {
                Note result = new Note(SenderName);
                result.Parameters = parameters;
                Notes = result;
            }
        }

        /// <summary>
        /// The AddNote.
        /// </summary>
        /// <param name="senderName">The senderName<see cref="string"/>.</param>
        /// <param name="parameters">The parameters<see cref="object[]"/>.</param>
        public void AddNote(string senderName, params object[] parameters)
        {
            SenderName = senderName;
            if (parameters != null)
            {
                Note result = new Note(senderName);
                result.Parameters = parameters;
                Notes = result;
            }
        }

        #endregion
    }
}
