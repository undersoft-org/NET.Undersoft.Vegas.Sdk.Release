using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
/***************************************************
    Copyright (c) 2020 Undersoft

    System.Multemic.IDeck
   
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                     
 
 ***************************************************/
namespace System.Multemic
{
    public interface IDeck<V>: IEnumerable, IEnumerable<V>, IList<V>, IProducerConsumerCollection<V>, IDisposable
    {
        new V this[int index] { get; set; }
        V this[object key] { get; set; }

        ICard<V> First { get; }
        ICard<V> Last { get; }

        ICard<V> Next(ICard<V> card);

        new int Count { get; }

        bool ContainsKey(long key);
        bool ContainsKey(object key);
        bool ContainsKey(IUnique key);

        bool Contains(ICard<V> item);
        bool Contains(IUnique<V> item);

        V Get(object key);
        V Get(long key);
        V Get(IUnique key);
        V Get(IUnique<V> key);

        bool TryGet(object key, out ICard<V> output);
        bool TryGet(object key, out V output);
        bool TryGet(long key, out V output);

        ICard<V> GetCard(object key);
        ICard<V> GetCard(long key);

        ICard<V> AddNew();
        ICard<V> AddNew(long key);
        ICard<V> AddNew(object key);

        bool Add(object key, V value);
        bool Add(long key, V value);
        void Add(ICard<V> card);
        void Add(IList<ICard<V>> cardList);
        void Add(IEnumerable<ICard<V>> cards);
        void Add(IList<V> cards);
        void Add(IEnumerable<V> cards);
        void Add(IUnique<V> cards);
        void Add(IList<IUnique<V>> cards);
        void Add(IEnumerable<IUnique<V>> cards);

        bool Enqueue(object key, V value);
        void Enqueue(ICard<V> card);
        bool Enqueue(V card);

           V Dequeue();
        bool TryDequeue(out ICard<V> item);
        bool TryDequeue(out V item);
        new bool TryTake(out V item);

        ICard<V> Put(object key, V value);
        ICard<V> Put(long key, V value);
        ICard<V> Put(ICard<V> card);
           void Put(IList<ICard<V>> cardList);
           void Put(IEnumerable<ICard<V>> cards);
           void Put(IList<V> cards);
           void Put(IEnumerable<V> cards);
       ICard<V> Put(V value);
       ICard<V> Put(IUnique<V> cards);
           void Put(IList<IUnique<V>> cards);
           void Put(IEnumerable<IUnique<V>> cards);

           V Remove(object key);
        bool Remove(ICard<V> item);
        bool Remove(IUnique<V> item);
        bool TryRemove(object key);

        void Renew(IEnumerable<V> cards);
        void Renew(IList<V> cards);
        void Renew(IList<ICard<V>> cards);
        void Renew(IEnumerable<ICard<V>> cards);

            new V[] ToArray();

        IEnumerable<ICard<V>> AsCards();

        IEnumerable<V> AsValues();

        IEnumerable<IUnique<V>> AsIdentifiers();

        new void CopyTo(Array array, int arrayIndex);

          new bool IsSynchronized { get; set; }
        new object SyncRoot { get; set; }

        ICard<V> NewCard(V value);
        ICard<V> NewCard(object key, V value);
        ICard<V> NewCard(long key, V value);
        ICard<V> NewCard(ICard<V> card);

        void CopyTo(ICard<V>[] array, int destIndex);

        new void Clear();
    }    
}
