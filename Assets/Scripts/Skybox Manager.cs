using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public Material Morning, Noon, Evening, Night;

    public void MakeMorning()
    {
        RenderSettings.skybox = Morning;
    }

    public void MakeNoon()
    {
        RenderSettings.skybox = Noon;
    }

    public void MakeEvening()
    {
        RenderSettings.skybox = Evening;
    }

    public void MakeNight()
    {
        RenderSettings.skybox = Night;
    }
}
