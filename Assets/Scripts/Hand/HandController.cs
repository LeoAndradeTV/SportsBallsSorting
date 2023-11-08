using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HandController : MonoBehaviour
{
    [SerializeField] private float handMoveSpeed;

    private float currentPositionX;
    private float currentPositionZ;

    private float minBoxPosition = -4.5f;
    private float maxBoxPosition = 4.5f;
    private const float HAND_Y_POSITION = 15f;

    private float horizontal;
    private float vertical;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
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
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void MoveHand()
    {
        currentPositionX += horizontal * handMoveSpeed * Time.deltaTime;
        currentPositionZ += vertical * handMoveSpeed * Time.deltaTime;

        currentPositionX = Mathf.Clamp(currentPositionX, minBoxPosition, maxBoxPosition);
        currentPositionZ = Mathf.Clamp(currentPositionZ, minBoxPosition, maxBoxPosition);

        transform.position = new Vector3(currentPositionX, 15, currentPositionZ);
    }

    private void SetLineRenderer()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, new Vector3(transform.position.x, 0, transform.position.z));
    }
}
