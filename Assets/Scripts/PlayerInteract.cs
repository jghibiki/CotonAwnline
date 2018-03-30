using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;


public class PlayerInteract : NetworkBehaviour {

	public GameObject diePrefab;

	//Tiles
	public GameObject mountainsTilePrefab;
	public GameObject brickTilePrefab;
	public GameObject sheepTilePrefab;
	public GameObject wheatTilePrefab;
	public GameObject woodTilePrefab;
	public GameObject seaTilePrefab;

	//Tokens
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


	//Cards
	public GameObject referenceCardPrefab;
	public GameObject brickCardPrefab;
	public GameObject woodCardPrefab;
	public GameObject oreCardPrefab;
	public GameObject sheepCardPrefab;
	public GameObject wheatCardPrefab;

	// buildings
	public GameObject settlementPrefab;
	public GameObject cityPrefab;

	public float grabHeight = 0.5f;

	private Transform objectBeingDragged;
	private Quaternion objectBeingDraggedOriginalRotation;

	private Vector3 initial_object_pos;
	private Vector3 intermediate_object_pos;
	private float cumulative_distance;

	private bool objects_locked = false;

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

		if(Input.GetKeyUp("l")){
			var lockable_objects = FindObjectsOfType<LockMovement>() as LockMovement[];


			foreach(LockMovement obj in lockable_objects){
				if(objects_locked){
					obj.Unlock();
				}
				else{
					obj.Lock();
				}
			}

			if(objects_locked){
				objects_locked = false;
			}
			else{
				objects_locked = true;
			}
		}

		if(Input.GetKeyUp("o")){
			RaycastHit hit;

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); 
			
