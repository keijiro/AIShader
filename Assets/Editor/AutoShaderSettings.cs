using UnityEngine;
using UnityEditor;

[System.Serializable]
[FilePath("UserSettings/AutoShaderSettings.asset", FilePathAttribute.Location.ProjectFolder)]
public sealed class AutoShaderSettings : ScriptableSingleton<AutoShaderSettings>
{
    public string apiKey = "SET YOUR API KEY";
    public void Save() => Save(true);
    void OnDisable() => Save();
}

sealed class AutoShaderSettingsProvider : SettingsProvider
{
    public AutoShaderSettingsProvider()
      : base("Project/Auto Shader", SettingsScope.Project) {}

    public override void OnGUI(string search)
    {
        var settings = AutoShaderSettings.instance;
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
      => new AutoShaderSettingsProvider();
}
