using UnityEngine;
using GoapFramework;
namespace MainGame
{
    public class FoodItemInteractable : MonoBehaviour, IInteractable
    {
        public int hungerToFill = 2;

        public InteractionType InteractionType => InteractionType.Consumable;

        public Vector3 Position => transform.position;

        public void Interact(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<HungerProperty>(out var hungerPropery)) return;

            if(hungerPropery.Eat(hungerToFill))
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}