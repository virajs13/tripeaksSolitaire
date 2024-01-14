using System.Collections.Generic;
using TriPeaksSolitaire.Core;
using UnityEngine;

namespace TriPeaksSolitaire.UI
{
    public class DrawPileView: MonoBehaviour, ICardPileView
    {
        [SerializeField] private Vector2 offset = new Vector2(0.0015f, 0f);
        private Vector2 pilePosition;
        private IDrawPile drawPile;

        private void Awake()
        {
            pilePosition = GetComponent<RectTransform>().position;
        }
        public void Initialise()
        {
            var canvasScaleFactor = GetComponentInParent<Canvas>().scaleFactor;
            drawPile = new DrawPile(pilePosition,offset*canvasScaleFactor);
            
        }

        public ICardPile CardPile => drawPile;

        public void LayOutCards(IEnumerable<Card> cards)
        {
            drawPile.PopulateCards(cards);
        }

        public Vector2 GetPosition()
        {
            return pilePosition;
        }
    }
}