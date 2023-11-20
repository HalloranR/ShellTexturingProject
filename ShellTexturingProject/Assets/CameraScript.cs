using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject target;
    public float step;

    public Transform orientation;

    public float horizontalInput;
    public float verticalInput;

    public bool orbit = false;

    Vector3 moveDirection;

    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }


    private void FixedUpdate()
    {
        if (orbit)
        {
            transform.RotateAround(target.transform.position, transform.up, 30 * Time.deltaTime);
        }
        else
        {
            MyInput();

            MoveCamera();
        }
    }

    public void ResetPos()
    {
        this.transform.position = startPos;
    }

    public void ChangeOrbit()
    {
        if (orbit)
        {
            orbit = false;
        }
        else
        {
            orbit = true;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MoveCamera()
    {
        moveDirection = orientation.up * verticalInput + orientation.right * horizontalInput;

        transform.position = transform.position + (moveDirection.normalized * step);

        transform.LookAt(target.transform.position);
    }
}
