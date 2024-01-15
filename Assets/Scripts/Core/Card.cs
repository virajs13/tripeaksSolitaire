using System;
using System.Collections;
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
        [SerializeField] private GameObject content;
        private Sequence flipUpSequence,flipDownSequence;

        public Action<Card> OnSelected;

        private RectTransform rectTransform;

        private Tween cardMoveTween;

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
            flipUpSequence.Append(content.transform.DORotate(flipRotation / 2f, flipDuration / 2f).From(Vector3.zero).OnComplete(() =>
            {
                cardBack.SetActive(false);
            }));
            flipUpSequence.Append(content.transform.DORotate(flipRotation, flipDuration / 2f).From(flipRotation / 2f));
            flipUpSequence.OnComplete((() =>
            {
                isFaceUp = true;
            }));
            
            if (!isFaceUp)
            {

                flipUpSequence.Play();
            }
         
            
        }

        [ContextMenu("Set face down")]
        public void SetFaceDown()
        {
            flipDownSequence = DOTween.Sequence();
            flipDownSequence.Append(content.transform.DORotate(flipRotation / 2f, flipDuration / 2f).From(flipRotation).OnComplete(() =>
            {
                cardBack.SetActive(true);
            }));
            flipDownSequence.Append(content.transform.DORotate(Vector3.zero, flipDuration / 2f).From(flipRotation / 2f));
            flipDownSequence.OnComplete((() =>
            {
                isFaceUp = false;
            }));
            if (isFaceUp)
            {
                flipDownSequence.Play();
            }
         
        }

        public void MoveTo(Vector2 targetPosition)
        {
            cardMoveTween = rectTransform.DOMove(targetPosition, moveDuration);
        }

        public IEnumerator WaitForCardMoveTween()
        {
            if (cardMoveTween!=null)
                yield return cardMoveTween.WaitForCompletion();
        }

        public void MoveInstant(Vector2 targetPosition)
        {
            rectTransform.position = targetPosition;
        }

        public void Shake()
        {
            content.transform.DOShakePosition(1f,Vector3.right*5f,fadeOut:true);
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