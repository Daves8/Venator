using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    //References
    [SerializeField] private Light _directionalLight;
    [SerializeField] private LightingPreset _preset;    
    [SerializeField] private float _timeSpeed;
    [SerializeField] private GameObject _clouds;
    [SerializeField] private GameObject _stars;

    //Variables
    [SerializeField, Range(0, 24)] public float _TimeOfDay;

    private void Update()
    {
        if (_preset == null)
            return;
        if (Application.isPlaying)
        {
            _TimeOfDay += Time.deltaTime * _timeSpeed;
            _TimeOfDay %= 24; // 0 - 24
            UpdateLighting(_TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(_TimeOfDay / 24f);
        }
        if (_TimeOfDay < 6 || _TimeOfDay > 19)
        {
            _clouds.SetActive(false);
            _stars.SetActive(true);
        }else
        {
            _clouds.SetActive(true);
            _stars.SetActive(false);
        }
    }
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = _preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = _preset.fogColor.Evaluate(timePercent);

        if (_directionalLight != null)
        {
            _directionalLight.color = _preset.directionalColor.Evaluate(timePercent);
            _directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, -170, 0));
        }
    }
    private void OnValidate()
    {
        if (_directionalLight != null) return;
        if (RenderSettings.sun != null)
        {
            _directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    _directionalLight = light;
                    return;

                }
            }
        }
    }
}
