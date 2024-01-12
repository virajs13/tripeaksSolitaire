using System;
using System.Collections.Generic;

namespace TriPeaksSolitaire.Core
{
    public class Deck
    {
        private Queue<Card> cards;

        public Deck()
        {
            cards = new Queue<Card>();
            InitializeDeck();
        }

        private void InitializeDeck()
        {
            // Clear any existing cards in the deck
            cards.Clear();

            // Create cards for each combination of value and suit
            foreach (Card.Suit suit in Enum.GetValues(typeof(Card.Suit)))
            {
                for (var value = 2; value <= 14; value++) // 11 for Jack, 12 for Queen, 13 for King, 14 for Ace
                {
                    var newCard = new Card(value, suit);
                    cards.Enqueue(newCard);
                }
            }
            
            //Shuffle Cards
            
            Shuffle();
        }

        public void Shuffle()
        {
            var shuffledCards = new List<Card>(cards);
            ShuffleCards(shuffledCards);
            cards = new Queue<Card>(shuffledCards);
        }

        public Card DealCard()
        {
            if (cards.Count > 0)
            {
                return cards.Dequeue();
            }
            else
            {
                // Handle case when the deck is empty (perhaps reshuffle or signal no more cards).
                return null;
            }
        }

        public void ResetDeck()
        {
            // Reset the deck to its initial state.
            InitializeDeck();
        }

        // Shuffling cards
        private void ShuffleCards(IList<Card> cardList)
        {
            var n = cardList.Count;
            while (n > 1)
            {
                n--;
                var k = UnityEngine.Random.Range(0, n + 1);
                // Swap elements at indices k and n
                (cardList[k], cardList[n]) = (cardList[n], cardList[k]);
            }
        }
    } 

}