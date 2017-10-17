using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using UnityEngine;

public static class SaveLoad{

    public static void Save()//CubeState Params)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "Cube.cp");
        bf.Serialize(file, ItCube.sides);//Params);
        file.Close();
    }

    public static bool Load()
    {
        //ItSide[] value = new ItSide[]();

        if (File.Exists(Application.persistentDataPath + "Cube.cp"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "Cube.cp", FileMode.Open);
            ItCube.sides = (ItSide[])bf.Deserialize(file);
            file.Close();
        }
        else
            return false;

        return true;
    }
}
