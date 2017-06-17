using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// grille virtuelle contenant les cellules virtuelles. Sert de sauvegarde des différentes grilles puisque serializable
/// </summary>
[System.Serializable]
public class GridHolder {
    public Coord gridSize;
    public List<CellHolder> cells;
}