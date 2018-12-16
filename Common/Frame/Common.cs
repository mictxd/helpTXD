using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace TXD.CF
{
    public static partial class ConstNames
    {
        public const string CfgSqlServerName = "MsgDestDB";

        public const string combobox请选择 = "请选择";
    }

    public static partial class HelpTXD
    {
        public static bool ToMD5(string Input, ref string Output, ref string ErrMSG)
        {
            int lv_maxLength = Input.Length;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = new byte[lv_maxLength];
            for (int lvi = 0; lvi < lv_maxLength; lvi++)
            {
                if (lvi >= Input.Length)
                {
                    data[lvi] = 0;
                }
                else
                {
                    data[lvi] = (byte) Input[lvi];
                }
            }
            byte[] result = md5.ComputeHash(data);
            Output = "";
            foreach (byte abyte in result)
            {
                Output = Output + (char) abyte;
            }
            return true;
        }

        //public static string Key2Base( string key)
        //{

        //    //System.Text.Encoder lvEncoder=;
        //    byte[] buffChar = Encoding.Unicode.GetBytes(key);
        //    return Convert.ToBase64String(buffChar);
        //}

        //public static string Base2Key(  string b64)
        //{
        //    return Encoding.Unicode.GetString(Convert.FromBase64String(b64));
        //}

        public static string TxdSHA1(string strSource, int versionStr)
        {
            if (String.IsNullOrEmpty(strSource))
            {
                return "";
            }
            string strResult = "";
            if (versionStr < 2)
            {
                SHA1 lvsha1 = SHA1.Create();

                byte[] bytResult = lvsha1.ComputeHash(System.Text.Encoding.Unicode.GetBytes(strSource));
                lvsha1.Clear();
                //转换成字符串，并取9到25位
                strResult = BitConverter.ToString(bytResult);

                //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉
                strResult = strResult.Replace("-", "");
            }
            if (strResult == "")
            {
                throw new Exception("可能由于版本问题。口令无法传递。");
            }
            return strResult;
        }
    }

    // 对称加密帮助类
    public class CryptoHelper
    {
        // 对称加密算法提供器
        private ICryptoTransform encryptor; // 加密器对象
        private ICryptoTransform decryptor; // 解密器对象
        private const int BufferSize = 1024;

        public CryptoHelper(string algorithmName, string key)
        {
            SymmetricAlgorithm provider = SymmetricAlgorithm.Create(algorithmName);
            byte[] bKeys = new byte[24];
            byte[] sssss = Encoding.Unicode.GetBytes(key);
            for (int lvi = 0; lvi < 24; lvi++)
            {
                if (lvi >= sssss.Length)
                {
                    break;
                }
                bKeys[lvi] = sssss[lvi];
            }
            provider.Key = bKeys;
            provider.IV = new byte[] {0x23, 0x54, 0xC6, 0x68, 0x90, 0xAB, 0xBD, 0xEF};

            encryptor = provider.CreateEncryptor();
            decryptor = provider.CreateDecryptor();
        }

        public CryptoHelper(string key) : this("TripleDES", key)
        {
        }

        // 加密算法
        public string Encrypt(string clearText)
        {
            // 创建明文流
            byte[] clearBuffer = Encoding.Unicode.GetBytes(clearText);
            MemoryStream clearStream = new MemoryStream(clearBuffer);

            // 创建空的密文流
            MemoryStream encryptedStream = new MemoryStream();

            CryptoStream cryptoStream =
                new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write);

            // 将明文流写入到buffer中
            // 将buffer中的数据写入到cryptoStream中
            int bytesRead = 0;
            byte[] buffer = new byte[BufferSize];
            do
            {
                bytesRead = clearStream.Read(buffer, 0, BufferSize);
                cryptoStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            cryptoStream.FlushFinalBlock();

            // 获取加密后的文本
            buffer = encryptedStream.ToArray();
            string encryptedText = Convert.ToBase64String(buffer);
            return encryptedText;
        }

        // 解密算法
        public string Decrypt(string encryptedText)
        {
            byte[] encryptedBuffer = Convert.FromBase64String(encryptedText);
            Stream encryptedStream = new MemoryStream(encryptedBuffer);

            MemoryStream clearStream = new MemoryStream();
            CryptoStream cryptoStream =
                new CryptoStream(encryptedStream, decryptor, CryptoStreamMode.Read);

            int bytesRead = 0;
            byte[] buffer = new byte[BufferSize];

            do
            {
                bytesRead = cryptoStream.Read(buffer, 0, BufferSize);
                clearStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            buffer = clearStream.GetBuffer();
            string clearText =
                Encoding.Unicode.GetString(buffer, 0, (int) clearStream.Length);

            return clearText;
        }

        public static string Encrypt_Static(string clearText, string key)
        {
            CryptoHelper helper = new CryptoHelper(key);
            return helper.Encrypt(clearText);
        }

        public static string Decrypt_Static(string encryptedText, string key)
        {
            CryptoHelper helper = new CryptoHelper(key);
            return helper.Decrypt(encryptedText);
        }
    }
}