using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _lookAtTarget;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private bool invertVertical;
    [SerializeField] private bool invertHorizontal;

    private float mouseX;
    private float mouseY;

    private float camPositionY = 5f;
    private float camPositionZ = 0;

    private void Update()
    {
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
}
