using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ClientMovement : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private Vector3 orderPoint;
    [SerializeField] private float clientSpeed;


    [Header("Fade effect")]
    [SerializeField] private float fadeTime;
    [SerializeField] private Material clientMaterial;


    private Client client;


    void Start()
    {
        transform.position = spawnPoint;
        client = GetComponent<Client>();


        StartCoroutine(FadeRoutine(1.0f, fadeTime, true));
        StartCoroutine(MoveRoutine());
    }


    void Update()
    {

    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration, bool fadeIn)
    {
        float elapsedTime = 0f;
        Color color = clientMaterial.color;
        float startAlpha = color.a;
        float endAlpha = targetAlpha;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float lerpValue = Mathf.Clamp01(elapsedTime / duration);

            if (fadeIn)
            {
                color.a = Mathf.Lerp(0f, endAlpha, lerpValue);
            }
            else
            {
                color.a = Mathf.Lerp(startAlpha, 0f, lerpValue);
            }

            clientMaterial.color = color;

            yield return null;
        }

        color.a = endAlpha;
        clientMaterial.color = color;
    }

    IEnumerator MoveRoutine()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        // Movimiento hacia la posición objetivo
        while (Vector3.Distance(transform.position, orderPoint) > 0.05f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / clientSpeed);
            transform.position = Vector3.Lerp(startPosition, orderPoint, t);
            yield return null;
        }

        Debug.Log("LLegue bro ");
    }

}
