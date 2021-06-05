using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
/***************************************************
    Copyright (c) 2020 Undersoft

    System.Sets.IDeck
   
        
    @author Darius Hanc                                                  
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                     
 
 ***************************************************/
namespace System.Sets
{
    public interface IMassDeck<V>: IDeck<V> where V : IUnique
    {
        V this[object key, ulong seed] { get; set; }

        bool ContainsKey(object key, ulong seed);

        bool Contains(V item, ulong seed);

        V Get(object key, ulong seed);       

        bool TryGet(object key, ulong seed, out ICard<V> output);
        bool TryGet(object key, ulong sedd, out V output);

        ICard<V> GetCard(object key, ulong seed);

        ICard<V> AddNew(object key, ulong seed);

        bool Add(object key, ulong seed, V value);
        bool Add(V value, ulong seed);
        void Add(IList<V> cards, ulong seed);
        void Add(IEnumerable<V> cards, ulong seed);

        bool Enqueue(object key, ulong seed, V value);
        bool Enqueue(V card, ulong seed);

        ICard<V> Put(object key, ulong seed, V value);
        ICard<V> Put(object key, ulong seed, object value);
           void Put(IList<V> cards, ulong seed);
           void Put(IEnumerable<V> cards, ulong seed);
       ICard<V> Put(V value, ulong seed);

           V Remove(object key, ulong seed);
        bool TryRemove(object key, ulong seed);

        ICard<V> NewCard(V value, ulong seed);
        ICard<V> NewCard(object key, ulong seed, V value);
        ICard<V> NewCard(ulong key, ulong seed, V value);
    }    
}
