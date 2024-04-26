using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace JustGame.Script.Card
{
    public enum CardSuit
    {
        None =-1,
        Heart,
        Diamond,
        Spade,
        Clubs,
    }

    public enum CardKind
    {
        None =-1,
        Kind_2,
        Kind_3,
        Kind_4,
        Kind_5,
        Kind_6,
        Kind_7,
        Kind_8,
        Kind_9,
        Kind_10,
        Kind_J,
        Kind_Q,
        Kind_K,
        Aces
    }
    
    public class CardController : MonoBehaviour
    {
        [SerializeField] private int m_index;
        [SerializeField] private CardSuit m_suit;
        [SerializeField] private CardKind m_kind;
        [SerializeField] private CardView m_cardView;
        [Header("UI")] 
        [SerializeField] private TextMeshProUGUI m_scoreText;

        private bool m_isSelected;

        public Action<int, bool, CardSuit, CardKind> OnSelectCard;
        
        public void Create(int index ,CardValue value)
        {
            m_index = index;
            m_suit = value.Suit;
            m_kind = value.Kind;

            m_scoreText.text = "";
        }
        
        public void Show()
        {
            m_cardView.SetView(m_suit, m_kind);
        }

        public void ShowScore()
        {
            transform.DOScale(1.15f, 0.15f).SetEase(Ease.OutExpo).SetLoops(2, LoopType.Yoyo);
            m_scoreText.text = GetScore().ToString();
        }

        private int GetScore()
        {
            switch (m_kind)
            {
                case CardKind.Kind_2:
                case CardKind.Kind_3:
                case CardKind.Kind_4:
                case CardKind.Kind_5:
                case CardKind.Kind_6:
                case CardKind.Kind_7:
                case CardKind.Kind_8:
                case CardKind.Kind_9:
                case CardKind.Kind_10:
                    return (int)m_kind + 2;
                case CardKind.Kind_J:
                case CardKind.Kind_Q:
                case CardKind.Kind_K:
                    return 10;
                case CardKind.Aces:
                    return 12;
            }
            return -1;
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(1) && m_isSelected)
            {
                DeselectCard();
            }
        }

        private void DeselectCard()
        {
            m_isSelected = false;
                            
            var curPosY = transform.position.y;
            transform.DOLocalMoveY(curPosY + (m_isSelected ? 1:-1), 0.3f).SetUpdate(true);
        
            OnSelectCard?.Invoke(m_index,m_isSelected,m_suit,m_kind);
        }

        public void ReturnCard()
        {
            transform.DOShakeRotation(0.3f, 50, 30, 45, randomnessMode: ShakeRandomnessMode.Harmonic).
                OnComplete(DeselectCard);
        }

        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_isSelected = !m_isSelected;
            }
            
            var curPosY = transform.position.y;
            transform.DOLocalMoveY(curPosY + (m_isSelected ? 1:-1), 0.3f).SetUpdate(true);
            
            OnSelectCard?.Invoke(m_index,m_isSelected,m_suit,m_kind);
        }
    }
}

