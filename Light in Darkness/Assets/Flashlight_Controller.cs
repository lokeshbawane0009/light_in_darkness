using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight_Controller : MonoBehaviour
{
    Vector3 mouseScreenPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = ((Vector2)mouseScreenPosition - (Vector2)transform.position).normalized;
        transform.up = direction;
    }
}
