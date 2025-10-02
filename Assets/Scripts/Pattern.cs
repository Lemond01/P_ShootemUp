using UnityEngine;

public struct Pattern
{
    public Pattern(Elements[] _arrows, char[] _arrowsChar)
    {
        arrows = _arrows;
        arrowsChar = _arrowsChar;
    }

    public Elements[] arrows;
    public char[] arrowsChar;
}

public enum Elements { Red, Blue, Up, Right, Down, Left }