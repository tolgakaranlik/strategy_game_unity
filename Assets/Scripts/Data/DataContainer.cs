using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataValue
{
    public enum TypeOfValue { String, Int, DataContainer, List }

    string valueStr;
    int valueInt;
    DataContainer valueDC;
    List<DataValue> valueList;
    TypeOfValue valueType;

    public TypeOfValue ValueType
    {
        get
        {
            return valueType;
        }
    }
    public DataValue()
    {
        valueType = TypeOfValue.Int;

        valueInt = 0;
        valueStr = "";
        valueList = null;
        valueDC = null;
    }

    public DataValue(int value)
    {
        valueType = TypeOfValue.Int;
        valueInt = value;
    }

    public DataValue(string value)
    {
        valueType = TypeOfValue.String;
        valueStr = value;
    }

    public DataValue(DataContainer value)
    {
        valueType = TypeOfValue.DataContainer;
        valueDC = value;
    }
    public DataValue(List<DataValue> value)
    {
        valueType = TypeOfValue.List;
        valueList = value;
    }
    public void Set(string value)
    {
        valueStr = value;
        valueType = TypeOfValue.String;
    }

    public void Set(int value)
    {
        valueInt = value;
        valueType = TypeOfValue.Int;
    }

    public void Set(DataContainer value)
    {
        valueDC = value;
        valueType = TypeOfValue.String;
    }

    public void Set(DataValue value)
    {
        valueDC = value.valueDC;
        valueType = value.valueType;
    }

    public void Set(List<DataValue> value)
    {
        valueList = value;
        valueType = TypeOfValue.List;
    }

    public object Value()
    {
        switch(ValueType)
        {
            case TypeOfValue.Int:
                return valueInt;
            case TypeOfValue.String:
                return valueStr;
            case TypeOfValue.DataContainer:
                return valueDC;
            case TypeOfValue.List:
                return valueList;
        }

        return null;
    }

    public override string ToString()
    {
        string result = "";

        switch (ValueType)
        {
            case DataValue.TypeOfValue.Int:
                result += Value() + ",";
                break;
            case DataValue.TypeOfValue.String:
                result += "\"" + DataContainer.Escape((string)Value()) + "\",";
                break;
            case DataValue.TypeOfValue.DataContainer:
                result += ((DataContainer)Value()).ToString() + ",";
                break;
            case DataValue.TypeOfValue.List:
                result += "[";

                var list = (List<DataValue>)Value();
                for (int i = 0; i < list.Count; i++)
                {
                    result += list[i].ToString();
                }

                if(result != "" && result.EndsWith(","))
                {
                    result = result.Substring(0, result.Length - 1);
                }

                result += "],";
                break;
        }

        return result;
    }
}
public class DataContainer
{
    public Dictionary<string, DataValue> Data;

    public DataContainer()
    {
        Data = new Dictionary<string, DataValue>();
    }

    public object Get(string name)
    {
        name = name.ToLower().Trim();

        if(!Data.ContainsKey(name))
        {
            return null;
        }

        return Data[name];
    }

    public void Add(string name, int value)
    {
        name = name.ToLower().Trim();

        if (!Data.ContainsKey(name))
        {
            Data.Add(name, new DataValue(value));
        } else
        {
            Data[name].Set(value);
        }
    }

    public void Add(string name, string value)
    {
        name = name.ToLower().Trim();

        if (!Data.ContainsKey(name))
        {
            Data.Add(name, new DataValue(value));
        }
        else
        {
            Data[name].Set(value);
        }
    }

    public void Add(string name, DataContainer value)
    {
        name = name.ToLower().Trim();

        if (!Data.ContainsKey(name))
        {
            Data.Add(name, new DataValue(value));
        }
        else
        {
            Data[name].Set(value);
        }
    }

    public void Add(string name, List<DataValue> value)
    {
        name = name.ToLower().Trim();

        if (!Data.ContainsKey(name))
        {
            Data.Add(name, new DataValue(value));
        }
        else
        {
            Data[name].Set(value);
        }
    }

    public void Add(string name, DataValue value)
    {
        name = name.ToLower().Trim();

        if (!Data.ContainsKey(name))
        {
            Data.Add(name, value);
        }
        else
        {
            Data[name].Set(value);
        }
    }

    public override string ToString()
    {
        string result = "";

        foreach (KeyValuePair<string, DataValue> pair in Data)
        {
            result += "\"" + Escape(pair.Key) + "\":" + pair.Value.ToString();
        }

        if(result != "")
        {
            result = result.Substring(0, result.Length - 1);
        }

        return "{" + result + "}";
    }

    public static string Escape(string parameter)
    {
        return parameter.Replace("\"", "\\\"").Replace("/", "\\/");
    }
}
