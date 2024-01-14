using System;
using TriPeaksSolitaire.Core;
using TriPeaksSolitaire.Game;
using UnityEngine;
using UnityEngine.UI;

namespace TriPeaksSolitaire.UI
{
    public class UIHandler: MonoBehaviour
    {
        
        [SerializeField] private Text scoreText;
        [SerializeField] private Text highScoreText;

        [SerializeField] private Button leaderBoardButton;
        [SerializeField] private Button newGameButton;


        [SerializeField] private Button buyDeckButton;
        
        
        public Action OnNewGameClicked;
        public Action OnLeaderboardClicked;
        public Action OnBuyDeckButtonClicked;
        private void Awake()
        {
            newGameButton.onClick.AddListener(NewGameButtonClick);
            leaderBoardButton.onClick.AddListener(LeaderboardButtonClick);
            buyDeckButton?.onClick.AddListener(BuyDeckButtonClick);
        }

        private void BuyDeckButtonClick()
        {
            OnBuyDeckButtonClicked?.Invoke();
        }

        private void LeaderboardButtonClick()
        {
            OnLeaderboardClicked?.Invoke();
        }

        private void NewGameButtonClick()
        {
            OnNewGameClicked?.Invoke();
        }

        public void UpdateScore(IScore scoreBoard)
        {
            scoreText.text = scoreBoard.Score.ToString();
            highScoreText.text = scoreBoard.HighScore.ToString();
        }

        private void OnDisable()
        {
            newGameButton.onClick.RemoveListener(NewGameButtonClick);
            leaderBoardButton.onClick.RemoveListener(LeaderboardButtonClick);
            buyDeckButton?.onClick.RemoveListener(BuyDeckButtonClick);
        }
    }
}