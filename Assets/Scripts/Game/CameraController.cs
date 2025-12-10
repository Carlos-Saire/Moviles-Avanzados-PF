using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private float sphereRadius = 0.5f;
    [SerializeField] private LayerMask interactableLayer;

    private IInteractable currentInteractable;


    void Update()
    {
        DetectInteractable();
    }
    void DetectInteractable()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, sphereRadius,transform.forward, out hit, interactDistance, interactableLayer))
        {
            currentInteractable = hit.collider.GetComponent<IInteractable>();
        }
        else
        {
            currentInteractable = null;
        }
    }

    void TryInteract()
    {
        if (currentInteractable != null)
            currentInteractable.Interact();
    }
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = currentInteractable != null ? Color.green : Color.red;

        Vector3 startPos = transform.position;
        Vector3 direction = transform.forward;

        Vector3 endPos = startPos + direction * interactDistance;

        Gizmos.DrawLine(startPos, endPos);

        Gizmos.DrawWireSphere(startPos, sphereRadius);

        Gizmos.DrawWireSphere(endPos, sphereRadius);
    }
}
