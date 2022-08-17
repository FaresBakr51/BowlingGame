using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



//[CreateAssetMenu(fileName ="new test",menuName = "Test")]
public class TestEditor : ScriptableObject
{

#if UNITY_EDITOR
    public Button _addbutt;
    [MenuItem("New TestNote/Creat New Test")]
    public static void CreatNewTest()
    {

        TestEditor test = CreateInstance<TestEditor>();
        test.name = "new ch";
        AssetDatabase.CreateAsset(test,"Assets/NewCharacters/NewCh.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = test;


    }

#endif
}
