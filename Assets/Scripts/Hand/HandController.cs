using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class HandController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    private float handMoveSpeed;
    private PlayerInputActions inputActions;
    private HandSpawner handSpawner;

    private Transform cameraTransform;

    private float currentPositionX;
    private float currentPositionZ;

    private float minBoxPosition = -4.2f;
    private float maxBoxPosition = 4.2f;

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
        handSpawner = GetComponent<HandSpawner>();
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.Pause.performed += Pause_performed;
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
        inputActions.Player.Pause.performed -= Pause_performed;


    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) { return; }

        GetInput();
        MoveHandAndBall();
        SetLineRenderer();
    }

    private void SetSpeed()
    {
        handMoveSpeed = GameManager.Instance.MovementSpeed / 7f;
    }

    private void GetInput()
    {
        if (!canMove) return;

        horizontal = inputActions.Player.Move.ReadValue<Vector2>().x;
        vertical = inputActions.Player.Move.ReadValue<Vector2>().y;

    }

    private void MoveHandAndBall()
    {
        currentPositionX = horizontal * handMoveSpeed * Time.deltaTime;
        currentPositionZ = vertical * handMoveSpeed * Time.deltaTime;

        transform.position += transform.right * currentPositionX + transform.forward * currentPositionZ;

        var forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        transform.forward = forward;
        KeepObjectInBounds(transform, minBoxPosition, maxBoxPosition, handSpawner.GetCurrentBallRadius().y);
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
        horizontal = 0;
        vertical = 0;
    }

    public void EnableMovement()
    {
        lineRenderer.enabled = true;
        canMove = true;
    }

    private void KeepObjectInBounds(Transform transform, float minBox, float maxBox, float currentBallRadius)
    {
        if (transform.position.x < minBox + currentBallRadius)
        {
            transform.position = new Vector3(minBox + currentBallRadius, transform.position.y, transform.position.z);
        }
        if (transform.position.x > maxBox - currentBallRadius)
        {
            transform.position = new Vector3(maxBox - currentBallRadius, transform.position.y, transform.position.z);
        }
        if (transform.position.z < minBox + currentBallRadius)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, minBox + currentBallRadius);
        }
        if (transform.position.z > maxBox - currentBallRadius)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, maxBox - currentBallRadius);
        }
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        switch (GameManager.Instance.CurrentState)
        {
            case GameState.Paused:
                ResumeGame();
                break;
            case GameState.Playing:
                PauseGame();
                break;
        }
    }

    public void PauseGame()
    {
        GameManager.Instance.PauseGame();
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);

        }
    }

    public void ResumeGame()
    {
        GameManager.Instance.ResumeGame();
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        GameManager.Instance.SaveMySettings();
    }
}
