using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataProvider : DataProvider
{
    protected string fileName;
    protected bool sourceOpen = false;

    public bool Open(string source)
    {
        sourceOpen = true;
        fileName = Application.persistentDataPath + "/" + source;
        return true;
    }

    // This function should not be used in mobile platforms
    public bool OpenPath(string source)
    {
        sourceOpen = true;
        fileName = source;
        return true;
    }

    public bool Close()
    {
        return true;
    }

    public virtual DataContainer Deserialize()
    {
        if (!sourceOpen)
        {
            throw new Exception("Source is not open");
        }

        DataContainer result = null;
        using(StreamReader sr = new StreamReader(fileName))
        {
            result = Parse(sr.ReadToEnd());
        }

        return result;
    }

    protected DataContainer Parse(string content)
    {
        JObject o = JObject.Parse(content);

        return ParseJObject(o);
    }

    private DataContainer ParseJObject(JObject o)
    {
        DataContainer dc = new DataContainer();
        DataValue dv;
        JToken token;

        foreach (var pair in o)
        {
            dv = new DataValue();
            token = pair.Value;

            dc.Add(pair.Key, ParseToken(token));
        }

        return dc;
    }

    private DataValue ParseToken(JToken token)
    {
        if (token.Type == JTokenType.String)
        {
            return new DataValue(token.Value<string>());
        }
        else if (token.Type == JTokenType.Integer)
        {
            return new DataValue(token.Value<int>());
        }
        else if (token.Type == JTokenType.Array)
        {
            List<DataValue> dv = new List<DataValue>();

            var arrayElements = token.Children();
            foreach (JToken element in arrayElements)
            {
                //element.Type
                dv.Add(ParseToken(element));
            }

            return new DataValue(dv);
        }
        else if (token.Type == JTokenType.Object)
        {
            return new DataValue(ParseJObject(token.Value<JObject>()));
        }

        return null;
    }

    public virtual DataContainer Deserialize(object[] args)
    {
        return Deserialize();
    }

    public virtual void Serialize(DataContainer param)
    {
        if (!sourceOpen)
        {
            throw new Exception("Source is not open");
        }

        using (StreamWriter sw = new StreamWriter(fileName))
        {
            sw.Write(param.ToString());
        }
    }
    public virtual void Serialize(DataContainer param, object[] args)
    {
        Serialize(param);
    }
}
