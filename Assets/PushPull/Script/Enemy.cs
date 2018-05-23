using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
public class Enemy : Player_parent {

	Vector2 deltaVec;

	void ControlWithKeyboard(){
		base.Movable (Input.GetKey (KeyCode.LeftArrow),
			Input.GetKey (KeyCode.RightArrow),
			Input.GetKey (KeyCode.UpArrow),
			Input.GetKey (KeyCode.DownArrow));
		base.Shootable (Input.GetKey (KeyCode.W),
			Input.GetKey (KeyCode.Q),
			Input.GetKeyUp (KeyCode.Q));
		/*
		base.Movable (Input.GetKey (KeyCode.D),
			Input.GetKey (KeyCode.G),
			Input.GetKey (KeyCode.R),
			Input.GetKey (KeyCode.F));
		
		base.Shootable(Input.GetKeyDown(KeyCode.W),
			Input.GetKeyDown(KeyCode.Q),
			Input.GetKeyUp(KeyCode.Q));
		*/
	}

	void ControlWithVirtualPad(){
		deltaVec = new Vector2(CnInputManager.GetAxis("Horizontal"),CnInputManager.GetAxis("Vertical"));
		base.Movable(deltaVec);
		base.Shootable(CnInputManager.GetButton ("Push"),
			CnInputManager.GetButton ("Pull"),
			CnInputManager.GetButtonUp ("Pull"));
	}

	void Update(){
		ControlWithKeyboard ();
		//ControlWithVirtualPad ();

	}
}


