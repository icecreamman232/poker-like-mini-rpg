using System;
using UnityEngine;

namespace JustGame.Script.Data
{
    [CreateAssetMenu(menuName = "JustGame/Scriptable Event/Transform Event")]
    public class TransformEvent : ScriptableObject
    {
        protected Action<Transform> m_listeners;
        
        public void AddListener(Action<Transform> addListener)
        {
            m_listeners += addListener;
        }

        public void RemoveListener(Action<Transform> removeListener)
        {
            m_listeners -= removeListener;
        }

        public void Raise(Transform value)
        {
            m_listeners?.Invoke(value);
        }
    }
}

