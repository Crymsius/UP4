﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //temporary

public class Grid : MonoBehaviour {

    /* Ce script attaché au GameObject GridRecipient gère le quadrillage
     * Création, supression, etc... Il est censé resservir à chaque nouvelle partie.
     */

    public Dictionary<int, Cell> coordinates = new Dictionary<int, Cell>(); // les clefs sont un encodage des coordonnées X/Y d'une case : key = 100*X+Y
    public GameObject firstCell; // cellule invisible, attribuée de base (objet CellGrid), à dupliquer pour toute création de grille.

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

                coordinates.Add(100 * i + j, cellData);
                // Considérations spatiales à adapter ou déplacer hors d'ici
                newCell.GetComponent<Transform>().localPosition = new Vector3(i-3, j-3, 0);

                newCell.SetActive(true);

                foreach (char C in ind[j]) // Attribution des éléments spéciaux à la cellule
                {
                    switch (C){
                        case 'N':
                            cellData.available = false;
                            break;
                        case 'V':
                            cellData.available = true;
                            newCell.GetComponent<SpriteRenderer>().sprite = Resources.Load("Empty", typeof(Sprite)) as Sprite;
                            break;
                        default:
                            break;
                    }
                }

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
