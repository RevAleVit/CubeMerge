using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaming : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Fill();
        Draw();
	}

    private void Fill()
    {
        //Set position of sides
        Vector3[] rotation = new Vector3[6];
        Vector3[] position = new Vector3[6];
        position[0] = new Vector3(0, 0, -0.5f);     rotation[0] = new Vector3();
        position[1] = new Vector3(0, 0, 0.5f);      rotation[1] = new Vector3();
        position[2] = new Vector3(-0.5f, 0, 0);     rotation[2] = new Vector3(0, 90, 0);
        position[3] = new Vector3(0.5f, 0, 0);      rotation[3] = new Vector3(0, 90, 0);
        position[4] = new Vector3(0, -0.5f, 0);     rotation[4] = new Vector3(90, 0, 0);
        position[5] = new Vector3(0, 0.5f, 0);      rotation[5] = new Vector3(90, 0, 0);


        //Fill Cube
        for (int k = 0; k < ItCube.sides.Length; k++)
        {
            ItCube.sides[k] = new ItSide();

            //Fill Side
            for (int i = 0; i < SomeValues.Rows; i++)
            {
                //Calculate position of row
                if (k == 4 || k == 5) position[k].x = i * 0.25f - 0.375f;
                else position[k].y = i * 0.25f - 0.375f;

                //Fill Row
                for (int j = 0; j < SomeValues.Cols; j++)
                {
                    ItCube.sides[k].fragments[i, j] = new ItFragment();

                    //Calculate position of fragment
                    if (k == 0 || k == 1) position[k].x = j * 0.25f - 0.375f;
                    else position[k].z = j * 0.25f - 0.375f;

                    ItCube.sides[k].fragments[i, j].gameobject.transform.position = position[k];
                    ItCube.sides[k].fragments[i, j].gameobject.transform.Rotate(rotation[k]);
                    ItCube.sides[k].fragments[i, j].address = new Address3(k, i, j); //Write address in format (Side, Row, Col);
                }
            }
        }
    }


    public void Draw()
    {
        foreach (ItSide side in ItCube.sides)
        {
            foreach (ItFragment fragment in side.fragments)
            {
                Vector3 TempPos = fragment.gameobject.transform.position;
                Quaternion TempRot = fragment.gameobject.transform.rotation;

                Destroy(fragment.gameobject);

                fragment.gameobject = Instantiate(Resources.Load("Fragment"), TempPos, TempRot) as GameObject;
                fragment.gameobject.transform.parent = this.transform;
                fragment.gameobject.GetComponent<Identificator>().address = fragment.address;
                fragment.gameobject.GetComponent<Identificator>().ApplyColor(fragment.color);
                fragment.gameobject.GetComponent<Identificator>().TurnSectors(fragment.value);                
            }
        }
    }


    public static void SwapFragments(Address3 a, Address3 b)
    {
        //Swap fragments in Massive
        ItFragment Temp = ItCube.sides[a.Side].fragments[a.Row,a.Col];
        ItCube.sides[a.Side].fragments[a.Row, a.Col] = ItCube.sides[b.Side].fragments[b.Row, b.Col];
        ItCube.sides[b.Side].fragments[b.Row, b.Col] = Temp;

        //Swap inner addresses
        Address3 TempA = ItCube.sides[a.Side].fragments[a.Row, a.Col].address;
        ItCube.sides[a.Side].fragments[a.Row, a.Col].address = ItCube.sides[b.Side].fragments[b.Row, b.Col].address;
        ItCube.sides[b.Side].fragments[b.Row, b.Col].address = TempA;
    }

    public static bool IsEmpty(Address3 address)
    {
        if(ItCube.sides[address.Side].fragments[address.Row, address.Col].value == 0)
        {
            GenerateFragment(address);
            return true;
        }
        else
        return false;
    }
    public static  void GenerateFragment(Address3 address)
    {
        ItCube.sides[address.Side].fragments[address.Row, address.Col].value = 0.25f;
        ItCube.sides[address.Side].fragments[address.Row, address.Col].color = Colors.RandomColor();
    }
}
