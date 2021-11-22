using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public Interactable focus; // Our current focus: Item, Enemy etc.

    public LayerMask movementMask;// The ground
    //public LayerMask interactionMask;	// Everything we can interact with

    Camera cam;

    PlayerMotor motor;

    NavMeshAgent agent;

    [SerializeField]
    float _maxDistance = 3f;
    [SerializeField]
    float _maxDrop = 3f;

    [SerializeField]
    [Range(0, 3f)]
    float turnSmoothTime = 0.12f;
    float turnSmoothVelocity;

    internal bool IsMove = true;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        var input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if(input.magnitude > 0.1f)
        {
            float targetAgle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAgle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAgle, 0f) * Vector3.forward;

            Vector3? point = GetDestination(moveDir.normalized);
            if (point.HasValue && IsMove)
            {
                agent.SetDestination(point.Value);
                //RemoteFocus();
            }
        }

        // If we press left mouse - move character by mouse
        if (Input.GetMouseButton(0))
        {
            // Shoot out a ray
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If we hit
            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                //Move our player to what we hit
                motor.MoveToPoint(hit.point);

                //Stop forcusing any object
                RemoteFocus();
            }
        }

        // If we press right mouse
        if (Input.GetMouseButton(1))
        {
            // Shoot out a ray
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If we hit
            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interacable = hit.collider.GetComponent<Interactable>();
                //Check if we hit an interactive 
                // if we did set it as focus
                if (interacable != null)
                {
                    SetFocus(interacable);
                }
            }
        }
    }

    Vector3? GetDestination(Vector3 direction)
    {
        for (float height = -_maxDrop; height < 1; height += 0.1f)
        {
            var destination = GetDestination(direction, height);
            if (destination.HasValue)
            {
                return destination.Value;
            }
        }
        return null;
    }

    Vector3? GetDestination(Vector3 direction, float height)
    {
        for(float distance = 1f; distance < _maxDistance; distance += 0.1f)
        {
            Vector3 offset = direction * distance + (Vector3.up * height);
            var destination = CheckForNavMeshAtOffset(offset);
            if (destination.HasValue)
            {
                return destination;
            }
        }
        return null;
    }

    private Vector3? CheckForNavMeshAtOffset(Vector3 offset)
    {
        var targetPoint = transform.position + offset;

        if (NavMesh.SamplePosition(targetPoint, out var hitInfo, 0.1f, NavMesh.AllAreas ))
        {
            Debug.DrawLine(targetPoint, targetPoint + Vector3.up * .1f, Color.green);
            return hitInfo.position;
        }
        else
        {
            Debug.DrawLine(targetPoint, targetPoint + Vector3.up * .05f, Color.yellow);
            return null;
        }
    }
   
    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
                focus.OnDeFocused();

            focus = newFocus;
            motor.FollowTarget(newFocus);
        }
        newFocus.OnFocused(transform);
    }

    void RemoteFocus()
    {
        if (focus != null)
            focus.OnDeFocused();

        focus = null;
        motor.StopFollowingTarget();
    }
}

