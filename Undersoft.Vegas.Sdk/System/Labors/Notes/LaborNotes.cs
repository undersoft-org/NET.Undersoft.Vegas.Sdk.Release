/*************************************************
   Copyright (c) 2021 Undersoft

   LaborNotes.cs
              
   @author: Dariusz Hanc                                                  
   @date: (28.05.2021)                                            
   @licence MIT                                       
 *************************************************/

namespace System.Labors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Sets;

    #region Enums

    /// <summary>
    /// Defines the EvokerType.
    /// </summary>
    public enum EvokerType
    {
        /// <summary>
        /// Defines the Always.
        /// </summary>
        Always,
        /// <summary>
        /// Defines the Single.
        /// </summary>
        Single,
        /// <summary>
        /// Defines the Schedule.
        /// </summary>
        Schedule,
        /// <summary>
        /// Defines the Nome.
        /// </summary>
        Nome
    }

    #endregion

    /// <summary>
    /// Defines the <see cref="LaborNotes" />.
    /// </summary>
    public class LaborNotes : Catalog<NoteBox>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Scope.
        /// </summary>
        private Scope Scope { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The AddRecipient.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="noteBox">The noteBox<see cref="NoteBox"/>.</param>
        public void AddRecipient(string key, NoteBox noteBox)
        {
            if (noteBox != null)
            {
                if (noteBox.Labor != null)
                {
                    Labor objv = noteBox.Labor;
                    Put(noteBox.RecipientName, noteBox);
                }
                else
                {
                    List<Labor> objvl = Scope.Subjects.AsCards().Where(m => m.Value.Labors.ContainsKey(key)).SelectMany(os => os.Value.Labors.AsCards().Select(o => o.Value)).ToList();
                    if (objvl.Any())
                    {
                        Labor objv = objvl.First();
                        noteBox.Labor = objv;
                        Put(key, noteBox);
                    }
                }
            }
            else
            {
                List<Labor> objvl = Scope.Subjects.AsCards().Where(m => m.Value.Labors.ContainsKey(key)).SelectMany(os => os.Value.Labors.AsCards().Select(o => o.Value)).ToList();
                if (objvl.Any())
                {
                    Labor objv = objvl.First();
                    NoteBox iobox = new NoteBox(objv.Laborer.LaborerName);
                    iobox.Labor = objv;
                    Put(key, iobox);
                }
            }
        }

        /// <summary>
        /// The Send.
        /// </summary>
        /// <param name="parametersList">The parametersList<see cref="IList{Note}"/>.</param>
        public void Send(IList<Note> parametersList)
        {
            foreach (Note parameters in parametersList)
            {
                if (parameters.RecipientName != null && parameters.SenderName != null)
                {
                    if (ContainsKey(parameters.RecipientName))
                    {
                        NoteBox iobox = Get(parameters.RecipientName);
                        if (iobox != null)
                            iobox.AddNote(parameters);
                    }
                    else if (parameters.Recipient != null)
                    {
                        Labor objv = parameters.Recipient;
                        NoteBox iobox = new NoteBox(objv.Laborer.LaborerName);
                        iobox.Labor = objv;
                        iobox.AddNote(parameters);
                        SetRecipient(iobox);
                    }
                    else if (Scope != null)
                    {
                        List<Labor> objvl = Scope.Subjects.AsCards()
                                                .Where(m => m.Value.Labors.ContainsKey(parameters.RecipientName))
                                                    .SelectMany(os => os.Value.Labors.AsCards().Select(o => o.Value)).ToList();
                        if (objvl.Any())
                        {
                            Labor objv = objvl.First();
                            NoteBox iobox = new NoteBox(objv.Laborer.LaborerName);
                            iobox.Labor = objv;
                            iobox.AddNote(parameters);
                            SetRecipient(iobox);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The Send.
        /// </summary>
        /// <param name="parameters">The parameters<see cref="Note"/>.</param>
        public void Send(Note parameters)
        {
            if (parameters.RecipientName != null && parameters.SenderName != null)
            {
                if (ContainsKey(parameters.RecipientName))
                {
                    NoteBox iobox = Get(parameters.RecipientName);
                    if (iobox != null)
                        iobox.AddNote(parameters);
                }
                else if (parameters.Recipient != null)
                {
                    Labor objv = parameters.Recipient;
                    NoteBox iobox = new NoteBox(objv.Laborer.LaborerName);
                    iobox.Labor = objv;
                    iobox.AddNote(parameters);
                    SetRecipient(iobox);
                }
                else if (Scope != null)
                {
                    List<Labor> objvl = Scope.Subjects.AsCards().Where(m => m.Value.Labors.ContainsKey(parameters.RecipientName)).SelectMany(os => os.Value.Labors.AsCards().Select(o => o.Value)).ToList();
                    if (objvl.Any())
                    {
                        Labor objv = objvl.First();
                        NoteBox iobox = new NoteBox(objv.Laborer.LaborerName);
                        iobox.Labor = objv;
                        iobox.AddNote(parameters);
                        SetRecipient(iobox);
                    }
                }
            }
        }

        /// <summary>
        /// The SetRecipient.
        /// </summary>
        /// <param name="value">The value<see cref="NoteBox"/>.</param>
        public void SetRecipient(NoteBox value)
        {
            if (value != null)
            {
                if (value.Labor != null)
                {
                    Labor objv = value.Labor;
                    Put(value.RecipientName, value);
                }
                else
                {
                    List<Labor> objvl = Scope.Subjects.AsCards().Where(m => m.Value.Labors.ContainsKey(value.RecipientName)).SelectMany(os => os.Value.Labors.AsCards().Select(o => o.Value)).ToList();
                    if (objvl.Any())
                    {
                        Labor objv = objvl.First();
                        value.Labor = objv;
                        Put(value.RecipientName, value);
                    }
                }
            }
        }

        #endregion
    }
}
