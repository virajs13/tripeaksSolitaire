using System.Collections.Generic;
using System.Linq;
using TriPeaksSolitaire.Core;
using TriPeaksSolitaire.UI;
using UnityEngine;

namespace TriPeaksSolitaire.Game
{
    public class CardPileViewHandler: ICardPileActions, ICardMoveActions
    {
        private ICardPileView boardPileView;
        private ICardPileView drawPileView;
        private ICardPileView wastePileView;
        private IScoreBoard scoreBoard;
       
        
        public CardPileViewHandler(ICardPileView boardPileView, ICardPileView drawPileView, ICardPileView wastePileView,IScoreBoard scoreBoard)
        {
            this.boardPileView = boardPileView;
            this.drawPileView = drawPileView;
            this.wastePileView = wastePileView;
            this.scoreBoard = scoreBoard;
            this.boardPileView.Initialise();
            this.drawPileView.Initialise();
            this.wastePileView.Initialise();
        }

        public void LayOutCardPiles(IEnumerable<Card> cards)
        {
            ResetCardPiles();
            LayoutDrawPile(cards.Skip(BoardPile.NUM_BOARD_CARDS));
            DrawCard();
            LayoutBoardPile(cards.Take(BoardPile.NUM_BOARD_CARDS));
           
          
            
            
        }

        public void PerformValidMove(Card card)
        {
            //move card from board pile to waste pile
            MoveCard(card,boardPileView,wastePileView);
            scoreBoard.IncrementMove();
            
        }

        public void DrawCard()
        {
            //move card from draw pile to waste pile
            MoveCard(GetNextDrawCard(),drawPileView,wastePileView);
            scoreBoard.Reset();
        }

        public void HandleCardClick(Card card)
        {
            if (IsValidBoardPileMove(card))
            {
                PerformValidMove(card);
            }
            else if(IsDrawPileClick(card))
            {
                DrawCard();
            }
            else
            {
                card.Shake();
            }
        }

        private bool IsValidBoardPileMove(Card card)
        {
            var wastePileTopCard = ((IWastePile)(wastePileView.CardPile)).TopCard();
            return card.IsFaceUp && boardPileView.CardPile.Contains(card) && IsValidMove(card,wastePileTopCard);
        }

        public bool IsValidMove(Card cardA, Card cardB)
        {
            var difference = Mathf.Abs(cardA.CardInfo.Value - cardB.CardInfo.Value);
            return difference is 1 or 12;
        }

        private bool IsDrawPileClick(Card card)
        {
            return ((IDrawPile)drawPileView.CardPile).Contains(card);
        }

        public bool IsBoardPileClear()
        {
            return boardPileView.CardPile.IsEmpty();
        }

        // Returns the card on top of the draw pile
        private Card GetNextDrawCard()
        {
            if (drawPileView.CardPile.IsEmpty())
            {
                LogError("No cards in draw pile");
                return null;
            }

            return ((IDrawPile)drawPileView.CardPile).TopCard();

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

        public void MoveCard(Card card, ICardPileView sourcePile,ICardPileView targetPile)
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
            targetPile.CardPile.Add(card);
        }
        
        void LogError(string message)
        {
            Debug.LogError($"[CARD PILE HANDLER]: {message}");
        }
    }
}