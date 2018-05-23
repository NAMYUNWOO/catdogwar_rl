using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderEnd : MonoBehaviour {

	/*
	Queue<GameObject> targetToMove = new Queue<GameObject>();
	Queue<Vector2> ScorerBoard = new Queue<Vector2> ();
	MyGameManager GameManager;
	public GameObject TargetGen;
	TargetGen tg;
	void DestroyTarget(GameObject coinOrSkull,bool isCoinTarget, bool isPlayerSide){
		TargetCoinAndSkull tcs = coinOrSkull.GetComponent<TargetCoinAndSkull> ();
		tcs.isDeadTarget = true;

		coinOrSkull.GetComponent<CircleCollider2D> ().enabled = false;

		//targetToMove.Enqueue (coinOrSkull);

		//if (isPlayerSide) {
		//	tcs.ScoreBoardtoGo = GameObject.Find ("PlayerScore").transform.localPosition;
		//} else {
		//	tcs.ScoreBoardtoGo = GameObject.Find ("EnemyScore").transform.localPosition;
		//}
		
		GameManager.ScoreCounter (isCoinTarget, isPlayerSide);
		tg.makeTarget (isCoinTarget);

	}

	void OnCollisionEnter2D(Collision2D collision){
		var gm = GameObject.Find ("GameManager").GetComponent<MyGameManager> ();
		string collisionTag = collision.gameObject.tag;
		string borderName = transform.name;
		bool isPlayerSide = borderName == "BorderRight";
		bool isCoinTarget = collisionTag == "TargetCoin";

		if (collisionTag == "TargetCoin" || collisionTag == "TargetSkull") {
			//Debug.Log ("collision " + collision.gameObject.tag);
			//Debug.Log ("border " + transform.name);
			DestroyTarget (collision.gameObject,isCoinTarget, isPlayerSide);
			//gm.ScoreCounter (isCoinTarget, isPlayerSide);
		}
	}
	*/
	void Start () {
		//Debug.Log (targetToMove.Peek ());	
		/*
		GameManager = GameObject.Find("GameManager").GetComponent<MyGameManager>();
		tg = TargetGen.GetComponent<TargetGen>();
		*/
	}

	void Update () {
		


	}
}
