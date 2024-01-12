using System.Collections.Generic;

namespace TriPeaksSolitaire.Core
{
    public class WastePile
    {
        private Queue<Card> cards;

        public WastePile()
        {
            cards = new Queue<Card>();
        }

        public void AddCard(Card card)
        {
            cards.Enqueue(card);
        }

        public Card TopCard()
        {
            if (cards.Count > 0)
            {
                return cards.Peek();
            }

            return null; // Handle the case when the pile is empty
        }

        public void Clear()
        {
            cards.Clear();
        }
    }
}