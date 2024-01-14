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
            boardPile = new BoardPile();
        }

        public ICardPile CardPile => boardPile;

        public void LayOutCards(IEnumerable<Card> cards)
        {
            //update card facings and assign it to slots
            ((IBoardPile)boardPile).LayoutCards(cards);
            ((IBoardPile)boardPile).UpdateLayoutPositions(pilePosition,offset);
            
        }

        public Vector2 GetPosition()
        {
            return pilePosition;
        }

        [ContextMenu("Update positions")]
        void UpdateLayoutPositions()
        {
            ((IBoardPile)boardPile).UpdateLayoutPositions(pilePosition,offset);
        }
    }
}