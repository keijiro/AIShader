using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace AIShader {

[CustomEditor(typeof(AIShaderImporter))]
sealed class AIShaderImporterEditor : ScriptedImporterEditor
{
    SerializedProperty _prompt;
    SerializedProperty _cached;

    const string ApiKeyErrorText =
      "API Key hasn't been set. Please check the project settings " +
      "(Edit > Project Settings > AI Shader > API Key).";

    public override void OnEnable()
    {
        base.OnEnable();
        _prompt = serializedObject.FindProperty("_prompt");
        _cached = serializedObject.FindProperty("_cached");
    }

    public override void OnInspectorGUI()
    {
        var hasApiKey = !string.IsNullOrEmpty(AIShaderSettings.instance.apiKey);

        serializedObject.Update();

        EditorGUILayout.PropertyField(_prompt);

        EditorGUI.BeginDisabledGroup(!hasApiKey);
        if (GUILayout.Button("Generate")) Regenerate();
        EditorGUI.EndDisabledGroup();

        if (!hasApiKey)
            EditorGUILayout.HelpBox(ApiKeyErrorText, MessageType.Error);

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
