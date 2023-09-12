using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace BarbaGames.Game.Audio
{
    public class SoundEventListener : MonoBehaviourSingleton<SoundEventListener>
    {
        [System.Serializable]
        public class EventResponse
        {
            public SoundEvent soundEvent;
            public UnityEvent response;
        }

        [SerializeField] private List<EventResponse> eventResponses;
        
        private void OnEnable()
        {
            foreach (var eventResponse in eventResponses)
            {
                eventResponse.soundEvent.RegisterListener(this);
            }
        }

        private void OnDisable()
        {
            foreach (var eventResponse in eventResponses)
            {
                eventResponse.soundEvent.UnregisterListener(this);
            }
        }
        
        /// <summary>
        /// Invokes all references of the event
        /// </summary>
        /// <param name="soundEvent"></param>
        public void OnEventRaised(SoundEvent soundEvent)
        {
            foreach (var eventResponse in eventResponses.Where(eventResponse => eventResponse.soundEvent == soundEvent))
            {
                eventResponse.response.Invoke();
                break;
            }
        }
    }
}