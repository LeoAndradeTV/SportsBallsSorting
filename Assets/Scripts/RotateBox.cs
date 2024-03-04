using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBox : MonoBehaviour
{
   
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, 10f * Time.deltaTime);
    }
}
