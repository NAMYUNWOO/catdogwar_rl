using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_parent : MonoBehaviour {
	float moveSpeed = 0.275f;
	public GameObject Gun;
	float xBorderLeft = 5.0f;
	float xBorderRight = 8.2f;
	float yBorderUp = 3.0f;
	float yBorderDown = -4.3f;
	public bool shootok = true;
	// Movable for virtual pad 
	void AnimationChange(bool isRun){
		if (isRun) {
			GetComponent<Animator> ().SetTrigger ("Run");
		} else {
			GetComponent<Animator> ().SetTrigger ("Idle");
		}
	}
	bool isShootOk(){
		return shootok;
	}
	protected void Movable(Vector2 deltaVec){
		AnimationChange(deltaVec.magnitude >= 0.1f);
		float xPos = deltaVec.x/2.0f;
		float yPos = deltaVec.y/2.0f;
		float xPosCur = transform.localPosition.x;
		float yPosCur = transform.localPosition.y;
		float newxPosCur = xPosCur + xPos;
		float newyPosCur = yPosCur + yPos;
		/*
		if (newxPosCur > xBorderLeft &&
			newxPosCur < xBorderRight) {
			xPosCur = newxPosCur;
		}
		*/
		if (newyPosCur < yBorderUp &&
			newyPosCur > yBorderDown) {
			yPosCur = newyPosCur;
		}
		transform.localPosition = new Vector2 (xPosCur, yPosCur);
		if (transform.localPosition.y > yBorderUp) {
			transform.localPosition = new Vector2 (xPosCur, yBorderUp);
		}
		if (transform.localPosition.y < yBorderDown) {
			transform.localPosition = new Vector2 (xPosCur, yBorderDown);
		}
	}


	// Movable for keyboard
	protected void Movable(bool left,bool right,bool up,bool down){
		AnimationChange (left || right || up || down);
		Vector2 playerPos = transform.localPosition; 
		if (left&& playerPos.x > xBorderLeft) {
			//transform.localPosition = new Vector2 (playerPos.x - moveSpeed, playerPos.y);
		}
		if (right && playerPos.x < xBorderRight) {
			//transform.localPosition = new Vector2 (playerPos.x + moveSpeed, playerPos.y)
		}
		if (up && playerPos.y < yBorderUp) {
			transform.localPosition = new Vector2 (playerPos.x, playerPos.y+ moveSpeed);
		}
		if (down && playerPos.y > yBorderDown) {
			transform.localPosition = new Vector2 (playerPos.x, playerPos.y- moveSpeed);
		}
		if (transform.localPosition.y > yBorderUp) {
			transform.localPosition = new Vector2 (playerPos.x, yBorderUp);
		}
		if (transform.localPosition.y < yBorderDown) {
			transform.localPosition = new Vector2 (playerPos.x, yBorderDown);
		}



	}


		
	protected void Shootable(bool pushKeyDown, bool pullKeyDown, bool pullKeyUp){
		Gun.GetComponent<Gun_> ().Shootable (pushKeyDown, pullKeyDown, pullKeyUp);
		
	}

	public void Action(params bool[] actions){
		bool up = actions [0];
		bool down = actions [1];
		bool pushKeyDown = actions [2];
		bool pullKeyDown = actions [3];
		AnimationChange (true);

		Vector2 playerPos = transform.localPosition;
		if (up && playerPos.y < yBorderUp) {
			transform.localPosition = new Vector2 (playerPos.x, playerPos.y+ moveSpeed);
		}
		else if (down && playerPos.y > yBorderDown) {
			transform.localPosition = new Vector2 (playerPos.x, playerPos.y- moveSpeed);
		}

		if (pushKeyDown || pullKeyDown) {
			Gun.GetComponent<Gun_> ().Shootable (pushKeyDown, pullKeyDown, false);
		} else {
			Gun.GetComponent<Gun_> ().Shootable (false, false, true);
		}
		if (transform.localPosition.y > yBorderUp) {
			transform.localPosition = new Vector2 (playerPos.x, yBorderUp);
		}
		if (transform.localPosition.y < yBorderDown) {
			transform.localPosition = new Vector2 (playerPos.x, yBorderDown);
		}

		//print ("positions3 : " + transform.localPosition.ToString ());

	}


	void Start () {

		if (transform.localPosition.x < 0.0f) {
			float temp = xBorderLeft;
			xBorderLeft = xBorderRight;
			xBorderRight = temp;
			xBorderLeft *=-1.0f;
			xBorderRight *= -1.0f;
		}
	}
		


}
