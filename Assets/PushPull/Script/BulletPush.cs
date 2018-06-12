using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletPush : Bullet {

	void OnTriggerEnter2D(Collider2D collider){
        MyGameManager mgm = GameObject.Find("GameManager").GetComponent<MyGameManager>();
        if (collider.gameObject.tag == "TargetCoin" || collider.gameObject.tag == "TargetSkull") {
			//Vector2 targetpos = collider.gameObject.transform.position;
			collider.gameObject.GetComponent<Rigidbody2D> ().AddForce(new Vector2 (base.pushPower, 0f));
            
            if (base.pushPower < 0.0f)
            { // dog side
                if (collider.gameObject.tag == "TargetSkull")
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
                if (collider.gameObject.tag == "TargetSkull")
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
        {
            /*
            if (base.pushPower < 0.0f)
            {
                mgm.dogReward = -0.1f;
            }
            else
            {
                mgm.catReward = -0.1f;
            }
            */
        }
        base.OnTriggerEnter2D (collider);
	}
}
