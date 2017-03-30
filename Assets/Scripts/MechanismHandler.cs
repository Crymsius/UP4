using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanismHandler : MonoBehaviour {

	/*
     * INPUTS : GameObject grille, les coordonnées de la case sur lequel le joueur a décidé de jouer
     * TRAITEMENTS : Analyse la chute du pion, déclenche tous les triggers, détecte les puissance 4
     * OUTPUTS : fin de tour auprès du GestionnairePartie, avec info puissance 4
    */

	public Grid myGrid { get; set; }

	public Atlas gridAtlas;

	public GameObject neutralPawn; // Linked depuis le dossier Prefabs
	public Sprite currentSprite { get; set; }

	public int gravity { get; set; } // direction de chute des pions
	public int gravityAngle { get; set; }
	//public Dictionary<string, int> fallIntegers = new Dictionary<string, int> () { { "up", 1 }, { "down", -1 }, { "left", -100 }, { "right", 100 }, { "UL", -99 }, { "UR", 101 }};

	// Use this for initialization
	IEnumerator Start () {
		gravity = 0; //0 : bas, 1: haut, 2: gauche, 3: droite;
		gravityAngle = -90;

		//attente que la grille d'editor soit détruite puis que la grille de jeu soit chargée.
		yield return new WaitForSeconds(2f); // à changer pour attendre que la grille soit chargée plutôt que juste "2s"

		myGrid = GameObject.Find ("Generated Grid(Clone)").GetComponent<Grid> ();
		gridAtlas = GenerateAtlas ();

	}

	public Atlas GenerateAtlas () {
		Atlas gridAtlas = new Atlas ();
		gridAtlas.gridDictionary = new Dictionary<Coord, Cell> ();
		// On parcourt l'ensemble des cases et on crée un Dictionary avec comme key les coordonnées de chacune des Cells
		foreach (Transform cellChild in myGrid.GetComponent<Transform>()) {
			gridAtlas.gridDictionary.Add (cellChild.GetComponent<Cell>().coordinates, cellChild.GetComponent<Cell>());
		}
		return gridAtlas;
	}

	public bool PawnFallCalculation (Cell startCell, int player) // Player peut etre une structure qui contient les visuels des pions, les noms, les taunts, etc...
	{
		// Calcule où le pion va s'arrêter de chuter depuis les coordonnées ou il a été lâché.
		Coord interCoords = startCell.coordinates;
		Cell currentCell = startCell;
		bool falling = true;

		while (falling)
		{
			currentCell = gridAtlas.gridDictionary[interCoords] as Cell;
			Cell nextCell = NextCell (currentCell, gravity);
			if (nextCell.available && !nextCell.Equals (currentCell))
//				/*(gravity == "down" && !currentCell.wallsAndTriggers.Contains("B")
//					|| gravity == "up" && !nextCell.wallsAndTriggers.Contains("B")
//					|| gravity == "right" && !currentCell.wallsAndTriggers.Contains("D")
//					|| gravity == "left" && !nextCell.wallsAndTriggers.Contains("D")
//				)*/
			{
			interCoords = nextCell.coordinates;
			}
			else
				falling = false;
		}
		//script plaçant le pion et lançant le visuel
		GameObject newPawn = Instantiate (neutralPawn);
		newPawn.GetComponent<SpriteRenderer> ().sprite = currentSprite;
		newPawn.transform.SetParent (currentCell.transform);
		newPawn.GetComponent<Transform> ().localPosition = Vector3.zero;
//		currentCell.GetComponentInChildren<SpriteRenderer>().sprite = currentSprite;

		PawnFallDo (newPawn, currentCell, player);
		//TODO : méthode activant tous les triggers et lançant le visuel

		//script de vérification de la puissance 4 et lançant le visuel
		return CheckAlign4 (currentCell, player);
	}

	public void PawnFallDo (GameObject pawn, Cell endCell, int player) // Place le pion, détecte et exécute tous les triggers
	{
//		pawn.transform.SetParent(endCell.transform);
//		pawn.GetComponent<Transform> ().localPosition = Vector3.zero;
		//float coeff = 1f;
//		while (coeff > 0) { //not working : pas d'update visuel depuis une while loop
//			pawn.GetComponent<Transform>().localPosition = coeff * pawn.GetComponent<Transform>().localPosition;
//			coeff -= Time.deltaTime;
//		}
		endCell.available = false;
		//endCell.content = player.ToString();
		//la partie suivante stocke tous les triggers traversés
//		int startCoords = endCell.coordinates.y;
//		do
//		{
//			startCoords -= fallIntegers[gravity];
//		} while (myGrid.allCellCoords.ContainsKey(startCoords)); // On a rejoint le bord du graphique, prêts à balayer en sens inverse.
//
//		List<string> triggers = new List<string>();
//		do
//		{
//			startCoords += fallIntegers[gravity];
//		//	foreach (string str in new List<string>() { "R", "L" })
//				//if (myGrid.coordinates[startCoords].wallsAndTriggers.Contains(str))
//				//	triggers.Add(str);
//		} 
//		while (startCoords != endCell.coordinates.y) ; // On a collecté tous les triggers traversés
//
//		if (triggers.Count != 0)
//			foreach (string str in triggers)
//				ExecuteTrigger(str);
	}
		
	public Cell NextCell (Cell currentCell, int gravity) {
		Coord currentCoordinates = currentCell.coordinates; 
		switch (gravity) {
		case 0: //vers le bas
			currentCoordinates.y = currentCoordinates.y-1;
			if (gridAtlas.gridDictionary.ContainsKey(currentCoordinates)) {
				return gridAtlas.gridDictionary[currentCoordinates];
			} else return currentCell;
		case 1: // vers le haut
			currentCoordinates.y = currentCoordinates.y+1;
			if (gridAtlas.gridDictionary.ContainsKey(currentCoordinates)) {
				return gridAtlas.gridDictionary[currentCoordinates];
			} else return currentCell;
		case 2: // vers la gauche
			currentCoordinates.x = currentCoordinates.x-1;
			if (gridAtlas.gridDictionary.ContainsKey(currentCoordinates)) {
				return gridAtlas.gridDictionary[currentCoordinates];
			} else return currentCell;
		case 3: // vers la droite
			currentCoordinates.x = currentCoordinates.x-1;
			if (gridAtlas.gridDictionary.ContainsKey(currentCoordinates)) {
				return gridAtlas.gridDictionary[currentCoordinates];
			} else return currentCell;
		default:
			return currentCell;
		}
	}

	public void ExecuteTrigger (string triggerName) // rotatifs 90° uniquement pour l'instant
	{
//		int rotate;
//		Dictionary<int, string> gravityDirections = new Dictionary<int, string>() { {270,"down" }, {0,"right" }, {90,"up" }, {180,"left" } };
//		switch (triggerName)
//		{
//		case "R":
//			rotate = 90;
//			gravityAngle = (gravityAngle + 270) % 360;
//			break;
//		case "L":
//			rotate = 270;
//			gravityAngle = (gravityAngle + 90) % 360;
//			break;
//		default:
//			rotate = 0;
//			break;
//		}
//		myGrid.gameObject.transform.eulerAngles = new Vector3(0, 0, (myGrid.gameObject.transform.eulerAngles.z + rotate) % 360);
//		gravity = gravityDirections[gravityAngle];
	}

	public bool CheckAlign4 (Cell newFilled, int player) {
//
//		int currentCoords = newFilled.coordinates.y;
//		int startCoords; bool isWinner = false;
//		foreach (string i in new List<string>() { "right", "UR", "up", "UL" }) //test selon les 4 directions
//		{
//			startCoords = currentCoords;
//			//while (startCoords / 100 > 0 && startCoords % 100 > 0)
//			while(myGrid.allCellCoords.ContainsKey(startCoords- fallIntegers[i]))
//				startCoords -= fallIntegers[i]; // On a rejoint le bord du graphique, prêts à balayer en sens inverse.
//			int count = 0;
//			while (myGrid.allCellCoords.ContainsKey(startCoords)) // On compte les pions CONSECUTIFS du joueur suivant la direction. Arrivé à 4 c'est la victoire.
//			{
//				if (myGrid.allCellCoords[startCoords].content == player.ToString())
//				{
//					if (count + 1 >= 4)
//						isWinner = true;
//					else if (!IsBlocked(startCoords, i)) count++;
//					else count = 0;
//				}
//				else count = 0;
//				startCoords += fallIntegers[i];
//			}
//		}
//		return isWinner;
		return true;
	}

	public bool IsBlocked (int coords, string direction) { //vérifie si un mur ne bloque pas l'établissement de la puissance 4
//		if (direction == "right" && myGrid.coordinates[coords].wallsAndTriggers.Contains("D")
//			|| direction == "up" && myGrid.coordinates[coords + 1].wallsAndTriggers.Contains("B")
//			|| direction == "UR" && myGrid.coordinates[coords + 101].wallsAndTriggers.Contains("C")
//			|| direction == "UL" && myGrid.coordinates[coords + 1].wallsAndTriggers.Contains("C")
//		)
		//	return true;
		//else
			return false;
	}

	// Update is called once per frame
	void Update () {

	}
}
