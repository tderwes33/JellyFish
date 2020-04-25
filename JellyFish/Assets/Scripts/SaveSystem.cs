using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveForce(force forceObject)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player1.data";

        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(forceObject);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadForce()
    {
        string path = Application.persistentDataPath + "/player1.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}
