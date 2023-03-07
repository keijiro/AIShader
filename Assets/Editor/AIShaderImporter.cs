using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace AIShader {

[ScriptedImporter(1, Extension)]
sealed class AIShaderImporter : ScriptedImporter
{
    public const string Extension = "aishader";

    #pragma warning disable CS0414

    [SerializeField, TextArea(3, 20)]
    string _prompt = "Simple solid fill shader. The color is exposed as a property.";

    [SerializeField, TextArea(3, 20)]
    string _cached = null;

    #pragma warning restore CS0414

    public override void OnImportAsset(AssetImportContext ctx)
    {
        var shader = ShaderUtil.CreateShaderAsset(ctx, _cached, false);
        ctx.AddObjectToAsset("MainAsset", shader);
        ctx.SetMainObject(shader);
    }

    [MenuItem("Assets/Create/AI Shader")]
    static void CreateNewAsset()
      => ProjectWindowUtil.CreateAssetWithContent("New AI Shader." + Extension, "");
}

} // namespace AIShader