			if(Physics.Raycast(ray, out hit, 100f)){
				CmdCreateCard(hit.point, Enums.CardTypes.reference_card);
			}
		}

		if(Input.GetKeyUp("1")){
			RaycastHit hit;

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); 
			
			if(Physics.Raycast(ray, out hit, 100f)){
				CmdCreateCard(hit.point, Enums.CardTypes.brick);
			}
		}

		if(Input.GetKeyUp("2")){
			RaycastHit hit;

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); 
			
			if(Physics.Raycast(ray, out hit, 100f)){
				CmdCreateCard(hit.point, Enums.CardTypes.wood);
			}
		}

		if(Input.GetKeyUp("3")){
			RaycastHit hit;

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); 
			
			if(Physics.Raycast(ray, out hit, 100f)){
				CmdCreateCard(hit.point, Enums.CardTypes.metal);
			}
		}

		if(Input.GetKeyUp("4")){
			RaycastHit hit;

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); 
			
			if(Physics.Raycast(ray, out hit, 100f)){
				CmdCreateCard(hit.point, Enums.CardTypes.sheep);
			}
		}
		
		if(Input.GetKeyUp("5")){
			RaycastHit hit;

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); 
			
			if(Physics.Raycast(ray, out hit, 100f)){
				CmdCreateCard(hit.point, Enums.CardTypes.wheat);
			}
		}

		if(Input.GetKeyUp("6")){
			RaycastHit hit;

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); 
			
			if(Physics.Raycast(ray, out hit, 100f)){
				CmdCreateSettlement(hit.point);
			}
		}

		if(Input.GetKeyUp("7")){
			RaycastHit hit;

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); 
			
			if(Physics.Raycast(ray, out hit, 100f)){
				CmdCreateCity(hit.point);
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

							Vector3 point;

							// see if we can snap tile to grid
							RaycastHit hit2;
							var layerMask = 1 << 9; //check if we hit a hex cell
							var adjusted_point = new Vector3(hit.point.x, hit.point.y+2, hit.point.z);

							if(Physics.Raycast(adjusted_point, -Vector3.up, out hit2, Mathf.Infinity, layerMask)){
								point = new Vector3( hit2.transform.position.x, hit2.transform.position.y+1, hit2.transform.position.z+1f);
								Debug.Log(point);
							}
							else{
								point = hit.point;
							}

							CmdCreateTile(point, handler.selected_tile_type);
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

							Vector3 point;

							// see if we can snap token to grid
							RaycastHit hit2;
							var layerMask = 1 << 9; //check if we hit a hex cell
							var adjusted_point = new Vector3(hit.point.x, hit.point.y+2, hit.point.z);

							if(Physics.Raycast(adjusted_point, -Vector3.up, out hit2, Mathf.Infinity, layerMask)){
								Debug.Log(hit2.transform.gameObject);
								point = new Vector3( 
									hit2.transform.gameObject.transform.position.x, 
									1.0f, 
									hit2.transform.gameObject.transform.position.z+1.0f);
								Debug.Log(point);
							}
							else{
								point = hit.point;
							}

							CmdCreateNumberToken(point, handler.selected_token_type);
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
					objectBeingDragged.position += new Vector3(0, grabHeight, 0);
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
				else if(objectBeingDragged.GetComponent<NumberToken>()){
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
						objectBeingDragged.position += new Vector3(0, grabHeight, 0);
					}
					else{
						objectBeingDragged.position += new Vector3(0, grabHeight, 0);
					}
					//objectBeingDragged.rotation = objectBeingDraggedOriginalRotation; 

				}
				else{ // resets rotation to prevent crazy rotation from gravity
					objectBeingDragged.position += new Vector3(0, 0, 0);				
					//objectBeingDragged.rotation = objectBeingDraggedOriginalRotation; 
				}


				if(Input.GetKey("q")){
					objectBeingDragged.transform.Rotate(Vector3.forward* Time.deltaTime * 75);
				}
				else if(Input.GetKey("e")){
					objectBeingDragged.transform.Rotate(-Vector3.forward* Time.deltaTime * 75);
				}

				if(Input.GetKeyUp("f")){
					var rotation = objectBeingDragged.rotation;
					objectBeingDragged.rotation = Quaternion.Euler(rotation.eulerAngles.x+180, rotation.eulerAngles.y, rotation.eulerAngles.z);
				}

				var sync_transform = objectBeingDragged.GetComponent<SyncTransform>();
				if(sync_transform != null){
					CmdSetAuth(objectBeingDragged.GetComponent<NetworkIdentity>().netId, GetComponent<NetworkIdentity>());
					sync_transform.OnMouseDrag();
				}

			}
		}


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

	[Command]
	void CmdCreateCard(Vector3 position, Enums.CardTypes card_type){
		GameObject prefab;

		if(card_type == Enums.CardTypes.reference_card){
			prefab = referenceCardPrefab;
		}
		else if(card_type == Enums.CardTypes.brick){
			prefab = brickCardPrefab;
		}
		else if(card_type == Enums.CardTypes.wood){
			prefab = woodCardPrefab;
		}
		else if(card_type == Enums.CardTypes.metal){
			prefab = oreCardPrefab;
		}
		else if(card_type == Enums.CardTypes.sheep){
			prefab = sheepCardPrefab;
		}
		else if(card_type == Enums.CardTypes.wheat){
			prefab = wheatCardPrefab;
		}
		else{
			return;
		}

		Quaternion rotation = Quaternion.identity * Quaternion.Euler(-90, 0, 0);
		var card = (GameObject)Instantiate(
			prefab,
			position - transform.forward,
			rotation
		);

		card.layer = 8;

		NetworkServer.Spawn(card);
	}

	[Command]
	void CmdCreateSettlement(Vector3 position){
		Quaternion rotation = Quaternion.identity * Quaternion.Euler(-90, 0, 0);
		var obj = (GameObject)Instantiate(
			settlementPrefab,
			position - transform.forward,
			rotation
		);

		obj.layer = 8;

		NetworkServer.Spawn(obj);
	}

	[Command]
	void CmdCreateCity(Vector3 position){
		Quaternion rotation = Quaternion.identity * Quaternion.Euler(0, 0, 0);
		var obj = (GameObject)Instantiate(
			cityPrefab,
			position - transform.forward,
			rotation
		);

		obj.layer = 8;

		NetworkServer.Spawn(obj);
	}

	[Command]
     public void CmdSetAuth(NetworkInstanceId objectId, NetworkIdentity player)
     {
         var iObject = NetworkServer.FindLocalObject(objectId);
         var networkIdentity = iObject.GetComponent<NetworkIdentity>();
         var otherOwner = networkIdentity.clientAuthorityOwner;        
 
         if (otherOwner == player.connectionToClient)
         {
             return;
         }else
         {
             if (otherOwner != null)
             {
                 networkIdentity.RemoveClientAuthority(otherOwner);
             }
             networkIdentity.AssignClientAuthority(player.connectionToClient);
         }        
     }

}