using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using UnityEditor.AssetImporters;

[ScriptedImporter(1, Extension)]
sealed class AutoShaderImporter : ScriptedImporter
{
    public const string Extension = "autoshader";

    public override void OnImportAsset(AssetImportContext ctx)
    {
        var form = new WWWForm();

        using (var post = UnityWebRequest.Post("https://www.google.com", form))
        {
            var req = post.SendWebRequest();

            var progress = 0.0f;

            while (true)
            {
                if (req.isDone)
                {
                    Debug.Log(post.responseCode);
                    break;
                }

                EditorUtility.DisplayProgressBar("Auto Shader Importer", "Generating...", progress);
                System.Threading.Thread.Sleep(100);
                progress += 0.01f;
            }
        }

        EditorUtility.ClearProgressBar();


        var shader = ShaderUtil.CreateShaderAsset(ctx, SampleResponse, false);
        ctx.AddObjectToAsset("MainAsset", shader);
        ctx.SetMainObject(shader);
    }
}
