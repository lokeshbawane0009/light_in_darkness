using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Cinemachine_Camera_Shake : MonoBehaviour
{

    public static Cinemachine_Camera_Shake Instance;
    CinemachineVirtualCamera camera;
    float shaketime, totalshaketime;

    void Awake()
    {
        Instance = this;
        camera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ScreenShake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannel.m_AmplitudeGain = intensity;
        totalshaketime= shaketime = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (shaketime > 0f)
        {
            shaketime -= Time.deltaTime;
            if (shaketime <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannel.m_AmplitudeGain = Mathf.Lerp(cinemachineBasicMultiChannel.m_AmplitudeGain, 0, 1 - (shaketime / totalshaketime));
            }
        }
    }
}
