using System;

namespace NetMqClient.Model
{
    [Serializable]
    public class Message
    {
        public string Text { get; set; }
        public int ValueCode { get; set; }
        public string Type { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
