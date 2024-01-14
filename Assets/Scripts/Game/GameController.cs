using System;
using TriPeaksSolitaire.Core;
using TriPeaksSolitaire.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TriPeaksSolitaire.Game
{
    public class GameController: MonoBehaviour,IGame
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
            
            cardPileViewHandler = new CardPileViewHandler(boardPileView,drawPileView,wastePileView);
            //shuffle deck
            deck.Shuffle();
            //layout all cards in card pile
            cardPileViewHandler.LayOutCardPiles(deck.GetAllCards());
            
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
        }
        
        void LogError(string message)
        {
            Debug.LogError($"[GAME CONTROLLER]: {message}");
        }
    }
}