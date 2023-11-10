using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _lookAtTarget;
    [SerializeField] private float cameraSpeed;

    private float mouseX;
    private float mouseY;

    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * cameraSpeed;
        mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * cameraSpeed;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!Input.GetButton("Fire2")) return;

        transform.LookAt(_lookAtTarget);
        transform.RotateAround(_lookAtTarget.position, Vector3.up, mouseX);
        transform.RotateAround(_lookAtTarget.position, Vector3.right, mouseY);
    }
}
