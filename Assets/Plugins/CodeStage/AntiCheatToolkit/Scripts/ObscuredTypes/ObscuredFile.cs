using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using CodeStage.AntiCheat.ObscuredTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Assets.Plugins.CodeStage.AntiCheatToolkit.Scripts.ObscuredTypes
{
    public enum CryptType
    {
        NoCrypt,
        Fast,
        Full
    }

    [Serializable]
    public class ObscuredFile<T>
    {
        public static ObscuredFile<T> Load(string path, bool autoLoadContent = true)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            if (path[0] != '/')
                path = '/' + path;
            
            var result = ObscuredPrefs.GetJObject<ObscuredFile<T>>("$fileInfo:" + path);

            if (result != null && autoLoadContent)
                result.ReloadContent();

            return result;
        }

        static readonly int[][] BlockSizesSeq = ObscuredCryptoKeys.FileFast;

        private const string Password = ObscuredCryptoKeys.FileFull;

        [JsonProperty("n", Order = 1)]
        public string Name { get; set; }

        [JsonProperty("d", Order = 2, NullValueHandling = NullValueHandling.Ignore)]
        private string _directory;

        [JsonProperty("v", Order = 3, NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        /// <summary>
        /// Опасно менять если Content ещё не загружен!
        /// Если не использовался autoLoadContent, то перед изменением нужно вызвать ReloadContent.
        /// </summary>
        [JsonProperty("c", Order = 4)]
        [JsonConverter(typeof(StringEnumConverter))]
        public CryptType Protected { get; set; }

        [JsonProperty("cr_t", Order = 5)]
        public DateTime CreationTime;

        [JsonProperty("lw_t", Order = 6)]
        public DateTime LastWriteTime;

        [JsonProperty("i", Order = 7, NullValueHandling = NullValueHandling.Ignore)]
        public string Info { get; set; }

        [JsonIgnore]
        public string Directory { get { return _directory ?? ""; } set { _directory = value; } }

        [JsonIgnore]
        public string Extension { get { try { return System.IO.Path.GetExtension(Name); } catch { return String.Empty; } } }

        private bool _readed = false;
        private T _content;

        [JsonIgnore]
        public bool Exists { get { return File.Exists(SystemPath); } }

        [JsonIgnore]
        public string Path { get { return Directory + "/" + Name; } }

        [JsonIgnore]
        public string SystemPath { get { return Application.persistentDataPath + "/" + Path; } }

        [JsonIgnore]
        public string InfoPrefsKey { get { return "$fileInfo:" + Path; } }

        [JsonIgnore]
        public T Content
        {
            get
            {
                if (!_readed && _content == null)
                {
                    _content = ReadContent();
                    _readed = true;
                }
                return _content;
            }
            set { _content = value; }
        }

        public ObscuredFile()
        {
            CreationTime = DateTime.UtcNow;
            LastWriteTime = CreationTime;
        }

        public void ReloadContent()
        {
            _content = ReadContent();
            _readed = true;
        }

        protected virtual T ReadContent()
        {
            if (!Exists)
                return default(T);

            FileStream file = File.Open(SystemPath, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(file);
            var data = binaryReader.ReadBytes((int) file.Length);
            file.Close();
            Decrypt(data);
            var str = ObscuredPrefs.Unzip(data);
            var result = JsonConvert.DeserializeObject<T>(str);
            return result;
        }

        protected virtual void WriteContent(T content)
        {
            if (content == null)
            {
                File.Delete(SystemPath);
                return;
            }

            FileStream file = File.Open(SystemPath, FileMode.Create);

            var str = JsonConvert.SerializeObject(content);
            var data = ObscuredPrefs.Zip(str);
            var binaryWriter = new BinaryWriter(file);
            binaryWriter.Write(data);
            Crypt(data);
            file.Close();
        }

        private void WriteFileInfo()
        {
            ObscuredPrefs.SetJObject(InfoPrefsKey, this);
        }

        public string Save()
        {
            if (string.IsNullOrEmpty(Name))
            {
                Debug.LogError("[ObscuredFile] Name is Empty");
                return Path;
            }

            LastWriteTime = DateTime.UtcNow;

            WriteFileInfo();

            WriteContent(Content);

            return Path;
        }

        public byte[] Crypt(byte[] data)
        {
            switch (Protected)
            {
                case CryptType.Fast:
                    FastEncryptor.Encrypt(data, BlockSizesSeq);
                    return data;
                case CryptType.Full:
                    return FullEncryptor.Encrypt(data, Password);
                default:
                    return data;
            }
        }

        public byte[] Decrypt(byte[] data)
        {
            switch (Protected)
            {
                case CryptType.Fast:
                    FastEncryptor.Decrypt(data, BlockSizesSeq);
                    return data;
                case CryptType.Full:
                    return FullEncryptor.Decrypt(data, Password);
                default:
                    return data;
            }
        }
    }

    [Serializable]
    public class ObscuredRawFile : ObscuredFile<byte[]>
    {
        public new static ObscuredRawFile Load(string path, bool autoLoadContent = true)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            if (path[0] != '/')
                path = '/' + path;

            var result = ObscuredPrefs.GetJObject<ObscuredRawFile>("$fileInfo:" + path);

            if (result != null && autoLoadContent)
                result.ReloadContent();

            return result;
        }

        protected override byte[] ReadContent()
        {
            if (!Exists)
                return null;

            FileStream file = File.Open(SystemPath, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(file);
            var data = binaryReader.ReadBytes((int)file.Length);
            data = Decrypt(data);
            file.Close();
            return data;
        }

        protected override void WriteContent(byte[] content)
        {
            if (content == null)
            {
                File.Delete(SystemPath);
                return;
            }

            FileStream file = File.Open(SystemPath, FileMode.Create);
            var binaryWriter = new BinaryWriter(file);
            var data = Crypt((byte[])content.Clone());
            binaryWriter.Write(data);
            file.Close();
        }
    }

    public class FullEncryptor
    {
        public static byte[] Encrypt(byte[] input, string password)
        {
            try
            {
                TripleDESCryptoServiceProvider service = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                byte[] key = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
                byte[] iv = md5.ComputeHash(Encoding.ASCII.GetBytes(password));

                return Transform(input, service.CreateEncryptor(key, iv));
            }
            catch (Exception)
            {
                return new byte[0];
            }
        }

        public static byte[] Decrypt(byte[] input, string password)
        {
            try
            {
                TripleDESCryptoServiceProvider service = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                byte[] key = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
                byte[] iv = md5.ComputeHash(Encoding.ASCII.GetBytes(password));

                return Transform(input, service.CreateDecryptor(key, iv));
            }
            catch (Exception)
            {
                return new byte[0];
            }
        }

        public static string Encrypt(string text, string password)
        {
            byte[] input = Encoding.UTF8.GetBytes(text);
            byte[] output = Encrypt(input, password);
            return Convert.ToBase64String(output);
        }

        public static string Decrypt(string text, string password)
        {
            byte[] input = Convert.FromBase64String(text);
            byte[] output = Decrypt(input, password);
            return Encoding.UTF8.GetString(output);
        }

        private static byte[] Transform(byte[] input, ICryptoTransform CryptoTransform)
        {
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(memStream, CryptoTransform, CryptoStreamMode.Write);

            cryptStream.Write(input, 0, input.Length);
            cryptStream.FlushFinalBlock();

            memStream.Position = 0;
            byte[] result = new byte[Convert.ToInt32(memStream.Length)];
            memStream.Read(result, 0, Convert.ToInt32(result.Length));

            memStream.Close();
            cryptStream.Close();

            return result;
        }
    }

    public class FastEncryptor
    {

        private static int[][] createBlockSizesSeq(string password, int count = 2)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] key = md5.ComputeHash(Encoding.ASCII.GetBytes(password));

            var blockSizesSeq = new int[count][];

            var t = 0;

            for (int i = 0; i < blockSizesSeq.Length; i++)
            {
                blockSizesSeq[i] = new int[7];

                for (int j = 0; j < 7; j++)
                {
                    blockSizesSeq[i][j] = key[t] % (t + j + 7);

                    t++;
                    if (t >= key.Length)
                        t = 0;
                }
            }

            return blockSizesSeq;
        }

        public static void Encrypt(byte[] data, string password, int count = 2)
        {
            Encrypt(data, createBlockSizesSeq(password, count));
        }

        public static void Encrypt(byte[] data, int[][] blockSizesSeq)
        {
            for (int i = 0; i < blockSizesSeq.Length; i++)
            {
                ApplyBlockSizes(data, blockSizesSeq[i]);
            }
        }

        public static void Decrypt(byte[] data, string password, int count = 2)
        {
            Decrypt(data, createBlockSizesSeq(password, count));
        }

        public static void Decrypt(byte[] data, int[][] blockSizesSeq)
        {
            for (int i = blockSizesSeq.Length - 1; i >= 0; i--)
            {
                ApplyBlockSizes(data, blockSizesSeq[i]);
            }
        }

        private static void ApplyBlockSizes(byte[] data, int[] BlockSizes)
        {
            if (data == null) return;

            var blockSizeIndex = 0;

            var startIndex = 0;

            while (startIndex <= data.Length)
            {
                if (data.Length - startIndex < BlockSizes[blockSizeIndex])
                {
                    Rotate(startIndex, data.Length - startIndex, data);
                    return;
                }

                Rotate(startIndex, BlockSizes[blockSizeIndex], data);

                startIndex += BlockSizes[blockSizeIndex];

                blockSizeIndex++;
                if (blockSizeIndex >= BlockSizes.Length)
                    blockSizeIndex = 0;
            }
        }

        private static void Rotate(int startIndex, int blockSize, byte[] data)
        {
            var fix = startIndex % 2 == 0;

            byte xor = (byte)((startIndex + blockSize) % 256);

            for (var i = 0; i < blockSize / 2; i++)
            {
                if (fix && i == 2) continue;

                var t = (byte)(data[startIndex + i] ^ xor);
                data[startIndex + i] = (byte)(data[startIndex + blockSize - i - 1] ^ xor);
                data[startIndex + blockSize - i - 1] = t;
            }
        }
    }

    public static class VeryFastEncryptor
    {
        public static byte[] CreateKey(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] key = md5.ComputeHash(Encoding.ASCII.GetBytes(password));

            return key;
        }

        public static void Encrypt(byte[] data, string password)
        {
            if (password == null) return;

            Encrypt(data, CreateKey(password));
        }

        public static void Encrypt(byte[] data, byte[] key)
        {
            if (data == null) return;

            if (key == null || key.Length == 0) return;

            var keyLength = key.Length;
            var dataLongLength = data.LongLength;

            for (long i = 0; i < dataLongLength; i++)
            {
                data[i] = (byte) (data[i] ^ key[i % keyLength]);
            }
        }

        public static void Decrypt(byte[] data, byte[] key)
        {
            Encrypt(data, key);
        }

        public static void Decrypt(byte[] data, string password)
        {
            Encrypt(data, password);
        }
    }
}
