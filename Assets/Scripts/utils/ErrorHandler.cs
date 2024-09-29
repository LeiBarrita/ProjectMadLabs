using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ErrorHandler
{
    public static bool ValueExists<T>(T value, string className, string functionName, string valueName)
    {
        if (value == null)
        {
            Debug.LogError($"{className} -> {functionName} -> value {valueName} is null");
            return false;
        }
        return true;
    }
}
