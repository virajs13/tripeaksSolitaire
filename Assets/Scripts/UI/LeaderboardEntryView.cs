using UnityEngine;
using UnityEngine.UI;

namespace TriPeaksSolitaire.UI
{
    public class LeaderboardEntryView : MonoBehaviour
    {
        [SerializeField] private Text rankText;
        [SerializeField] private Text playerNameText;
        [SerializeField] private Text scoreText;

        public void SetEntryData(int rank, string playerName, int score)
        {
            rankText.text = rank.ToString();
            playerNameText.text = playerName;
            scoreText.text = score.ToString();
        }
    }
}