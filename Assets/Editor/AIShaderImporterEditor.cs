using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace AIShader {

[CustomEditor(typeof(AIShaderImporter))]
sealed class AIShaderImporterEditor : ScriptedImporterEditor
{
    SerializedProperty _prompt;
    SerializedProperty _cached;

    public override void OnEnable()
    {
        base.OnEnable();
        _prompt = serializedObject.FindProperty("_prompt");
        _cached = serializedObject.FindProperty("_cached");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_prompt);
        if (GUILayout.Button("Generate")) Regenerate();
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_cached);
        serializedObject.ApplyModifiedProperties();
        ApplyRevertGUI();
    }

    static string WrapPrompt(string input)
      => "Create an unlit shader for Unity. " + input +
         " Please don't add any note nor explanation to the response." +
         " I only need the code body.";

    void Regenerate()
    {
        var json = OpenAIUtil.InvokeChat(WrapPrompt(_prompt.stringValue));
        var response = JsonUtility.FromJson<OpenAI.Response>(json);
        _cached.stringValue = response.choices[0].message.content;
    }

    [MenuItem("Assets/Create/AI Shader")]
    public static void CreateNewAsset()
      => ProjectWindowUtil.CreateAssetWithContent
           ("New AI Shader." + AIShaderImporter.Extension, "");
}

} // namespace AIShader
