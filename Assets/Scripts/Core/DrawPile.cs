using System;
using System.Collections.Generic;
using UnityEngine;

namespace TriPeaksSolitaire.Core
{
    public interface IDrawPile : ICardPile
    {
        Card DrawCard();
        void PopulateCards(IEnumerable<Card> cards);
    }
    public class DrawPile: IDrawPile
    {
        private Stack<Card> cardsPile;
        public void Add(Card card)
        {
            if (card == null)
            {
                LogError("Card is null, Can not add");
            }
            cardsPile.Push(card);
            OnPileUpdated?.Invoke();
        }

        public void Remove(Card card)
        {
            DrawCard();
        }

        public bool Contains(Card card)
        {
            return cardsPile.Contains(card);
        }

        public bool IsEmpty()
        {
            return cardsPile.Count == 0;
        }

        public void Clear()
        {
            cardsPile.Clear();
            OnPileUpdated?.Invoke();
        }

        public event Action OnPileUpdated;

        public Card DrawCard()
        {
            if (IsEmpty()) return null;
            var card = cardsPile.Pop();
            OnPileUpdated?.Invoke();
            return card;

        }

        public void PopulateCards(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
            {
                cardsPile.Push(card);
            }
        }
        
        void LogError(string message)
        {
            Debug.LogError($"[DRAW PILE]: {message}");
        }
    }
}