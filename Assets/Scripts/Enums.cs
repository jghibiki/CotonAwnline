using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums {
	public enum TileType {
		wood,
		sheep,
		wheat,
		metal,
		brick,
		sea,
		none
	};

	public enum GameMode {
		setup
	}

	public enum PlayerInteractionMode {
		normal,
		create_tile,
		create_number_token
	}

	public enum NumberToken {
		two,
		three,
		four,
		five,
		six,
		eight,
		nine,
		ten,
		eleven,
		twelve
	}
}
