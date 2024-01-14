using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TriPeaksSolitaire.Core
{
    public interface IBoardPile : ICardPile
    {
        void LayoutCards(IEnumerable<Card> cardsForBoard);
    }
    public class BoardPile: IBoardPile
    {
        const int NUM_ROWS = 4;
        const int NUM_COLUMNS = 10;

        public const int NUM_BOARD_CARDS = 28;
        
        //card layout will be represented by 2D array of slots
        //true - valid slot 
        //false - invalid slot
        //to update card active status (Face Up/Face down), we can check validity of slots below and below to the right
        //extra row and column for slots on edges
        private bool[,] cardSlotLayout = new bool[NUM_ROWS+1, NUM_COLUMNS+1];
        
        //mapping of all cards to it's slot index 
        private Dictionary<Card, CardSlotIndex> cardsPile = new Dictionary<Card, CardSlotIndex>();
        //all active cards (Face Up)
        private Dictionary<Card,CardSlotIndex> activeCardsPile = new Dictionary<Card, CardSlotIndex>();
        
        public void LayoutCards(IEnumerable<Card> cardsForBoard)
        {
            Clear();
            SetupCardSlotLayout();
            AssignCardsToCardSlots(cardsForBoard);
            UpdateCardFacings();
        }

        private void UpdateCardFacings()
        {
            foreach (var cardSlot in cardsPile)
            {
                UpdateCardFacing(cardSlot.Key);
            }
        }

        private void UpdateCardFacing(Card card)
        {
            if (card == null)
                return;

            var index = cardsPile[card];
            if (cardSlotLayout[index.x, index.y - 1] == false && cardSlotLayout[index.x + 1, index.y - 1] == false)
            {
                card.SetFaceUp();
                //add to active cards
                Add(card);
                //set slot as invalid
                SetCardSlot(index, false);
                //mark card as selectable
                card.IsSelectable = true;

            }
            else
            {
                card.SetFaceDown();
                card.IsSelectable = false;
            }
            
        }

        //set card slot status
        private void SetCardSlot(CardSlotIndex index, bool value)
        {
            if (IsIndexInRange(index))
            {
                cardSlotLayout[index.x, index.y] = value;
            }
            else
            {
                LogError("Out of range index: " + index);
            }
        }
        
        private bool IsIndexInRange(CardSlotIndex index)
        {
            return index.x >= 0 && index.x < cardSlotLayout.GetLength(0) &&
                   index.y >= 0 && index.y < cardSlotLayout.GetLength(1);
        }

        private void AssignCardsToCardSlots(IEnumerable<Card> cardsForBoard)
        {
            var cardsIterator = cardsForBoard.GetEnumerator();
            foreach (var cardSlotIndex in GetCardSlotIndices())
            {
                var card = GetNext(cardsIterator);
                cardsPile[card] = cardSlotIndex;
            }
        }

        T GetNext<T>(IEnumerator<T> enumerator)
        {
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            } 
            // Handle the case where there is no next element.
            throw new InvalidOperationException("Enumerator has no next element.");
        }
        private void SetupCardSlotLayout()
        {
            var rows = cardSlotLayout.GetLength(0);
            var columns = cardSlotLayout.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (IsValidSlot(i, j,columns))
                    {
                        cardSlotLayout[i, j] = true;
                    }
                    else
                    {
                        cardSlotLayout[i, j] = false;
                    }
                }
            }
        }

        private bool IsValidSlot(int i, int j, int cols)
        {
            return ((((j % 3) <= i) && ((j / 3 < 3) && i < 3)) || (i % 3 == 0 && i != 0 && j < cols - 1));
        }

        IEnumerable<CardSlotIndex> GetCardSlotIndices()
        {
            for (var r = 0; r < NUM_ROWS; r++)
            {
                for (var c = 0; c < NUM_COLUMNS; c++)
                {
                    if (cardSlotLayout[r, c])
                    {
                         yield return new CardSlotIndex(r, c);
                    }
                }
            }
        }

       
        public void Add(Card card)
        {
            //add card to active cards pile
            
            //check if it is in active card pile
            if (activeCardsPile.ContainsKey(card))
            {
                LogError("Card already present in active card pile");
                return;
            }
            
            //check if card is in cards pile
            if (!cardsPile.ContainsKey(card))
            {
                LogError("Card not present in cards pile");
                return;
            }

            activeCardsPile[card] = cardsPile[card];
            OnPileUpdated?.Invoke();
            
        }

        public void Remove(Card card)
        {
            //remove card from active cards pile 

            if (activeCardsPile.ContainsKey(card))
            {
                activeCardsPile.Remove(card);
                OnPileUpdated?.Invoke();
            }
            
        }

        public bool Contains(Card card)
        {
            //if card is in active cards
            return activeCardsPile.ContainsKey(card);
        }

        public bool IsEmpty()
        {
            //if active cards is empty
            return !activeCardsPile.Any();
        }

        public void Clear()
        {
            Array.Clear(cardSlotLayout,0,cardSlotLayout.Length);
            cardsPile.Clear();
            activeCardsPile.Clear();
            OnPileUpdated?.Invoke();
            
        }

        public event Action OnPileUpdated;


        void LogError(string message)
        {
            Debug.LogError($"[BOARD PILE]: {message}");
        }


        private struct CardSlotIndex
        {
            public readonly int x, y;
            
            
            public CardSlotIndex(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            
        }
    }
}