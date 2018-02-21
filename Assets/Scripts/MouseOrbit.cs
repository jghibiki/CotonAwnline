using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOrbit : MonoBehaviour
{

    public Transform target;
    public int degrees = 0;
    public int dragSpeed = 2;
	public int depth = -1;

	public float speed = .001f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

		if(target != null){

			transform.position = target.position + new Vector3(0, 0, 0);

			var x = Input.GetAxis("Horizontal");
			var y = Input.GetAxis("Vertical");

			Vector3 moveVector = (transform.forward * y) + (transform.right * x);
			moveVector *= speed * Time.deltaTime; 
			moveVector.y = 0;

			target.transform.Translate(moveVector);

			if(Input.GetAxis("Mouse ScrollWheel") > 0 && GetComponent<Camera>().fieldOfView > 10){
				GetComponent<Camera>().fieldOfView --;
			}
			if(Input.GetAxis("Mouse ScrollWheel") < 0 && GetComponent<Camera>().fieldOfView < 100){
				GetComponent<Camera>().fieldOfView ++;
			}

			if (Input.GetMouseButton(1))
			{
				degrees = 10;
				transform.RotateAround(
					target.position,
					Vector3.up,
					Input.GetAxis("Mouse X") * degrees
				);

				transform.RotateAround(
					target.position,
					Vector3.left,
					Input.GetAxis("Mouse Y") * dragSpeed
				);

				transform.rotation = Quaternion.Euler(
					transform.eulerAngles.x,
					transform.eulerAngles.y,
					0f	
				);
						
			}

			if (!Input.GetMouseButton(1))
			{
				transform.RotateAround(
					target.position,
					Vector3.up,
					degrees * Time.deltaTime
				);
			}
			else
			{
				degrees = 0;
			}

            var newPos = target.transform.position;
			newPos.y = Mathf.Clamp(newPos.y, 0.0f, 200f);
			Debug.Log(newPos);
			target.transform.position = newPos;

		}
		
    }

	public void SetTarget(Transform target){
		this.target = target;
	}
}
