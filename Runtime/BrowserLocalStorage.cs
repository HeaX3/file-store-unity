using System;
using System.Globalization;
using System.IO;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace FileStore
{
    public class BrowserLocalStorage : FileStorage
    {
        private string DataPath { get; }

        public BrowserLocalStorage(string relativePath = null)
        {
#if UNITY_EDITOR
            var baseDirectory = relativePath != null ? Path.Combine("Data", relativePath) : "Data";
#else
            var baseDirectory = relativePath != null ? Path.Combine("Data", relativePath) : "Data_DEV";
#endif
            DataPath = baseDirectory;
        }

        public override void DeleteAll()
        {
            _DeleteAll();
        }

        public override void Clean()
        {
            _Clean();
        }

        public override void Write(string file, string text)
        {
            var path = GetAbsolutePath(file);
            _Write(path, text);
        }

        public override void Write(string file, byte[] data)
        {
            var path = GetAbsolutePath(file);
            _Write(path, Convert.ToBase64String(data));
        }

        public override void WriteTemporary(string file, string text, DateTime expiration)
        {
            var path = GetAbsolutePath(file);
            _WriteTemporary(path, text, expiration.ToString(CultureInfo.InvariantCulture));
        }

        public override void WriteTemporary(string file, byte[] data, DateTime expiration)
        {
            var path = GetAbsolutePath(file);
            _WriteTemporary(path, Convert.ToBase64String(data), expiration.ToString(CultureInfo.InvariantCulture));
        }

        public override string ReadText(string file)
        {
            var path = GetAbsolutePath(file);
            return _Read(path);
        }

        public override byte[] ReadBytes(string file)
        {
            var path = GetAbsolutePath(file);
            var result = _Read(path);
            return !string.IsNullOrWhiteSpace(result) ? Convert.FromBase64String(result) : null;
        }

        public string GetAbsolutePath(string path)
        {
            var result = !Path.IsPathRooted(path) ? Path.Combine(DataPath, path) : path;
            return result.Replace("\\", "/");
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void _DeleteAll();
        [DllImport("__Internal")]
        private static extern void _Clean();
        [DllImport("__Internal")]
        private static extern void _Write(string file, string data);
        [DllImport("__Internal")]
        private static extern void _WriteTemporary(string file, string data, string expiration);
        [DllImport("__Internal")]
        private static extern string _Read(string file);

#else

        private static void _DeleteAll()
        {
            throw new PlatformNotSupportedException();
        }

        private static void _Clean()
        {
            throw new PlatformNotSupportedException();
        }

        private static void _Write(string file, string data)
        {
            throw new PlatformNotSupportedException();
        }

        private static void _WriteTemporary(string file, string data, string expiration)
        {
            throw new PlatformNotSupportedException();
        }

        private static string _Read(string file)
        {
            throw new PlatformNotSupportedException();
        }
#endif
    }
}