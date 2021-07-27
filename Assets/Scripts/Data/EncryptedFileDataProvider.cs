using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EncryptedFileDataProvider : FileDataProvider
{
    protected const int keysize = 256;
    protected const int derivationIterations = 1000;
    public override DataContainer Deserialize()
    {
        if (!sourceOpen)
        {
            throw new Exception("Source is not open");
        }

        // read regularly
        DataContainer result = null;
        string data;
        using(StreamReader sr = new StreamReader(fileName))
        {
            data = sr.ReadToEnd();
        }

        // apply decryption here
        result = Parse(Encryption.Decrypt(data));

        return result;
    }

    public override DataContainer Deserialize(object[] args)
    {
        return Deserialize();
    }

    public override void Serialize(DataContainer param)
    {
        if (!sourceOpen)
        {
            throw new Exception("Source is not open");
        }

        using (StreamWriter sw = new StreamWriter(fileName))
        {
            sw.Write(Encryption.Encrypt(param.ToString(), Encryption.CreatePassPhrase()));
        }
    }
    public override void Serialize(DataContainer param, object[] args)
    {
        Serialize(param);
    }
}
