using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriPeaksSolitaire.Game;
using UnityEngine;

namespace TriPeaksSolitaire.Core
{
    public interface IBoardPile : ICardPile
    {
        void LayoutCards(IEnumerable<Card> cardsForBoard);

        Card PossibleMove(Card card);
    }
    public class BoardPile: IBoardPile
    {
        const int NUM_ROWS = 4;
        const int NUM_COLUMNS = 10;

        public const int NUM_BOARD_CARDS = 28;
        
        //card layout will be represented by 2D array of slots
        //to update card active status (Face Up/Face down), we can check validity of slots below and below to the right
        //extra row and column for slots on edges
        private Card[,] cardsPile = new Card[NUM_ROWS+1, NUM_COLUMNS+1];
        
        private Dictionary<Card, Vector2> cardPositions = new Dictionary<Card, Vector2>();

        private List<Card> activeCards = new List<Card>();

        private Vector2 viewPosition;
        private Vector2 offset;

        public BoardPile(Vector2 viewPosition, Vector2 offset)
        {
            this.viewPosition = viewPosition;
            this.offset = offset;
        }
        public  void LayoutCards(IEnumerable<Card> cardsForBoard)
        {
            Clear();
            SetupCards(cardsForBoard);
            SetupCardPositions();
            MoveCards();
            UpdateCardFacings();
        }

        public Card PossibleMove(Card card)
        {
            foreach (var activeCard in activeCards)
            {
                if (GameController.IsValidMove(activeCard, card))
                    return activeCard;
            }

            return null;
        }

        private void MoveCards()
        {
            foreach (var card in EnumerateCards())
            {
                Vector3 position = cardPositions[card];
                card.transform.SetAsFirstSibling();
                card.MoveTo(position);
            }
        }

        //get all non null cards
        IEnumerable<Card> EnumerateCards()
        {
            return GetCardIndices()
                .Select(index => cardsPile[index.x, index.y])
                .Where(card => card != null);
        }

        private void SetupCardPositions()
        {
            foreach (CardIndex index in GetCardIndices())
            {
                Card card = cardsPile[index.x, index.y];
                cardPositions[card] = GetCardPosition(index);
            }
            
        }
        private void SetupCards(IEnumerable<Card> cardsForBoard)
        {
            var cardsIterator = cardsForBoard.GetEnumerator();
            foreach (var index in GetCardIndices())
            {
                cardsPile[index.x, index.y] = GetNext(cardsIterator);
            }
        }
        
        IEnumerable<CardIndex> GetCardIndices()
        {
            for (var r = NUM_ROWS; r >=0; r--)
            {
                for (var c = NUM_COLUMNS; c >=0; c--)
                {
                    if (IsValidSlot(r,c,NUM_COLUMNS+1))
                    {
                        yield return new CardIndex(r, c);
                    }
                }
            }
        }
        
        private bool IsValidSlot(int i, int j, int cols)
        {
            return ((((j % 3) <= i) && ((j / 3 < 3) && i < 3)) || (i % 3 == 0 && i != 0 && j < cols - 1));
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
        


        Vector2 GetCardPosition(CardIndex index)
        {
            var x = viewPosition.x + (index.y - (3 + 0.5f * index.x)) * offset.x;
            var y = viewPosition.y - (index.x * offset.y);

            return new Vector2(x, y);
        }

        private void UpdateCardFacings()
        {
            Log("Updating card facings");
            foreach (CardIndex index in GetCardIndices())
            {
                UpdateCardFacing(index.x, index.y);
            }
        }

        private void UpdateCardFacing(int x, int y)
        {
            if (x < 0 || x >= cardsPile.GetLength(0) || y < 0 || y >= cardsPile.GetLength(1))
            {
                // Indices are out of bounds
                return;
            }
            var card = cardsPile[x, y];
            if (card == null)
                return;
           
            if (y +1 > cardsPile.GetLength(1) || x + 1 > cardsPile.GetLength(0))
            {
                return;
            }
            
            if (cardsPile[x+1, y] == null && cardsPile[x + 1, y + 1] == null)
            {
               
                if (!activeCards.Contains(card))
                {
                    activeCards.Add(card);
                    card.SetFaceUp();
                    card.IsSelectable = true;
                }

               
            }
            else
            {
                // card.SetFaceDown();
                if (activeCards.Contains(card))
                {
                    activeCards.Remove(card);
                }
                // card.IsSelectable = false;
            }

        }

        //set card slot status
        private void SetCard(Card card, int x, int y)
        {
            cardsPile[x, y] = card;
            UpdateCardFacings();
        }
        

       

       
       

       
        public void Add(Card card)
        {
            
        }

        public void Remove(Card card)
        {
            //remove card from active cards pile 

            foreach (CardIndex index in GetCardIndices())
            {
                if (card == cardsPile[index.x, index.y])
                {
                    SetCard(null, index.x, index.y);
                    if (activeCards.Contains(card))
                    {
                        activeCards.Remove(card);
                    }
                    return;
                }
            }
            
            LogError("Card not found");
            
        }

        public bool Contains(Card card)
        {
            //if card is in active cards
            return EnumerateCards().Any(card.Equals);
        }

        public bool IsEmpty()
        {
            return !EnumerateCards().Any();
        }

        public void Clear()
        {
            Array.Clear(cardsPile,0,cardsPile.Length);
            cardPositions.Clear();
            activeCards.Clear();
            
        }

        public event Action OnPileUpdated;


        void LogError(string message)
        {
            Debug.LogError($"[BOARD PILE]: {message}");
        }
        void Log(string message)
        {
            Debug.Log($"[BOARD PILE]: {message}");
        }


        private struct CardIndex
        {
            public readonly int x, y;
            
            
            public CardIndex(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            
        }
    }
}