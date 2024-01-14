using System;
using System.Collections.Generic;
using UnityEngine;

namespace TriPeaksSolitaire.Core
{

    public interface IWastePile : ICardPile
    { 
        Card TopCard();
    }
    public class WastePile: IWastePile
    {
        private Stack<Card> cardsPile;

        public WastePile()
        {
            cardsPile = new Stack<Card>();
        }

        public void Add(Card card)
        {
            if (card == null)
            {
                LogError("Card is null, Can not add");
            }
            
            cardsPile.Push(card);
            UpdateTopCardFacing();
            OnPileUpdated?.Invoke();
        }

        public void Remove(Card card)
        {
            if (IsEmpty()) return;
            cardsPile.Pop();
            UpdateTopCardFacing();
            OnPileUpdated?.Invoke();
        }

        public bool Contains(Card card)
        {
            return cardsPile.Contains(card);
        }

        public bool IsEmpty()
        {
            return cardsPile.Count == 0;
        }

        public Card TopCard()
        {
            return !IsEmpty() ? cardsPile.Peek() : null;
        }

        void UpdateTopCardFacing()
        {
            var topCard = TopCard();
            if (!topCard) return;
            topCard.SetFaceUp();
            topCard.IsSelectable = false;
        }

        public void Clear()
        {
            cardsPile.Clear();
            OnPileUpdated?.Invoke();
        }

        public event Action OnPileUpdated;

        void LogError(string message)
        {
            Debug.LogError($"[WASTE PILE]: {message}");
        }
    }
}