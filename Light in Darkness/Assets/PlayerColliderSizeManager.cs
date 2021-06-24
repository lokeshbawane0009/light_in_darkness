using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderSizeManager : MonoBehaviour
{
    public CapsuleCollider2D collider2d;
    public PlayerController PC;

    void MinimizeCollieratRoll()
    {
        collider2d.offset = new Vector2(-0.1323509f, -1.31f);
        collider2d.size = new Vector3(1.369743f, 1f);
    }

    void ResetCollider()
    {
        collider2d.offset = new Vector2(-0.1323509f, -0.1323519f);
        collider2d.size = new Vector3(1.369743f, 3.684847f);
    }

    void HeadCheck()
    {
        PC.HeadClearanceCheck();
    }

    void ChangePositionOfCollidertoNew()
    {
        Debug.Log("Called");
        PC.ledgeGrabbed = false;
        PC.rb.gravityScale = 1.5f;
        transform.parent.position = transform.position;
    }
}
