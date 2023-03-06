using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace AIShader {

[ScriptedImporter(1, Extension)]
sealed class AIShaderImporter : ScriptedImporter
{
    public const string Extension = "aishader";

    public override void OnImportAsset(AssetImportContext ctx)
    {
        var txt = "";

        var data = "{ \"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"user\", \"content\": \"Create an unlit shader for Unity. It gets two input textures and overlays one texture to another. The opacity value should be specified via a float property. Please dont add any note nor explanation to the response. I only need the code body.\"}] }";
        var raw = System.Text.Encoding.UTF8.GetBytes(data);

        using (var post = UnityWebRequest.Put("https://api.openai.com/v1/chat/completions", raw))
        {
            post.method = "POST";
            post.SetRequestHeader("Content-Type", "application/json");
            post.SetRequestHeader("Authorization", "Bearer " + AIShaderSettings.instance.apiKey);

            var req = post.SendWebRequest();

            var progress = 0.0f;

            while (true)
            {
                if (req.isDone)
                {
                    txt = post.downloadHandler.text;
                    break;
                }

                EditorUtility.DisplayProgressBar("AI Shader Importer", "Generating...", progress);
                System.Threading.Thread.Sleep(100);
                progress += 0.01f;
            }
        }

        EditorUtility.ClearProgressBar();

        //var txt = System.IO.File.ReadAllText("Sample.json");
        var res = JsonUtility.FromJson<OpenAI.Response>(txt);
        var code = res.choices[0].message.content;

        Debug.Log(code);

        var shader = ShaderUtil.CreateShaderAsset(ctx, code, false);
        ctx.AddObjectToAsset("MainAsset", shader);
        ctx.SetMainObject(shader);
    }
}

} // namespace AIShader
