using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface DataProvider
{
    bool Open(string source);

    void Serialize(DataContainer param, object[] args);

    DataContainer Deserialize(object[] args);

    bool Close();
}
