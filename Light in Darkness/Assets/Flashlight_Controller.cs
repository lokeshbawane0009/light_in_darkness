using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class Flashlight_Controller : MonoBehaviour
{
    public Vector3 mouseScreenPosition;
    public Quaternion quaternion;
    bool moving = false;
    public Light2D flashlight;
    public PolygonCollider2D FlashLightCollider;
    public float speed = 10f;
    public bool isFlashlightAvail = false;
    public Vector2 left_x, right_x;
    public GameObject tx;
    public Camera cc;
    public Scrollbar flashlight_bar; 

    public GameObject crosshair;

    float df_inner_angle, df_outer_angle, df_falloff_intensity, df_intensity;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.transform.GetComponent<Enemy_Spider>().Damagable(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.transform.GetComponent<Enemy_Spider>().Damagable(false);
        }
    }
    int layerMask;
    // Start is called before the first frame update
    private void Start()
    {
        flashlight_bar.size = 1;
        df_inner_angle = flashlight.pointLightInnerAngle;
        df_outer_angle = flashlight.pointLightOuterAngle;
        df_falloff_intensity = flashlight.falloffIntensity;
        df_intensity = flashlight.intensity;
        layerMask = LayerMask.GetMask("Enemy","Default");
    }

    Vector3 direction;
    // Update is called once per frame
    void LateUpdate()
    {
        
        //mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = ((Vector2)crosshair.transform.position - (Vector2)transform.position).normalized;
        //Debug.Log(direction);

        if (moving)
        {
            transform.up = direction;
            quaternion = transform.rotation;
            //Debug.Log(quaternion);
        }
        else
        {
            transform.up = direction;
        }
    }

    public void Update()
    {
        FocusLight();
        Crosshair_Controller();
    }
    public RaycastHit2D hit;
    public Transform hittrans;

    public Vector2 C_mouseScreenPosition;

    private void FocusLight()
    {
        left_x = (Vector2)FlashLightCollider.points.GetValue(0);
        right_x = (Vector2)FlashLightCollider.points.GetValue(1);

        if (!isFlashlightAvail)
        {
            if (flashlight_bar.size > 0.5f)
                isFlashlightAvail = true;
        }
        else
        {
            if (flashlight_bar.size <= 0)
                isFlashlightAvail = false;
        }

        if (Input.GetButton("Fire2")&& isFlashlightAvail && flashlight_bar.size >= 0)
        {
            flashlight_bar.size -= 0.2f * Time.deltaTime;
            flashlight.pointLightInnerAngle = Mathf.Lerp(flashlight.pointLightInnerAngle, 2f, Time.deltaTime * speed);
            flashlight.pointLightOuterAngle = Mathf.Lerp(flashlight.pointLightOuterAngle, 5f, Time.deltaTime * speed);
            //flashlight.falloffIntensity = Mathf.Lerp(flashlight.falloffIntensity, 0f, Time.fixedDeltaTime * speed);
            flashlight.intensity = Mathf.Lerp(flashlight.intensity, 20f, Time.deltaTime * speed);
            FlashLightCollider.points = new [] { new Vector2(Mathf.Lerp(left_x.x, 1f, Time.deltaTime * speed),23.2f), new Vector2(Mathf.Lerp(right_x.x, -1f, Time.deltaTime * speed), 23.2f), new Vector2(0,0) };
            hit= Physics2D.Raycast(transform.position, transform.up, 22f,layerMask);
            if (hit)
            {
                hittrans = hit.transform;
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<Enemy_Spider>().Damage(flashlight.intensity,tx,hit.point);
                }
            }
            else
            {
                hittrans = null;
            }
            
            Debug.DrawRay(transform.position, transform.up * 20f,Color.green);
        }
        else
        {
            flashlight_bar.size += 0.3f * Time.deltaTime;
            flashlight.pointLightInnerAngle = Mathf.Lerp(flashlight.pointLightInnerAngle, df_inner_angle, Time.deltaTime * speed);
            flashlight.pointLightOuterAngle = Mathf.Lerp(flashlight.pointLightOuterAngle, df_outer_angle, Time.deltaTime * speed);
            //flashlight.falloffIntensity = Mathf.Lerp(flashlight.falloffIntensity, df_falloff_intensity, Time.fixedDeltaTime * speed);
            flashlight.intensity = Mathf.Lerp(flashlight.intensity, df_intensity, Time.deltaTime * speed);
            FlashLightCollider.points = new[] { new Vector2(Mathf.Lerp(left_x.x, 4f, Time.deltaTime * speed), 23.2f), new Vector2(Mathf.Lerp(right_x.x, -4f, Time.deltaTime * speed), 23.2f), new Vector2(0, 0) };
        }
    }

    public void Setmoving(bool value)
    {
        moving = value;
    }

    float distance;
    Vector2 v = Vector2.one;

    [Range(0,1)]
    public float sensitivity;
    void Crosshair_Controller()
    {
        C_mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(transform.position, C_mouseScreenPosition);
        crosshair.transform.position = Vector2.SmoothDamp(crosshair.transform.position, C_mouseScreenPosition,ref v,sensitivity);
        if (distance < 5f)
        {
            crosshair.transform.position = ((crosshair.transform.position - transform.position).normalized * 5f) + transform.position;
            Debug.Log(crosshair.transform.position);
        }
 
        /*float x = crosshair.transform.localPosition.x;
        x = Mathf.Clamp(crosshair.transform.localPosition.x, 5f, 22f);
        crosshair.transform.localPosition = new Vector3(x, crosshair.transform.localPosition.y, 0);*/
    }
}
