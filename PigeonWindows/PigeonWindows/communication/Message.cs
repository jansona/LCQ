using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWindows
{
    //聊天记录存储对象
    [Serializable]
    public class Message
    {
        //聊天对象的ip
        //public string EndIp { get; set; }
        //聊天对象的文本内容
        public string Text { get; set; }
        public Message() { }
        public Message(string text)
        {
            //EndIp = endIp;
            Text = text;
        }
        static string encryptKey = "Oyea";    //定义密钥  

        //保存之前加密text
        public static string Encrypt(string str)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();  

            byte[] key = Encoding.Unicode.GetBytes(encryptKey); 

            byte[] data = Encoding.Unicode.GetBytes(str);

            MemoryStream MStream = new MemoryStream(); 

            //使用内存流实例化加密流对象   
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);

            CStream.Write(data, 0, data.Length);      

            CStream.FlushFinalBlock();

            return Convert.ToBase64String(MStream.ToArray());//返回加密后的字符串  
        }
        //导入之后，使用Decrypt解密得到聊天记录
        public static string Decrypt(string str)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();     

            byte[] key = Encoding.Unicode.GetBytes(encryptKey); 

            byte[] data = Convert.FromBase64String(str); 

            MemoryStream MStream = new MemoryStream();   

            //使用内存流实例化解密流对象       
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);

            CStream.Write(data, 0, data.Length);     

            CStream.FlushFinalBlock(); 

            return Encoding.Unicode.GetString(MStream.ToArray());       //返回解密后的字符串  
        }

    }
}
