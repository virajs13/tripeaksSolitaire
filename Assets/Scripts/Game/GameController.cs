using System;
using System.Collections.Generic;
using TriPeaksSolitaire.Core;
using TriPeaksSolitaire.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

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