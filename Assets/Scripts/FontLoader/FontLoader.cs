using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 #if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(FontLoaderProperties))]
public class FontLoader : Editor 
{
    SerializedProperty serializedFont;
    SerializedProperty serializedTextFile;
    public override void OnInspectorGUI()
    {
        if (serializedObject == null)
            return;

        var loadedVars = serializedObject.FindProperty("vars");

        if (loadedVars == null)
        {
            Debug.Log("no vars");
            return;
        }

        serializedFont = loadedVars.FindPropertyRelative("font");
        serializedTextFile = loadedVars.FindPropertyRelative("textFile"); 
        
        if (serializedFont == null || serializedTextFile == null)
        {
            Debug.Log("no serializedFont or serializedTextFile");
            return;
        }

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(serializedFont, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(serializedTextFile, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();


        if (GUILayout.Button("LoadFont", GUILayout.Width(255)))
        {
            loadFont();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        SceneView.RepaintAll();

        serializedObject.ApplyModifiedProperties();
    }

    Dictionary<string, string> getVariables(string list)
    {
        Dictionary<string, string> dic = new Dictionary<string,string>();
        string[] tokens = list.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string token in tokens)
        {
            if (token!="")
            {
                string[] variables = token.Split(new char[] { '=' }, System.StringSplitOptions.RemoveEmptyEntries);
                dic.Add(variables[0], variables[1]);
            }
        }
        return dic;
    }

    void loadFont()
    {
        TextAsset textAsset = (TextAsset)serializedTextFile.objectReferenceValue;
        int numChars = 0;
        int imgWidth = 0;
        int imgHeight = 0;
        Stack stringStack = new Stack();
        foreach (string str in textAsset.text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries))
        {
            string s = str.Replace("\" \"", "\"\"");
            string[] tokens = s.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (tokens[0] == "common")
            {
                string subStr = s.Substring(tokens[0].Length+1);
                Dictionary<string, string> vars = getVariables(subStr);
                
                imgWidth = int.Parse(vars["scaleW"]);
                imgHeight = int.Parse(vars["scaleH"]);

            }
            else if (tokens[0] == "chars")
            {
                string subStr = s.Substring(tokens[0].Length);
                Dictionary<string, string> vars = getVariables(subStr);
                numChars = int.Parse(vars["count"]);
            }
            else if (tokens[0] == "char")
            {
                string subStr = s.Substring(tokens[0].Length);
                Dictionary<string, string> vars = getVariables(subStr);
                stringStack.Push(vars);
            }
        }

        CharacterInfo[] chrInfos = new CharacterInfo[stringStack.Count];
        for (int i = 0; i < chrInfos.Length; i++)
        {
            Dictionary<string, string> vars = (Dictionary<string, string>)stringStack.Pop();

            chrInfos[i].flipped = false;
            chrInfos[i].index = int.Parse(vars["id"]);
            chrInfos[i].style = FontStyle.Normal;
            chrInfos[i].width = int.Parse(vars["xadvance"]);
            chrInfos[i].size = 0;
            chrInfos[i].vert = new Rect(int.Parse(vars["xoffset"]), -int.Parse(vars["yoffset"]), int.Parse(vars["width"]), -int.Parse(vars["height"]));
            chrInfos[i].uv = new Rect(float.Parse(vars["x"]) / imgWidth, 1 - (float.Parse(vars["y"]) + int.Parse(vars["height"])) / imgHeight, float.Parse(vars["width"]) / imgWidth, float.Parse(vars["height"]) / imgWidth);
        }

        Font font = (Font)serializedFont.objectReferenceValue;
        font.characterInfo = chrInfos;
    }
}
#endif