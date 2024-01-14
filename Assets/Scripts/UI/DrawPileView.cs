using System.Collections.Generic;
using TriPeaksSolitaire.Core;
using UnityEngine;

namespace TriPeaksSolitaire.UI
{
    public class DrawPileView: MonoBehaviour, ICardPileView
    {
        private IDrawPile drawPile;

        public void Initialise()
        {
            drawPile = new DrawPile();
            
        }

        public ICardPile CardPile => drawPile;

        public void LayOutCards(IEnumerable<Card> cards)
        {
            throw new System.NotImplementedException();
        }

        public Vector2 GetPosition()
        {
            throw new System.NotImplementedException();
        }
    }
}