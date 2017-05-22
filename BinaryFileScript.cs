using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class BinaryFileScript {

    public void SaveData<T>(T t, string filePath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filePath, FileMode.Create);
        formatter.Serialize(stream, t);
        stream.Close();
    }


    public T LoadData<T>(string filePath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filePath, FileMode.Open);
        T t = (T)formatter.Deserialize(stream);
        stream.Close();
        return t;
    }
    public T WWWLoadData<T>(string filePath)
    {
        WWW www = new WWW(filePath);
        while(!www.isDone)
        {

        }
        MemoryStream stream = new MemoryStream(www.bytes);
        BinaryFormatter formatter = new BinaryFormatter();
        T t = (T)formatter.Deserialize(stream);
        stream.Close();
        return t;
    }

    public T LoadGameData<T>(string path)
    {
        if (!File.Exists(path)) Debug.Log("File X");
        Stream stream = File.Open(path, FileMode.Open, FileAccess.Read);
        BinaryFormatter formatter = new BinaryFormatter();
        T t = (T)formatter.Deserialize(stream);
        stream.Close();
        return t;
    }

}
