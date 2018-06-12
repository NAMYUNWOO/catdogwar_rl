using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPull : Bullet {

	void OnTriggerEnter2D(Collider2D collider){
        MyGameManager mgm = GameObject.Find("GameManager").GetComponent<MyGameManager>();
        if (collider.gameObject.tag == "TargetCoin" || collider.gameObject.tag == "TargetSkull") {
			//Vector2 targetpos = collider.gameObject.transform.position;

			collider.gameObject.GetComponent<Rigidbody2D> ().AddForce(new Vector2 (base.pullPower, 0f));
            
            if (base.pullPower > 0.0f)
            { // dog side
                if (collider.gameObject.tag == "TargetCoin")
                {
                    // dog action correct
                    mgm.dogReward = 0.01f;
                }
                else
                {
                    // dog action incorrect
                    mgm.dogReward = -0.01f;
                }
            }
            else
            { // cat side
                if (collider.gameObject.tag == "TargetCoin")
                {
                    // cat action correct 
                    mgm.catReward = 0.01f;
                }
                else
                {
                    // cat action incorrect
                    mgm.catReward = -0.01f;
                }
            }


        }
        else
        {   /*
            if (base.pullPower > 0.0f)
            {
                mgm.dogReward = -0.1f;
            }
            else
            {
                mgm.catReward = -0.1f;
            }
            */
        }
		//Destroy (gameObject);
		base.OnTriggerEnter2D (collider);

	}
}
