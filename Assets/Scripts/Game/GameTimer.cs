using UnityEngine;
using UnityEngine.UI;

namespace TriPeaksSolitaire.Game
{
    public interface ITimer
    {
        float ElapsedTime { get; }
        void StartTimer();
        void StopTimer();
    }


    public class GameTimer : MonoBehaviour, ITimer
    {
        public Text timerText;

        private float timer;
        private bool isGameRunning = false;

        public float ElapsedTime => timer;

        private void Start()
        {
            InitializeTimer();
            StartTimer();
        }

        private void Update()
        {
            if (isGameRunning)
            {
                UpdateTimer();
            }
        }

        private void InitializeTimer()
        {
            timer = 0f;
            UpdateTimerDisplay();
        }

        private void UpdateTimer()
        {
            timer += Time.deltaTime;
            UpdateTimerDisplay();
        }

        private void UpdateTimerDisplay()
        {
            timerText.text = FormatTime(timer);
        }

        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60F);
            int seconds = Mathf.FloorToInt(time % 60F);
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        public void StartTimer()
        {
            isGameRunning = true;
        }

        public void StopTimer()
        {
            isGameRunning = false;
        }
    }
}