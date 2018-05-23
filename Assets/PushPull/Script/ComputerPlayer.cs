using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerPlayer : MonoBehaviour {



	float moveSpeed = 0.2f;
	float invertMult;
	public bool invertX;
	Vector2 gunPos;
	public GameObject Push;
	public GameObject Pull;
	float xBorderLeft = 5.0f;
	float xBorderRight = 8.2f;
	float yBorderUp = 4.4f;
	float yBorderDown = -2.4f;
	float[] target_Coin_Skull = new float[8];
	int TARGETNUM = 2;
	GameObject[] coinObjs;
	GameObject[] skullObjs;
	public bool actionable = true;
	float ComputerPower = 18f; 
	int cntFrame = 5;
	void CollectState()
	{
		coinObjs = GameObject.FindGameObjectsWithTag ("TargetCoin");
		skullObjs = GameObject.FindGameObjectsWithTag ("TargetSkull");

	}

	void AgentStep(int action)
	{	

		CollectState ();

		if (!actionable || (coinObjs.Length<1  && skullObjs.Length < 1)) {
			return;
		}




		transform.GetChild (0).GetComponent<SpriteRenderer> ().enabled = false;
		Vector2 playerPos = transform.position;

		//float moveX = 0.0f;



		if (action >= 5) {
			GetComponent<Animator> ().SetTrigger ("Run");
			if (action == 5 && playerPos.y < yBorderUp) {
				transform.localPosition = new Vector2 (playerPos.x, playerPos.y + moveSpeed);
			}
			if (action == 6 && playerPos.y > yBorderDown) {
				transform.localPosition = new Vector2 (playerPos.x, playerPos.y - moveSpeed);
			}
			return;

		}else if (action <= 3) {
			GetComponent<Animator> ().SetTrigger ("Idle");
			GameObject newBullet;
			gunPos = gameObject.transform.GetChild(0).position;

			float coindistY = 0.0f;
			float skulldistY = 0.0f;
			float skulldistX = 0.0f;
			if (coinObjs.Length < 1) {
				coindistY = 100.0f;
			} else {
				coindistY = (playerPos.y - coinObjs [0].transform.position.y);
			}

			if (skullObjs.Length < 1) {
				skulldistY = 100.0f;

				skulldistX = 100.0f;
			} else {
				skulldistY = (playerPos.y - skullObjs[0].transform.position.y);

				skulldistX = (playerPos.x - skullObjs[0].transform.position.x);
			}


			float coindistY_Abs = Mathf.Abs (coindistY);
			float skulldistY_Abs = Mathf.Abs (skulldistY);
			float skulldistX_Abs = Mathf.Abs (skulldistX);

			if (skulldistX_Abs < 2.0f) {
				if (skulldistY_Abs < 0.05f) {
					newBullet = Instantiate (Push, gunPos, Quaternion.identity);
					newBullet.GetComponent<Bullet> ().isPlayerBullet = transform.tag == "Player";
					newBullet.GetComponent<Bullet> ().Power = ComputerPower;
					//gameObject.GetComponent<Player_parent> ().shootok = false;
				} else {
					if (skulldistY < 0.0f) {
						
						AgentStep (5);
					} else {
						
						AgentStep (6);
					}
				}
				return;
			} else {
				/* Below is heuristic action */
				if (Mathf.Min(coindistY_Abs ,skulldistY_Abs) > 0.05f) {

					if (skulldistY_Abs < coindistY_Abs ) {
						if (skulldistY < 0.0f) {
							
							AgentStep (5);
						} else {
							
							AgentStep (6);
						}
					} else {
						if (coindistY < 0.0f) {
							
							AgentStep (5);
						} else {
							
							AgentStep (6);
						}
					}
				}


				if (gameObject.GetComponent<Player_parent> ().shootok) {

					gunPos = gameObject.transform.GetChild(0).position;
					if (coindistY_Abs < skulldistY_Abs) {
						transform.GetChild (0).GetComponent<SpriteRenderer> ().enabled = true;
						newBullet = Instantiate (Pull, gunPos, Quaternion.identity);
					} else {
						newBullet = Instantiate (Push, gunPos, Quaternion.identity);
					}
					newBullet.GetComponent<Bullet>().isPlayerBullet = transform.name == "Player";
					newBullet.GetComponent<Bullet> ().Power = ComputerPower;
					//gameObject.GetComponent<Player_parent> ().shootok = false;
				}

			}
			return;

		}

	}

	void Start () {	
		if (invertX) {
			invertMult = -1f;	
			//float y1 = Random.Range (-2.2f, 4.4f);
			//gameObject.transform.localPosition = new Vector2 (-8.0f, 0f);
		} else {
			invertMult = 1f;
			//float y2 = Random.Range (-2.2f, 4.4f);
			//gameObject.transform.localPosition = new Vector2(8.0f, 0f);
		}
		CollectState ();
	}

	// Update is called once per frame
	void Update () {
		if (!actionable)
			return;
		if (cntFrame == 5) {
			CollectState ();		
			cntFrame = 0;
		}
		AgentStep (3);


		cntFrame ++;
	}



}
