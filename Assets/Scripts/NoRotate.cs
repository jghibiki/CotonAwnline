using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRotate : MonoBehaviour {

	public float rotation_x;
	public float rotation_y;
	public float rotation_z;

	public bool disable = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!disable){
			transform.rotation = Quaternion.Euler(rotation_x, rotation_y, rotation_z);
		}
	}

}
