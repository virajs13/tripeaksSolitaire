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
        [SerializeField] private Transform deckHolder;
        private IDeck deck;
        
        private CardPileViewHandler cardPileViewHandler;

        private void Start()
        {
            Initialise();
        }

        private void Initialise()
        {
            SetupDeck();
            SetupCardPiles();
        }

        private void SetupCardPiles()
        {
            cardPileViewHandler = new CardPileViewHandler(this,boardPileView,drawPileView,wastePileView);
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

        private void SetupDeck()
        {
            deck = new Deck(cardPrefab,deckHolder);
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
        }

        public bool IsValidMove(Card cardA, Card cardB)
        {
            var difference = Mathf.Abs(cardA.CardInfo.Value - cardB.CardInfo.Value);
            return difference is 1 or 12;
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