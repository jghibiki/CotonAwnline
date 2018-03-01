using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

	float speed = 5f;

	private Camera camera;

	public override void OnStartLocalPlayer(){

		if (isLocalPlayer){
			camera = Camera.main;
			camera.transform.position = transform.position;
			camera.transform.parent = transform;
            Camera.main.GetComponent<MouseOrbit>().SetTarget(gameObject.transform);
		}
	}

	void Start(){

	}

	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer)
            return;


		// Handle Upward movement
		if(Input.GetKey(KeyCode.R)){
			
			Vector3 moveVector = (transform.forward * speed) + (transform.right * speed);
			moveVector.y += speed * Time.deltaTime; 
			moveVector.x = 0; 
			moveVector.z = 0; 
			transform.Translate(moveVector);

			var pos = transform.position;
			pos.y = Mathf.Clamp(pos.y, 0, 100);
			transform.position = pos;
		}

		// Handle Downward movement
		if(Input.GetKey(KeyCode.F)){
			
			Vector3 moveVector = (transform.forward * speed) + (transform.right * speed);
			moveVector.y -= speed * Time.deltaTime; 
			moveVector.x = 0; 
			moveVector.z = 0; 
			transform.Translate(moveVector);

			var pos = transform.position;
			pos.y = Mathf.Clamp(pos.y, 0, 100);
			transform.position = pos;
		}

	}
}
