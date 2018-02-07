using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

	public override void OnStartLocalPlayer(){
		GetComponent<MeshRenderer>().material.color = Color.red;
	}

	// Use this for initialization
	void Start () {

        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal")*0.1f;
        var z = Input.GetAxis("Vertical")*0.1f;

        transform.Translate(x, 0, z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
