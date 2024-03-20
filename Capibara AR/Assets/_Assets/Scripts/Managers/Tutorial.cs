using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [Header("Tutorial setup")]
    [SerializeField] List<GameObject> tutorialItems; //tutorial containers
    public UnityEvent OnTutorialStepsCompleted;

    private int actualIndex = 0;

    public void NextStep()
    {
        actualIndex++;
        if(tutorialItems.Count <= actualIndex)
        {
            OnTutorialStepsCompleted?.Invoke();
            return;
        }
        tutorialItems[actualIndex].SetActive(true);
    }

    public void StartTutorial()
    {
        foreach (GameObject tutorial in tutorialItems)
        {
            tutorial.SetActive(false);
        }

        tutorialItems[0].SetActive(true);
    }
}
