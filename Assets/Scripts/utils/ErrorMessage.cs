using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Deprecated
public static class ErrorMessage
{
    public static void PrintNullError(string className, string functionName, string valueName)
    {
        Debug.LogError($"{className} -> {functionName} -> value {valueName} is null");
    }
}
