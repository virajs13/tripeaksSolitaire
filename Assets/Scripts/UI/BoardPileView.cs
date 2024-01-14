using System.Collections.Generic;
using TriPeaksSolitaire.Core;
using UnityEngine;

namespace TriPeaksSolitaire.UI
{
    public class BoardPileView: MonoBehaviour,ICardPileView
    {
        private Vector2 pilePosition;
        private ICardPile boardPile;
        [SerializeField] private Vector2 offset = new Vector2(0.09f, 0.11f);

        
        private void Awake()
        {
            pilePosition = GetComponent<RectTransform>().position;
        }
        public void Initialise()
        {
            var canvasScaleFactor = GetComponentInParent<Canvas>().scaleFactor;
            boardPile = new BoardPile(pilePosition,offset*canvasScaleFactor);
        }

        public ICardPile CardPile => boardPile;

        public void LayOutCards(IEnumerable<Card> cards)
        {
            //update card facings and assign it to slots
            ((IBoardPile)boardPile).LayoutCards(cards);
            
        }

        public Vector2 GetPosition()
        {
            return pilePosition;
        }

       
    }
}