using System.Uniques;
using System.Collections.Generic;
using System.Multemic;

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Figures.Catalog64
    
    Implementation of CardBook abstract class
    using 64 bit hash code and long representation;  
        
    @author Darius Hanc                                                  
    @project Computing Wheel Advancements                                   
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                             
 
 ******************************************************************/
namespace System.Instant
{  
    public abstract class FigureSharedAlbum : SharedAlbum<IFigure>
    {
        public FigureSharedAlbum() : base(17, HashBits.bit64)
        {          
        }
        public FigureSharedAlbum(int _cardSize = 17) : base(_cardSize, HashBits.bit64)
        {
        }
        public FigureSharedAlbum(ICollection<IFigure> collections, int _cardSize = 17) : base(collections, _cardSize, HashBits.bit64)
        {
        }
        public FigureSharedAlbum(IEnumerable<IFigure> collections, int _cardSize = 17) : base(collections, _cardSize, HashBits.bit64)
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
            return new Card64<IFigure>[size];
        }

        public override ICard<IFigure>[] EmptyCardList(int size)
        {
            cards = new Card64<IFigure>[size];
            return cards;
        }
      
        private ICard<IFigure>[] cards;
        public ICard<IFigure>[] Cards { get => cards; }

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
