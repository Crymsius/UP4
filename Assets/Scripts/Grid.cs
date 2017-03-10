using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //temporary

public class Grid : MonoBehaviour {

    /* Ce script attaché au GameObject GridRecipient gère le quadrillage
     * Création, supression, etc... Il est censé resservir à chaque nouvelle partie.
     */

    public Dictionary<int, Cell> coordinates = new Dictionary<int, Cell>(); // les clefs sont un encodage des coordonnées ligneX/colonneY d'une case : key = X+100*Y
    public GameObject firstCell; // Linkée depuis le dossier Prefabs
    public GameObject firstWallD;
    public GameObject firstWallB;
    public GameObject firstWallC;
    public GameObject rotateR;
    public GameObject rotateL;

    // Use this for initialization
    void Start () {
        
	}

    public void createGrid(string encodage) //TO BE IMPROVED
    {
        /* Crée une grille à partir d'un encodage
         * Le code comprend les séparateurs '+' pour nouvelle ligne, et '-' pour case suivante
         * Pour chaque case, il suffit de faire se succéder les caractères définissant le contenu de la case.
         * N : néant, V : vide, D : mur à droite, B : mur en bas, C : coin bas-gauche, (A COMPLETER)
         * Chaque ligne doit toujours avoir une case N à sa gauche et à sa droite. Chaque colonne doit toujours avoir une case N en haut et en bas.
         * Ne jamais faire apparaître de coordonnées négatives
         */
        string[] rows = encodage.Split(new string[] { "+" }, System.StringSplitOptions.None);
        for(int i = 0; i < rows.Length; i++) // Pour chaque colonne
        {
            string[] ind = rows[i].Split(new string[] { "-" }, System.StringSplitOptions.None);
            for(int j=0; j < ind.Length; j++) // Pour chaque cellule
            {
                GameObject newCell = Instantiate(firstCell);
                Cell cellData = newCell.GetComponent<Cell>();
                cellData.transform.SetParent(gameObject.transform);

                coordinates.Add(100 * j + i, cellData);
                cellData.coordinates = 100 * j + i;
                // Considérations spatiales de la ligne suivante à adapter ou déplacer hors d'ici
                // newCell.GetComponent<Transform>().localPosition = new Vector3(j-3, i-3, 0);
                newCell.GetComponent<RectTransform>().anchorMin = new Vector3((float)j / rows.Length, (float)i / ind.Length,0);
                newCell.GetComponent<RectTransform>().anchorMax = new Vector3((float)(j + 1) / rows.Length, (float)(i + 1) / ind.Length,0);
                newCell.GetComponent<RectTransform>().offsetMin = Vector3.zero;
                newCell.GetComponent<RectTransform>().offsetMax = Vector3.zero;
                newCell.GetComponent<RectTransform>().localScale = Vector3.one*35f;

                newCell.SetActive(true);

                GameObject newWall = gameObject; // L'instantiation ne sert à rien
                foreach (char C in ind[j]) // Attribution des éléments spéciaux à la cellule
                {
                    switch (C){ // A factoriser
                        case 'N':
                            cellData.available = false;
                            break;
                        case 'V':
                            cellData.available = true;
                            newCell.GetComponent<SpriteRenderer>().sprite = Resources.Load("Empty", typeof(Sprite)) as Sprite;
                            break;
                        case 'D':
                            cellData.wallsAndTriggers.Add("D");
                            newWall = Instantiate(firstWallD);
                            newWall.transform.SetParent(newCell.transform);
                            newWall.GetComponent<RectTransform>().anchorMin = new Vector2(0.9f,0);
                            newWall.GetComponent<RectTransform>().anchorMax = new Vector2(1.1f,1);
                            break;
                        case 'B':
                            cellData.wallsAndTriggers.Add("B");
                            newWall = Instantiate(firstWallB);
                            newWall.transform.SetParent(newCell.transform);
                            newWall.GetComponent<RectTransform>().anchorMin = new Vector2(0, -0.1f);
                            newWall.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.1f);
                            break;
                        case 'C':
                            cellData.wallsAndTriggers.Add("C");
                            newWall = Instantiate(firstWallC);
                            newWall.transform.SetParent(newCell.transform);
                            newWall.GetComponent<RectTransform>().anchorMin = new Vector2(-0.1f, -0.1f);
                            newWall.GetComponent<RectTransform>().anchorMax = new Vector2(0.1f, 0.1f);
                            break;
                        case 'R':
                            cellData.wallsAndTriggers.Add("R");
                            newWall = Instantiate(rotateR);
                            newWall.transform.SetParent(newCell.transform);
                            newWall.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                            newWall.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                            break;
                        case 'L':
                            cellData.wallsAndTriggers.Add("L");
                            newWall = Instantiate(rotateL);
                            newWall.transform.SetParent(newCell.transform);
                            newWall.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                            newWall.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                            break;
                        default:
                            break;
                    }
                }
                newWall.GetComponent<RectTransform>().offsetMin = Vector2.zero;
                newWall.GetComponent<RectTransform>().offsetMax = Vector2.zero;
                newWall.GetComponent<RectTransform>().localScale = Vector3.one;

            }
        }

    }

    public void EraseGrid()
    {
        foreach (GameObject child in gameObject.transform)
            if (child.name != "GridCell")
                Destroy(child);
        coordinates = new Dictionary<int, Cell>();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
