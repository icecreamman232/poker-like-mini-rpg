using System;
using System.Collections.Generic;
using JustGame.Script.Card;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class CardValue
{
    public CardSuit Suit;
    public CardKind Kind;
}

[Serializable]
public struct CardCounter
{
    public CardKind Kind;
    public int Amount;
}

public enum PokerHands
{
    Royal_Flush,
    Straight_Flush,
    Four_A_Kind,
    Full_House,
    Flush,
    Straight,
    Three_A_Kind,
    Two_Pair,
    Pair,
    No_hands,
}

public class CardManager : MonoBehaviour
{
    [SerializeField] private int m_numberCardToCreate;
    [SerializeField] private bool m_isUseForceHand;
    [SerializeField] private CardValue[] m_forceHand;
    [SerializeField] private Transform[] m_pivotArray;
    [SerializeField] private CardController m_cardPrefab;

    [SerializeField]private List<CardSuit> m_suitCardSelectedList;
    [SerializeField]private List<CardKind> m_kindCardSelectedList;
    [SerializeField]private CardCounter[] m_cardCounterArr;
    
    private void Start()
    {
        if (m_isUseForceHand)
        {
            for (int i = 0; i < m_numberCardToCreate; i++)
            {
                var card = Instantiate(m_cardPrefab, m_pivotArray[i].position,Quaternion.identity);
                card.Create(i,m_forceHand[i]);
                card.Show();
                card.OnSelectCard += OnSelectCard;
            }
        }
        else
        {
            for (int i = 0; i < m_numberCardToCreate; i++)
            {
                var card = Instantiate(m_cardPrefab, m_pivotArray[i].position,Quaternion.identity);
                card.Create(i,GetRandomCardValue());
                card.Show();
                card.OnSelectCard += OnSelectCard;
            }
        }

        m_cardCounterArr = new CardCounter[13];

        for (int i = 0; i < 13; i++)
        {
            m_cardCounterArr[i].Kind = (CardKind)i;
            m_cardCounterArr[i].Amount = 0;
        }
        

        m_suitCardSelectedList = new List<CardSuit>();
        for (int i = 0; i < 5; i++)
        {
            m_suitCardSelectedList.Add(CardSuit.None);
        }
        
        m_kindCardSelectedList = new List<CardKind>();
        for (int i = 0; i < 5; i++)
        {
            m_kindCardSelectedList.Add(CardKind.None);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CheckHandAndPrintDebug();
        }
    }


    private CardValue GetRandomCardValue()
    {
        var newCard = new CardValue();
        newCard.Suit = (CardSuit)Random.Range(0, 4);
        newCard.Kind = (CardKind)Random.Range(0, 13);
        return newCard;
    }

    private void OnSelectCard(int index,bool isSelected, CardSuit suit, CardKind kind)
    {
        if (isSelected)
        {
            m_suitCardSelectedList[index] = suit;
            m_kindCardSelectedList[index] = kind;
        }
        else
        {
            m_suitCardSelectedList[index] = CardSuit.None;
            m_kindCardSelectedList[index] = CardKind.None;
        }
        
    }

    #region Check hand methods
    private bool IsRoyalFlushHand()
    {
        //Check royal straight
        bool isStraight = m_kindCardSelectedList[0] == CardKind.Aces
                          && m_kindCardSelectedList[1] == CardKind.Kind_K
                          && m_kindCardSelectedList[2] == CardKind.Kind_Q
                          && m_kindCardSelectedList[3] == CardKind.Kind_J
                          && m_kindCardSelectedList[4] == CardKind.Kind_10;


        if (!isStraight)
        {
            return false;
        }
        
        //Check flush
        for (int i = 0; i < m_suitCardSelectedList.Count; i++)
        {
            if (m_suitCardSelectedList[i] == CardSuit.None)
            {
                return false;
            }
            
            if (m_suitCardSelectedList[i] != m_suitCardSelectedList[0])
            {
                return false;
            }
        }

        return true;
    }

    private bool IsStraightFlush()
    {
        //Check if it's RoyalFlush
        if (m_kindCardSelectedList[0] == CardKind.Aces)
        {
            return false;
        }
        
        //Check flush
        for (int i = 0; i < m_suitCardSelectedList.Count; i++)
        {
            if (m_suitCardSelectedList[i] == CardSuit.None)
            {
                return false;
            }
            
            if (m_suitCardSelectedList[i] != m_suitCardSelectedList[0])
            {
                return false;
            }
        }

        
        for (int i = 0; i < m_kindCardSelectedList.Count-1; i++)
        {
            if (m_kindCardSelectedList[i] == CardKind.None)
            {
                return false;
            }

            if ((int)m_kindCardSelectedList[i] + 1 != (int)m_kindCardSelectedList[i + 1])
            {
                return false;
            }
        }
        
        return false;
    }

    private bool IsFourOfAKindHand()
    {
        CardKind isSameKind = m_kindCardSelectedList[0];
        var numberSameKind = 1;
        
        for (int i = 1; i < m_kindCardSelectedList.Count; i++)
        {
            if(m_kindCardSelectedList[i] == CardKind.None) continue;
            if (m_kindCardSelectedList[i] == isSameKind)
            {
                numberSameKind++;
            }
            else
            {
                isSameKind = m_kindCardSelectedList[i];
                numberSameKind = 1;
            }
        }
        
        if (numberSameKind == 4)
        {
            return true;
        }
        
        return false;
    }

    private bool IsFullHouseHand()
    {
        ResetCounterArray();
        
        for (int i = 0; i < m_kindCardSelectedList.Count; i++)
        {
            if(m_kindCardSelectedList[i] == CardKind.None) continue;
            m_cardCounterArr[(int)m_kindCardSelectedList[i]].Amount++;
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
    

    private bool IsTwoPair()
    {
        
        return false;
    }

    private void ResetCounterArray()
    {
        for (int i = 0; i < m_cardCounterArr.Length; i++)
        {
            m_cardCounterArr[i].Amount = 0;
        }
    }
    
    private PokerHands CheckHands()
    {
        if(IsRoyalFlushHand())
        {
            return PokerHands.Royal_Flush;
        }

        if (IsStraightFlush())
        {
            return PokerHands.Straight_Flush;
        }

        if (IsFourOfAKindHand())
        {
            return PokerHands.Four_A_Kind;
        }

        if (IsFullHouseHand())
        {
            return PokerHands.Full_House;
        }
        
        
        
        
        if (IsTwoPair())
        {
            return PokerHands.Two_Pair;
        }
        
        return PokerHands.No_hands;
    }
    
    //private 
    
    #endregion
    
    #region Test method
    [ContextMenu("Test hands")]
    private void CheckHandAndPrintDebug()
    {
        var hand = CheckHands();
        Debug.Log($"Hand is {hand}");
    }
    
    #endregion
}
