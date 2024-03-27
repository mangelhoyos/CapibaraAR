using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBell : MonoBehaviour
{
    [SerializeField] private HamburguerAssemblyHandler assemblyHandler;
    private const float TABLEBELLTIMER = 2f;
    private bool onCooldown = false;

    public void TouchBell()
    {
        if (!onCooldown)
            StartCoroutine(TouchBellCoroutine());
    }

    IEnumerator TouchBellCoroutine()
    {
        onCooldown = true;
        AudioManager.instance.Play("Bell");
        assemblyHandler.DeliverHamburguer();

        yield return new WaitForSeconds(TABLEBELLTIMER);

        onCooldown = false;
    }
}
