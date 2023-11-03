#if UNITY_EDITOR
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace FileStore
{
    public static class KeyGenerator
    {
        [MenuItem("Tools/FileStore/Generate Encryption Key Pair")]
        public static void GenerateKeyPair()
        {
            //Code needed to generate key and iv for encryption of file 
            RijndaelManaged rijndael = new RijndaelManaged();
            rijndael.GenerateKey();
            rijndael.GenerateIV();
            var key = rijndael.Key;
            var iv = rijndael.IV;
            Debug.Log("key : " + System.Convert.ToBase64String(key));
            Debug.Log("IV : " + System.Convert.ToBase64String(iv));
        }
    }
}
#endif