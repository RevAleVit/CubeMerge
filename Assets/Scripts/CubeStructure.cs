using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Like Vector3, but with other names of fields
public class Address3
{
    public int Side;
    public int Row;
    public int Col;

    public Address3(int side,  int row, int col)
    {
        Side = side;
        Row = row;
        Col = col;
    }
}


public static class SomeValues
{
    public static int Rows = 4;
    public static int Cols = 4;
}


public static class ItCube
{
    public static ItSide[] sides = new ItSide[6];    
}

public class ItSide
{
    public ItFragment[,] fragments = new ItFragment[SomeValues.Rows, SomeValues.Cols];
}

public class ItFragment
{
    public GameObject gameobject = new GameObject();
    public Color color = new Color(0.5f, 0.6f, 0.6f);
    //public Color color = new Color(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
    public float value = 0;
    public Address3 address; //Address of fragment in Cube Massive
}

public static class Colors
{
    private static Color[] variants = new Color[] {new Color(0.1f, 0.1f, 0.1f), new Color(0.8f, 0.8f, 0.8f), new Color(0.8f, 0f, 0f), new Color(0, 0.8f, 0), new Color(0, 0, 0.8f), new Color(0.8f, 0.8f, 0)};

    public  static Color RandomColor()
    {
        return variants[Random.Range(0, variants.Length)];
    }

}