using System;
using System.Collections;
using TriPeaksSolitaire.Game;
using UnityEngine;
using UnityEngine.UI;

namespace TriPeaksSolitaire.UI
{
    public class UIHandler: MonoBehaviour
    {
        
        [SerializeField] private Text scoreText;
        [SerializeField] private Text highScoreText;
        [SerializeField] private Text gameHintText;

        [SerializeField] private Button leaderBoardButton;
        [SerializeField] private Button newGameButton;


        [SerializeField] private Button buyDeckPopupButton;
        [SerializeField] private Button newGamePopupButton;
        [SerializeField] private GameObject buyDeckPopup;
        [SerializeField] private GameObject gameWinPopup;

        [SerializeField] private GameTimer gameTimer;
        [SerializeField] private LeaderboardHandler leaderboardHandler;
        public Action OnNewGameClicked;
        public Action OnBuyDeckButtonClicked;
        private void Awake()
        {
            newGameButton.onClick.AddListener(NewGameButtonClick);
            leaderBoardButton.onClick.AddListener(LeaderboardButtonClick);
            buyDeckPopupButton.onClick.AddListener(BuyDeckButtonClick);
            newGamePopupButton.onClick.AddListener(NewGameButtonClick);
        }

        private void BuyDeckButtonClick()
        {
            OnBuyDeckButtonClicked?.Invoke();
            buyDeckPopup.SetActive(false);
        }

        private void LeaderboardButtonClick()
        {
            leaderboardHandler.gameObject.SetActive(true);
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

        public void ShowBuyDeckPopup()
        {
            buyDeckPopup.SetActive(true);
        }

        public void ShowGameWinPopup()
        {
            gameWinPopup.SetActive(true);
            gameTimer.StopTimer();
        }

        public void ShowGameHintText(string hintText)
        {
            StartCoroutine(AttemptToShowGameHint(hintText));
        }

        private IEnumerator AttemptToShowGameHint(string hintText)
        {
            gameHintText.text = hintText;
            yield return new WaitForSeconds(4f);
            gameHintText.text = string.Empty;
            
        }


        private void OnDisable()
        {
            newGameButton.onClick.RemoveListener(NewGameButtonClick);
            leaderBoardButton.onClick.RemoveListener(LeaderboardButtonClick);
            buyDeckPopupButton.onClick.RemoveListener(BuyDeckButtonClick);
            newGamePopupButton.onClick.RemoveListener(NewGameButtonClick);
        }
    }
}