using System.Runtime.InteropServices;
using System.Extract;
using System.Uniques;
using System.Collections.Generic;
using System.Sets;

/******************************************************************
    Copyright (c) 2020 Undersoft

    System.Instant.FigureBook
    
    Implementation of BaseAlbum abstract class
    using 64 bit hash code and long representation;  
        
    @author Darius Hanc                                                  
    @project NET.Undersoft.Sdk             
    @version 0.8.D (Feb 7, 2020)                                            
    @licence MIT                                             
 
 ******************************************************************/
namespace System.Instant
{  
    public abstract class FigureBaseAlbum : BaseAlbum<IFigure>
    {
        public FigureBaseAlbum() : base()
        {
        }
        public FigureBaseAlbum(int capacity) : base(capacity)
        {
        }
        public FigureBaseAlbum(ICollection<IFigure> collections) : base(collections)
        {
        }
        public FigureBaseAlbum(IEnumerable<IFigure> collections) : base(collections)
        {
        }

        public override ICard<IFigure> EmptyCard()
        {
            return new Card<IFigure>();
        }

        public override ICard<IFigure> NewCard(ulong  key, IFigure value)
        {
            return new Card<IFigure>(key, value);
        }
        public override ICard<IFigure> NewCard(object key, IFigure value)
        {
            return new Card<IFigure>(key, value);
        }
        public override ICard<IFigure> NewCard(ICard<IFigure> value)
        {
            return new Card<IFigure>(value);
        }
        public override ICard<IFigure> NewCard(IFigure value)
        {
            return new Card<IFigure>(value);
        }

        public override ICard<IFigure>[] EmptyCardTable(int size)
        {
            return new Card<IFigure>[size];
        }

        public override ICard<IFigure>[] EmptyBaseDeck(int size)
        {
            return new Card<IFigure>[size];
        }      

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
