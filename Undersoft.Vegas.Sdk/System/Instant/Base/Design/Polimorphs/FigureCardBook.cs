using System.Runtime.InteropServices;
using System.Extract;
using System.Uniques;
using System.Collections.Generic;
using System.Multemic;

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Instant.FigureBook
    
    Implementation of CardBook abstract class
    using 64 bit hash code and long representation;  
        
    @author Darius Hanc                                                  
    @project NET.Undersoft.Sdk             
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                             
 
 ******************************************************************/
namespace System.Instant
{  
    public abstract class FigureCardBook : CardBook<IFigure>
    {
        public FigureCardBook() : base(17, HashBits.bit64)
        {
        }
        public FigureCardBook(int deckSize = 17) : base(deckSize, HashBits.bit64)
        {
        }
        public FigureCardBook(ICollection<IFigure> collections, int deckSize = 17) : base(collections, deckSize, HashBits.bit64)
        {
        }
        public FigureCardBook(IEnumerable<IFigure> collections, int deckSize = 17) : base(collections, deckSize, HashBits.bit64)
        {
        }

        public override ICard<IFigure> EmptyCard()
        {
            return new Card64<IFigure>();
        }

        public override ICard<IFigure> NewCard(long key, IFigure value)
        {
            return new Card64<IFigure>(key, value);
        }
        public override ICard<IFigure> NewCard(object key, IFigure value)
        {
            return new Card64<IFigure>(key, value);
        }
        public override ICard<IFigure> NewCard(ICard<IFigure> value)
        {
            return new Card64<IFigure>(value);
        }
        public override ICard<IFigure> NewCard(IFigure value)
        {
            return new Card64<IFigure>(value);
        }

        public override ICard<IFigure>[] EmptyCardTable(int size)
        {
            return new FigureCard[size];
        }

        public override ICard<IFigure>[] EmptyCardList(int size)
        {
            cards = new FigureCard[size];
            return cards;
        }      

        private FigureCard[] cards;
        public ICard<IFigure>[] Cards => cards;

        protected override bool InnerAdd(IFigure value)
        {
            return InnerAdd(NewCard(value));
        }
        protected override ICard<IFigure> InnerPut(IFigure value)
        {
            return InnerPut(NewCard(value));
        }

    }
}
