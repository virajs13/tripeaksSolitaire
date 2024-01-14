using System;
using System.Collections.Generic;
using UnityEngine;

namespace TriPeaksSolitaire.Core
{
    public interface IDrawPile : ICardPile
    {
        Card DrawCard();

        Card TopCard();
        void PopulateCards(IEnumerable<Card> cards);
    }
    public class DrawPile: IDrawPile
    {
        private Stack<Card> cardsPile;
        private Vector2 viewPosition;
        private Vector2 offset;
        
        public DrawPile(Vector2 viewPosition,Vector2 offset)
        {
            cardsPile = new Stack<Card>();
            this.viewPosition = viewPosition;
            this.offset = offset;
        }
        public void Add(Card card)
        {
            if (card == null)
            {
                LogError("Card is null, Can not add");
            }
            card.MoveInstant(GetNextPosition());
            card.transform.SetAsLastSibling();
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

        public Card TopCard()
        {
            if (IsEmpty()) return null;
            var card = cardsPile.Peek();
            return card;
        }

        public void PopulateCards(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
            {
               Add(card);
            }
        }

        private Vector2 GetNextPosition()
        {
            var rootPosition = viewPosition + cardsPile.Count * offset;
            return rootPosition;
        }

        void LogError(string message)
        {
            Debug.LogError($"[DRAW PILE]: {message}");
        }
    }
}