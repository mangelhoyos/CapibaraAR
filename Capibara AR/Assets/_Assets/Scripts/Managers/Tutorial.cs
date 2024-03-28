using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [Header("Tutorial setup")]
    [SerializeField] private CanvasGroup introScreen;
    public UnityEvent OnTutorialStepsCompleted;

    [SerializeField] private GameObject placementTutorial;
    [SerializeField] private GameObject placementCanvas;
    [SerializeField] private GameObject planeFinderController;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject gameTutorial;
    [SerializeField] private GameObject gameUI;

    private GameObject placedObject;

    IEnumerator FadeOutIntroScreen()
    {
        yield return new WaitForSeconds(3);

        float TRANSITIONTIME = 2f;
        float elapsedTime = 0;

        while (elapsedTime < TRANSITIONTIME)
        {
            introScreen.alpha = Mathf.Lerp(1, 0, elapsedTime / TRANSITIONTIME);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        introScreen.gameObject.SetActive(false);
    }

    public void StartPlacement()
    {
        AudioManager.instance.Play("Button");
        placementTutorial.SetActive(false);
        placementCanvas.SetActive(true);
        planeFinderController.SetActive(true);
    }

    public void ShowPlacementHint()
    {
        AudioManager.instance.Play("Button");
        placementTutorial.SetActive(true);
        planeFinderController.SetActive(false);
    }

    public void StartTutorial()
    {
        StartCoroutine(FadeOutIntroScreen());
    }

    public void PlaceCapibaraFood(GameObject placedObject)
    {
        AudioManager.instance.Play("SetTable");
        planeFinderController.SetActive(false);
        confirmPanel.SetActive(true);
        this.placedObject = placedObject;
    }

    public void SetConfirmPlaceStatus(bool confirmed)
    {
        AudioManager.instance.Play("Button");
        if (confirmed)
        {
            confirmPanel.SetActive(false);
            placementCanvas.SetActive(false);
            gameTutorial.SetActive(true);
            AudioManager.instance.SetWithFade("Transition", 1f, true);
            AudioManager.instance.SetWithFade("Planning", 1f, false);
        }
        else
        {
            Destroy(placedObject);
            placedObject = null;
            planeFinderController.SetActive(true);
        }
    }

    public void StartGame()
    {
        gameTutorial.SetActive(false);
        gameUI.SetActive(true);
        GameManager.Instance.StartGame();
        OnTutorialStepsCompleted?.Invoke();
        AudioManager.instance.SetWithFade("Transition", 1f, false);
        AudioManager.instance.SetWithFade("Soundtrack", 2f, true);
        AudioManager.instance.Play("InitService");
    }
}
