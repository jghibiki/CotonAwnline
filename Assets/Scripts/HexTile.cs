using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour {

	public bool ignore_collisions = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision hit){
		if(!ignore_collisions){
			if(hit.transform.gameObject.name.Contains("HexCell")){
				Vector3 new_pos = new Vector3( hit.transform.position.x, 0, hit.transform.position.z);
				transform.position = new_pos;
				transform.rotation = Quaternion.Euler(-90, 0, 0);
				ignore_collisions = true;
			}
		}
	}
}
