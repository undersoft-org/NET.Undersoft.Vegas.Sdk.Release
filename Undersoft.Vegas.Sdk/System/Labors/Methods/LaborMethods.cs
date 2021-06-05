using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Instant;
using System.Multemic;
using System.Uniques;
using System.Extract;
using System.Linq;

namespace System.Labors
{   

    public class LaborMethods : Catalog<IDeputy>
    {
        public override ICard<IDeputy> EmptyCard()
        {
            return new LaborMethod();
        }

        public override ICard<IDeputy>[] EmptyCardTable(int size)
        {
            return new LaborMethod[size];
        }
        public override ICard<IDeputy>[] EmptyCardList(int size)
        {
            return new LaborMethod[size];
        }

        public override ICard<IDeputy> NewCard(long key, IDeputy value)
        {
            return new LaborMethod(key, value);
        }

        public override ICard<IDeputy> NewCard(object key, IDeputy value)
        {
            return new LaborMethod(key, value);
        }

        public override ICard<IDeputy> NewCard(IDeputy value)
        {
            return new LaborMethod(value);
        }
    }
}
