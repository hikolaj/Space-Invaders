using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public bool DirectionUp = true;
    public float Speed = 0.01f;

    public void Move()
    {
        transform.position += (DirectionUp ? Vector3.up : Vector3.down )* Speed;
    }
}
