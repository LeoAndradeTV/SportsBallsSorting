using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HandController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    private float handMoveSpeed;

    private Transform cameraTransform;

    private float currentPositionX;
    private float currentPositionZ;

    private float minBoxPosition = -4.5f;
    private float maxBoxPosition = 4.5f;
    private const float HAND_Y_POSITION = 15f;

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
        SetSpeed();
        ActionsManager.BallHasCollided += EnableMovement;
        GameManager.Instance.OnSettingsChanged += SetSpeed;
    }

    private void OnDisable()
    {
        ActionsManager.BallHasCollided -= EnableMovement;
        GameManager.Instance.OnSettingsChanged -= SetSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        CheckForPauseInput();

        if (GameManager.Instance.CurrentState == GameState.Paused) { return; }

        GetInput();
        MoveHand();
        SetLineRenderer();
    }

    private void SetSpeed()
    {
        handMoveSpeed = GameManager.Instance.MovementSpeed / 15f;
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

    private void CheckForPauseInput()
    {
        if (GameManager.Instance.CurrentState == GameState.Paused && Input.GetKeyUp(KeyCode.Escape))
        {
            GameManager.Instance.ResumeGame();
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
            return;
        }

        if (GameManager.Instance.CurrentState == GameState.Playing && Input.GetKeyUp(KeyCode.Escape))
        {
            GameManager.Instance.PauseGame();
            pauseMenu.SetActive(true);
            return;
        }
    }
}
