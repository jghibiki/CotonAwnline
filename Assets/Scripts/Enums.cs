using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums {
	public enum TyleType {
		lumber,
		wool,
		wheat,
		brick,
		ore
	};

	public enum GameState {
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
