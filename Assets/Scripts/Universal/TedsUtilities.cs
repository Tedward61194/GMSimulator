using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TedsUtilities// : MonoBehaviour
{
    public static Vector3 ToVector3(string inString) {

        string outString;
        Vector3 result;
        var splitString = new string[3];

        // Trim extranious parenthesis

        outString = inString.Substring(1, inString.Length - 2);

        // Split delimted values into an array

        splitString = outString.Split(","[0]);

        // Build new Vector3 from array elements

        result.x = float.Parse(splitString[0]);
        result.y = float.Parse(splitString[1]);
        result.z = float.Parse(splitString[2]);

        return result;

    }
}
