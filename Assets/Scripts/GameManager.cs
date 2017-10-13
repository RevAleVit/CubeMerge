using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //Some base values
    public static class SomeValues
    {
        public static int SideSize = 4;

        public static int InLineLength = 3;

        public static Color DefaultColor = new Color(0.5f, 0.6f, 0.6f);

        public static float StartFrom = 0.25f;
    }

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
            for (int i = 0; i < SomeValues.SideSize; i++)
            {
                //Calculate position of row
                if (k == 4 || k == 5) position[k].x = i * 0.25f - 0.375f;
                else position[k].y = i * 0.25f - 0.375f;

                //Fill Row
                for (int j = 0; j < SomeValues.SideSize; j++)
                {
                    ItCube.sides[k].fragments[i, j] = new ItFragment();

                    //Calculate position of fragment
                    if (k == 0 || k == 1) position[k].x = j * 0.25f - 0.375f;
                    else position[k].z = j * 0.25f - 0.375f;

                    ItCube.sides[k].fragments[i, j].gameobject.transform.position = position[k];
                    ItCube.sides[k].fragments[i, j].gameobject.transform.Rotate(rotation[k]);
                    ItCube.sides[k].fragments[i, j].address = new Address3(k, i, j); //Write address (Side, Row, Col);
                }
            }
        }
    }

    //Draw cube on scene
    public void Draw()
    {
        foreach (ItSide side in ItCube.sides)
        {
            foreach (ItFragment fragment in side.fragments)
            {
                //Save position of current fragment
                Vector3 TempPos = fragment.gameobject.transform.position;
                Quaternion TempRot = fragment.gameobject.transform.rotation;
                Destroy(fragment.gameobject); //Destroy old object

                //Draw fragment
                fragment.gameobject = Instantiate(Resources.Load("Fragment"), TempPos, TempRot) as GameObject;
                fragment.gameobject.transform.parent = this.transform;

                //From massive to object on scene
                fragment.gameobject.GetComponent<Identificator>().address = fragment.address;
                fragment.gameobject.GetComponent<Identificator>().ApplyColor(fragment.color);
                fragment.gameobject.GetComponent<Identificator>().SwitchSectors(fragment.value);
            }
        }
    }

    public static bool SwapFragments(Address3 a, Address3 b)
    {
        //Swap fragments in Massive
        ItFragment Temp = ItCube.sides[a.Side].fragments[a.Row,a.Col];
        ItCube.sides[a.Side].fragments[a.Row, a.Col] = ItCube.sides[b.Side].fragments[b.Row, b.Col];
        ItCube.sides[b.Side].fragments[b.Row, b.Col] = Temp;

        //Swap inner addresses
        Address3 TempA = ItCube.sides[a.Side].fragments[a.Row, a.Col].address;
        ItCube.sides[a.Side].fragments[a.Row, a.Col].address = ItCube.sides[b.Side].fragments[b.Row, b.Col].address;
        ItCube.sides[b.Side].fragments[b.Row, b.Col].address = TempA;
        
        if (CheckForMergeReady(a) || CheckForMergeReady(b)) return true;
        else return false;
        
    }

    //check fragment on an empty
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

    private static List<Color> lColors = new List<Color> { new Color(0.1f, 0.1f, 0.1f), new Color(0.8f, 0.8f, 0.8f), new Color(0.8f, 0f, 0f), new Color(0, 0.8f, 0), new Color(0, 0, 0.8f), new Color(0.8f, 0.8f, 0) };
    
    //Generate new colored fragment
    public static  void GenerateFragment(Address3 address)
    {
        ItCube.sides[address.Side].fragments[address.Row, address.Col].value = SomeValues.StartFrom;
        ItCube.sides[address.Side].fragments[address.Row, address.Col].color = lColors[Random.Range(0, lColors.Count)]; //Random color from colors list

        CheckForMergeReady(address);
    }
    
    private static void Merge(Address3[] InLine)
    {
        //Up central fragment on next level
        ItCube.sides[InLine[1].Side].fragments[InLine[1].Row, InLine[1].Col].value *= 2;

        //Clear data in other fragments
        ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, InLine[0].Col].value = ItCube.sides[InLine[2].Side].fragments[InLine[2].Row, InLine[2].Col].value = 0;
        ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, InLine[0].Col].color = ItCube.sides[InLine[2].Side].fragments[InLine[2].Row, InLine[2].Col].color = SomeValues.DefaultColor;

        CheckForMergeReady(InLine[1]);
        
        Magic.MagicHere(InLine[1]);
    }

    private static bool CheckForMergeReady(Address3 address)
    {
        bool WasMerged = false;

        //Check for full fragment
        if (ItCube.sides[address.Side].fragments[address.Row, address.Col].value == 1)
        {
            CheckForFullSide(address);
            return WasMerged;
        }


        Address3[] InLine = new Address3[SomeValues.InLineLength];

        //Check first way(vartical\horizontal)
        InLine[0] = ItCube.sides[address.Side].fragments[address.Row, 0].address;
        for(int i = 1, f = 1; i < SomeValues.SideSize && f < InLine.Length; i++)
        {
            if(
                ItCube.sides[InLine[f-1].Side].fragments[InLine[f-1].Row, i].value == 0 || ItCube.sides[InLine[f-1].Side].fragments[InLine[f-1].Row, i].value == 1 ||
                ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, InLine[0].Col].color != ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, i].color ||                
                ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, InLine[0].Col].value != ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, i].value
                )
                f = 0;
            InLine[f] = ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, i].address;
            f++;
        }
        if (InLine[InLine.Length-1] != null) //Check for full line
        {
            Merge(InLine);
            WasMerged = true;
        }

        InLine = new Address3[SomeValues.InLineLength];

        //Check second way
        InLine[0] = ItCube.sides[address.Side].fragments[0, address.Col].address;
        for (int i = 1, f = 1; i < SomeValues.SideSize && f < InLine.Length; i++)
        {
            if (
                ItCube.sides[InLine[f-1].Side].fragments[i, InLine[f-1].Col].value == 0 || ItCube.sides[InLine[f-1].Side].fragments[i, InLine[f-1].Col].value == 1 ||
                ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, InLine[0].Col].color != ItCube.sides[InLine[0].Side].fragments[i, InLine[0].Col].color ||
                ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, InLine[0].Col].value != ItCube.sides[InLine[0].Side].fragments[i, InLine[0].Col].value
                )
                f = 0;
                        
            InLine[f] = ItCube.sides[InLine[0].Side].fragments[i, InLine[0].Col].address;
            f++;
        }
        if (InLine[InLine.Length - 1] != null) //Check for full line
        {
            Merge(InLine);
            WasMerged = true;
        }
        
        return WasMerged;
    }

    private static void CheckForFullSide(Address3 address)
    {
        foreach(ItFragment fragment in ItCube.sides[address.Side].fragments)
        {
            if (fragment.value < 1 || fragment.color != ItCube.sides[address.Side].fragments[address.Row,address.Col].color) return;
        }
        lColors.Remove(ItCube.sides[address.Side].fragments[0, 0].color);

        if (lColors.Count == 0) GameEnd.enabled = true;
    }

    [SerializeField] private static Canvas GameEnd;
    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Scene");
    }
    
    public void GoToSettings()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Settings");
    }

}
