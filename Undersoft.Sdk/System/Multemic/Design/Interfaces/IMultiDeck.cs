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
    public interface IMultiDeck<V>: IDeck<V> where V : IUnique
    {
        V this[object key, uint seed] { get; set; }

        bool ContainsKey(object key, uint seed);

        bool Contains(V item, uint seed);

        V Get(object key, uint seed);       

        bool TryGet(object key, uint seed, out ICard<V> output);
        bool TryGet(object key, uint sedd, out V output);

        ICard<V> GetCard(object key, uint seed);

        ICard<V> AddNew(uint seed);
        ICard<V> AddNew(object key, uint seed);

        bool Add(object key, uint seed, V value);
        bool Add(V value, uint seed);
        void Add(IList<V> cards, uint seed);
        void Add(IEnumerable<V> cards, uint seed);

        bool Enqueue(object key, uint seed, V value);
        bool Enqueue(V card, uint seed);

        ICard<V> Put(object key, uint seed, V value);
        ICard<V> Put(object key, uint seed, object value);
           void Put(IList<V> cards, uint seed);
           void Put(IEnumerable<V> cards, uint seed);
       ICard<V> Put(V value, uint seed);

           V Remove(object key, uint seed);
        bool TryRemove(object key, uint seed);

        ICard<V> NewCard(V value, uint seed);
        ICard<V> NewCard(object key, uint seed, V value);
        ICard<V> NewCard(long key, uint seed, V value);
    }    
}
