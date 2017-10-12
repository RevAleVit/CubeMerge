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
    public static ItSide[] sides;

    public static void Fill()
    {
        sides = new ItSide[6];


        //Set position of sides
        Vector3[] rotation = new Vector3[6];
        Vector3[] position = new Vector3[6];
        position[0] = new Vector3(0, 0, -0.5f);     rotation[0] = new Vector3();
        position[1] = new Vector3(0, 0, 0.5f);      rotation[1] = new Vector3();
        position[2] = new Vector3(-0.5f, 0, 0);     rotation[2] = new Vector3(0, 90, 0);
        position[3] = new Vector3(0.5f, 0, 0);      rotation[3] = new Vector3(0, 90, 0);
        position[4] = new Vector3(0, -0.5f, 0);     rotation[4] = new Vector3(90,0,0);
        position[5] = new Vector3(0, 0.5f, 0);      rotation[5] = new Vector3(90,0,0);


        //Fill Cube
        for (int k = 0; k < sides.Length; k++)
        {
            sides[k] = new ItSide();

            //Fill Side
            for (int i = 0; i < SomeValues.Rows; i++)
            {
                //Calculate position of row
                if (k==4||k==5) position[k].x = i * 0.25f - 0.375f;
                else position[k].y = i * 0.25f - 0.375f;

                //Fill Row
                for (int j = 0; j < SomeValues.Cols; j++)
                {
                    sides[k].fragments[i, j] = new ItFragment();

                    //Calculate position of fragment
                    if(k==0||k==1) position[k].x = j * 0.25f - 0.375f; 
                    else position[k].z = j * 0.25f - 0.375f;

                    sides[k].fragments[i, j].gameobject.transform.position = position[k];
                    sides[k].fragments[i, j].gameobject.transform.Rotate(rotation[k]);
                    sides[k].fragments[i, j].address = new Address3(k, i, j); //Write address in format (Side, Row, Col);
                }
            }
        }
    }
}

public class ItSide
{
    public ItFragment[,] fragments = new ItFragment[SomeValues.Rows, SomeValues.Cols];
}

public class ItFragment
{
    public GameObject gameobject = new GameObject();
    //public Color color = new Color(0.5f, 0.6f, 0.6f);
    public Color color = new Color(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
    public int value = 0;
    public Address3 address; //Address of fragment in Cube Massive
}