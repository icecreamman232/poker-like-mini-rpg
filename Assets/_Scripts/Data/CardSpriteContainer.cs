using JustGame.Script.Card;
using UnityEngine;

namespace JustGame.Script.Data
{
    [CreateAssetMenu(menuName = "JustGame/Card Sprites")]
    public class CardSpriteContainer : ScriptableObject
    {
        [SerializeField] private Sprite[] m_sprites;

        public Sprite GetSuitSprite(CardSuit suit)
        {
            return m_sprites[(int)suit];
        }

        public Sprite GetKindSprite(CardRank kind)
        {
            return m_sprites[(int)kind + 4];
        }
    }
}

