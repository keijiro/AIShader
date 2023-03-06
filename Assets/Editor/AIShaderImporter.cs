using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace AIShader {

[ScriptedImporter(1, Extension)]
sealed class AIShaderImporter : ScriptedImporter
{
    public const string Extension = "aishader";

    [SerializeField, TextArea] string _prompt = "It gets two input textures and overlays one texture to another. The opacity value should be specified via a float property.";

    string WrapPrompt(string input)
      => "Create an unlit shader for Unity. " + _prompt + " Please don't add any note nor explanation to the response. I only need the code body.";

    public override void OnImportAsset(AssetImportContext ctx)
    {
        var json = OpenAIUtil.InvokeChat(WrapPrompt(_prompt));
        var response = JsonUtility.FromJson<OpenAI.Response>(json);
        var code = response.choices[0].message.content;

        Debug.Log(code);

        var shader = ShaderUtil.CreateShaderAsset(ctx, code, false);
        ctx.AddObjectToAsset("MainAsset", shader);
        ctx.SetMainObject(shader);
    }

    [MenuItem("Assets/Create/AI Shader")]
    public static void CreateNewAsset()
      => ProjectWindowUtil.CreateAssetWithContent("New AI Shader." + Extension, "");
}

} // namespace AIShader
