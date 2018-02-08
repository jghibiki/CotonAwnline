using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

	public GameObject chipPrefab;

	float sensitivityX = 1f;
	float sensitivityY = 1f;
	float minimumY = 1f;
	float maximumY = 2f;

	float speed = 10f;

	public override void OnStartLocalPlayer(){
		GetComponent<MeshRenderer>().material.color = Color.red;

		Camera.main.GetComponent<MouseOrbit>().SetTarget(gameObject.transform);
	}

	void Start(){

	}

	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer)
            return;


		if(Input.GetKey(KeyCode.R)){
			
			Vector3 moveVector = (transform.forward * speed) + (transform.right * speed);
			moveVector.y += speed * Time.deltaTime; 
			moveVector.x = 0; 
			moveVector.z = 0; 
			transform.Translate(moveVector);
		}

		if(Input.GetKey(KeyCode.F)){
			
			Vector3 moveVector = (transform.forward * speed) + (transform.right * speed);
			moveVector.y -= speed * Time.deltaTime; 
			moveVector.x = 0; 
			moveVector.z = 0; 
			transform.Translate(moveVector);
		}
		
		if(Input.GetKeyUp(KeyCode.Space)){

			RaycastHit hit;

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); 
			 
			if(Physics.Raycast(ray, out hit, 100f)){
				CmdCreateChip(hit.point);
			}
		}

	}

	[Command]
	void CmdCreateChip(Vector3 position){
		var chip = (GameObject)Instantiate(
			chipPrefab,
			position - transform.forward,
			Quaternion.identity
		);
	}
}
