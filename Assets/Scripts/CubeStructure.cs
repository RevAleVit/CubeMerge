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

public class ItSide
{
    public ItFragment[,] fragments = new ItFragment[SomeValues.SideSize, SomeValues.SideSize];
}

public class ItFragment
{
    public GameObject gameobject = new GameObject(); //Object on scene
    public Color color = SomeValues.DefaultColor; //Color of fragment
    public float value = 0; //Value of fragment(0.25 / 0.5 / 1)
    public Address3 address; //Address of fragment in Cube Massive
}