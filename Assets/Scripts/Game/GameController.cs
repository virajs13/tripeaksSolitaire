using System;
using System.Collections.Generic;
using TriPeaksSolitaire.Core;
using TriPeaksSolitaire.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace TriPeaksSolitaire.Game
{
    public class GameController: MonoBehaviour,IGame,IGameActions
    {
        [SerializeField] private Card cardPrefab;
        [SerializeField] private BoardPileView boardPileView;
        [SerializeField] private DrawPileView drawPileView;
        [SerializeField] private WastePileView wastePileView;
        [SerializeField] private UIHandler uiHandler;
        [SerializeField] private Transform deckHolder;
        private IDeck deck;
        private Scoreboard scoreBoard;
        
        private CardPileViewHandler cardPileViewHandler;

        private void Start()
        {
            Initialise();
        }

        private void Initialise()
        {
            deck = new Deck(cardPrefab,deckHolder);
            scoreBoard = new Scoreboard();
            cardPileViewHandler = new CardPileViewHandler(boardPileView,drawPileView,wastePileView,scoreBoard);
            SetupCardPiles();
            uiHandler.OnNewGameClicked = NewGame;
            uiHandler.OnBuyDeckButtonClicked = BuyDeck;
            uiHandler.UpdateScore(scoreBoard);

        }
        
       

        private void SetupCardPiles()
        {
            //shuffle deck
            deck.Shuffle();
            //layout all cards in card pile
            var allCards = deck.GetAllCards();
            SubscribeCardClickEvent(allCards);
            cardPileViewHandler.LayOutCardPiles(allCards);
            
        }

        private void SubscribeCardClickEvent(IEnumerable<Card> allCards)
        {
            foreach (var card in allCards)
            {
                card.OnSelected = CardClicked;
            }
        }
        

        public void NewGame()
        {
            SceneManager.LoadScene("Game");
        }

        public void BuyDeck()
        {
            throw new System.NotImplementedException();
        }

        public void CardClicked(Card card)
        {
            if (card == null)
            {
                LogError("card is null");
                return;
            }
            cardPileViewHandler.HandleCardClick(card);
            uiHandler.UpdateScore(scoreBoard);
            CheckForGameEndConditions();
        }

        private void CheckForGameEndConditions()
        {

            if (IsGameWin())
            {
                uiHandler.ShowGameWinPopup();
                return;
            }

            if (cardPileViewHandler.IsDrawPileClear())
            {
                uiHandler.ShowBuyDeckPopup();
                return;
            }

            if (!AnyMovesPossible())
            {
                ShowBuyDeckPopupRandom();
                return;
            }
            
            
        }

        private void ShowBuyDeckPopupRandom()
        {
            // Set the probability of showing the popup 
            int probability = 5;

            // Generate a random number between 1 and 100
            Random random = new Random();
            int randomChance = random.Next(1, 101);

            // Check if the random number falls within the desired probability range
            if (randomChance <= probability)
            {
                uiHandler.ShowBuyDeckPopup();
            }
        }

        bool IsGameWin()
        {
            return cardPileViewHandler.IsBoardPileClear();
        }


        public static bool IsValidMove(Card cardA, Card cardB)
        {
            var difference = Mathf.Abs(cardA.CardInfo.Value - cardB.CardInfo.Value);
            return difference is 1 or 12;
        }
        bool AnyMovesPossible()
        {
            var wastePileTopCard = ((IWastePile)(wastePileView.CardPile)).TopCard();
            if (wastePileTopCard == null) return false;
            return ((IBoardPile)boardPileView.CardPile).PossibleMove(wastePileTopCard) != null;
        }

        void LogError(string message)
        {
            Debug.LogError($"[GAME CONTROLLER]: {message}");
        }

        public void StartNew(IEnumerable<Card> cards)
        {
            SubscribeCardClickEvent(cards);
            cardPileViewHandler.LayOutCardPiles(cards);
        }
    }
}