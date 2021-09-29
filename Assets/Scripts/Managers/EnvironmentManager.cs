using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AzureSky;

public class EnvironmentManager : MonoBehaviour
{
    public AudioSource RoosterCrowing;
    public AudioSource OwlHowling;
    public AzureTimeController DayNightController;
    public AzureWeatherProfile[] WeatherProfiles;
    public void RoosterCrow()
    {
        RoosterCrowing.Play();
    }

    public void OwlHowl()
    {
        OwlHowling.Play();
    }

    public void TimeOfDayChange()
    {
        Vector2 timeOfDay = DayNightController.GetTimeOfDay();
        if (timeOfDay.x == 7 && timeOfDay.y == 0)
        {
            RoosterCrow();
        }

        if(timeOfDay.x == 18 && timeOfDay.y == 30)
        {
            OwlHowl();
        }

        if (timeOfDay.x == 14 && timeOfDay.y == 11)
        {
            // Rain for 30 secs
            var weather = DayNightController.GetComponent<AzureWeatherController>();
            weather.SetNewWeatherProfile(WeatherProfiles[1 + Random.Range(0, 4)], 5);

            StartCoroutine(ClearWaether());
        }
    }

    IEnumerator ClearWaether()
    {
        yield return new WaitForSeconds(15);

        var weather = DayNightController.GetComponent<AzureWeatherController>();
        weather.SetNewWeatherProfile(WeatherProfiles[0], 5);
    }
}
