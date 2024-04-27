using System;
using UnityEngine;

namespace JustGame.Scripts.ScriptableEvent
{
    [CreateAssetMenu(menuName = "JustGame/Scriptable Event/Vector2 Event")]
    public class Vector2Event : ScriptableObject
    {
        protected Action<Vector2> m_listeners;
        
        public void AddListener(Action<Vector2> addListener)
        {
            m_listeners += addListener;
        }

        public void RemoveListener(Action<Vector2> removeListener)
        {
            m_listeners -= removeListener;
        }

        public void Raise(Vector2 value)
        {
            m_listeners?.Invoke(value);
        }
    }

}
