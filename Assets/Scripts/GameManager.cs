using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Some base values
public static class SomeValues
{
    public static int SideSize = 4;

    public static int InLineLength = 3;

    public static Color DefaultColor = new Color(0.5f, 0.6f, 0.6f);

    public static float StartFrom = 0.25f;

    public static List<Color> ColorsList = new List<Color> { new Color(0.1f, 0.1f, 0.1f), new Color(0.8f, 0.8f, 0.8f), new Color(0.8f, 0f, 0f), new Color(0, 0.8f, 0), new Color(0, 0, 0.8f), new Color(0.8f, 0.8f, 0) };
}

public class GameManager : MonoBehaviour {
    public static List<Color> lColors;

    private static bool FirstSceneOpen = true;
    // Use this for initialization
    void Start () {
        if (!FirstSceneOpen || !SaveLoad.Load())
        {
            lColors = new List<Color>(SomeValues.ColorsList);
            Fill();            
        }

        FirstSceneOpen = false;
        Draw();        

        //Load Difficult Level
        if (!PlayerPrefs.HasKey("Difficult"))
            PlayerPrefs.SetFloat("Difficult", 0.25f);
        SomeValues.StartFrom = PlayerPrefs.GetFloat("Difficult");
    }

    private void Fill()
    {        
        //Fill Cube
        for (int k = 0; k < ItCube.sides.Length; k++)
        {
            ItCube.sides[k] = new ItSide();
            //Fill Side
            for (int i = 0; i < SomeValues.SideSize; i++)
            {
                //Fill Row
                for (int j = 0; j < SomeValues.SideSize; j++)
                {
                    ItCube.sides[k].fragments[i, j] = new ItFragment();
                }
            }
        }

        SaveLoad.Save();
    }

    //Draw cube on scene
    public void Draw()
    {
        //Set position of sides
        Quaternion[] rotation = new Quaternion[6];
        Vector3[] position = new Vector3[6];
        position[0] = new Vector3(0, 0, -0.5f);     rotation[0] = new Quaternion();
        position[1] = new Vector3(0, 0, 0.5f);      rotation[1] = new Quaternion();
        position[2] = new Vector3(-0.5f, 0, 0);     rotation[2] = new Quaternion();     rotation[2].eulerAngles = new Vector3(0, 90, 0);
        position[3] = new Vector3(0.5f, 0, 0);      rotation[3] = new Quaternion();     rotation[3].eulerAngles = new Vector3(0, 90, 0);
        position[4] = new Vector3(0, -0.5f, 0);     rotation[4] = new Quaternion();     rotation[4].eulerAngles = new Vector3(90, 0, 0);
        position[5] = new Vector3(0, 0.5f, 0);      rotation[5] = new Quaternion();     rotation[5].eulerAngles = new Vector3(90, 0, 0);
        

        for (int k  =  0; k < ItCube.sides.Length; k++)
        {
            for (int i = 0; i < SomeValues.SideSize; i++)
            {
                //Calculate position of row
                if (k == 4 || k == 5) position[k].x = i * 0.25f - 0.375f;
                else position[k].y = i * 0.25f - 0.375f;

                for (int j = 0; j < SomeValues.SideSize; j++)
                {
                    //Calculate position of fragment
                    if (k == 0 || k == 1) position[k].x = j * 0.25f - 0.375f;
                    else position[k].z = j * 0.25f - 0.375f;

                    //Destroy old object
                    Destroy(ItCube.sides[k].fragments[i, j].gameobject);

                    //Draw fragment
                    ItCube.sides[k].fragments[i, j].gameobject = Instantiate(Resources.Load("Fragment"), position[k], rotation[k]) as GameObject;
                    ItCube.sides[k].fragments[i, j].gameobject.transform.parent = this.transform;

                    //From massive to object on scene
                    ItCube.sides[k].fragments[i, j].gameobject.GetComponent<Identificator>().address = new Address3(k, i, j);
                    ItCube.sides[k].fragments[i, j].gameobject.GetComponent<Identificator>().ApplyColor(ItCube.sides[k].fragments[i, j].GetColor());
                    ItCube.sides[k].fragments[i, j].gameobject.GetComponent<Identificator>().SwitchSectors(ItCube.sides[k].fragments[i, j].value);
                }
            }
        }
    }

