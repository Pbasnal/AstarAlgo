using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class AgentMemory : MonoBehaviour
    {
        public Dictionary<string, GameObject> gameObjects;

        public Dictionary<InteractionType, HashSet<IInteractable>> interactables;

        public AgentMemory()
        {
            gameObjects = new Dictionary<string, GameObject>();
            interactables = new Dictionary<InteractionType, HashSet<IInteractable>>();
        }

        public HashSet<IInteractable> GetAll(InteractionType interactionType)
        { 
            if(interactables.ContainsKey(interactionType)) return interactables[interactionType];
            return null;
        }

        public bool AddInteractable(IInteractable interactable)
        {
            if (!interactables.ContainsKey(interactable.InteractionType))
            {
                interactables.Add(interactable.InteractionType, new HashSet<IInteractable>());
            }
            return interactables[interactable.InteractionType].Add(interactable);
        }

        public void Remove(IInteractable interactable)
        {
            if (!interactables.ContainsKey(interactable.InteractionType)) return;
            
            interactables[interactable.InteractionType].Remove(interactable);
        }
    }
}