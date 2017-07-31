using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pokeball : MonoBehaviour
{
    public Image image;
    public Vector3 initialPosition;
    public Vector3 initialScale;
    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = Ball;
        initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        initialScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    public float ToPokeTime;
    public float HitToEnergyTime;
    public float UpAmmount;
    public float EnergyTime;
    public float FallTime;
    public float RotatingTime;
    public float FailureSprite;
    public Sprite Energy;
    public Sprite Success;
    public Sprite Failure;
    public Sprite Ball;
    public IEnumerator PokeballAnimationCatch(float targetY)
    {
        {
            var totalTime = 0.0f;
            var initY = transform.position.y;
            var initX = transform.position.x;
            while (totalTime < ToPokeTime)
            {
                var frac = totalTime / ToPokeTime;
                transform.position = new Vector2(initX + Mathf.Sin(frac * frac * Mathf.PI), Mathf.Lerp(initY, targetY, frac * frac));
                transform.localScale = Vector3.one * Mathf.Lerp(1.0f, 0.5f, frac * frac);
                totalTime += Time.deltaTime;
                yield return null;
            }
        }

        {
            var totalTime = 0.0f;
            var initY = transform.position.y;
            var initX = transform.position.x;
            while (totalTime < HitToEnergyTime)
            {
                var frac = totalTime / HitToEnergyTime;
                transform.position = new Vector2(initX, Mathf.Lerp(initY, initY + UpAmmount, Mathf.Sqrt(frac)));
                totalTime += Time.deltaTime;
                yield return null;
            }
        }

        
    }

    public IEnumerator PokeballAnimationWobble()
    {
        {
            var totalTime = 0.0f;
            var initY = transform.position.y;
            var initX = transform.position.x;
            image.sprite = Energy;
            while (totalTime < EnergyTime)
            {
                totalTime += Time.deltaTime;
                yield return null;
            }

        }
        {
            var totalTime = 0.0f;
            var initY = transform.position.y;
            var initX = transform.position.x;
            image.sprite = Ball;
            while (totalTime < FallTime)
            {
                var frac = totalTime / FallTime;
                transform.position = new Vector2(initX, Mathf.Lerp(initY, -1, frac*frac));
                totalTime += Time.deltaTime;
                yield return null;
            }
        }
{
            var totalTime = 0.0f;
            var initY = transform.position.y;
            var initX = transform.position.x;
            image.sprite = Ball;
            while (totalTime < RotatingTime)
            {
                var frac = totalTime / RotatingTime;
                transform.position = new Vector3(initX + 0.1f*Mathf.Sin(6 * frac * Mathf.PI), initY, 0.0f);
                totalTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
        transform.localScale = initialScale;
    }

}