    public static bool SwapFragments(Address3 a, Address3 b)
    {
        //Swap fragments in Massive
        ItFragment Temp = ItCube.sides[a.Side].fragments[a.Row,a.Col];
        ItCube.sides[a.Side].fragments[a.Row, a.Col] = ItCube.sides[b.Side].fragments[b.Row, b.Col];
        ItCube.sides[b.Side].fragments[b.Row, b.Col] = Temp;
                
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

    //Generate new colored fragment
    public static  void GenerateFragment(Address3 address)
    {
        ItCube.sides[address.Side].fragments[address.Row, address.Col].value = SomeValues.StartFrom;
        ItCube.sides[address.Side].fragments[address.Row, address.Col].SetColor(lColors[Random.Range(0, lColors.Count)]);  //Random color from colors list

        CheckForMergeReady(address);
    }
    
    private static void Merge(Address3[] InLine)
    {
        //Up central fragment on next level
        ItCube.sides[InLine[1].Side].fragments[InLine[1].Row, InLine[1].Col].value *= 2;

        //Clear data in other fragments
        ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, InLine[0].Col].value = ItCube.sides[InLine[2].Side].fragments[InLine[2].Row, InLine[2].Col].value = 0;
        ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, InLine[0].Col].SetColor(SomeValues.DefaultColor);
        ItCube.sides[InLine[2].Side].fragments[InLine[2].Row, InLine[2].Col].SetColor(SomeValues.DefaultColor);

        Magic.MagicHere(InLine[1]);
        CheckForMergeReady(InLine[1]);

        SaveLoad.Save();
    }

    private static bool CheckForMergeReady(Address3 address) //return true if some line was merged,false - if not
    {
        //Check for full fragment
        if (ItCube.sides[address.Side].fragments[address.Row, address.Col].value == 1)
        {
            if (CheckForFullSide(address))
                SidesControll(address);
            return false; //Line wasn't merged
        }

        Address3[] InLine = new Address3[SomeValues.InLineLength];

        //Check first way(vartical\horizontal)
        InLine[0] = new Address3(address.Side, address.Row, 0);
        for (int i = 1, f = 1; i < SomeValues.SideSize && f < InLine.Length; i++)
        {
            if(
                ItCube.sides[InLine[f-1].Side].fragments[InLine[f-1].Row, i].value == 0 || ItCube.sides[InLine[f-1].Side].fragments[InLine[f-1].Row, i].value == 1 ||
                ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, InLine[0].Col].GetColor() != ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, i].GetColor() ||                
                ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, InLine[0].Col].value != ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, i].value
                )
                f = 0;
            InLine[f] = new Address3(address.Side, address.Row, i);
            f++;
        }
        if (InLine[InLine.Length-1] != null) //Check for full line
        {
            Merge(InLine);
            return true; //Line was merged
        }

        InLine = new Address3[SomeValues.InLineLength];

        //Check second way
        InLine[0] = new Address3(address.Side, 0, address.Col);
        for (int i = 1, f = 1; i < SomeValues.SideSize && f < InLine.Length; i++)
        {
            if (
                ItCube.sides[InLine[f-1].Side].fragments[i, InLine[f-1].Col].value == 0 || ItCube.sides[InLine[f-1].Side].fragments[i, InLine[f-1].Col].value == 1 ||
                ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, InLine[0].Col].GetColor() != ItCube.sides[InLine[0].Side].fragments[i, InLine[0].Col].GetColor() ||
                ItCube.sides[InLine[0].Side].fragments[InLine[0].Row, InLine[0].Col].value != ItCube.sides[InLine[0].Side].fragments[i, InLine[0].Col].value
                )
                f = 0;

            InLine[f] = new Address3(address.Side, i, address.Col);
            f++;
        }
        if (InLine[InLine.Length - 1] != null) //Check for full line
        {
            Merge(InLine);
            return true;//Line was merged
        }
        
        return false; //Line wasn't merged
    }

    private static bool CheckForFullSide(Address3 address)
    {
        foreach(ItFragment fragment in ItCube.sides[address.Side].fragments)
        {
            if (fragment.value < 1 || fragment.GetColor() != ItCube.sides[address.Side].fragments[address.Row,address.Col].GetColor()) return false;
        }
        return true;
    }
    
    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Scene");
    }
    
    public void GoToSettings()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Settings");
    }

    public static void AfterMagic(Address3 address)
    {
        if(CheckForFullSide(address))
            SidesControll(address);

        SaveLoad.Save();
    }

    //Removing Colors \ Checking for full cube
    private static void SidesControll(Address3 address)
    {
        if (lColors.Count == 0)
        {
            for (int i = 0; i < ItCube.sides.Length; i++) //Check All Sides
            {
                if (!CheckForFullSide(new Address3(i, 0, 0)))
                    return;
            }
            GameObject.FindGameObjectWithTag("CongratulationScreen").GetComponent<Canvas>().enabled = true;
            SaveLoad.Save();
        }
        else
            lColors.Remove(ItCube.sides[address.Side].fragments[0, 0].GetColor());
    }

}
