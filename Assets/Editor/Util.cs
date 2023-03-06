using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

namespace AIShader {

static class OpenAIUtil
{
    public static byte[] CreateRequestBody(string prompt)
    {
        var msg = new OpenAI.RequestMessage();
        msg.role = "user";
        msg.content = prompt;

        var req = new OpenAI.Request();
        req.model = "gpt-3.5-turbo";
        req.messages = new [] { msg };

        return System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(req));
    }

    public static string InvokeChat(string prompt)
    {
        var res = "";

        using (var post = UnityWebRequest.Put(OpenAI.Api.Url, CreateRequestBody(prompt)))
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
                    res = post.downloadHandler.text;
                    break;
                }

                EditorUtility.DisplayProgressBar("AI Shader Importer", "Generating...", progress);
                System.Threading.Thread.Sleep(100);
                progress += 0.01f;
            }
        }

        EditorUtility.ClearProgressBar();

        return res;
    }
}

} // namespace AIShader
