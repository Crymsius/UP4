using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCell : MonoBehaviour {

	[Range(-1,1)]
	public int direction = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,0, direction);
	}
}
