using System;
using System.Collections.Generic;
using TriPeaksSolitaire.Core;
using UnityEngine;

namespace TriPeaksSolitaire.UI
{
    public class WastePileView: MonoBehaviour, ICardPileView
    {
        private Vector2 pilePosition;
        private IWastePile wastePile;
        public ICardPile CardPile => wastePile;

        private void Start()
        {
            pilePosition = transform.position;
        }

        public void Initialise()
        {
            this.wastePile = new WastePile();
            this.wastePile.OnPileUpdated += SetTopCard;
        }

        private void SetTopCard()
        {
            var topCard = wastePile.TopCard();
            topCard.MoveTo(pilePosition);
        }
        
        public void LayOutCards(IEnumerable<Card> cards)
        {
            
        }

        public Vector2 GetPosition()
        {
            return pilePosition;
        }
    }
}