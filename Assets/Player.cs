using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 position;
    public float speed;
    public bool isMoving;


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
