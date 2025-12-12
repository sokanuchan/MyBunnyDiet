using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Runtime.Serialization;

public class SerializationManager
{
    private static string savePath = Application.persistentDataPath + "/saves/";
    private static string saveExtension = ".save";

    public static bool Save(string saveName, object saveData)
    {
        // get binary formatter
        BinaryFormatter formatter = GetBinaryFormatter();

        // create saves directory if not exists
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        // serialize data into save file
        string saveFilePath = savePath + saveName + saveExtension;
        FileStream saveFile = File.Create(saveFilePath);
        formatter.Serialize(saveFile, saveData);
        saveFile.Close();

        return true;
    }

    public static object Load(string saveName)
    {
        // check that save file path exists
        string saveFilePath = savePath + saveName + saveExtension;
        if (!File.Exists(saveFilePath))
        {
            return false;
        }

        // get binary formatter
        BinaryFormatter formatter = GetBinaryFormatter();

        // deserialize data from save file
        FileStream saveFile = File.Open(saveFilePath, FileMode.Open);
        try
        {
            object save = formatter.Deserialize(saveFile);
            saveFile.Close();
            return save;
        }
        catch (Exception exceptionMessage)
        {
            Debug.LogError("Failed to load file at path " + saveFilePath + " Exception message: " + exceptionMessage);
            saveFile.Close();
            return null;
        }
    }

    public static BinaryFormatter GetBinaryFormatter()
    {
        // init binary formatter
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        return binaryFormatter;
    }
}
