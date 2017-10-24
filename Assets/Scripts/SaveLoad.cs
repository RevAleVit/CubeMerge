using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public static class SaveLoad {

    [System.Serializable]
    private class SavesCollection
    {
        public ItSide[] CubeState { get; set; }
        public int MagicCount { get; set; }
        public List<float[]> lColors{ get; set;}

        public SavesCollection()
        {
            CubeState = ItCube.sides;
            MagicCount = Magic.MagicsCount;

            lColors = new List<float[]>();
            foreach(Color color in GameManager.lColors)
                lColors.Add(FromColorToFloat(color)); //Convert Color to float[] and add to lColors list in SaveCollection
        }
    }

    public static void Save()
    {
        SavesCollection ForSaveNow = new SavesCollection();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Cube.cp");
        bf.Serialize(file, ForSaveNow);
        file.Close();
    }

    public static bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/Cube.cp"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Cube.cp", FileMode.Open);
            SavesCollection Loaded;
            try
            {
                Loaded = (SavesCollection)bf.Deserialize(file);
            }
            catch { return false; }

            file.Close();

            ItCube.sides = Loaded.CubeState;
            Magic.MagicsCount = Loaded.MagicCount;
            
            if (Loaded.lColors == null) //Check value "lColors" for null in Loaded collection, because old Saves haven't this field
                GameManager.lColors = new List<Color>(SomeValues.ColorsList); //Set defaults
            else
            {
                GameManager.lColors = new List<Color>();
                foreach (float[] value in Loaded.lColors)
                    GameManager.lColors.Add(FromFloatToColor(value)); // Convert from float[] to Color and add to lColors list in GameManager
            }

            Debug.Log(GameManager.lColors);
        }
        else
            return false;

        return true;
    }

    public static float[] FromColorToFloat(Color value)
    {
        float[] outvalue = new float[3];

        outvalue[0] = value.r;
        outvalue[1] = value.g;
        outvalue[2] = value.b;

        return outvalue;
    }

    public static Color FromFloatToColor(float[] value)
    {
        return new Color(value[0], value[1], value[2]);
    }
}
