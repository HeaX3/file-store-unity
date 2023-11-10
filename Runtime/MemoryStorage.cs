using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace FileStore
{
    public class MemoryStorage : IFileStorage
    {
        private readonly Dictionary<string, byte[]> files = new();
        private readonly Dictionary<string, DateTime> expirations = new();

        public void DeleteAll()
        {
            files.Clear();
        }

        private void Delete(string file)
        {
            files.Remove(file);
            expirations.Remove(file);
        }

        public void Clean()
        {
            var now = DateTime.UtcNow;
            var expiredPaths = expirations.Where(e => e.Value <= now).Select(e => e.Key);
            foreach (var path in expiredPaths)
            {
                files.Remove(path);
                expirations.Remove(path);
            }
        }

        private void Clean(string file)
        {
            if (expirations.TryGetValue(file, out var expiration) && DateTime.UtcNow > expiration)
            {
                files.Remove(file);
                expirations.Remove(file);
            }
        }

        public void Write(string file, string text)
        {
            files[file] = Encoding.UTF8.GetBytes(text);
            expirations.Remove(file);
        }

        public void WriteEncrypted(string file, string text)
        {
            files[file] = Encoding.UTF8.GetBytes(text);
            expirations.Remove(file);
        }

        public void Write(string file, byte[] data)
        {
            files[file] = data;
            expirations.Remove(file);
        }

        public void WriteEncrypted(string file, byte[] data)
        {
            files[file] = data;
            expirations.Remove(file);
        }

        public void Write(string file, JToken json)
        {
            files[file] = Encoding.UTF8.GetBytes(json.ToString());
            expirations.Remove(file);
        }

        public void WriteEncrypted(string file, JToken json)
        {
            files[file] = Encoding.UTF8.GetBytes(json.ToString());
            expirations.Remove(file);
        }

        public void WriteTemporary(string file, string text, DateTime expiration)
        {
            files[file] = Encoding.UTF8.GetBytes(text);
            expirations[file] = expiration;
        }

        public void WriteTemporaryEncrypted(string file, string text, DateTime expiration)
        {
            files[file] = Encoding.UTF8.GetBytes(text);
            expirations[file] = expiration;
        }

        public void WriteTemporary(string file, byte[] data, DateTime expiration)
        {
            files[file] = data;
            expirations[file] = expiration;
        }

        public void WriteTemporaryEncrypted(string file, byte[] data, DateTime expiration)
        {
            files[file] = data;
            expirations[file] = expiration;
        }

        public void WriteTemporary(string file, JToken json, DateTime expiration)
        {
            files[file] = Encoding.UTF8.GetBytes(json.ToString());
            expirations[file] = expiration;
        }

        public void WriteTemporaryEncrypted(string file, JToken json, DateTime expiration)
        {
            files[file] = Encoding.UTF8.GetBytes(json.ToString());
            expirations[file] = expiration;
        }

        public string ReadText(string file)
        {
            Clean(file);
            return files.TryGetValue(file, out var f) ? Encoding.UTF8.GetString(f) : null;
        }

        public string ReadTextEncrypted(string file)
        {
            Clean(file);
            return files.TryGetValue(file, out var f) ? Encoding.UTF8.GetString(f) : null;
        }

        public byte[] ReadBytes(string file)
        {
            Clean(file);
            return files.TryGetValue(file, out var f) ? f : null;
        }

        public byte[] ReadBytesEncrypted(string file)
        {
            Clean(file);
            return files.TryGetValue(file, out var f) ? f : null;
        }

        public JObject ReadJson(string file)
        {
            Clean(file);
            try
            {
                return JObject.Parse(ReadText(file));
            }
            catch
            {
                return null;
            }
        }

        public JObject ReadJsonEncrypted(string file)
        {
            Clean(file);
            try
            {
                return JObject.Parse(ReadText(file));
            }
            catch
            {
                return null;
            }
        }

        public JArray ReadJsonArray(string file)
        {
            Clean(file);
            try
            {
                return JArray.Parse(ReadText(file));
            }
            catch
            {
                return null;
            }
        }

        public JArray ReadJsonArrayEncrypted(string file)
        {
            Clean(file);
            try
            {
                return JArray.Parse(ReadText(file));
            }
            catch
            {
                return null;
            }
        }
    }
}