using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour {

	public Enums.TileType tile_type = Enums.TileType.none;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public class Wood : HexTile {
		public Enums.TileType tile_type = Enums.TileType.wood;
	}

	public class Sea : HexTile {
		public Enums.TileType tile_type = Enums.TileType.sea;
	}

	public class Sheep : HexTile {
		public Enums.TileType tile_type = Enums.TileType.sheep;
	}
}
