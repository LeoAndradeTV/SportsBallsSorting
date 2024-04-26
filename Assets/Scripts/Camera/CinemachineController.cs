using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineController : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cinemachine;

    private PlayerInputActions inputActions;

    private float xSpeed;
    private float ySpeed;

    // Start is called before the first frame update
    void OnEnable ()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.CameraMoveToggle.performed += CameraMoveToggle_performed;
        inputActions.Player.CameraMoveToggle.canceled += CameraMoveToggle_canceled;
        //cinemachine.enabled = false;

        xSpeed = cinemachine.m_XAxis.m_MaxSpeed;
        ySpeed = cinemachine.m_YAxis.m_MaxSpeed;

        ResetCameraMovement();
        StopCameraMovement();

        GameManager.Instance.OnSettingsChanged += ResetCameraMovement;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnSettingsChanged -= ResetCameraMovement;
    }

    private void CameraMoveToggle_canceled(InputAction.CallbackContext context)
    {
        //cinemachine.enabled = false;

        StopCameraMovement();
    }

    private void CameraMoveToggle_performed(InputAction.CallbackContext context)
    {
        //cinemachine.enabled = true;

        ResetCameraMovement();
    }

    private void StopCameraMovement()
    {
        cinemachine.m_XAxis.m_MaxSpeed = 0;
        cinemachine.m_YAxis.m_MaxSpeed = 0;
    }

    private void ResetCameraMovement()
    {
        cinemachine.m_XAxis.m_MaxSpeed = GameManager.Instance.CameraSpeed * 5;
        cinemachine.m_YAxis.m_MaxSpeed = GameManager.Instance.CameraSpeed / 20;
    }
}
