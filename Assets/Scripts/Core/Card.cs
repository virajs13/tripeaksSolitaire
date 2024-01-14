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

        private Sequence flipSequence;

        public Action<ICard> OnSelected;

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
            flipSequence = DOTween.Sequence();
            flipSequence.Append(transform.DORotate(flipRotation / 2f, flipDuration / 2f).SetRelative().OnComplete(() =>
            {
                cardBack.SetActive(false);
            }));
            flipSequence.Append(transform.DORotate(flipRotation / 2f, flipDuration / 2f).SetRelative().OnComplete(() =>
            {
                isFaceUp = true;
            }));
            
            if (!isFaceUp)
            {

                flipSequence.Play();
            }
            
        }

        [ContextMenu("Set face down")]
        public void SetFaceDown()
        {
            flipSequence = DOTween.Sequence();
            flipSequence.Append(transform.DORotate(flipRotation / 2f, flipDuration / 2f).SetRelative().OnComplete(() =>
            {
                cardBack.SetActive(true);
            }));
            flipSequence.Append(transform.DORotate(flipRotation / 2f, flipDuration / 2f).SetRelative().OnComplete(() =>
            {
                isFaceUp = false;
            }));
            if (isFaceUp)
            {
                flipSequence.Play();
            }
        }

        public void MoveTo(Vector2 targetPosition)
        {
            transform.DOMove(targetPosition, moveDuration);
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