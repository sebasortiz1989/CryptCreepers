using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera vCam;
    CinemachineBasicMultiChannelPerlin noise;

    // Start is called before the first frame update
    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float duration = 0.1f, float amplitude = 1.5f, float frecuency = 20f)
    {
        StopCoroutine(ApplyNoiseRoutine(duration, amplitude, frecuency));
        StartCoroutine(ApplyNoiseRoutine(duration, amplitude, frecuency));
    }

    IEnumerator ApplyNoiseRoutine(float duration, float amplitude, float frecuency)
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frecuency;
        yield return new WaitForSeconds(duration);
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }

}
