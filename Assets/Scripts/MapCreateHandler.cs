using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapCreateHandler : MonoBehaviour {

	public Enums.TileType selected_tile_type = Enums.TileType.none;

	public Color button_highlight_color = Color.red;

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


	void OnClickTileButton(Enums.TileType tile_type){


		foreach(var name in button_names.Values){

			var btn = GameObject.Find(name).GetComponent<Image>();
			btn.color = Color.white;
		}

		var button_name = button_names[tile_type];
		var button = GameObject.Find(button_name).GetComponent<Image>();
		button.color = button_highlight_color;

		selected_tile_type = tile_type;

	}
}
