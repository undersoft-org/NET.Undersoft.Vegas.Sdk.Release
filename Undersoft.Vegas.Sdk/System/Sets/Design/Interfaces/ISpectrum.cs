using System.Collections.Generic;

/*********************************************************************************       
    Copyright (c) 2020 Undersoft

    System.Sets.ISpectrum
    
    @authors Darius Hanc & PhD Radek Rudek 
    @project NETStandard.Undersoft.SDK                                    
    @version 0.8.D (Feb 7, 2020)                                           
    
 **********************************************************************************/
namespace System.Sets
{
    public interface ISpectrum<V> : IEnumerable<BaseCard<V>>
    {
        int Size { get; }

        int Count { get;  }

        int Next(int key);

        int Previous(int key);

        bool Contains(int key);

        bool Add(int key, V value);

        bool Set(int key, V value);

        bool Remove(int key);
    }

}