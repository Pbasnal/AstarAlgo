using UnityEngine;

namespace MainGame
{
    public enum InteractionType
    {
        Consumable
    }

    public interface IInteractable
    {
        InteractionType InteractionType { get; }

        Vector3 Position { get; }

        void Interact(GameObject gameObject);
    }
}