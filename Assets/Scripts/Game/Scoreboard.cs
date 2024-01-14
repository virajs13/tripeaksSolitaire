using System.Collections.Generic;
using TriPeaksSolitaire.Core;
using UnityEngine;

namespace TriPeaksSolitaire.Game
{
    public class Scoreboard: IScore,IScoreBoard
    {
        private int score;
        private int highScore;

        public int Score => score;

        public int HighScore => highScore;
        
        private int currentMove = 0;

        private const int ScorePerMove = 1;

      


        public Scoreboard()
        {
            GetHighScore();
        }

        public void Reset()
        {
            currentMove = 0;
        }

        public void IncrementMove()
        {
            currentMove++;
            score += CalculateScore();
            SetHighScore(score);
            Log($"Moves: {currentMove} Score: {score}");
        }

        void  GetHighScore()
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        void SetHighScore(int score)
        {
            if (score <= highScore) return;
            highScore = score;
            // Save the new high score to PlayerPrefs or another storage solution.
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        private int CalculateScore()
        {
            if (currentMove <= 1)
            {
                return 0;
            }
            return ScorePerMove;
        }
        
        void Log(string message)
        {
            Debug.Log($"[SCOREBOARD]: {message}");
        }
        
    }
}