namespace TriPeaksSolitaire.Core
{
    public interface ICardPile
    {
        void Add(Card card);
        void Remove(Card card);
        bool Contains(Card card);
        bool IsEmpty();
        void Clear();
        
    }
}