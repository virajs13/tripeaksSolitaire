namespace TriPeaksSolitaire.Game
{
    public interface IScore
    {
        int Score { get; }
        int HighScore { get; }
    }

    public interface IScoreBoard
    {
        void Reset();
        void IncrementMove();
    }
}