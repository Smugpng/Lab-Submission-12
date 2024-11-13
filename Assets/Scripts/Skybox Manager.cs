using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public Material Morning, Noon, Evening, Night;

    public void Start()
    {
        RenderSettings.skybox = Night;
    }
}
