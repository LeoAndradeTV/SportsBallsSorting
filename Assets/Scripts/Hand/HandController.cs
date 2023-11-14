using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HandController : MonoBehaviour
{
    [SerializeField] private float handMoveSpeed;

    private Transform cameraTransform;

    private float currentPositionX;
    private float currentPositionZ;

    private float minBoxPosition = -4.5f;
    private float maxBoxPosition = 4.5f;
    private const float HAND_Y_POSITION = 13f;

    private float horizontal;
    private float vertical;
    private LineRenderer lineRenderer;

    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        cameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        ActionsManager.BallHasCollided += EnableMovement;
    }

    private void OnDisable()
    {
        ActionsManager.BallHasCollided -= EnableMovement;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        MoveHand();
        SetLineRenderer();
    }

    private void GetInput()
    {
        if (!canMove) return;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void MoveHand()
    {
        currentPositionX = horizontal * handMoveSpeed * Time.deltaTime;
        currentPositionZ = vertical * handMoveSpeed * Time.deltaTime;

        transform.position += transform.right * currentPositionX + transform.forward * currentPositionZ;

        var forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        transform.forward = forward;

        KeepHandInBounds(minBoxPosition, maxBoxPosition);
    }

    private void SetLineRenderer()
    {
        Vector3 secondLinePosition = new Vector3(transform.position.x, 0, transform.position.z);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit info, 1000f))
        {
            secondLinePosition = info.point;
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, secondLinePosition);
    }

    public void DisableMovement()
    {
        lineRenderer.enabled = false;
        canMove = false;
    }

    public void EnableMovement()
    {
        lineRenderer.enabled = true;
        canMove = true;
    }

    private void KeepHandInBounds(float minBox, float maxBox)
    {
        if (transform.position.x < minBox)
        {
            transform.position = new Vector3(minBox, transform.position.y, transform.position.z);
        }
        if (transform.position.x > maxBox)
        {
            transform.position = new Vector3(maxBox, transform.position.y, transform.position.z);
        }
        if (transform.position.z < minBox)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, minBox);
        }
        if (transform.position.z > maxBox)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, maxBox);
        }


    }
}
