using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AzureSky;

public class WeatherChanger : MonoBehaviour
{
    public AzureTimeController DayNightController;
    public AzureWeatherProfile ProfileOnEnter;
    public AzureWeatherProfile ProfileOnExit;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var weather = DayNightController.GetComponent<AzureWeatherController>();
            weather.SetNewWeatherProfile(ProfileOnEnter, 5);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            var weather = DayNightController.GetComponent<AzureWeatherController>();
            weather.SetNewWeatherProfile(ProfileOnExit, 5);
        }
    }
}
