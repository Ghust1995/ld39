using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 position;
    public float speed;
    public float walkingSpeed;
    public bool isMoving;


    public void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKey(KeyCode.W))
            {
                position += Vector2.up * walkingSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                position += Vector2.left * walkingSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                position += Vector2.down * walkingSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                position += Vector2.right * walkingSpeed * Time.deltaTime;
            }
        }
    }

    public void MoveTowards(Vector2 target, Action callback)
    {
        if (isMoving) return;
        StartCoroutine(MoveToTarget(target, callback));
    }

    public void MoveTowards(Vector2 target)
    {
        if (isMoving) return;
        StartCoroutine(MoveToTarget(target, () => { }));
    }

    public IEnumerator MoveToTarget(Vector2 target, Action callback)
    {
        isMoving = true;
        var direction = (target - position).normalized;
        while ((target - position).normalized - direction == Vector2.zero)
        {
            position += direction * speed * Time.deltaTime;
            yield return null;
        }
        isMoving = false;
        callback();
    }
}
