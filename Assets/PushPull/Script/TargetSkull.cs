using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class TargetSkull : Target {
	void OnCollisionEnter2D(Collision2D collision){
		base.myOnCollisionEnter2D(collision,false);

	}


	public override void Start () {
		base.Start();
		base.effect = -1f;
		//SpriteRenderer rend = GetComponent<SpriteRenderer>();
		//rend.color = new Color (0.0f, 0f, 255.0f, 1.0f);
	}

	public override void Update(){
		base.Update ();
	}

}

