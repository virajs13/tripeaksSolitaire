using TriPeaksSolitaire.Core;

namespace TriPeaksSolitaire.Game
{
    public interface IGame
    {
        void NewGame();
        void BuyDeck();
        void CardClicked(Card card);
        bool IsValidMove(Card cardA, Card cardB);

    }
}