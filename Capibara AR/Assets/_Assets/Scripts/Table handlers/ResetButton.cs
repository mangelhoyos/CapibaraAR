using System.Collections;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    [SerializeField] Animator buttonAnim;
    [SerializeField] HamburguerAssemblyHandler assemblyHandler;

    private const string BUTTONPRESSANIMATIONNAME = "ButtonPressAnimation";
    private const float BUTTONAVAILABLETIMER = 2f;

    private bool canBePushed = true;

    public void ButtonPressed()
    {
        if(canBePushed)
            StartCoroutine(ButtonPressCoroutine());
    }

    IEnumerator ButtonPressCoroutine()
    {
        canBePushed = false;

        buttonAnim.Play(BUTTONPRESSANIMATIONNAME);
        assemblyHandler.ResetAssembly();

        yield return new WaitForSeconds(BUTTONAVAILABLETIMER);

        canBePushed = true;
    }
}
