using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class GunE : NetworkBehaviour {

	public GameObject Push;
	public GameObject Pull;
	bool PushOrPull = false;

	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}


	public void Shootable(bool pushKeyDown, bool pullKeyDown, bool pullKeyUp){
		if (pushKeyDown) {
			PushOrPull = false;
			initBullet ();
			//newBullet.transform.parent = transform.parent.transform;
			//newBullet.GetComponent<Bullet>().isPlayerBullet = transform.name == "Player";
			//newBullet.transform.parent = transform.parent.transform; 
		}
		if (pullKeyDown) {
			PushOrPull = true;
			initBullet ();
			transform.GetComponent<SpriteRenderer> ().enabled = true;
			//newBullet.transform.parent = transform.parent.transform;
			//newBullet.GetComponent<Bullet>().isPlayerBullet = transform.name == "Player";
			//newBullet.transform.parent = transform.parent.transform; 
		}
		if (pullKeyUp) {
			transform.GetComponent<SpriteRenderer> ().enabled = false;
		}

	}


	void initBullet(){
		GameObject obj;
		if (!PushOrPull) {
			obj = (GameObject)Instantiate (Push, transform.position, Quaternion.identity);
			NetworkServer.Spawn (obj);

		}else{
			obj = (GameObject)Instantiate (Pull, transform.position, Quaternion.identity);
			NetworkServer.Spawn (obj);

		}


	}
}
