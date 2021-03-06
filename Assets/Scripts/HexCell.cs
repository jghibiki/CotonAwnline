﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {

	private HexTile tile; // The tile that is placed on this location

	// Use this for initialization
	void Start () {

		if(transform.position.z % 2 == 0){
			var new_pos = transform.position;
			new_pos.x -= 0.5f;
			transform.position = new_pos;
		}

		var new_pos2 = transform.position;
		new_pos2.z -= transform.position.z * 0.1f;
		transform.position = new_pos2;
		
		gameObject.layer = 9; //adds hex cell to layer 9
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
