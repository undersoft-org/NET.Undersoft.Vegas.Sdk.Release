/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.FigureSleeves.cs
   
   @project: Undersoft.Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (31.05.2021) 
   @licence MIT
 *************************************************/

namespace System.Instant
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Instant.Linking;
    using System.Instant.Treatments;
    using System.IO;
    using System.Sets;
    using System.Runtime.InteropServices;
    using System.Uniques;

    public abstract class FigureSleeves : ISleeves
    {
        public IInstant Instant { get; set; }

        public bool Prime { get; set; }

        public abstract IFigure this[int index] { get; set; }

        public abstract object this[int index, string propertyName] { get; set; }

        public abstract object this[int index, int fieldId] { get; set; }

        public abstract IFigures Sleeves { get; set; }

        public abstract IFigures Figures { get; set; }

        public IFigures View { get => Sleeves.View; set => Sleeves.View = value; }

        public FigureFilter Filter { get => Sleeves.Filter; set => Sleeves.Filter = value; }
        public FigureSort Sort { get => Sleeves.Sort; set => Sleeves.Sort = value; }
        public Func<IFigure, bool> QueryFormula { get => Sleeves.QueryFormula; set => Sleeves.QueryFormula = value; }

        public int Serialize(Stream tostream, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary)
        {
            throw new NotImplementedException();
        }
        public int Serialize(ISerialBuffer buffor, int offset, int batchSize, SerialFormat serialFormat = SerialFormat.Binary)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(Stream fromstream, SerialFormat serialFormat = SerialFormat.Binary)
        {
            throw new NotImplementedException();
        }
        public object Deserialize(ISerialBuffer buffer, SerialFormat serialFormat = SerialFormat.Binary)
        {
            throw new NotImplementedException();
        }

        public object[] GetMessage()
        {
            return new[] { this };
        }
        public object GetHeader()
        {
            return Figures;
        }

        public void Clear()
        {
            Sleeves.Clear();
        }

        public void Flush()
        {
            Sleeves.Flush();
        }

        public IFigure NewFigure()
        {
            return Figures.NewFigure();
        }

        public ICard<IFigure> Next(ICard<IFigure> card)
        {
            return Sleeves.Next(card);
        }

        public bool ContainsKey(object key)
        {
            return Sleeves.ContainsKey(key);
        }
        public bool ContainsKey(IUnique key)
        {
            return Sleeves.ContainsKey(key);
        }
        public bool Contains(IFigure item)
        {
            return Sleeves.Contains(item);
        }
        public bool Contains(ICard<IFigure> item)
        {
            return Sleeves.Contains(item);
        }
        public bool Contains(IUnique<IFigure> item)
        {
            return Sleeves.Contains(item);
        }

        public IFigure Get(object key)
        {
            return Sleeves.Get(key);
        }
        public IFigure Get(IUnique<IFigure> key)
        {
            return Sleeves.Get(key);
        }
        public IFigure Get(IUnique key)
        {
            return Sleeves.Get(key);
        }

        public bool TryGet(object key, out ICard<IFigure> output)
        {
            return Sleeves.TryGet(key, out output);
        }
        public bool TryGet(object key, out IFigure output)
        {
            return Sleeves.TryGet(key, out output);
        }

        public ICard<IFigure> GetCard(object key)
        {
            return Sleeves.GetCard(key);
        }

        public ICard<IFigure> AddNew()
        {
            ICard<IFigure> item = Figures.AddNew();
            if (item != null)
            {
                Sleeves.Add(item);
            }
            return item;
        }
        public ICard<IFigure> AddNew(ulong key)
        {
            ICard<IFigure> item = Figures.AddNew(key);
            if (item != null)
            {
                Sleeves.Add(item);
            }
            return item;
        }
        public ICard<IFigure> AddNew(object key)
        {
            ICard<IFigure> item = Figures.AddNew();
            if (item != null)
            {
                Sleeves.Add(item);
            }
            return item;
        }

        public bool Add(ulong key, IFigure item)
        {
            IFigure _item;
            if (Figures.TryGet(key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Add(key, _item);
            }
            else
            {
                return Sleeves.TryAdd(Figures.Put(item).Value);
            }
        }
        public void Add(IFigure item)
        {
            ulong key = item.UniqueKey;
            IFigure _item;
            if (Figures.TryGet(key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                Sleeves.Add(key, _item);
            }
            else
            {
                Sleeves.Add(Figures.Put(item).Value);
            }

        }
        public bool Add(object key, IFigure item)
        {
            IFigure _item;
            ulong _key = key.UniqueKey64();
            if (Figures.TryGet(_key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Add(_key, _item);
            }
            else
                return Sleeves.TryAdd(Figures.Put(item).Value);
        }
        public void Add(ICard<IFigure> item)
        {
            ulong key = item.Key;
            ICard<IFigure> _item;
            if (Figures.TryGet(key, out _item))
            {
                if (!ReferenceEquals(_item.Value, item.Value))
                {
                    _item.Value.ValueArray = item.Value.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                Sleeves.Add(_item);
            }
            else
                Sleeves.Add(Figures.Put(item));
        }
        public void Add(IList<ICard<IFigure>> cardList)
        {
            foreach (var card in cardList)
                Sleeves.Add(card);
        }
        public void Add(IEnumerable<ICard<IFigure>> cards)
        {
            foreach (var card in cards)
                Add(card);
        }
        public void Add(IList<IFigure> cards)
        {
            foreach (var card in cards)
                Add(card);
        }
        public void Add(IEnumerable<IFigure> cards)
        {
            foreach (var card in cards)
                Add(card);
        }
        public void Add(IUnique<IFigure> item)
        {
            IFigure _item;
            if (Figures.TryGet(item, out _item))
            {
                IFigure value = item.Value;
                if (!ReferenceEquals(_item, value))
                {
                    _item.ValueArray = value.ValueArray;
                    _item.UniqueKey = value.UniqueKey;
                }
                Sleeves.Add(_item);
            }
            else
                Sleeves.Add(Figures.Put(item).Value);
        }
        public void Add(IList<IUnique<IFigure>> items)
        {
            foreach (IUnique<IFigure> item in items)
                Add(item);
        }
        public void Add(IEnumerable<IUnique<IFigure>> items)
        {
            foreach (IUnique<IFigure> item in items)
                Add(item);
        }
        public bool TryAdd(IFigure item)
        {
            ulong key = item.UniqueKey;
            IFigure _item;
            if (Figures.TryGet(key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Add(key, _item);
            }
            else
                return Sleeves.TryAdd(Figures.Put(item).Value);
        }

        public bool Enqueue(object key, IFigure value)
        {
            return Sleeves.Enqueue(key, value);
        }
        public void Enqueue(ICard<IFigure> card)
        {
            Sleeves.Enqueue(card);
        }
        public bool Enqueue(IFigure card)
        {
            return Sleeves.Enqueue(card);
        }

        public IFigure Dequeue()
        {
            return Sleeves.Dequeue();
        }
        public bool TryDequeue(out ICard<IFigure> item)
        {
            return Sleeves.TryDequeue(out item);
        }
        public bool TryDequeue(out IFigure item)
        {
            return Sleeves.TryDequeue(out item);
        }

        public bool TryTake(out IFigure item)
        {
            return Sleeves.TryTake(out item);
        }

        public ICard<IFigure> Put(ulong key, IFigure item)
        {
            IFigure _item;
            if (Figures.TryGet(key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Put(key, _item);
            }
            else
                return Sleeves.Put(Figures.Put(key, item).Value);
        }

        public ICard<IFigure> Put(object key, IFigure item)
        {
            ulong _key = key.UniqueKey();
            IFigure _item;
            if (Figures.TryGet(_key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Put(_key, _item);
            }
            else
                return Sleeves.Put(Figures.Put(key, item).Value);
        }
        public ICard<IFigure> Put(ICard<IFigure> card)
        {
            IFigure item = card.Value;
            IFigure _item;
            if (Figures.TryGet(card.Key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Put(card);
            }
            else
                return Sleeves.Put(Figures.Put(card));
        }

        public void Put(IList<ICard<IFigure>> cardList)
        {
            foreach (var card in cardList)
                Put(card);
        }
        public void Put(IEnumerable<ICard<IFigure>> cards)
        {
            foreach (var card in cards)
                Put(card);
        }
        public void Put(IList<IFigure> cards)
        {
            foreach (var card in cards)
                Put(card);
        }
        public void Put(IEnumerable<IFigure> cards)
        {
            foreach (var card in cards)
                Put(card);
        }
        public ICard<IFigure> Put(IFigure item)
        {
            ulong key = item.UniqueKey;
            IFigure _item;
            if (Figures.TryGet(key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Put(key, _item);
            }
            else
                return Sleeves.Put(Figures.Put(item).Value);
        }
        public ICard<IFigure> Put(IUnique<IFigure> value)
        {
            ulong key = value.UniqueKey;
            IFigure item = value.Value;
            IFigure _item;
            if (Figures.TryGet(key, out _item))
            {
                if (!ReferenceEquals(_item, item))
                {
                    _item.ValueArray = item.ValueArray;
                    _item.UniqueKey = item.UniqueKey;
                }
                return Sleeves.Put(key, _item);
            }
            else
                return Sleeves.Put(Figures.Put(item).Value);
        }
        public void Put(IList<IUnique<IFigure>> items)
        {
            foreach (var item in items)
                Put(item);
        }
        public void Put(IEnumerable<IUnique<IFigure>> items)
        {
            foreach (var item in items)
                Put(item);
        }

        public IFigure Remove(object key)
        {
            return Sleeves.Remove(key);
        }
        public bool Remove(IFigure item)
        {
            if (Sleeves.Remove(item) != null)
                return true;
            return false;
        }
        public bool Remove(ICard<IFigure> item)
        {
            return Sleeves.Remove(item);
        }
        public bool Remove(IUnique<IFigure> item)
        {
            return Sleeves.Remove(item);
        }
        public bool TryRemove(object key)
        {
            return Sleeves.TryRemove(key);
        }
        public void RemoveAt(int index)
        {
            Sleeves.RemoveAt(index);
        }

        public void Renew(IEnumerable<IFigure> cards)
        {
            Sleeves.Renew(cards);
        }
        public void Renew(IList<IFigure> cards)
        {
            Sleeves.Renew(cards);
        }
        public void Renew(IList<ICard<IFigure>> cards)
        {
            Sleeves.Renew(cards);
        }
        public void Renew(IEnumerable<ICard<IFigure>> cards)
        {
            Sleeves.Renew(cards);
        }

        public IFigure[] ToArray()
        {
            return Sleeves.ToArray();
        }

        public void CopyTo(IFigure[] array, int arrayIndex)
        {
            Sleeves.CopyTo(array, arrayIndex);
        }
        public void CopyTo(Array array, int arrayIndex)
        {
            Sleeves.CopyTo(array, arrayIndex);
        }
        public void CopyTo(ICard<IFigure>[] array, int destIndex)
        {
            Sleeves.CopyTo(array, destIndex);
        }

        public ICard<IFigure> NewCard(IFigure value)
        {
            return Sleeves.NewCard(value);
        }
        public ICard<IFigure> NewCard(ICard<IFigure> value)
        {
            return Sleeves.NewCard(value);
        }
        public ICard<IFigure> NewCard(object key, IFigure value)
        {
            return Sleeves.NewCard(key, value);
        }
        public ICard<IFigure> NewCard(ulong  key, IFigure value)
        {
            return Sleeves.NewCard(key, value);
        }

        public int IndexOf(IFigure item)
        {
            return Sleeves.IndexOf(item);
        }

        public void Insert(int index, IFigure item)
        {
            Figures.Add(item);
            Sleeves.Insert(index, item);
        }

        public IEnumerator<IFigure> GetEnumerator()
        {
            return Sleeves.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Sleeves.GetEnumerator();
        }

        public byte[] GetBytes()
        {
            return Sleeves.GetBytes();
        }

        public byte[] GetUniqueBytes()
        {
            return Sleeves.GetUniqueBytes();
        }

        public bool Equals(IUnique other)
        {
            return Sleeves.Equals(other);
        }

        public int CompareTo(IUnique other)
        {
            return Sleeves.CompareTo(other);
        }

        public IEnumerable<ICard<IFigure>> AsCards()
        {
            return Sleeves.AsCards();
        }

        public IEnumerable<IFigure> AsValues()
        {
            return Sleeves.AsValues();
        }

        public IEnumerable<IUnique<IFigure>> AsIdentifiers()
        {
            return Sleeves.AsIdentifiers();
        }

        public bool ContainsKey(ulong key)
        {
            return Sleeves.ContainsKey(key);
        }

        public IFigure Get(ulong key)
        {
            return Sleeves.Get(key);
        }

        public bool TryGet(ulong key, out IFigure output)
        {
            return Sleeves.TryGet(key, out output);
        }

        public ICard<IFigure> GetCard(ulong key)
        {
            return Sleeves.GetCard(key);
        }

        public int SerialCount { get; set; }
        public int DeserialCount { get; set; }
        public int ProgressCount { get; set; }

        public int ItemsCount => Sleeves.Count;

        public int Count => Sleeves.Count;

        public IRubrics Rubrics { get => Sleeves.Rubrics; set => Sleeves.Rubrics = value; }

        public IRubrics KeyRubrics { get => Sleeves.KeyRubrics; set => Sleeves.KeyRubrics = value; }

        public Type FigureType { get => Figures.FigureType; set => Figures.FigureType = value; }

        public int FigureSize { get => Figures.FigureSize; set => Figures.FigureSize = value; }

        public ICard<IFigure> First => Sleeves.First;

        public ICard<IFigure> Last => Sleeves.Last;

        public bool IsSynchronized { get => Sleeves.IsSynchronized; set => Sleeves.IsSynchronized = value; }
        public object SyncRoot { get => Sleeves.SyncRoot; set => Sleeves.SyncRoot = value; }

        public bool IsReadOnly => Sleeves.IsReadOnly;

        bool ICollection.IsSynchronized => Sleeves.IsSynchronized;

        object ICollection.SyncRoot => Sleeves.SyncRoot;

        public object[] ValueArray { get => Sleeves.ValueArray; set => Sleeves.ValueArray = value; }

        public Ussn SerialCode { get => Sleeves.SerialCode; set => Sleeves.SerialCode = value; }

        public IUnique Empty => Sleeves.Empty;

        public ulong UniqueKey { get => Sleeves.UniqueKey; set => Sleeves.UniqueKey = value; }

        public Type Type { get => Figures.Type; set => Figures.Type = value; }

        public Linker Linker { get => Sleeves.Linker; set => Sleeves.Linker = value; }

        public Treatment Treatment
        {
            get => Sleeves.Treatment;
            set => Sleeves.Treatment = value;
        }

        public IFigure Summary
        {
            get => Sleeves.Summary;
            set => Sleeves.Summary = value;
        }

        public IDeck<IComputation> Computations { get => Figures.Computations; set => Figures.Computations = value; }
        public ulong UniqueSeed { get => Sleeves.UniqueSeed; set => Sleeves.UniqueSeed = value; }

        object IFigure.this[int fieldId] { get => Sleeves[fieldId]; set => Sleeves[fieldId] = (IFigure)value; }
        public object this[string propertyName] { get => Sleeves[propertyName]; set => Sleeves[propertyName] = value; }
        public IFigure this[object key] { get => Sleeves[key]; set => Sleeves[key] = value; }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Sleeves.Dispose();
                }
                Sleeves = null;
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }
}
