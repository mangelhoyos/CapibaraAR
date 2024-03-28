using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMovement : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private Animator animator;
    [SerializeField] private float clientSpeed;
    [SerializeField] private float clientRotationSpeed;


    [Header("Fade effect")]
    [SerializeField] private float fadeTime;
    [SerializeField] private Material[] clientMaterials;
    [SerializeField] private List<Renderer> clientMeshRenderers = new List<Renderer>();

    private Client client;
    private Vector3 exitPoint;
    private Vector3 spawnPoint;
    private Vector3 orderPoint;

    private Transform tablePoint;

    void Start()
    {
        tablePoint = GameManager.Instance.tablePosition;
        orderPoint = tablePoint.position + -tablePoint.forward * 1.0f;

        spawnPoint = orderPoint + -tablePoint.right * 15.0f;
        exitPoint = orderPoint + tablePoint.right * 15.0f;

        transform.position = spawnPoint;

        client = GetComponent<Client>();

        MoveClientToOrderPoint();
    }

    public void MoveClientToOrderPoint()
    {
        StartCoroutine(FadeRoutine(1.0f, fadeTime, true));
        StartCoroutine(MoveRoutine(orderPoint));
    }

    public void MoveClientToExitPoint()
    {
        StartCoroutine(MoveRoutine(exitPoint));
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration, bool fadeIn)
    {

        foreach(SkinnedMeshRenderer meshRenderer in clientMeshRenderers)
        {
            meshRenderer.material = clientMaterials[0];
        }

        float elapsedTime = 0.0f;
        Color color = clientMaterials[0].color;
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

            clientMaterials[0].color = color;

            yield return null;
        }

        color.a = endAlpha;
        clientMaterials[0].color = color;

        foreach (SkinnedMeshRenderer meshRenderer in clientMeshRenderers)
        {
            meshRenderer.material = clientMaterials[1];
        }
    }

    IEnumerator MoveRoutine(Vector3 targetPoint)
    {
        Debug.Log("Me estoy moviendo rey");
        StartCoroutine(RotateClient(targetPoint));
        animator.SetBool("isWalking", true);

        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (Vector3.Distance(transform.position, targetPoint) > 0.05f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / clientSpeed);
            transform.position = Vector3.Lerp(startPosition, targetPoint, t);

            if (targetPoint == exitPoint && transform.position.x <= -10.0f) StartCoroutine(FadeRoutine(0.0f, fadeTime, false));

            yield return null;
        }

        animator.SetBool("isWalking", false);

        if (targetPoint == orderPoint) 
        {
            transform.rotation = tablePoint.rotation;
            client.GenerateOrder();
        }

        if(targetPoint == exitPoint)
        {
            Destroy(gameObject);
        }

    }

    private IEnumerator RotateClient(Vector3 rotateDirection)
    {

        Debug.Log("El rotaciao: " + rotateDirection);
        Debug.Log(" Este es el table point: " + tablePoint);

        Quaternion targetRotation = Quaternion.LookRotation(rotateDirection - transform.position, tablePoint.up);
        float angle = Quaternion.Angle(transform.rotation, targetRotation);
        while (angle > 0.1f)
        {
            float step = clientRotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
     
            angle = Quaternion.Angle(transform.rotation, targetRotation);

            yield return null;
        }
    }

}
