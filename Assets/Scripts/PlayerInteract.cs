using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInteract : NetworkBehaviour {

	public GameObject chipPrefab;
	public GameObject tilePrefab;
	public GameObject diePrefab;

	public float grabHeight = 0.5f;

	private Transform objectBeingDragged;
	private Quaternion objectBeingDraggedOriginalRotation;

	private Vector3 initial_object_pos;
	private Vector3 intermediate_object_pos;
	private float cumulative_distance;

	private  Enums.PlayerInteractionMode player_interaction_mode = Enums.PlayerInteractionMode.normal;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;

		var game_manager = GameObject.Find("GameManager").GetComponent<GameManagerController>();

		if (game_manager.current_game_mode == Enums.GameMode.setup){
			HandleSetupInteractions(game_manager);
		}
		
	}

	void HandleSetupInteractions(GameManagerController game_manager){

		// Normal Interaction mode.
		if(player_interaction_mode == Enums.PlayerInteractionMode.normal){

			HandleDragAndDrop(game_manager.current_game_mode);
		}

		if(Input.GetKeyUp("i")){
			RaycastHit hit;

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); 
			
			if(Physics.Raycast(ray, out hit, 100f)){
				CmdCreateDie(hit.point);
			}
		}

		// Create Tile Interaction Mode.
		else if(player_interaction_mode == Enums.PlayerInteractionMode.create_tile){
			// TODO: need somewhere to get the currently selected type of tile from.

			// Creates a tile.
			if(Input.GetButtonUp("Fire1")){
				RaycastHit hit;

				Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); 
				
				if(Physics.Raycast(ray, out hit, 100f)){
					CmdCreateTile(hit.point);
				}
			}

		}
	}


	void HandleDragAndDrop(Enums.GameMode game_mode){

		// Handles pick up part of drag and drop
		if(Input.GetButtonDown("Fire1")){

			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			var layerMask = 1 << 8; //only raycast to layer 8 

			if(Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask)){
				
				objectBeingDragged = hit.transform ;				

				if(objectBeingDragged.GetComponent<Die>()){
					objectBeingDragged.position += new Vector3(0, grabHeight*3, 0);
				}
				else{
					objectBeingDragged.position += new Vector3(0, grabHeight, 0);
				}
				objectBeingDraggedOriginalRotation = objectBeingDragged.rotation;
				
				initial_object_pos = objectBeingDragged.transform.position;
				intermediate_object_pos = objectBeingDragged.transform.position;
				cumulative_distance = 0f;
			}
		}


		// Handles drop part of drag and drop
		if(Input.GetButtonUp("Fire1")){
			
			if(objectBeingDragged != null){

				if(objectBeingDragged.GetComponent<Die>()){

					Rigidbody rigidbody = objectBeingDragged.GetComponent<Rigidbody>();

					var final_object_pos = objectBeingDragged.transform.position;

					float roll_vigor = Mathf.Clamp(cumulative_distance/2, 1f, 5f);

					var calculated_torque = roll_vigor * Vector3.Cross(intermediate_object_pos, final_object_pos) * 1000;
					rigidbody.AddTorque(calculated_torque, ForceMode.Impulse);


					var calculated_force = (final_object_pos - intermediate_object_pos).normalized *
						rigidbody.mass *
						roll_vigor *
						100;
					calculated_force.y = 50f;
					calculated_force.z *= 2f;
					calculated_force.x *= 2f;

					rigidbody.AddForce(calculated_force, ForceMode.Force);

					Debug.Log("Roll Vigor: " + roll_vigor);
					Debug.Log("Initial Object Pos: " + initial_object_pos);
					Debug.Log("Final Object Pos: " + final_object_pos);
					Debug.Log("Calculated Torque: " + calculated_torque);
					Debug.Log("Calculated Force: " + calculated_force);

				}

				// If objectBeingDragged is a hex tile, snap to grid.
				if( objectBeingDragged.GetComponent<HexTile>()){
					RaycastHit hit;
					var layerMask = 1 << 9; //check if we hit a hex cell
					if(Physics.Raycast(objectBeingDragged.position, -Vector3.up, out hit, Mathf.Infinity, layerMask)){
						
						Vector3 new_pos = new Vector3( hit.transform.position.x, 0f, hit.transform.position.z);
						objectBeingDragged.position = new_pos;
						objectBeingDragged.rotation = Quaternion.Euler(-90, 0, 0);

					}
				}
			}

			objectBeingDragged = null;

		}

		// Handles drag part of drag and drop.
		if(Input.GetButton("Fire1")){
			if(objectBeingDragged != null){

				// track the path distance we have dragged this object.
				var object_pos = objectBeingDragged.transform.position;
				cumulative_distance += Vector3.Distance(intermediate_object_pos, object_pos);
				intermediate_object_pos = object_pos;

				var ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				var layerMask = 1 << 8;
				layerMask = ~layerMask; // invert to exclude layer 8
				if(Physics.Raycast(ray2.origin, ray2.direction, out hit, Mathf.Infinity, layerMask)){
					objectBeingDragged.position = hit.point;

					if(objectBeingDragged.GetComponent<Die>()){
						objectBeingDragged.position += new Vector3(0, grabHeight*3, 0);
					}
					else{
					objectBeingDragged.position += new Vector3(0, grabHeight, 0);
					}
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

	[Command]
	void CmdCreateDie(Vector3 position){
		var die = (GameObject)Instantiate(
			diePrefab,
			position - transform.forward,
			Quaternion.identity
		);

		die.layer = 8;

		NetworkServer.Spawn(die);
	}
}
