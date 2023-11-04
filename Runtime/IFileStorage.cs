using System;
using Unity.Plastic.Newtonsoft.Json.Linq;

namespace FileStore
{
    public interface IFileStorage
    {
        void DeleteAll();
        void Clean();
        void Write(string file, string text);
        void WriteEncrypted(string file, string text);
        void Write(string file, byte[] data);
        void WriteEncrypted(string file, byte[] data);
        void Write(string file, JToken json);
        void WriteEncrypted(string file, JToken json);
        void WriteTemporary(string file, string text, DateTime expiration);
        void WriteTemporaryEncrypted(string file, string text, DateTime expiration);
        void WriteTemporary(string file, byte[] data, DateTime expiration);
        void WriteTemporaryEncrypted(string file, byte[] data, DateTime expiration);
        void WriteTemporary(string file, JToken json, DateTime expiration);
        void WriteTemporaryEncrypted(string file, JToken json, DateTime expiration);
        string ReadText(string file);
        string ReadTextEncrypted(string file);
        byte[] ReadBytes(string file);
        byte[] ReadBytesEncrypted(string file);
        JObject ReadJson(string file);
        JObject ReadJsonEncrypted(string file);
        JArray ReadJsonArray(string file);
        JArray ReadJsonArrayEncrypted(string file);
    }
}