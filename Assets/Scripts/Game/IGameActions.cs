using System.Collections.Generic;
using TriPeaksSolitaire.Core;

namespace TriPeaksSolitaire.Game
{
    public interface IGameActions
    {
        void StartNew(IEnumerable<Card> cards);
    }
}