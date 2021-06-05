namespace System.Sets
{
    public interface IUnique<V> : IUnique
    {
        #region Properties

        V Value { get; set; }

        #endregion

        #region Methods

        ulong UniquesAsKey();

        int[] UniqueOrdinals();

        object[] UniqueValues();

        #endregion
    }
}