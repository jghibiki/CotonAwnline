using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMovement : MonoBehaviour {

	public bool locked = false;

	private Vector3 position;
	private Quaternion rotation;
	
	// Update is called once per frame
	void Update () {
		if(locked){
			if(position != transform.position){
				transform.position = position;
			}

			if(rotation != transform.rotation){
				transform.rotation = rotation;
			}
		}
	}

	public void Lock(){
		locked = true;
		position = transform.position;
		rotation = transform.rotation;
	}

	public void Unlock(){
		locked = false;
	}

	public void Toggle(){
		if(locked){
			Unlock();
		}
		else{
			Lock();
		}
	}
}
