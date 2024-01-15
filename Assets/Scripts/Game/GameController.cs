using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private float gameHintIdleTime = 5f;
        private float lastHintShownTimeStamp = 0;

        private Card currentClickedCard;
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

        private void Update()
        {

            if (Time.time - lastHintShownTimeStamp > gameHintIdleTime)
            {
                CheckForGameHints();
            }
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
            deck = new Deck(cardPrefab,deckHolder);
            //shuffle deck
            deck.Shuffle();
            //layout all cards in card pile
            var allCards = deck.GetCards(BoardPile.NUM_BOARD_CARDS);
            SubscribeCardClickEvent(allCards);
            cardPileViewHandler.LayoutDrawPile(allCards);
        }

        public void CardClicked(Card card)
        {
            if (card == null)
            {
                LogError("card is null");
                return;
            }

            currentClickedCard = card;
            cardPileViewHandler.HandleCardClick(card);
            uiHandler.UpdateScore(scoreBoard);
            lastHintShownTimeStamp = Time.time;
           StartCoroutine(CheckForGameEndConditions());
           
        }

        private IEnumerator CheckForGameEndConditions()
        {
            if (currentClickedCard)
                yield return currentClickedCard.WaitForCardMoveTween();
            if (IsGameWin())
            {
                uiHandler.ShowGameWinPopup();
                yield break;
            }

            if (cardPileViewHandler.IsDrawPileClear())
            {
                uiHandler.ShowBuyDeckPopup();
                yield break;
            }
           
        }

        private void CheckForGameHints()
        {
            lastHintShownTimeStamp = Time.time;
            if (IsGameWin()) return;
            if (cardPileViewHandler.IsDrawPileClear()) return;
            if (AnyMovesPossible())
            {
                var wastePileTopCard = ((IWastePile)(wastePileView.CardPile)).TopCard();
                uiHandler.ShowGameHintText($"Tap on {wastePileTopCard.CardInfo.GetHintCardValues()}");
            }
            else
            {
                uiHandler.ShowGameHintText("Draw a card");
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