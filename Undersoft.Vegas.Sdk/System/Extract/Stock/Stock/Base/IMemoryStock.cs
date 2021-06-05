using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace System.Extract.Stock
{
    public interface IMemoryStock
    {
        bool WriteStock();
        bool ReadStock();
        bool OpenStock();
        bool CloseStock();
        IStock Stock { get; set; }
    }
}
