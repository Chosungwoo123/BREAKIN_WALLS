using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public void ShakeStart(float duration, float magnitube)
    {
        StartCoroutine(Shake(duration, magnitube));
    }
    
    private IEnumerator Shake(float duration, float magnitube)
    {
        Vector3 originalPos = new Vector3(0, 0, -10);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitube;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitube;

            transform.position = new Vector3(xOffset, yOffset, originalPos.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
    }
}