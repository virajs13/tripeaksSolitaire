using System.Collections.Generic;
using TriPeaksSolitaire.Core;
using UnityEngine;

namespace TriPeaksSolitaire.UI
{
    public class BoardPileView: MonoBehaviour,ICardPileView
    {
        private ICardPile boardPile;

        public void Initialise()
        {
            throw new System.NotImplementedException();
        }

        public ICardPile CardPile => boardPile;

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