using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float forwardMove;
    public float leftMove;

    public float speed;
    private float baseSpeed;

    public float sensitivityX;
    public float sensitivityY;

    private float rotationY;
    private float rotationX;

    Quaternion originalRotation;

    private bool moveSpeedSwitch;

    // Use this for initialization
    void Start () {
        speed = 0.2f;
        baseSpeed = speed;

        sensitivityY = 4f;
        sensitivityX = 4f;

        rotationY = 0f;
        rotationX = 0f;

        originalRotation = transform.localRotation;

        moveSpeedSwitch = true;
    }
	
	// Update is called once per frame
	void Update () {
        forwardMove = 0f;
        leftMove = 0f;

        // Keyboard input
        if (Input.GetKey(KeyCode.W))
        {
            forwardMove += speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            forwardMove -= speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            leftMove -= speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            leftMove += speed;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeedSwitch = !moveSpeedSwitch;
        }

        if (moveSpeedSwitch)
        {
            speed = baseSpeed;
        }
        else
        {
            speed = baseSpeed / 4;
        }

        // Corrects for diagonal movement speed
        if (forwardMove != 0 && leftMove != 0)
        {
            forwardMove /= Mathf.Sqrt(2);
            leftMove /= Mathf.Sqrt(2);
        }

        var direction = new Vector3(leftMove, 0, forwardMove);
        var correctedDirection = this.transform.TransformDirection(direction);
        correctedDirection.y = 0f;
        this.transform.position += correctedDirection;

        if (Input.GetMouseButton(1))
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);

            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
	}
}
