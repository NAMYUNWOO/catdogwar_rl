using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class TargetSpawn : MonoBehaviour {

	public GameObject coin;
	public GameObject skull;
	int targetCoinLen;
	int targetSkullLen;



	void Start () {
		makeTarget (true);
		makeTarget (false);
		
	}
	



	void makeTarget(bool doesMakeCoin){
		float x = Random.Range (-10.0f, 10.0f);
		int yidx = Random.Range(0, 2);
		float[] y = { -150.0f, 150.0f};
		float xpos = Random.Range (-3.0f, 3.0f);
		float ypos = Random.Range (-2.0f, 2.0f);
		transform.position =  new Vector2 (xpos, ypos);
		int zero_one = Random.Range (0, 2);
		GameObject obj;
		if (doesMakeCoin) {
			obj = (GameObject)Instantiate (coin, transform.position, Quaternion.identity);


		}else{
			obj = (GameObject)Instantiate (skull, transform.position, Quaternion.identity);


		}
		obj.GetComponent<Rigidbody2D> ().AddForce(new Vector2 (x*10f,y[yidx]));

	}

}
