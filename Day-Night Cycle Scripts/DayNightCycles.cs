using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycles : MonoBehaviour
{

    [SerializeField] private Light sun;
    [SerializeField, Range(0, 24)] public float timeOfDay;
    [SerializeField] private float sunRotationSpeed;
    [Header("LightingPreset")]
    [SerializeField] private Gradient skyColor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient sunColor;

    public DayNight dn;

    private bool lightflag = false;

    // Start is called before the first frame update
    void Start()
    {
        dn = FindObjectOfType<DayNight>();
    }

    // Update is called once per frame
    void Update()
    {
        timeOfDay += Time.deltaTime * sunRotationSpeed;
        if (timeOfDay > 24)
            timeOfDay = 0;
        if ((int)timeOfDay == 18)
        {
            if (!lightflag)
            {
                Debug.Log(dn.isNight);
                dn.isNight = !dn.isNight;
                dn.isMoonLight = false; // Reset moonlight when switching between day and night
                dn.ChangeMaterial();
                lightflag = true;
            }
            
        }
        else if((int)timeOfDay == 6) 
        {
            if (lightflag)
            {
                dn.isNight = !dn.isNight;
                dn.isMoonLight = false; // Reset moonlight when switching between day and night
                dn.ChangeMaterial();
                lightflag = false;
            }   
        }

        UpdateSunRotation();
        UpdateLighting();
        
    }

    private void OnValidate()
    {
        UpdateSunRotation();
        UpdateLighting();

    }

    private void UpdateSunRotation()
    {
        float sunRotation = Mathf.Lerp(-90, 270, timeOfDay / 24);
        sun.transform.rotation = Quaternion.Euler(sunRotation, sun.transform.rotation.y, sun.transform.rotation.z);
    }

    private void UpdateLighting()
    {
        float timeFraction = timeOfDay / 24;
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timeFraction);
        RenderSettings.ambientSkyColor = skyColor.Evaluate(timeFraction);
        sun.color = sunColor.Evaluate(timeFraction);
    }
}
