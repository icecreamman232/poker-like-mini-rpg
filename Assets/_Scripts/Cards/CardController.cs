using UnityEngine;

namespace JustGame.Script.Card
{
    public enum CardSuit
    {
        Heart,
        Diamond,
        Spade,
        Clubs,
    }

    public enum CardKind
    {
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
        [SerializeField] private CardSuit m_suit;
        [SerializeField] private CardKind m_kind;
        [SerializeField] private CardView m_cardView;

        public void Create(CardValue value)
        {
            m_suit = value.Suit;
            m_kind = value.Kind;
        }
        
        public void Show()
        {
            m_cardView.SetView(m_suit, m_kind);
        }
    }
}

