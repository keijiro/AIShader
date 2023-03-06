using UnityEngine;
using UnityEditor;

namespace AIShader {

[FilePath("UserSettings/AIShaderSettings.asset",
          FilePathAttribute.Location.ProjectFolder)]
public sealed class AIShaderSettings : ScriptableSingleton<AIShaderSettings>
{
    public string apiKey = null;
    public void Save() => Save(true);
    void OnDisable() => Save();
}

sealed class AIShaderSettingsProvider : SettingsProvider
{
    public AIShaderSettingsProvider()
      : base("Project/AI Shader", SettingsScope.Project) {}

    public override void OnGUI(string search)
    {
        var settings = AIShaderSettings.instance;
        var key = settings.apiKey;
        EditorGUI.BeginChangeCheck();
        key = EditorGUILayout.TextField("API Key", key);
        if (EditorGUI.EndChangeCheck())
        {
            settings.apiKey = key;
            settings.Save();
        }
    }

    [SettingsProvider]
    public static SettingsProvider CreateCustomSettingsProvider()
      => new AIShaderSettingsProvider();
}

} // namespace AIShader
