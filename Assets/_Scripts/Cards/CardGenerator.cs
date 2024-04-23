using System;
using JustGame.Script.Card;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class CardValue
{
    public CardSuit Suit;
    public CardKind Kind;
}

public class CardGenerator : MonoBehaviour
{
    [SerializeField] private int m_numberCardToCreate;
    [SerializeField] private Transform[] m_pivotArray;
    [SerializeField] private CardController m_cardPrefab;

    private void Start()
    {
        for (int i = 0; i < m_numberCardToCreate; i++)
        {
            var card = Instantiate(m_cardPrefab, m_pivotArray[i].position,Quaternion.identity);
            card.Create(GetRandomCardValue());
            card.Show();
        }
    }

    private CardValue GetRandomCardValue()
    {
        var newCard = new CardValue();
        newCard.Suit = (CardSuit)Random.Range(0, 4);
        newCard.Kind = (CardKind)Random.Range(0, 13);
        return newCard;
    }
}
