using System;
using JustGame.Script.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JustGame.Script.Card
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private CardSpriteContainer m_spriteContainer;
        [SerializeField] private SpriteRenderer m_cardBodySprite;
        [SerializeField] private SpriteRenderer m_suitSprite;
        [SerializeField] private SpriteRenderer m_kindUpSprite;
        [SerializeField] private Color m_redColor;
        [SerializeField] private Color m_blackColor;
        [Header("Rotate")]
        [SerializeField] private float m_rotateSpeed;
        [SerializeField] private float m_maxAngle;
        private bool m_isIdle;
        private float m_angle;
        private bool m_isRotateToRight;
        
        public void SetView(int index, CardSuit suit, CardRank kind)
        {
            m_cardBodySprite.sortingOrder = index * 3;
            m_suitSprite.sortingOrder = index * 3 + 1;
            m_kindUpSprite.sortingOrder = index * 3 +2;
            
            m_suitSprite.sprite = m_spriteContainer.GetSuitSprite(suit);
            m_kindUpSprite.sprite = m_spriteContainer.GetKindSprite(kind);

            if (suit == CardSuit.Heart || suit == CardSuit.Diamond)
            {
                m_kindUpSprite.color = m_redColor;
            }
            else
            {
                m_kindUpSprite.color = m_blackColor;
            }

            StartRotate();
        }

        public void StartRotate()
        {
            m_angle = Random.Range(-m_maxAngle, m_maxAngle);
            m_isRotateToRight = Random.Range(0, 2) == 0;
            m_isIdle = true;
        }
        
        public void StopRotate()
        {
            m_isIdle = false;
            transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
        }
        

        private void Update()
        {
            if (!m_isIdle) return;
            
            transform.rotation = Quaternion.AngleAxis(m_angle, Vector3.forward);
            m_angle += m_rotateSpeed * Time.deltaTime * (m_isRotateToRight ? 1: -1);
            if (Mathf.Abs(m_angle) >= m_maxAngle)
            {
                m_isRotateToRight = !m_isRotateToRight;
            }
        }
    }
}
