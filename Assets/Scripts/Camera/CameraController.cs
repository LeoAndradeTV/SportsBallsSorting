using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _lookAtTarget;
    [SerializeField] private bool invertVertical;
    [SerializeField] private bool invertHorizontal;

    private PlayerInputActions inputActions;

    private float mouseX;
    private float mouseY;
    private float cameraSpeed;
    private bool canMoveCamera = false;

    private Vector2 screenPosition;
    private Touch touch;

    private void OnEnable()
    {
        SetSpeed();
        GameManager.Instance.OnSettingsChanged += SetSpeed;
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.CameraMoveToggle.performed += CameraMoveToggle_performed;
        inputActions.Player.CameraMoveToggle.canceled += CameraMoveToggle_canceled;
    }

    private void CameraMoveToggle_canceled(InputAction.CallbackContext obj)
    {
        mouseX = 0;
        mouseY = 0;
        canMoveCamera = false;
    }

    private void CameraMoveToggle_performed(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) { return; }
#if UNITY_STANDALONE || UNITY_EDITOR   
        canMoveCamera = true;
#endif

#if UNITY_IOS
        Touch touch = Input.GetTouch(0);
        if (TouchedBox(touch))
        {
            canMoveCamera = true;
            Debug.Log("Can Move Camera!");
        }
#endif
    }

    private void OnDisable()
    {
        GameManager.Instance.OnSettingsChanged -= SetSpeed;
    }

    private bool TouchedBox(Touch touch)
    {
        if (touch.position.x > (Screen.width / 4) && touch.position.x < (Screen.width / 4) * 3)
        {
            return true;
        }
        return false;
    }

    private void Update()
    {
        if (!canMoveCamera) return;

        //Debug.Log(inputActions.Player.MoveCameraX.ReadValue<float>());
        //Debug.Log(inputActions.Player.MoveCameraY.ReadValue<float>());

        mouseX = inputActions.Player.MoveCameraX.ReadValue<float>() * Time.deltaTime * cameraSpeed;
        mouseY = inputActions.Player.MoveCameraY.ReadValue<float>() * Time.deltaTime * cameraSpeed;

        mouseX = invertHorizontal ? mouseX * -1 : mouseX;
        mouseY = invertVertical ? mouseY * -1 : mouseY;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(_lookAtTarget);
        _lookAtTarget.Rotate(Vector3.up, mouseX);
        transform.RotateAround(_lookAtTarget.position, _lookAtTarget.up, mouseX);
        transform.RotateAround(_lookAtTarget.position, _lookAtTarget.right, mouseY);
    }

    private void SetSpeed()
    {
        cameraSpeed = GameManager.Instance.CameraSpeed * 10f;
    }
}
