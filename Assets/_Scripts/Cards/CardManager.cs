using System;
using System.Collections;
using System.Collections.Generic;
using JustGame.Script.Card;
using JustGame.Scripts.Attribute;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class CardValue
{
    public CardSuit Suit;
    public CardRank Rank;
}

[Serializable]
public struct CardCounter
{
    public CardRank Kind;
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
    High_Card,
}

public class CardManager : MonoBehaviour
{
    [SerializeField] private int m_numberCardToCreate;
    [SerializeField] private bool m_isUseForceHand;
    [SerializeField] private CardRule m_cardRule;
    [SerializeField] private CardValue[] m_forceHand;
    [SerializeField] private Transform[] m_pivotArray;
    [SerializeField] private CardController m_cardPrefab;
    
    [SerializeField] [ReadOnly] private List<CardValue> m_selectedCardList;
    [SerializeField] [ReadOnly] private List<CardController> m_listCardGO;

    private bool m_isFightingCard;
    private bool m_isDiscarding;
    
    public void FightCurrentHand()
    {
        StartCoroutine(FightRoutine());
    }

    private IEnumerator FightRoutine()
    {
        if (m_isFightingCard)
        {
            yield break;
        }

        
        var hands = m_cardRule.CheckHand(m_selectedCardList);
        
        m_isFightingCard = true;

        if (hands == PokerHands.High_Card)
        {
            var highCardIndex = FindBiggestRank();
            //Show score on first card of high card hands
            m_listCardGO[highCardIndex].ShowScore();
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            for (int i = 0; i < m_selectedCardList.Count; i++)
            {
                if (m_selectedCardList[i].Rank != CardRank.None)
                {
                    m_listCardGO[i].ShowScore();
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
        
       
        
        
        //Destroy selected hand
        for (int i = 0; i < m_selectedCardList.Count; i++)
        {
            if (m_selectedCardList[i].Rank != CardRank.None)
            {
                m_selectedCardList[i].Rank = CardRank.None;
                m_selectedCardList[i].Suit = CardSuit.None;
                Destroy(m_listCardGO[i].gameObject);
                m_listCardGO[i] = null;
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        CreateNewCards();

        m_isFightingCard = false;

    }


    public void DiscardSelectedCards()
    {
        StartCoroutine(DiscardRoutine());
    }

    private IEnumerator DiscardRoutine()
    {
        if (m_isDiscarding)
        {
            yield break;
        }

        m_isDiscarding = true;
        
        //Destroy selected hand
        for (int i = 0; i < m_selectedCardList.Count; i++)
        {
            if (m_selectedCardList[i].Rank != CardRank.None)
            {
                m_selectedCardList[i].Rank = CardRank.None;
                m_selectedCardList[i].Suit = CardSuit.None;
                Destroy(m_listCardGO[i].gameObject);
                m_listCardGO[i] = null;
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        CreateNewCards();
        
        m_isDiscarding = false;
    }
    
    private void Start()
    {
        m_listCardGO = new List<CardController>();
        
        if (m_isUseForceHand)
        {
            for (int i = 0; i < m_numberCardToCreate; i++)
            {
                var card = Instantiate(m_cardPrefab, m_pivotArray[i].position,Quaternion.identity);
                card.Create(i,m_forceHand[i]);
                card.Show();
                card.OnSelectCard += OnSelectCard;
                m_listCardGO.Add(card);
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
                m_listCardGO.Add(card);
            }
        }

        m_selectedCardList = new List<CardValue>();

        for (int i = 0; i < 5; i++)
        {
            m_selectedCardList.Add(new CardValue()
            {
                Rank = CardRank.None,
                Suit = CardSuit.None
            });
        }
        
    }
    
    /// <summary>
    /// Generate new cards that go to empty slot of the hands
    /// </summary>
    private void CreateNewCards()
    {
        for (int i = 0; i < m_listCardGO.Count; i++)
        {
            if (m_listCardGO[i] == null)
            {
                var card = Instantiate(m_cardPrefab, m_pivotArray[i].position,Quaternion.identity);
                card.Create(i,GetRandomCardValue());
                card.Show();
                card.OnSelectCard += OnSelectCard;
                m_listCardGO[i] = card;
            }
        }
    }


    private CardValue GetRandomCardValue()
    {
        var newCard = new CardValue();
        newCard.Suit = (CardSuit)Random.Range(0, 4);
        newCard.Rank = (CardRank)Random.Range(0, 13);
        return newCard;
    }

    private void OnSelectCard(int index,bool isSelected, CardSuit suit, CardRank rank)
    {
        if (isSelected)
        {
            m_selectedCardList[index].Suit = suit;
            m_selectedCardList[index].Rank = rank;
        }
        else
        {
            m_selectedCardList[index].Suit = CardSuit.None;
            m_selectedCardList[index].Rank = CardRank.None;
        }
        
    }

    private int FindBiggestRank()
    {
        var curRank = CardRank.None;
        var result = 0;
        for (int i = 0; i < m_selectedCardList.Count; i++)
        {
            if ((int)m_selectedCardList[i].Rank > (int)curRank)
            {
                curRank = m_selectedCardList[i].Rank;
                result = i;
            }
        }
        return result;
    }

    
}
