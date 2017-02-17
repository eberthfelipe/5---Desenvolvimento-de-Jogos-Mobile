using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ReturnMenu ()
	{
		
		Application.LoadLevel ("Gameplay");

	}
}
