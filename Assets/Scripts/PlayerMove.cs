using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

	public GameObject chipPrefab;
	public GameObject tilePrefab;

	float sensitivityX = 1f;
	float sensitivityY = 1f;
	float minimumY = 1f;
	float maximumY = 2f;

	float speed = 5f;

	float grabHeight = 0.5f;

	private Transform objectBeingDragged;
	private Quaternion objectBeingDraggedOriginalRotation;

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

		
		if(Input.GetKeyUp(KeyCode.Space)){

			RaycastHit hit;

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); 
			 
			if(Physics.Raycast(ray, out hit, 100f)){
				CmdCreateChip(hit.point);
			}
		}

		if(Input.GetKeyUp(KeyCode.T)){

			RaycastHit hit;

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); 
			 
			if(Physics.Raycast(ray, out hit, 100f)){
				CmdCreateTile(hit.point);
			}
		}

		if(Input.GetButtonDown("Fire1")){
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			var layerMask = 1 << 8; //only raycast to layer 8 

			if(Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask)){
				
				objectBeingDragged = hit.transform ;				
				objectBeingDragged.position += new Vector3(0, grabHeight, 0);
				objectBeingDraggedOriginalRotation = objectBeingDragged.rotation;
			}
		}

		if(Input.GetButtonUp("Fire1")){
			objectBeingDragged = null;
		}

		if(Input.GetButton("Fire1")){
			if(objectBeingDragged != null){
				var ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				var layerMask = 1 << 8;
				layerMask = ~layerMask; // invert to exclude layer 8
				if(Physics.Raycast(ray2.origin, ray2.direction, out hit, Mathf.Infinity, layerMask)){
					objectBeingDragged.position = hit.point;
					objectBeingDragged.position += new Vector3(0, grabHeight, 0);
					objectBeingDragged.rotation = objectBeingDraggedOriginalRotation; 

				}
				else{ // resets rotation to prevent crazy rotation from gravity
					objectBeingDragged.position += new Vector3(0, 0, 0);				
					objectBeingDragged.rotation = objectBeingDraggedOriginalRotation; 
				}
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

		chip.layer = 8;

		NetworkServer.Spawn(chip);
	}

	[Command]
	void CmdCreateTile(Vector3 position){
		Quaternion rotation = Quaternion.identity * Quaternion.Euler(90, 0, 0);

		var tile = (GameObject)Instantiate(
			tilePrefab,
			position - transform.forward,
			rotation
		);

		tile.layer = 8;

		NetworkServer.Spawn(tile);
	}
}
