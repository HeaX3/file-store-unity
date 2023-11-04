using System;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

namespace FileStore
{
    public abstract class FileStorage : IFileStorage
    {
        public void WriteEncrypted(string file, string text)
        {
            Write(file, FileEncryption.Encrypt(text ?? ""));
        }

        public void WriteEncrypted(string file, byte[] data)
        {
            Write(file, FileEncryption.Encrypt(data ?? Array.Empty<byte>()));
        }

        public void Write(string file, JToken json)
        {
            Write(file, json?.ToString());
        }

        public void WriteEncrypted(string file, JToken json)
        {
            WriteEncrypted(file, json?.ToString());
        }
        public void WriteTemporaryEncrypted(string file, string text, DateTime expiration)
        {
            WriteTemporary(file, FileEncryption.Encrypt(text ?? ""), expiration);
        }

        public void WriteTemporaryEncrypted(string file, byte[] data, DateTime expiration)
        {
            WriteTemporary(file, FileEncryption.Encrypt(data ?? Array.Empty<byte>()), expiration);
        }

        public void WriteTemporary(string file, JToken json, DateTime expiration)
        {
            WriteTemporary(file, json?.ToString(), expiration);
        }

        public void WriteTemporaryEncrypted(string file, JToken json, DateTime expiration)
        {
            WriteTemporaryEncrypted(file, json?.ToString(), expiration);
        }

        public string ReadTextEncrypted(string file)
        {
            var data = ReadBytes(file);
            return data != null ? FileEncryption.DecryptText(data) : null;
        }

        public byte[] ReadBytesEncrypted(string file)
        {
            var data = ReadBytes(file);
            return data != null ? FileEncryption.DecryptBytes(data) : null;
        }

        public JObject ReadJson(string file)
        {
            try
            {
                var text = ReadText(file);
                return !string.IsNullOrWhiteSpace(text) ? JObject.Parse(text) : default;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return default;
            }
        }

        public JObject ReadJsonEncrypted(string file)
        {
            try
            {
                var text = ReadTextEncrypted(file);
                return !string.IsNullOrWhiteSpace(text) ? JObject.Parse(text) : default;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return default;
            }
        }

        public JArray ReadJsonArray(string file)
        {
            try
            {
                var text = ReadText(file);
                return !string.IsNullOrWhiteSpace(text) ? JArray.Parse(text) : default;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return default;
            }
        }

        public JArray ReadJsonArrayEncrypted(string file)
        {
            try
            {
                var text = ReadTextEncrypted(file);
                return !string.IsNullOrWhiteSpace(text) ? JArray.Parse(text) : default;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return default;
            }
        }

        public abstract void DeleteAll();
        public abstract void Clean();
        public abstract void Write(string file, string text);
        public abstract void Write(string file, byte[] data);
        public abstract void WriteTemporary(string file, string text, DateTime expiration);
        public abstract void WriteTemporary(string file, byte[] data, DateTime expiration);
        public abstract string ReadText(string file);
        public abstract byte[] ReadBytes(string file);
    }
}