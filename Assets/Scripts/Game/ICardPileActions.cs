using System.Collections.Generic;
using TriPeaksSolitaire.Core;
using TriPeaksSolitaire.UI;

namespace TriPeaksSolitaire.Game
{
    public interface ICardMoveActions
    {
        void PerformValidMove(Card card);
        void DrawCard();
    }

    public interface ICardPileActions
    {
        void LayOutCardPiles(IEnumerable<Card> cards);
        void HandleCardClick(Card card);
        void MoveCard(Card card, ICardPileView sourcePile, ICardPileView targetPile);

        bool IsValidMove(Card cardA, Card cardB);
    }
}