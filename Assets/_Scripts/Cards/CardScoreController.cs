using System.Collections.Generic;
using JustGame.Scripts.Attribute;
using JustGame.Scripts.ScriptableEvent;
using UnityEngine;

namespace JustGame.Script.Card
{
    public class CardScoreController : MonoBehaviour
    {
        [SerializeField] private int m_initChallengerScore;
        [SerializeField] private IntEvent m_challengerScoreEvent;
        [SerializeField] private IntEvent m_playerScoreEvent;
        [SerializeField] [ReadOnly] private int m_playerScore;
        
        private void Start()
        {
            m_challengerScoreEvent.Raise(m_initChallengerScore);
        }
        public void CalculateScore(List<CardValue> selectedCardList, List<int> cardRuleIndexListOfWinningHand)
        {
            int score = 0;
            for (int i = 0; i < cardRuleIndexListOfWinningHand.Count; i++)
            {
                var rank = selectedCardList[cardRuleIndexListOfWinningHand[i]].Rank;
                score += GetScore(rank);
            }

            UpdatePlayerScore(score);
        }
        
        private int GetScore(CardRank rank)
        {
            switch (rank)
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
                    return (int)rank + 2;
                case CardRank.Kind_J:
                case CardRank.Kind_Q:
                case CardRank.Kind_K:
                    return 10;
                case CardRank.Aces:
                    return 12;
            }
            return -1;
        }

        
        private void UpdatePlayerScore(int score)
        {
            m_playerScore += score;
            m_playerScoreEvent.Raise(m_playerScore);
        }

        
    }
}

