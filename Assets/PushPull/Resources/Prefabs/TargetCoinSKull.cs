using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class TargetCoinSKull : NetworkBehaviour {

	public bool isDeadTarget = false;
	public Vector2 ScoreBoardtoGo;

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Enemy") {
			
			Vector2 vel = gameObject.GetComponent<Rigidbody2D> ().velocity;
			GetComponent<Rigidbody2D> ().velocity = new Vector2(vel.x*-1f,vel.y*-1f);

		}

	}

	void Start () {
		ScoreBoardtoGo = GameObject.Find ("PlayerScore").transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (isDeadTarget) {
			if (Vector3.Distance (transform.position, ScoreBoardtoGo) > 1.0f) {
				transform.localPosition = Vector2.MoveTowards (transform.position, ScoreBoardtoGo, 1.0f);
			}else{
				Destroy (gameObject);
			}
		}
		
	}
}
