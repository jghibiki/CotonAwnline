using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncTransform : NetworkBehaviour {

	Vector3 position1;
    Quaternion rotation1;
    bool move;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		move = false;
	}

	public void OnMouseDrag () {
		position1 = gameObject.transform.position;
		rotation1 = gameObject.transform.rotation;
		CmdSendMovement(position1, rotation1);
		Debug.Log("On Mouse Drag: ");
	}
 
	public void OnMouseExit () {
	
		Debug.Log("On Mouse Exit: ");
	}
	
	[Command]
	public void CmdSendMovement(Vector3 position1, Quaternion rotation1)
	{
		gameObject.transform.position = position1;
		gameObject.transform.rotation = rotation1;
	}

}
