using System.Collections.Generic;
using TriPeaksSolitaire.Core;
using UnityEngine;

namespace TriPeaksSolitaire.UI
{
    public interface ICardPileView
    {
        void Initialise();
        ICardPile CardPile { get; }
        void LayOutCards(IEnumerable<Card> cards);

        Vector2 GetPosition();
    }
}