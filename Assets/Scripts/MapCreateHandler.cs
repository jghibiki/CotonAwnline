using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapCreateHandler : MonoBehaviour {

	public Enums.TileType selected_tile_type = Enums.TileType.none;
	public Enums.NumberToken selected_token_type = Enums.NumberToken.none;

	public Color button_highlight_color = Color.gray;

	private Dictionary<Enums.TileType, string> button_names = new Dictionary<Enums.TileType, string>(){
		{ Enums.TileType.brick, "BrickButton" },
		{ Enums.TileType.metal, "MetalButton" },
		{ Enums.TileType.sheep, "SheepButton" },
		{ Enums.TileType.wheat, "WheatButton" },
		{ Enums.TileType.wood,  "WoodButton"  },
		{ Enums.TileType.sea,   "SeaButton"   }
	};

	// Use this for initialization
	void Start () {

		//Register event handlers for buttons
		Button btn;
		string button_name;

		button_name = button_names[Enums.TileType.brick];
		btn = GameObject.Find(button_name).GetComponent<Button>();
		btn.onClick.AddListener(OnClickTileBrick);

		button_name = button_names[Enums.TileType.metal];
		btn = GameObject.Find(button_name).GetComponent<Button>();
		btn.onClick.AddListener(OnClickTileMetal);

		button_name = button_names[Enums.TileType.sheep];
		btn = GameObject.Find(button_name).GetComponent<Button>();
		btn.onClick.AddListener(OnClickTileSheep);

		button_name = button_names[Enums.TileType.wheat];
		btn = GameObject.Find(button_name).GetComponent<Button>();
		btn.onClick.AddListener(OnClickTileWheat);

		button_name = button_names[Enums.TileType.wood];
		btn = GameObject.Find(button_name).GetComponent<Button>();
		btn.onClick.AddListener(OnClickTileWood);

		button_name = button_names[Enums.TileType.sea];
		btn = GameObject.Find(button_name).GetComponent<Button>();
		btn.onClick.AddListener(OnClickTileSea);

		// Number Token Button
		btn = GameObject.Find("NumbersButton").GetComponent<Button>();
		btn.onClick.AddListener(OnClickNumberTokenButton);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickTileBrick(){
		Button btn;
		OnClickTileButton(Enums.TileType.brick);
	}

	public void OnClickTileMetal(){
		OnClickTileButton(Enums.TileType.metal);
	}

	public void OnClickTileSheep(){
		OnClickTileButton(Enums.TileType.sheep);
	}

	public void OnClickTileWheat(){
		OnClickTileButton(Enums.TileType.wheat);
	}
	
	public void OnClickTileWood(){
		OnClickTileButton(Enums.TileType.wood);
	}

	public void OnClickTileSea(){
		OnClickTileButton(Enums.TileType.sea);
	}

	public void OnClickNumberTokenButton(){

		// highlight button

		this.ClearSelected();

		var btn = GameObject.Find("NumbersButton").GetComponent<Image>();
		btn.color = button_highlight_color;

		var number_dropdown = GameObject.Find("NumbersDropdown").GetComponent<Dropdown>();

		if(number_dropdown.value == 0){
			selected_token_type = Enums.NumberToken.two;
		}
		else if(number_dropdown.value == 1){
			selected_token_type = Enums.NumberToken.three;
		}
		else if(number_dropdown.value == 2){
			selected_token_type = Enums.NumberToken.four;
		}
		else if(number_dropdown.value == 3){
			selected_token_type = Enums.NumberToken.five;
		}
		else if(number_dropdown.value == 4){
			selected_token_type = Enums.NumberToken.six;
		}
		else if(number_dropdown.value == 5){
			selected_token_type = Enums.NumberToken.eight;
		}
		else if(number_dropdown.value == 6){
			selected_token_type = Enums.NumberToken.nine;
		}
		else if(number_dropdown.value == 7){
			selected_token_type = Enums.NumberToken.ten;
		}
		else if(number_dropdown.value == 8){
			selected_token_type = Enums.NumberToken.eleven;
		}
		else if(number_dropdown.value == 9){
			selected_token_type = Enums.NumberToken.twelve;
		}

		var player_interact = GameObject.FindWithTag("Player").GetComponent<PlayerInteract>();
		player_interact.player_interaction_mode = Enums.PlayerInteractionMode.create_number_token;

	}

	void OnClickTileButton(Enums.TileType tile_type){

		this.ClearSelected();

		var button_name = button_names[tile_type];
		var button = GameObject.Find(button_name).GetComponent<Image>();
		button.color = button_highlight_color;

		selected_tile_type = tile_type;

		// set player interaction mode
		var player_interact = GameObject.FindWithTag("Player").GetComponent<PlayerInteract>();
		player_interact.player_interaction_mode = Enums.PlayerInteractionMode.create_tile;

	}

	public void ClearSelected(){

        var num_btn = GameObject.Find("NumbersButton").GetComponent<Image>();
        num_btn.color = Color.white;

		foreach(var name in button_names.Values){
			var btn = GameObject.Find(name).GetComponent<Image>();
			btn.color = Color.white;
		}

		selected_tile_type = Enums.TileType.none;

		// set player interaction mode
		var player_interact = GameObject.FindWithTag("Player").GetComponent<PlayerInteract>();
		player_interact.player_interaction_mode = Enums.PlayerInteractionMode.normal;

	}
}
