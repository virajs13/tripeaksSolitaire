using System.Collections.Generic;
using TriPeaksSolitaire.UI;
using UnityEngine;

namespace TriPeaksSolitaire.Game
{
   public class LeaderboardHandler : MonoBehaviour
{
    [SerializeField] private Transform leaderboardContainer;
    [SerializeField] private GameObject leaderboardEntryPrefab;

    private List<PlayerScore> playerScores = new List<PlayerScore>();

    [System.Serializable]
    private class PlayerScore
    {
        public string playerName;
        public int score;
    }

    [System.Serializable]
    private class LeaderboardData
    {
        public List<PlayerScore> playerScores;
    }

    void Start()
    {
        LoadMockData();
        SortAndDisplayLeaderboard();
    }

    private void LoadMockData()
    {
        string json = Resources.Load<TextAsset>("MockLeaderboardData").text;
        LeaderboardData leaderboardData = JsonUtility.FromJson<LeaderboardData>(json);

        if (leaderboardData != null)
        {
            playerScores = leaderboardData.playerScores;
        }
        else
        {
            Debug.LogError("Failed to load mock leaderboard data.");
        }
    }

    public void AddPlayerScore(string playerName, int score)
    {
        PlayerScore newScore = new PlayerScore { playerName = playerName, score = score };
        playerScores.Add(newScore);

        SortAndDisplayLeaderboard();
    }

    private void SortAndDisplayLeaderboard()
    {
        // Sort the leaderboard based on scores
        playerScores.Sort((a, b) => b.score.CompareTo(a.score));

        // Display leaderboard
        UpdateLeaderboardUI();
    }

    private void UpdateLeaderboardUI()
    {
        // Clear existing entries
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        // Instantiate and populate entries
        for (int i = 0; i < playerScores.Count; i++)
        {
            InstantiateLeaderboardEntry(i + 1, playerScores[i].playerName, playerScores[i].score);
        }
    }

    private void InstantiateLeaderboardEntry(int rank, string playerName, int score)
    {
        GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
        LeaderboardEntryView entryView = entry.GetComponent<LeaderboardEntryView>();

        if (entryView != null)
        {
            entryView.SetEntryData(rank, playerName, score);
        }
        else
        {
            Debug.LogError("Leaderboard entry prefab is missing LeaderboardEntryUI component.");
        }
    }
}
}