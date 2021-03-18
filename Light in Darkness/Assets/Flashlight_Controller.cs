using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flashlight_Controller : MonoBehaviour
{
    public Vector3 mouseScreenPosition;
    public Quaternion quaternion;
    bool moving = false;
    public Light2D flashlight;
    public float speed = 5f;

    float df_inner_angle, df_outer_angle, df_falloff_intensity, df_intensity;

    // Start is called before the first frame update
    private void Start()
    {
        df_inner_angle = flashlight.pointLightInnerAngle;
        df_outer_angle = flashlight.pointLightOuterAngle;
        df_falloff_intensity = flashlight.falloffIntensity;
        df_intensity = flashlight.intensity;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        FocusLight();

        mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = ((Vector2)mouseScreenPosition - (Vector2)transform.position).normalized;
        //Debug.Log(direction);

        if (moving)
        {
            transform.up = direction;
            quaternion = transform.rotation;
            Debug.Log(quaternion);
        }
        else
        {
            transform.up = direction;
        }
    }

    private void FocusLight()
    {
        if (Input.GetButton("Fire2"))
        {
            flashlight.pointLightInnerAngle = Mathf.Lerp(flashlight.pointLightInnerAngle, 2f, Time.fixedDeltaTime * speed);
            flashlight.pointLightOuterAngle = Mathf.Lerp(flashlight.pointLightOuterAngle, 5f, Time.fixedDeltaTime * speed);
            //flashlight.falloffIntensity = Mathf.Lerp(flashlight.falloffIntensity, 0f, Time.fixedDeltaTime * speed);
            flashlight.intensity = Mathf.Lerp(flashlight.intensity, 5, Time.fixedDeltaTime * speed);
        }
        else
        {
            flashlight.pointLightInnerAngle = Mathf.Lerp(flashlight.pointLightInnerAngle, df_inner_angle, Time.fixedDeltaTime * speed);
            flashlight.pointLightOuterAngle = Mathf.Lerp(flashlight.pointLightOuterAngle, df_outer_angle, Time.fixedDeltaTime * speed);
            //flashlight.falloffIntensity = Mathf.Lerp(flashlight.falloffIntensity, df_falloff_intensity, Time.fixedDeltaTime * speed);
            flashlight.intensity = Mathf.Lerp(flashlight.intensity, df_intensity, Time.fixedDeltaTime * speed);
        }
    }

    public void Setmoving(bool value)
    {
        moving = value;
    }
}
