using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TargetGen : MonoBehaviour {

	public GameObject coin;
	public GameObject skull;
	int targetCoinLen;
	int targetSkullLen;
	public bool endGame = false;
	public bool isInfinityWar = false;
	Color32[] color32 = {new Color32(224, 135, 66, 255),new Color32(132, 67, 237, 255),new Color32(115, 238, 80, 255),new Color32(71, 148, 238, 255),new Color32(242, 240, 89, 255),new Color32(220, 65, 53, 255)};
	void Start () {
		makeTarget (true);
		makeTarget (false);

	}



	public void makeTarget(bool doesMakeCoin){
		float x = Random.Range (-10.0f, 10.0f);
		int yidx = Random.Range(0, 2);
		float[] y = { -150.0f, 150.0f};
		float xpos = Random.Range (-3.0f, 3.0f);
		float ypos = Random.Range (-2.0f, 2.0f);
		transform.position =  new Vector2 (xpos, ypos);

		GameObject obj;
		if (doesMakeCoin) {
			int randStone = Random.Range (0, 6);
			if (isInfinityWar)
				coin.GetComponent<SpriteRenderer> ().color = color32[randStone];
			obj = (GameObject)Instantiate (coin, transform.position, Quaternion.identity);


		}else{
			obj = (GameObject)Instantiate(skull, transform.position, Quaternion.identity);

		}
		obj.GetComponent<Rigidbody2D> ().AddForce(new Vector2 (x*10f,y[yidx]));

	}

	public void targetUpdate(){
		if (endGame)
			return;
		int coinAmt = GameObject.FindGameObjectsWithTag ("TargetCoin").Length;
		int skullAmt = GameObject.FindGameObjectsWithTag ("TargetSkull").Length;

		if (coinAmt < 1)
			makeTarget (true);
		if (skullAmt < 1)
			makeTarget (false);
		
	}


	void Update(){
		targetUpdate ();
	}

}
