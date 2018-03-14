using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;


public class PlayerInteract : NetworkBehaviour {

	public GameObject chipPrefab;
	public GameObject tilePrefab;
	public GameObject diePrefab;

	public GameObject mountainsTilePrefab;
	public GameObject brickTilePrefab;
	public GameObject sheepTilePrefab;
	public GameObject wheatTilePrefab;
	public GameObject woodTilePrefab;
	public GameObject seaTilePrefab;

	public GameObject numberTwoToken;
	public GameObject numberThreeToken;
	public GameObject numberFourToken;
	public GameObject numberFiveToken;
	public GameObject numberSixToken;
	public GameObject numberEightToken;
	public GameObject numberNineToken;
	public GameObject numberTenToken;
	public GameObject numberElevenToken;
	public GameObject numberTwelveToken;

	public float grabHeight = 0.5f;

	private Transform objectBeingDragged;
	private Quaternion objectBeingDraggedOriginalRotation;

	private Vector3 initial_object_pos;
	private Vector3 intermediate_object_pos;
	private float cumulative_distance;

	public Enums.PlayerInteractionMode player_interaction_mode = Enums.PlayerInteractionMode.normal;


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

		if(Input.GetKeyUp("i")){
			RaycastHit hit;

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); 
			
			if(Physics.Raycast(ray, out hit, 100f)){
				CmdCreateDie(hit.point);
			}
		}

		// Normal Interaction mode.
		if(player_interaction_mode == Enums.PlayerInteractionMode.normal){

			HandleDragAndDrop(game_manager.current_game_mode);
		}

		if(player_interaction_mode == Enums.PlayerInteractionMode.create_tile){
			// Creates a tile.
			if(Input.GetButtonUp("Fire1")){
				RaycastHit hit;

				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				if(Physics.Raycast(ray, out hit, 100f)){

					Debug.Log(EventSystem.current.IsPointerOverGameObject(-1));

					if (!EventSystem.current.IsPointerOverGameObject(-1)){

						MapCreateHandler handler = GameObject.Find("MapCreationUI").GetComponent<MapCreateHandler>();
						if (handler != null) {

							CmdCreateTile(hit.point, handler.selected_tile_type);
							handler.ClearSelected();

						}
					}
				}
			}
		}

		if(player_interaction_mode == Enums.PlayerInteractionMode.create_number_token){
			// Creates a number token.
			if(Input.GetButtonUp("Fire1")){
				RaycastHit hit;

				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				if(Physics.Raycast(ray, out hit, 100f)){

					Debug.Log(EventSystem.current.IsPointerOverGameObject(-1));

					if (!EventSystem.current.IsPointerOverGameObject(-1)){

						MapCreateHandler handler = GameObject.Find("MapCreationUI").GetComponent<MapCreateHandler>();
						if (handler != null) {

							CmdCreateNumberToken(hit.point, handler.selected_token_type);
							handler.ClearSelected();
						}
					}
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
	void CmdCreateTile(Vector3 position, Enums.TileType tileType){
		Quaternion rotation = Quaternion.identity * Quaternion.Euler(90, 0, 0);

		GameObject prefab;

		if(tileType == Enums.TileType.metal){
			prefab = mountainsTilePrefab;
		}
		else if(tileType == Enums.TileType.brick){
			prefab = brickTilePrefab;
		}
		else if(tileType == Enums.TileType.sheep){
			prefab = sheepTilePrefab;
		}
		else if(tileType == Enums.TileType.wheat){
			prefab = wheatTilePrefab;
		}
		else if(tileType == Enums.TileType.wood){
			prefab = woodTilePrefab;
		}
		else if(tileType == Enums.TileType.sea){
			prefab = seaTilePrefab;
		}
		else{
			return;
		}

		var tile = (GameObject)Instantiate(
			prefab,
			position - transform.forward,
			rotation
		);

		tile.layer = 8;

		NetworkServer.Spawn(tile);
	}

	[Command]
	void CmdCreateNumberToken(Vector3 position, Enums.NumberToken token_type){
		Quaternion rotation = Quaternion.identity * Quaternion.Euler(-90, 0, 0);

		GameObject prefab;

		if(token_type == Enums.NumberToken.two){
			prefab = numberTwoToken;
		}
		else if(token_type == Enums.NumberToken.three){
			prefab = numberThreeToken;
		}
		else if(token_type == Enums.NumberToken.four){
			prefab = numberFourToken;
		}
		else if(token_type == Enums.NumberToken.five){
			prefab = numberFiveToken;
		}
		else if(token_type == Enums.NumberToken.six){
			prefab = numberSixToken;
		}
		else if(token_type == Enums.NumberToken.eight){
			prefab = numberEightToken;
		}
		else if(token_type == Enums.NumberToken.nine){
			prefab = numberNineToken;
		}
		else if(token_type == Enums.NumberToken.ten){
			prefab = numberTenToken;
		}
		else if(token_type == Enums.NumberToken.eleven){
			prefab = numberElevenToken;
		}
		else if(token_type == Enums.NumberToken.twelve){
			prefab = numberTwelveToken;
		}
		else{
			return;
		}

		var token = (GameObject)Instantiate(
			prefab,
			position - transform.forward,
			rotation
		);

		token.layer = 8;

		NetworkServer.Spawn(token);
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
