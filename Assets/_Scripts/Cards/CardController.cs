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

    public enum CardRank
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
        [SerializeField] private CardRank m_kind;
        [SerializeField] private CardView m_cardView;
        [Header("UI")] 
        [SerializeField] private TextMeshProUGUI m_scoreText;

        private bool m_isSelected;

        public Action<int, bool, CardSuit, CardRank> OnSelectCard;
        
        public void Create(int index ,CardValue value)
        {
            m_index = index;
            m_suit = value.Suit;
            m_kind = value.Rank;

            m_scoreText.text = "";
        }
        
        public void Show()
        {
            m_cardView.SetView(m_index,m_suit, m_kind);
        }

        public void ShowScore()
        {
            transform.DOScale(1.15f, 0.15f).SetEase(Ease.OutExpo).SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    m_scoreText.text = string.Empty;
                });
            m_scoreText.text = GetScore().ToString();
            
            m_cardView.StopRotate();
        }

        private int GetScore()
        {
            switch (m_kind)
            {
                case CardRank.Kind_2:
                case CardRank.Kind_3:
                case CardRank.Kind_4:
                case CardRank.Kind_5:
                case CardRank.Kind_6:
                case CardRank.Kind_7:
                case CardRank.Kind_8:
                case CardRank.Kind_9:
                case CardRank.Kind_10:
                    return (int)m_kind + 2;
                case CardRank.Kind_J:
                case CardRank.Kind_Q:
                case CardRank.Kind_K:
                    return 10;
                case CardRank.Aces:
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
            
            m_cardView.StartRotate();
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

            if (m_isSelected)
            {
                m_cardView.StopRotate();
            }
            else
            {
                m_cardView.StartRotate();
            }
        }
    }
}

