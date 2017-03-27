using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

}
