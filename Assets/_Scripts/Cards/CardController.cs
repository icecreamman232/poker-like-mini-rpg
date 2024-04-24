using System;
using DG.Tweening;
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
        Aces,
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
    }
    
    public class CardController : MonoBehaviour
    {
        [SerializeField] private int m_index;
        [SerializeField] private CardSuit m_suit;
        [SerializeField] private CardKind m_kind;
        [SerializeField] private CardView m_cardView;

        private bool m_isSelected;

        public Action<int, bool, CardSuit, CardKind> OnSelectCard;
        
        public void Create(int index ,CardValue value)
        {
            m_index = index;
            m_suit = value.Suit;
            m_kind = value.Kind;
        }
        
        public void Show()
        {
            m_cardView.SetView(m_suit, m_kind);
        }

        private void OnMouseDown()
        {
            m_isSelected = !m_isSelected;
            var curPosY = transform.position.y;
            transform.DOLocalMoveY(curPosY + (m_isSelected ? 1:-1), 0.3f).SetUpdate(true);
            
            OnSelectCard?.Invoke(m_index,m_isSelected,m_suit,m_kind);
        }
    }
}

