using System;
using System.Collections.Generic;
using TriPeaksSolitaire.Core;
using UnityEngine;

namespace TriPeaksSolitaire.UI
{
    public class WastePileView: MonoBehaviour, ICardPileView
    {
       
        private IWastePile wastePile;
        public ICardPile CardPile => wastePile;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

        }

        public void Initialise()
        {
            this.wastePile = new WastePile();
            this.wastePile.OnPileUpdated += SetTopCard;
        }

        private void SetTopCard()
        {
            var topCard = wastePile.TopCard();
            if (topCard)
            {
                topCard.transform.SetAsLastSibling();
                topCard.MoveTo(GetPosition());
            }
        }
        
        public void LayOutCards(IEnumerable<Card> cards)
        {
            
        }

        public Vector2 GetPosition()
        {
            return rectTransform.position;
        }
    }
}