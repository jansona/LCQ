using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWindows
{
    //聊天记录存储对象
    [Serializable]
    public class Message
    {
        //聊天对象的ip
        public string EndIp { get; set; }
        //聊天对象的文本内容
        public string Text { get; set; }
        public Message() { }
        public Message(string endIp, string text)
        {
            EndIp = endIp;
            Text = text;
        }
    }
}
