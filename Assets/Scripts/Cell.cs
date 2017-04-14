using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell : MonoBehaviour {
	
	public Coord coordinates;
	public GameHandler myHandler { get; set; }

	public Walls walls;
	public Trigger trigger;

    public bool hidden = false;     //cell cachée mais jouable
    public bool full = false;       //cell cachée et non jouable -> mur

    public bool available = true;	// Peut-on placer un pion dessus ?

	void Start () {
		myHandler = GameObject.Find ("GeneralHandler").GetComponent<GameHandler> ();
	}

	void OnMouseDown () // déclenché avec clic sur la grille
	{
		if (available && !myHandler.running)
			StartCoroutine (myHandler.PutAPawn (this));
	}

	// Update is called once per frame
	void Update () {
	}

	[System.Serializable]
	public struct Walls
	{
		public bool wallx;
		public bool wally;
		public bool wallxy;
	}

	[System.Serializable]
	public struct Trigger
	{
		public bool isTrigger;
		[Range(0,3)]
		public int triggerType; //0 : 90r | 1 : 90l | 2 : 180 | 3 : gravity
	}
}