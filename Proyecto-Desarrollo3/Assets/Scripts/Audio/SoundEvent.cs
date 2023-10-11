using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "SoundEvent")]
    public class SoundEvent : ScriptableObject
    {
        public List<SoundEventListener> listeners;

        /// <summary>
        /// Invoke event
        /// </summary>
        public void Raise()
        {
            foreach (SoundEventListener listener in listeners)
            {
                listener.OnEventRaised(this);
            }
        }
        
        /// <summary>
        /// Register listener to event
        /// </summary>
        /// <param name="listener"></param>
        public void RegisterListener(SoundEventListener listener)
        {
            if(!listeners.Contains(listener)) listeners.Add(listener);
        }
        
        /// <summary>
        /// Unregister listener to event
        /// </summary>
        /// <param name="listener"></param>
        public void UnregisterListener(SoundEventListener listener)
        {
            if(listeners.Contains(listener)) listeners.Remove(listener);
        }
    }
}