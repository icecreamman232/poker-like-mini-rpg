using System;
using System.Collections.Generic;
using JustGame.Scripts.Attribute;
using UnityEngine;

namespace JustGame.Script.Card
{
    /// <summary>
    /// Store rules and how to calculate it in selected hands
    /// </summary>
    public class CardRule : MonoBehaviour
    {
        [SerializeField][ReadOnly] private CardCounter[] m_cardCounterArr;

        private void Awake()
        {
            m_cardCounterArr = new CardCounter[13];

            for (int i = 0; i < 13; i++)
            {
                m_cardCounterArr[i].Kind = (CardRank)i;
                m_cardCounterArr[i].Amount = 0;
            }
        }

        public PokerHands CheckHand(List<CardValue> selectedCardList)
        {
            if(IsRoyalFlushHand(selectedCardList))
            {
                return PokerHands.Royal_Flush;
            }

            if (IsStraightFlush(selectedCardList))
            {
                return PokerHands.Straight_Flush;
            }

            if (IsFourOfAKindHand(selectedCardList))
            {
                return PokerHands.Four_A_Kind;
            }

            if (IsFullHouseHand(selectedCardList))
            {
                return PokerHands.Full_House;
            }

            if (IsFlushHand(selectedCardList))
            {
                return PokerHands.Flush;
            }

            if (IsStraightHand(selectedCardList))
            {
                return PokerHands.Straight;
            }

            if (IsThreeKindHand(selectedCardList))
            {
                return PokerHands.Three_A_Kind;
            }
        
            if (IsTwoPair(selectedCardList))
            {
                return PokerHands.Two_Pair;
            }

            if (IsPair(selectedCardList))
            {
                return PokerHands.Pair;
            }
        
            return PokerHands.High_Card;
        }
        
        #region Check hand methods
        private bool IsRoyalFlushHand(List<CardValue> selectedCardList)
        {
            //Check royal straight
            bool isStraight = selectedCardList[0].Rank == CardRank.Aces
                              && selectedCardList[1].Rank == CardRank.Kind_K
                              && selectedCardList[2].Rank == CardRank.Kind_Q
                              && selectedCardList[3].Rank == CardRank.Kind_J
                              && selectedCardList[4].Rank == CardRank.Kind_10;


            if (!isStraight)
            {
                return false;
            }
            
            //Check flush
            for (int i = 0; i < selectedCardList.Count; i++)
            {
                if (selectedCardList[i].Suit == CardSuit.None)
                {
                    return false;
                }
                
                if (selectedCardList[i].Suit != selectedCardList[0].Suit)
                {
                    return false;
                }
            }

            return true;
        } //Check

        private bool IsStraightFlush(List<CardValue> selectedCardList) //Check
        {
            //Check if it's RoyalFlush
            if (selectedCardList[0].Rank == CardRank.Aces)
            {
                return false;
            }
            
            //Check flush
            for (int i = 0; i < selectedCardList.Count; i++)
            {
                if (selectedCardList[i].Suit == CardSuit.None)
                {
                    return false;
                }
                
                if (selectedCardList[i].Suit != selectedCardList[0].Suit)
                {
                    return false;
                }
            }

            
            for (int i = 0; i < selectedCardList.Count-1; i++)
            {
                if (selectedCardList[i].Rank == CardRank.None)
                {
                    return false;
                }

                if ((int)selectedCardList[i].Rank + 1 != (int)selectedCardList[i + 1].Rank)
                {
                    return false;
                }
            }
            
            return true;
        }

        private bool IsFourOfAKindHand(List<CardValue> selectedCardList) //Check
        {
            CardRank isSameKind = selectedCardList[0].Rank;
            var numberSameKind = 1;
            
            for (int i = 1; i < selectedCardList.Count; i++)
            {
                if(selectedCardList[i].Rank == CardRank.None) continue;
                if (selectedCardList[i].Rank == isSameKind)
                {
                    numberSameKind++;
                }
                else
                {
                    isSameKind = selectedCardList[i].Rank;
                    numberSameKind = 1;
                }
            }
            
            if (numberSameKind == 4)
            {
                return true;
            }
            
            return false;
        }

