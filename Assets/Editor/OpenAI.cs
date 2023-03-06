namespace AIShader.OpenAI
{
    [System.Serializable]
    public struct Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public struct Choice
    {
        public int index;
        public Message message;
    }

    [System.Serializable]
    public struct Response
    {
        public string id;
        public Choice[] choices;
    }
}
