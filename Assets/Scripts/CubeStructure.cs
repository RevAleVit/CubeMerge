using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Like Vector3(nearly..), but with other names of fields
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

public static class ItCube
{
    public static ItSide[] sides = new ItSide[6];    
}

[System.Serializable]
public class ItSide
{
    public ItFragment[,] fragments = new ItFragment[SomeValues.SideSize, SomeValues.SideSize];
}

[System.Serializable]
public class ItFragment
{
    public ItFragment()
    {
        SetColor(SomeValues.DefaultColor);
    }

    public float value = 0; //Value of fragment(0.25 / 0.5 / 1)
    
    private float[] color = new float[3];

    public Color GetColor()
    {
        return new Color(color[0], color[1], color[2]);
    }

    public void SetColor(Color value)
    {
        color[0] = value.r;
        color[1] = value.g;
        color[2] = value.b;
    }

    [System.NonSerialized]
    public GameObject gameobject = new GameObject(); //Object on scene

    //[System.NonSerialized]
    //public Color color = SomeValues.DefaultColor; //Color of fragment

}