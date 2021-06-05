using System.Collections.Generic;
using System.Sets;
using System.Uniques;

namespace System.Instant
{
    [Serializable]
    public class FieldMapping
    {
        public FieldMapping(string dbDeckName) : this(dbDeckName, new Deck<int>(), new Deck<int>()) { }                
        public FieldMapping(string dbDeckName, IDeck<int> keyOrdinal, IDeck<int> columnOrdinal)
        {
            KeyOrdinal = keyOrdinal;
            ColumnOrdinal = columnOrdinal;
            DbTableName = dbDeckName;
        }

        public string DbTableName { get; set; }

        public IDeck<int> KeyOrdinal { get; set; }
        public IDeck<int> ColumnOrdinal { get; set; }
    }

    [Serializable]
    public class FieldMappings : BaseAlbum<FieldMapping>
    {
        public override ICard<FieldMapping> EmptyCard()
        {
            return new Card<FieldMapping>();
        }

        public override ICard<FieldMapping>[] EmptyCardTable(int size)
        {
            return new Card<FieldMapping>[size];
        }
        public override ICard<FieldMapping>[] EmptyBaseDeck(int size)
        {
            return new Card<FieldMapping>[size];
        }

        public override ICard<FieldMapping> NewCard(object key, FieldMapping value)
        {
            return new Card<FieldMapping>(key, value);
        }

        public override ICard<FieldMapping> NewCard(ulong  key, FieldMapping value)
        {
            return new Card<FieldMapping>(key, value);
        }

        public override ICard<FieldMapping> NewCard(FieldMapping value)
        {
            return new Card<FieldMapping>(value.DbTableName.UniqueKey(), value);
        }
        public override ICard<FieldMapping> NewCard(ICard<FieldMapping> value)
        {
            return new Card<FieldMapping>(value);
        }
    }

}
