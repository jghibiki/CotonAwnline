using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(transform.position.y < -50){
			transform.position = Vector3.zero + new Vector3(0f, 3f, 0f);
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().rotation = Quaternion.identity;
		}
		
	}
}
