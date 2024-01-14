using System.Collections.Generic;
using System.Linq;
using TriPeaksSolitaire.Core;
using TriPeaksSolitaire.UI;
using UnityEngine;

namespace TriPeaksSolitaire.Game
{
    public class CardPileViewHandler
    {
        private ICardPileView boardPileView;
        private ICardPileView drawPileView;
        private ICardPileView wastePileView;
        
        public CardPileViewHandler(ICardPileView boardPileView, ICardPileView drawPileView, ICardPileView wastePileView)
        {
            this.boardPileView = boardPileView;
            this.drawPileView = drawPileView;
            this.wastePileView = wastePileView;
        }

        public void LayOutCardPiles(IEnumerable<Card> cards)
        {
            ResetCardPiles();
            LayoutBoardPile(cards.Take(BoardPile.NUM_BOARD_CARDS));
            LayoutDrawPile(cards.Skip(BoardPile.NUM_BOARD_CARDS));


        }

        private void LayoutDrawPile(IEnumerable<Card> drawCards)
        {
            drawPileView.LayOutCards(drawCards);
        }

        private void LayoutBoardPile(IEnumerable<Card> boardCards)
        {
           boardPileView.LayOutCards(boardCards);
        }

        void ResetCardPiles()
        {
            boardPileView.CardPile.Clear();
            drawPileView.CardPile.Clear();
            wastePileView.CardPile.Clear();
        }

        void MoveCard(Card card, ICardPileView sourcePile,ICardPileView targetPile)
        {
            if (card == null)
            {
                LogError("Card is null");
                return;
            }

            if (!sourcePile.CardPile.Contains(card))
            {
                LogError($"{card.CardInfo} is not present in source pile");
            }
            
            if (targetPile.CardPile.Contains(card))
            {
                LogError($"{card.CardInfo} is already present in target pile");
            }
            
            sourcePile.CardPile.Remove(card);
            //
            targetPile.CardPile.Add(card);
        }
        
        void LogError(string message)
        {
            Debug.LogError($"[CARD PILE HANDLER]: {message}");
        }
    }
}