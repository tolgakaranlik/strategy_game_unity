using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataTest : MonoBehaviour
{
    public InputField Input;

    DataContainer container1 = new DataContainer();
    DataContainer container2 = new DataContainer();
    DataContainer container3 = new DataContainer();
    DataContainer container4 = new DataContainer();
    DataContainer container5 = new DataContainer();

    // Start is called before the first frame update
    void Start()
    {
        container1.Add("name", "tolga");
        container1.Add("age", 41);
        container1.Add("last_name", "karanlikoglu");

        container2.Add("name", "rengin");
        container2.Add("age", 11);
        container2.Add("last_name", "karanlikoglu");

        container3.Add("type", "person");
        container3.Add("details", container1);

        var people = new List<DataValue>();
        people.Add(new DataValue(container1));
        people.Add(new DataValue(container2));

        container4.Add("type", "people");
        container4.Add("details", people);

        var ages = new List<DataValue>();
        ages.Add(new DataValue(1));
        ages.Add(new DataValue(2));
        ages.Add(new DataValue(container4));

        container5.Add("type", "container");
        container5.Add("ages", ages);
    }

    public void Test1()
    {
        Input.text = container1.ToString();
    }

    public void Test2()
    {
        Input.text = container3.ToString();
    }

    public void Test3()
    {
        Input.text = container4.ToString();
    }

    public void Test4()
    {
        Input.text = container5.ToString();
    }

    public void WriteToFile()
    {
        FileDataProvider fdp = new FileDataProvider();

        fdp.OpenPath("c:\\temp\\test.txt");
        fdp.Serialize(container5);

        Debug.Log("Saved to file");
    }

    public void ReadFromFile()
    {
        FileDataProvider fdp = new FileDataProvider();

        fdp.OpenPath("c:\\temp\\test.txt");
        DataContainer dc = fdp.Deserialize();

        Input.text = dc.ToString();
    }

    public void WriteToEncryptedFile()
    {
        EncryptedFileDataProvider fdp = new EncryptedFileDataProvider();

        fdp.OpenPath("c:\\temp\\test_encrypted.txt");
        fdp.Serialize(container5);

        Debug.Log("Saved to file");
    }

    public void ReadFromEncryptedFile()
    {
        EncryptedFileDataProvider fdp = new EncryptedFileDataProvider();

        fdp.OpenPath("c:\\temp\\test_encrypted.txt");
        DataContainer dc = fdp.Deserialize();

        Input.text = dc.ToString();
    }
}
