using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Board
{
    [System.Serializable]
    public struct RowData 
    {
        public bool[] row;
    }

    public Grid grid;
    public RowData[] rows = new RowData[7]; //Grid 7x7
}
