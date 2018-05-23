using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
public class Target : NetworkBehaviour {


	protected float timecnt = 0; 
	protected float effect;
	Vector2 ScorerBoard;
	protected bool isDeadTarget = false;


	protected void DestroyTarget(bool isCoinTarget,bool isBorderRight){
		gameObject.tag = "DeadTarget";
		var gm = GameObject.Find ("GameManager").GetComponent<MyGameManager> ();
		GetComponent<CircleCollider2D> ().enabled = false;
		isDeadTarget = true;
		if (isBorderRight) {
			ScorerBoard = GameObject.Find ("PlayerScore").transform.localPosition;
		}
		if (isCoinTarget) {
			gm.coincnt--;
		} else {
			gm.skullcnt--;
		}
		//GameObject.Find ("TargetGen").GetComponent<TargetGen> ().GenerateTarget(isCoinTarget);
	}

	// when you dont destroy target you should restart target
	void FinishedTargetReStart(){
		float xpos = Random.Range (-3.0f, 3.0f);
		float ypos = Random.Range (-2.0f, 2.0f);
		float x = Random.Range (-10.0f, 10.0f);
		int yidx = Random.Range(0, 2);
		float[] y = { -150.0f, 150.0f};
		transform.localPosition = new Vector2 (xpos, ypos);
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		GetComponent<Rigidbody2D> ().AddForce(new Vector2 (x*10f,y[yidx]));
	}

	protected void myOnCollisionEnter2D(Collision2D collision,bool isCoinTarget){
		var gm = GameObject.Find ("GameManager").GetComponent<MyGameManager> ();
		string collisionTag = collision.gameObject.tag;

		if (!isCoinTarget && collisionTag == "Player" || collisionTag == "Enemy") {
			gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (transform.position.x * -10f, 0));
			return;
		}
			
		if (collisionTag == "BorderEnd" || collisionTag == "Player" || collisionTag == "Enemy") {
			string collisionName = collision.gameObject.name;
			bool isPlayerSide = collisionName == "BorderRight" || collisionName == "Player";


			DestroyTarget (isCoinTarget, isPlayerSide);
			//FinishedTargetReStart();
			gm.ScoreCounter (isCoinTarget, isPlayerSide);
		}

			
	}



	public float getEffect(){
		return effect;
	}
		
	public virtual void Start () {
		
		ScorerBoard = GameObject.Find ("EnemyScore").transform.localPosition;
		float x = Random.Range (-10.0f, 10.0f);
		int yidx = Random.Range(0, 2);
		float[] y = { -150.0f, 150.0f};
		GetComponent<Rigidbody2D> ().AddForce(new Vector2 (x*10f,y[yidx]));
	}

	// Update is called once per frame
	/*
	void TractTarget(){
		if (transform.localPosition.x < MyGameManager.EnemyBorderX) {
			Vector2 enemyPos = GameObject.FindGameObjectWithTag("Enemy").transform.localPosition;
			transform.localPosition = Vector2.MoveTowards(transform.localPosition, enemyPos, 0.5f);
		}

		if (transform.localPosition.x > MyGameManager.PlayerBorderX) {
			Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
			transform.localPosition = Vector2.MoveTowards(transform.localPosition, playerPos, 0.5f);
		}
	}
	*/
	void DeadTargetGoesToScoreBoard(){
		if (Vector3.Distance (transform.localPosition, ScorerBoard) < 1.0f) {
			Destroy (gameObject);
		} else {
			transform.localPosition = Vector2.MoveTowards (transform.localPosition, ScorerBoard, 1.0f);
		}
	}

	public virtual void Update () {
		if (isDeadTarget) {
			DeadTargetGoesToScoreBoard();
		}

	}


}
