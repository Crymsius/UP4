using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// cell virtuelle contenant les infos d'une cellule. Sert de sauvegarde du contenu d'une grille
/// </summary>
[System.Serializable]
public class CellHolder {
    public Coord coordinates;
    public Cell.Walls walls;
    public Cell.Trigger triggers;
    public bool hidden;
    public bool full;
    public bool available;
}