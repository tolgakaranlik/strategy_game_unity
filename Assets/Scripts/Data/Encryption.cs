using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;

public static class Encryption
{
    public static byte[] Salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

    public static string Encrypt(string clearText, string encryptionKey)
    {
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, Salt);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }

                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }

        //Debug.Log("Set encryption key: " + encryptionKey);
        //Debug.Log("Set chiper text: " + clearText);

        return Convert.ToChar((char)encryptionKey.Length + 65) + encryptionKey + clearText;
    }
    public static string Decrypt(string cipherText)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(cipherText.Substring(0, 1));
        string encryptionKey = cipherText.Substring(1, bytes[0] - 65);
        cipherText = cipherText.Substring(bytes[0] - 64);

        //Debug.Log("Detected encryption key: " + encryptionKey);
        //Debug.Log("Detected chiper text: " + cipherText);

        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, Salt);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }

        return cipherText;
    }

    public static string CreatePassPhrase()
    {
        int length = UnityEngine.Random.Range(11, 19);
        string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
        string result = "";

        for (int i = 0; i < length; i++)
        {
            result += alphabet.Substring(UnityEngine.Random.Range(0, alphabet.Length), 1);
        }

        return result;
    }
}
