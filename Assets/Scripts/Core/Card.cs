using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TriPeaksSolitaire.Core
{

    public interface ICard
    {
        CardInfo CardInfo { get; }
        bool IsSelectable { get; set; }
        void SetCardInfo(CardInfo cardInfo);
        bool IsFaceUp { get; }
        void SetFaceUp();
        void SetFaceDown();
        void MoveTo(Vector2 targetPosition);
    }
    public class Card: MonoBehaviour,ICard
    {
        private CardInfo cardInfo;
        private bool isFaceUp;

        public CardInfo CardInfo => cardInfo;

        public bool IsSelectable { get; set; } = true;

        public bool IsFaceUp => isFaceUp;
        
        private Vector3 flipRotation = new Vector3(0f, 180f, 0f);

        [SerializeField] private Image faceImage;
        [SerializeField] private GameObject cardBack;
        [SerializeField] private float flipDuration = 1f;
        [SerializeField] private float moveDuration = 1f;

        private Sequence flipUpSequence,flipDownSequence;

        public Action<ICard> OnSelected;

        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            isFaceUp = false;
        }

        public void SetCardInfo(CardInfo cardInfo)
        {
            this.cardInfo = cardInfo;
            faceImage.sprite = LoadCardFaceSprite("CardSprites/"+ cardInfo.GetCardResourcePath());
        }

        private Sprite LoadCardFaceSprite(string cardFaceSpritePath)
        {
            
            Sprite loadedSprite = Resources.Load<Sprite>(cardFaceSpritePath);

            if (loadedSprite == null)
            {
                Debug.LogError("Sprite not found at path: " + cardFaceSpritePath);
            }

            return loadedSprite;
        }

        [ContextMenu("Set face up")]
        public void SetFaceUp()
        {
            flipUpSequence = DOTween.Sequence();
            flipUpSequence.Append(transform.DORotate(flipRotation / 2f, flipDuration / 2f).SetRelative().OnComplete(() =>
            {
                cardBack.SetActive(false);
            }));
            flipUpSequence.Append(transform.DORotate(flipRotation / 2f, flipDuration / 2f).SetRelative());
            
            if (!isFaceUp)
            {

                flipUpSequence.Play();
            }
            isFaceUp = true;
            
        }

        [ContextMenu("Set face down")]
        public void SetFaceDown()
        {
            flipDownSequence = DOTween.Sequence();
            flipDownSequence.Append(transform.DORotate(flipRotation / 2f, flipDuration / 2f).SetRelative().OnComplete(() =>
            {
                cardBack.SetActive(true);
            }));
            flipDownSequence.Append(transform.DORotate(flipRotation / 2f, flipDuration / 2f).SetRelative());
            if (isFaceUp)
            {
                flipDownSequence.Play();
            }
            isFaceUp = false;
        }

        public void MoveTo(Vector2 targetPosition)
        {
            rectTransform.DOMove(targetPosition, moveDuration);
        }

        public void MoveInstant(Vector2 targetPosition)
        {
            rectTransform.position = targetPosition;
        }
        //Assigned to UI event
        public void OnCardClick()
        {
            if(!IsSelectable) return;
            Log("Selected");
            OnSelected?.Invoke(this);
        }

        
        void Log(string message)
        {
            Debug.Log($"[CARD][{cardInfo}]: {message}");
        }

#region For testing

        [ContextMenu("Set Random Card Info")]
        public void SetRandomCardInfo()
        {
            SetCardInfo(CardInfo.GetRandomCardInfo());
        }
        

#endregion
    }
}