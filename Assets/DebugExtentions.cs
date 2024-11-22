using UnityEngine;

/// <summary>
// Bendrasis generic pletimo metodas "PrintDebug<T>"
// Praplestas C# tipas "this T value."
/// </summary>
public static class DebugExtensions
{
    public static void PrintDebug<T>(this T value, string message = "")
    {
        if (string.IsNullOrEmpty(message))
        {
            Debug.Log($"{typeof(T).Name}: {value}");
        }
        else
        {
            Debug.Log($"{message}: {value}");
        }
    }
}
