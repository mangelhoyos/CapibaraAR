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
    private Vector3 entryPoint;
    private Vector3 exitPoint;

    public bool hasOrder;


    void Start()
    {
        entryPoint = spawnPoint + Vector3.right;
        exitPoint = spawnPoint - Vector3.right;
        transform.position = entryPoint;
        //clientMaterial.color.a = 0.0f;
        client = GetComponent<Client>();

        //Debugin process and test
        //StartCoroutine(FadeRoutine(1.0f, fadeTime, true));
        //StartCoroutine(MoveRoutine(orderPoint));

        Debug.Log("Tiene orden el pana?" + hasOrder);
    }

    [ContextMenu("Move Client to order")]
    public void MoveClientToOrderPoint()
    {
        StartCoroutine(FadeRoutine(1.0f, fadeTime, true));
        StartCoroutine(MoveRoutine(orderPoint));
    }

    [ContextMenu("Move client to exit")]
    public void MoveClientToExitPoint()
    {
        StartCoroutine(MoveRoutine(exitPoint));
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration, bool fadeIn)
    {
        float elapsedTime = 0.0f;
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
                color.a = Mathf.Lerp(startAlpha, 0.0f, lerpValue);
            }

            clientMaterial.color = color;

            yield return null;
        }

        color.a = endAlpha;
        clientMaterial.color = color;
    }

    IEnumerator MoveRoutine(Vector3 targetPoint)
    {
        Debug.Log("Me estoy moviendo rey");

        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        // Movimiento hacia el punto objetivo
        while (Vector3.Distance(transform.position, targetPoint) > 0.05f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / clientSpeed);
            transform.position = Vector3.Lerp(startPosition, targetPoint, t);

            if (targetPoint == exitPoint && transform.position.z >= 10.0f) StartCoroutine(FadeRoutine(0.0f, fadeTime, false));

            yield return null;
        }

        // Logro del destino
        Debug.Log("Llegué bro");

        if (targetPoint == orderPoint) 
        {
            client.GenerateOrder();
            hasOrder = true;
            Debug.Log("Ya ordeno Pepapig" + hasOrder);
        }

        if(targetPoint == exitPoint)
        {
            Destroy(gameObject);
        }

    }

}
