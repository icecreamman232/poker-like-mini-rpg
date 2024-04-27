using System;
using System.Collections;
using System.Collections.Generic;
using JustGame.Scripts.ScriptableEvent;
using TMPro;
using UnityEngine;

public class UpdateScoreUI : MonoBehaviour
{
    [SerializeField] private IntEvent m_scoreUpdateEvent;
    [SerializeField] private TextMeshProUGUI m_scoreText;

    private void Awake()
    {
        m_scoreUpdateEvent.AddListener(OnUpdateScore);
    }
    
    private void OnDestroy()
    {
        m_scoreUpdateEvent.RemoveListener(OnUpdateScore);
    }

    private void OnUpdateScore(int score)
    {
        m_scoreText.text = score.ToString();
    }
}
