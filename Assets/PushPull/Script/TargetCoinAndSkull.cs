using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TargetCoinAndSkull : MonoBehaviour {

	public bool isDeadTarget = false;
	public Vector2 ScoreBoardtoGo;

	void OnCollisionEnter2D(Collision2D collider){
		if ((collider.gameObject.tag == "Player" || collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "BorderEnd")&& gameObject.tag == "TargetCoin") {
			/*
			if (gameObject.tag == "TargetSkull") {
				Vector2 vel = gameObject.GetComponent<Rigidbody2D> ().velocity;
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (vel.x * -1f, vel.y * -1f);
			} else {
			*/
			/*
			if(collider.gameObject.tag == "Enemy")
				ScoreBoardtoGo = GameObject.Find ("EnemyScore").transform.localPosition;
			isDeadTarget = true;
			GetComponent<CircleCollider2D> ().enabled = false;
			*/

			MyGameManager mgm = GameObject.Find ("GameManager").GetComponent<MyGameManager> ();      
            mgm.ScoreCounter (gameObject.tag == "TargetCoin", gameObject.transform.position.x > 0.0f );
            mgm.scoreAddVec_coin = gameObject.transform.position;
            Destroy (gameObject);
			/*
			if (gameObject.tag == "TargetCoin")
				GameObject.Find ("TargetGen").GetComponent<TargetGen> ().makeTarget (true);
			else
				GameObject.Find ("TargetGen").GetComponent<TargetGen> ().makeTarget (false);

			*/
		}
		if (collider.gameObject.tag == "BorderEnd" && gameObject.tag == "TargetSkull"){
			
			MyGameManager mgm = GameObject.Find ("GameManager").GetComponent<MyGameManager> ();
            mgm.ScoreCounter (gameObject.tag == "TargetCoin", gameObject.transform.position.x > 0.0f);
            mgm.scoreAddVec_skul = gameObject.transform.position;
            Destroy (gameObject);
		}

	}

	void Start () {
		ScoreBoardtoGo = GameObject.Find ("PlayerScore").transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.x < -9f || transform.position.x > 9f || transform.position.y > 5f || transform.position.y < -5f)
			Destroy (gameObject);
		/*
		if (isDeadTarget) {
			if (Vector3.Distance (transform.position, ScoreBoardtoGo) > 1.0f) {
				transform.localPosition = Vector2.MoveTowards (transform.position, ScoreBoardtoGo, 1.0f);
			}else{
				Destroy (gameObject);
			}
		}
		*/

	}
}
