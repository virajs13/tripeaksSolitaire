using System;
using System.Collections.Generic;
using TriPeaksSolitaire.Utils;
using UnityEngine;

namespace TriPeaksSolitaire.Core
{
    public interface IDeck
    {
        void Shuffle();
        IEnumerable<Card> GetAllCards();
    }

    public class Deck : IDeck
    {
        public const int NUM_CARDS = 52;
        private Card[] cards = new Card[NUM_CARDS];
        private ObjectPool<Card> cardPool;

        public Deck(Card cardPrefab)
        {
            cardPool = new ObjectPool<Card>(cardPrefab, NUM_CARDS);
            InitializeDeck();
        }

        private void InitializeDeck()
        {
            Reset();
            
            Shuffle();
        }

        void Reset()
        {
            foreach (var cardInfo in CardInfo.EnumerateCardInfo())
            {
                var card = cardPool.GetObject();
                card.SetCardInfo(cardInfo);
                cards[cardInfo.GetCardId()] = card;
            }
        }
        public void Shuffle()
        {
            for (int i = 0; i < cards.Length; i++)
            {
                SwapCards(i, UnityEngine.Random.Range(i, cards.Length));
            }
        }
        
        void SwapCards(int a, int b)
        {
            (cards[a], cards[b]) = (cards[b], cards[a]);
        }
        
        public IEnumerable<Card> GetAllCards()
        {
            for (int i = 0; i < cards.Length; i++)
            {
                yield return cards[i];
            }
        }
        
        void LogError(string message)
        {
            Debug.LogError($"[DECK]: {message}");
        }
        
        
    } 

}