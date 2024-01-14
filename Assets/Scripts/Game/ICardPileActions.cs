using System.Collections.Generic;
using TriPeaksSolitaire.Core;
using TriPeaksSolitaire.UI;

namespace TriPeaksSolitaire.Game
{
    public interface ICardPileActions
    {
        void MoveCard(Card card, ICardPileView sourcePile, ICardPileView targetPile);
        void LayOutCardPiles(IEnumerable<Card> cards);
        void PerformValidMove(Card card);
        void DrawCard();

    }
}