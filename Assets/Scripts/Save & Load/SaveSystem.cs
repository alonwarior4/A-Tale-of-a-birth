using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGameData(StoryManager storyManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveData";

        FileStream stream = new FileStream(path , FileMode.Create);

        ManagerData managerData = new ManagerData(storyManager);

        formatter.Serialize(stream, managerData);
        stream.Close();
    }

    public static ManagerData LoadGameData()
    {
        string path = Application.persistentDataPath + "/SaveData";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ManagerData managerData = formatter.Deserialize(stream) as ManagerData;
            stream.Close();
            return managerData;
        }
        else
        {
            Debug.LogError("No path or Save Data Found in Game Directory");
            return null;
        }               
    }
}
