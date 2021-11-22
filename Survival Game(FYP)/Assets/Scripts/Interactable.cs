using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactionTransform;

    bool isFocus = false; // Is this interactable currently being focused?
    Transform player;

    bool hasInteracted = false; // Have we already interacted with the object?

    public virtual void Interact()
    {
        // This method is meant ro be overwritten
        Debug.Log("Interaction with " + transform.name);
    }

    protected virtual void Update()
    {
        if (isFocus && !hasInteracted) // If currently being focused
        {
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if (distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }         
        }
        
    }
    // Called when the object starts being focused
    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }
    // Called when the object is no longer focused
    public void OnDeFocused()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
