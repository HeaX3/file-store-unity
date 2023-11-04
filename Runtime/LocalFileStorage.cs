using System;
using System.IO;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

#if UNITY_WEBGL && !UNITY_EDITOR
#endif

namespace FileStore
{
    public class LocalFileStorage : FileStorage
    {
        private string DataPath { get; }

        public LocalFileStorage(string relativePath = null)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            throw new Exception("LocalFileStorage does not work in the browser");
#endif
#if UNITY_EDITOR
            var baseDirectory = relativePath != null ? relativePath : "Data_DEV";
#else
            var baseDirectory = relativePath != null ? relativePath : "Data";
#endif
            DataPath = Path.Combine(Application.persistentDataPath, baseDirectory);
        }

        public override void DeleteAll()
        {
            Debug.Log("Delete all local files");
            if (!Directory.Exists(DataPath)) return;
            Directory.Delete(DataPath, true);
        }

        public override void Clean()
        {
            if (!File.Exists(DataPath)) return;
            var date = DateTime.UtcNow;
            foreach (var metaFile in Directory.EnumerateFiles(DataPath, "*.meta", SearchOption.AllDirectories)
                         .ToArray())
            {
                var meta = JObject.Parse(File.ReadAllText(metaFile));
                var expiration = (DateTime)meta["expire"];
                if (date < expiration) continue;
                var file = metaFile[..^5];
                if (File.Exists(file)) File.Delete(file);
                File.Delete(metaFile);
            }
        }

        public override void Write(string file, string text)
        {
            var path = GetAbsolutePath(file);
            CreateMissingDirectories(path);
            File.WriteAllText(path, text);
        }

        public override void Write(string file, byte[] data)
        {
            var path = GetAbsolutePath(file);
            CreateMissingDirectories(path);
            File.WriteAllBytes(path, data);
        }

        public override void WriteTemporary(string file, string text, DateTime expiration)
        {
            var path = GetAbsolutePath(file);
            CreateMissingDirectories(path);
            File.WriteAllText(path, text);
            File.WriteAllText(path + ".meta", new JObject
            {
                ["expire"] = expiration
            }.ToString());
        }

        public override void WriteTemporary(string file, byte[] data, DateTime expiration)
        {
            var path = GetAbsolutePath(file);
            CreateMissingDirectories(path);
            File.WriteAllBytes(path, data);
            File.WriteAllText(path + ".meta", new JObject
            {
                ["expire"] = expiration
            }.ToString());
        }

        public override string ReadText(string file)
        {
            var path = GetAbsolutePath(file);
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }

        public override byte[] ReadBytes(string file)
        {
            var path = GetAbsolutePath(file);
            return File.Exists(path) ? File.ReadAllBytes(path) : null;
        }

        private static void CreateMissingDirectories(string path)
        {
            var dirName = Path.GetDirectoryName(path);
            if (dirName == null) return;
            Directory.CreateDirectory(dirName);
        }

        public string GetAbsolutePath(string path)
        {
            var result = !Path.IsPathRooted(path) ? Path.Combine(DataPath, path) : path;
            return result.Replace("\\", "/");
        }
    }
}