        private bool IsFullHouseHand(List<CardValue>  selectedCardList) //Check
        {
            ResetCounterArray();
            for (int i = 0; i < selectedCardList.Count; i++)
            {
                if(selectedCardList[i].Rank == CardRank.None) continue;
                m_cardCounterArr[(int)selectedCardList[i].Rank].Amount++;
            }

            int numberOf3Kind = 0;
            int numberOfPair = 0;
            
            for (int i = 0; i < m_cardCounterArr.Length; i++)
            {
                if (m_cardCounterArr[i].Amount == 3)
                {
                    numberOf3Kind++;
                    continue;
                }
                
                if (m_cardCounterArr[i].Amount == 2)
                {
                    numberOfPair++;
                }
            }

            if (numberOf3Kind == 1 && numberOfPair == 1)
            {
                return true;
            }
            
            return false;
        }


        private bool IsFlushHand(List<CardValue>  selectedCardList) //Check
        {
            for (int i = 0; i < selectedCardList.Count - 1; i++)
            {
                if (selectedCardList[i].Suit == CardSuit.None)
                {
                    return false;
                }

                if (selectedCardList[i].Suit != selectedCardList[i + 1].Suit)
                {
                    return false;
                }
            }
            
            return true;
        }

        private bool IsStraightHand(List<CardValue>  selectedCardList) //Check
        {
            for (int i = 0; i < selectedCardList.Count - 1; i++)
            {
                if (selectedCardList[i].Rank == CardRank.None)
                {
                    return false;
                }

                if ((int)(selectedCardList[i].Rank + 1) != (int)selectedCardList[i + 1].Rank)
                {
                    return false;
                }
            }
            
            return true;
        }

        private bool IsThreeKindHand(List<CardValue>  selectedCardList) //Check
        {
            ResetCounterArray();
            
            for (int i = 0; i < selectedCardList.Count; i++)
            {
                if(selectedCardList[i].Rank == CardRank.None) continue;
                m_cardCounterArr[(int)selectedCardList[i].Rank].Amount++;
            }

            bool potentialHasThreeKind = false;
            
            for (int i = 0; i < m_cardCounterArr.Length; i++)
            {
                if (m_cardCounterArr[i].Amount == 3)
                {
                    potentialHasThreeKind = true;
                }
            }

            if (potentialHasThreeKind)
            {
                var numberSelectedCard = 0;
                for (int i = 0; i < selectedCardList.Count; i++)
                {
                    if (selectedCardList[i].Rank != CardRank.None)
                    {
                        numberSelectedCard++;
                    }
                }

                if (numberSelectedCard > 3)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            
            return false;
        }
        
        
        private bool IsTwoPair(List<CardValue>  selectedCardList) //Check
        {
            ResetCounterArray();
            
            for (int i = 0; i < selectedCardList.Count; i++)
            {
                if(selectedCardList[i].Rank == CardRank.None) continue;
                m_cardCounterArr[(int)selectedCardList[i].Rank].Amount++;
            }

            int pairNumber = 0;
            
            for (int i = 0; i < m_cardCounterArr.Length; i++)
            {
                if (m_cardCounterArr[i].Amount == 2)
                {
                    pairNumber++;
                }
            }

            bool potentialHasPair = pairNumber >= 2;
            

            if (potentialHasPair)
            {
                var numberSelectedCard = 0;
                for (int i = 0; i < selectedCardList.Count; i++)
                {
                    if (selectedCardList[i].Rank != CardRank.None)
                    {
                        numberSelectedCard++;
                    }
                }

                if (numberSelectedCard > 4)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            
            
            return false;
        }

        private bool IsPair(List<CardValue>  selectedCardList) //Check
        {
            ResetCounterArray();
            for (int i = 0; i < selectedCardList.Count; i++)
            {
                if(selectedCardList[i].Rank == CardRank.None) continue;
                m_cardCounterArr[(int)selectedCardList[i].Rank].Amount++;
            }
            
            for (int i = 0; i < m_cardCounterArr.Length; i++)
            {
                if (m_cardCounterArr[i].Amount == 2)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        #endregion
    
        private void ResetCounterArray()
        {
            for (int i = 0; i < m_cardCounterArr.Length; i++)
            {
                m_cardCounterArr[i].Amount = 0;
            }
        }
    }
}

