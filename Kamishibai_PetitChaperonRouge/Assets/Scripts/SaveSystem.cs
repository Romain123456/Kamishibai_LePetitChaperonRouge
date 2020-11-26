using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{

    public static void SaveLivre(LivreManagement livreScript)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        //Changer nom du livre pour chaque livre !
        string path = Application.persistentDataPath + LivreManagement.nomSave;

        FileStream stream = new FileStream(path, FileMode.Create);

        LivreData data = new LivreData(livreScript);

        formatter.Serialize(stream, data);
        stream.Close();
    }


    public static LivreData LoadLivre()
    {
        //Changer nom du livre pour chaque livre !
        string path = Application.persistentDataPath + LivreManagement.nomSave;

        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LivreData data = formatter.Deserialize(stream) as LivreData;
            stream.Close();

            return data;
        } else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }


    
}
