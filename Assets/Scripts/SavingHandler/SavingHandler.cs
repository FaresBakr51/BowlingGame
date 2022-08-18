using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
[System.Serializable]
public class Character
{

    public string name;
    public int id;

    public Character(string name, int id)
    {
        this.name = name;
        this.id = id;
    }
}

public static class SavingHandler {

#if UNITY_EDITOR
    static List<Character> chList = new List<Character>();
    [MenuItem("TestUnlouc/UnlouckChar")]
    public static void TestUnlouck()
    {



        chList.Add(new Character("isaiah", 0));
        SaveToJSON(chList, "chData.json");


    }
#endif
    public static void SaveToJSON<T>(List<T> toSave, string filename)
    {
        Debug.Log(GetPath(filename));
        WriteFile(GetPath(filename), JsonHelper.ToJson<T>(toSave.ToArray()));


    }
    public static void SaveToJSON<T>(T toSave, string filename)
    {
        Debug.Log(GetPath(filename));
        WriteFile(GetPath(filename), JsonUtility.ToJson(toSave));


    }
    public static List<T> ReadListFromJSON<T>(string filename)
    {

        string conent = ReadFile(GetPath(filename));
        if (conent == "{}")
        {
            return new List<T>();
        }
        List<T> info = JsonHelper.FromJson<T>(conent).ToList();
        return info;

    }
    public static  T ReadFromJSON<T>(string filename)
    {

        string conent = ReadFile(GetPath(filename));
        if (conent == "{}")
        {
            return default;
        }
        T info = JsonUtility.FromJson<T>(conent);
        return info;

    }
    public static void WriteFile(string path,string content)
    {
        FileStream file = new FileStream(path, FileMode.Create);
        using (StreamWriter writer = new StreamWriter(file))
        {
            writer.Write(content);
        }

    }

    public static string GetPath(string filename)
    {

     return Application.persistentDataPath +"/" + filename;
    }
    public static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        else
        {
            return null;
        }
    }
}
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }


    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}


