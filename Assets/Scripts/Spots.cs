using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spots : MonoBehaviour
{
    private bool[][] spotsTaken = new bool[6][];

    // Move all rows up by one
    public void PushAllUp()
    {
        for (int i = spotsTaken.Length - 1; i > 0; i--)
        {
            spotsTaken[i] = spotsTaken[i - 1];
        }
    }

    // Insert a new bool array at index 0
    public void Insert(bool[] insert)
    {
        spotsTaken[0] = insert;
    }

    // Return spotsTaken
    public bool[][] GetSpotsTaken()
    {
        return spotsTaken;
    }

    // Print the desired row { [0], [1], ..., [9] }
    public void Print(int row)
    {
        // Case 1: Row is empty, log
        if (spotsTaken[row] == null)
        {
            Debug.Log("Empty");
            return;
        }

        // Case 2: Log the row
        string outString = row + ": { ";
        foreach (var boolean in spotsTaken[row])
        {
            outString += boolean + " ";
        }
        outString += "}";
        Debug.Log(outString);
    }

    // Move one true value to a false value, and make the orignal one false
    public bool Move(int row, int currentColumn, int newColumn)
    {
        bool[] rowArray = spotsTaken[row];

        // Case 1: The row is empty;
        if (rowArray == null) return false;

        // Case 2: The spot trying to be moved to is already true
        if (rowArray[newColumn]) return false;

        // Case 3: Switch
        rowArray[currentColumn] = false;
        rowArray[newColumn] = true;
        return true;
    }
}
