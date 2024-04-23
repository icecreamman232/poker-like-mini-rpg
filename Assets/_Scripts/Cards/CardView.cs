using JustGame.Script.Data;
using UnityEngine;

namespace JustGame.Script.Card
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private CardSpriteContainer m_spriteContainer;
        [SerializeField] private SpriteRenderer m_suitSprite;
        [SerializeField] private SpriteRenderer m_kindUpSprite;
        [SerializeField] private SpriteRenderer m_kindDownSprite;
        [SerializeField] private Color m_redColor;
        [SerializeField] private Color m_blackColor;

        public void SetView(CardSuit suit, CardKind kind)
        {
            m_suitSprite.sprite = m_spriteContainer.GetSuitSprite(suit);
            m_kindUpSprite.sprite = m_spriteContainer.GetKindSprite(kind);
            m_kindDownSprite.sprite = m_spriteContainer.GetKindSprite(kind);

            if (suit == CardSuit.Heart || suit == CardSuit.Diamond)
            {
                m_kindUpSprite.color = m_redColor;
                m_kindDownSprite.color = m_redColor;
            }
            else
            {
                m_kindUpSprite.color = m_blackColor;
                m_kindDownSprite.color = m_blackColor;
            }
        }
    }
}
