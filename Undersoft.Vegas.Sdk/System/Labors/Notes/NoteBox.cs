/*************************************************
   Copyright (c) 2021 Undersoft

   NoteBox.cs
              
   @author: Dariusz Hanc                                                  
   @date: (28.05.2021)                                            
   @licence MIT                                       
 *************************************************/

namespace System.Labors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Sets;

    /// <summary>
    /// Defines the <see cref="NoteBox" />.
    /// </summary>
    public class NoteBox : Board<NoteTopic>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteBox"/> class.
        /// </summary>
        /// <param name="Recipient">The Recipient<see cref="string"/>.</param>
        public NoteBox(string Recipient)
        {
            RecipientName = Recipient;
            Evokers = new NoteEvokers();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Evokers.
        /// </summary>
        public NoteEvokers Evokers { get; set; }

        /// <summary>
        /// Gets or sets the Labor.
        /// </summary>
        public Labor Labor { get; set; }

        /// <summary>
        /// Gets or sets the RecipientName.
        /// </summary>
        public string RecipientName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The AddNote.
        /// </summary>
        /// <param name="value">The value<see cref="IList{Note}"/>.</param>
        public void AddNote(IList<Note> value)
        {
            if (value != null && value.Any())
            {
                foreach (Note antio in value)
                {
                    NoteTopic queue = null;
                    if (antio.SenderName != null)
                    {
                        if (!ContainsKey(antio.SenderName))
                        {
                            queue = new NoteTopic(antio.SenderName, this);
                            if (Add(antio.SenderName, queue))
                            {
                                if (antio.EvokerOut != null)
                                    Evokers.Add(antio.EvokerOut);
                                queue.AddNote(antio);
                            }
                        }
                        else if (TryGet(antio.SenderName, out queue))
                        {
                            if (value != null && value.Count > 0)
                            {
                                if (antio.EvokerOut != null)
                                    Evokers.Add(antio.EvokerOut);
                                queue.AddNote(antio);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The AddNote.
        /// </summary>
        /// <param name="value">The value<see cref="Note"/>.</param>
        public void AddNote(Note value)
        {
            if (value.SenderName != null)
            {
                NoteTopic queue = null;
                if (!ContainsKey(value.SenderName))
                {
                    queue = new NoteTopic(value.SenderName, this);
                    if (Add(value.SenderName, queue))
                    {
                        if (value.EvokerOut != null)
                            Evokers.Add(value.EvokerOut);
                        queue.AddNote(value);
                    }
                }
                else if (TryGet(value.SenderName, out queue))
                {
                    if (value.EvokerOut != null)
                        Evokers.Add(value.EvokerOut);
                    queue.AddNote(value);
                }
            }
        }

        /// <summary>
        /// The AddNote.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="List{Note}"/>.</param>
        public void AddNote(string key, List<Note> value)
        {
            NoteTopic queue = null;
            if (!ContainsKey(key))
            {
                queue = new NoteTopic(key, this);
                if (Add(key, queue) && value != null && value.Count > 0)
                {
                    foreach (Note notes in value)
                    {
                        if (notes.EvokerOut != null)
                            Evokers.Add(notes.EvokerOut);
                        notes.SenderName = key;
                        queue.AddNote(notes);
                    }
                }
            }
            else if (TryGet(key, out queue))
            {
                if (value != null && value.Count > 0)
                {
                    foreach (Note notes in value)
                    {
                        if (notes.EvokerOut != null)
                            Evokers.Add(notes.EvokerOut);
                        notes.SenderName = key;
                        queue.AddNote(notes);
                    }
                }
            }
        }

        /// <summary>
        /// The AddNote.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="Note"/>.</param>
        public void AddNote(string key, Note value)
        {
            value.SenderName = key;
            NoteTopic queue = null;
            if (!ContainsKey(key))
            {
                queue = new NoteTopic(key, this);
                if (Add(key, queue))
                {
                    if (value.EvokerOut != null)
                        Evokers.Add(value.EvokerOut);
                    queue.AddNote(value);
                }
            }
            else if (TryGet(key, out queue))
            {
                if (value.EvokerOut != null)
                    Evokers.Add(value.EvokerOut);
                queue.AddNote(value);
            }
        }

        /// <summary>
        /// The AddNote.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="ioqueues">The ioqueues<see cref="object"/>.</param>
        public void AddNote(string key, object ioqueues)
        {
            NoteTopic queue = null;
            if (!ContainsKey(key))
            {
                queue = new NoteTopic(key, this);
                if (Add(key, queue) && ioqueues != null)
                {
                    queue.AddNote(ioqueues);
                }
            }
            else if (TryGet(key, out queue))
            {
                if (ioqueues != null)
                {
                    queue.AddNote(ioqueues);
                }
            }
        }

        /// <summary>
        /// The GetNote.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <returns>The <see cref="Note"/>.</returns>
        public Note GetNote(string key)
        {
            NoteTopic _ioqueue = null;
            if (TryGet(key, out _ioqueue))
                return _ioqueue.Dequeue();
            return null;
        }

        /// <summary>
        /// The GetParams.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <returns>The <see cref="object[]"/>.</returns>
        public object[] GetParams(string key)
        {
            NoteTopic _ioqueue = null;
            Note temp = null;
            if (TryGet(key, out _ioqueue))
                if (_ioqueue.TryDequeue(out temp))
                    return temp.Parameters;
            return null;
        }

        /// <summary>
        /// The MeetsRequirements.
        /// </summary>
        /// <param name="keys">The keys<see cref="List{string}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool MeetsRequirements(List<string> keys)
        {
            return this.AsCards().Where(q => keys.Contains(q.Value.SenderName)).All(v => v.Value.Count > 0);
        }

        /// <summary>
        /// The QualifyToEvoke.
        /// </summary>
        public void QualifyToEvoke()
        {
            List<NoteEvoker> toEvoke = new List<NoteEvoker>();
            foreach (NoteEvoker relay in Evokers.AsValues())
            {
                if (relay.RelationNames.All(r => ContainsKey(r)))
                    if (relay.RelationNames.All(r => this[r].AsValues().Any()))
                    {
                        toEvoke.Add(relay);
                    }
            }

            if (toEvoke.Any())
            {
                foreach (NoteEvoker evoke in toEvoke)
                {
                    if (MeetsRequirements(evoke.RelationNames))
                    {
                        List<Note> antios = TakeOut(evoke.RelationNames);

                        if (antios.All(a => a != null))
                        {
                            object[] parameters = new object[0];
                            object begin = Labor.Laborer.Input;
                            if (begin != null)
                                parameters = parameters.Concat((object[])begin).ToArray();
                            foreach (Note antio in antios)
                            {
                                if (antio.Parameters.GetType().IsArray)
                                    parameters = parameters.Concat(antio.Parameters.SelectMany(a => (object[])a).ToArray()).ToArray();
                                else
                                    parameters = parameters.Concat(antio.Parameters).ToArray();
                            }

                            Labor.Execute(parameters);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The TakeOut.
        /// </summary>
        /// <param name="keys">The keys<see cref="List{string}"/>.</param>
        /// <returns>The <see cref="List{Note}"/>.</returns>
        public List<Note> TakeOut(List<string> keys)
        {
            List<Note> antios = this.AsCards().Where(q => keys.Contains(q.Value.SenderName)).Select(v => v.Value.Notes).ToList();
            return antios;
        }

        #endregion
    }
}
