using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _lookAtTarget;
    [SerializeField] private bool invertVertical;
    [SerializeField] private bool invertHorizontal;

    private float mouseX;
    private float mouseY;
    private float cameraSpeed;

    private void OnEnable()
    {
        SetSpeed();
        GameManager.Instance.OnSettingsChanged += SetSpeed;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnSettingsChanged -= SetSpeed;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) { return; }

        if (!Input.GetButton("Fire2"))
        {
            mouseX = 0; 
            mouseY = 0;
            return;
        }

        mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * cameraSpeed;
        mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * cameraSpeed;

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
