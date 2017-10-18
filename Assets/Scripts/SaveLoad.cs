using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using UnityEngine;

public static class SaveLoad {

    [System.Serializable]
    private class SavesCollection
    {
        public ItSide[] CubeState { get; set; }
        public int MagicCount { get; set; }

        public SavesCollection()
        {
            CubeState = ItCube.sides;
            MagicCount = Magic.MagicsCount;
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
        }
        else
            return false;

        return true;
    }
}
