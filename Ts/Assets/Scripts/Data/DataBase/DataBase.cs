using System.Collections;
using System.Collections.Generic;
using System;

public class DataBase<T>
{


    protected Dictionary<int, T> Table = new Dictionary<int, T>();

    public T Get(int index)
    {
        return Table[index];
    }
}

public class DataTable
{
    private Dictionary<string, string> StringTable;
    private Dictionary<string, int> IntTable;
    private Dictionary<string, float> FloatTable;

    public DataTable()
    {
        StringTable = new Dictionary<string, string>();
        IntTable = new Dictionary<string, int>();
        FloatTable = new Dictionary<string, float>();
    }

    internal void AddSrtingValue(string key, string value)
    {
        StringTable.Add(key, value);
    }

    internal string GetSrtingValue(string key)
    {
        if (StringTable[key] != null)
        {
            return StringTable[key];
        }
        else
        {
            return "";
        }
    }

    internal void AddIntValue(string key, int value)
    {
        IntTable.Add(key, value);
    }

    internal int GetIntValue(string key)
    {
        if (IntTable[key] != 0)
        {
            return IntTable[key];
        }
        else
        {
            return 0;
        }
    }

    internal void AddFloatValue(string key, float value)
    {
        FloatTable.Add(key, value);
    }

    internal float GetFloatValue(string key)
    {
        if (FloatTable[key] != 0.0f)
        {
            return FloatTable[key];
        }
        else
        {
            return 0.0f;
        }
    }
}
