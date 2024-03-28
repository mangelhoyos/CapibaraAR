using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderMessage : MonoBehaviour
{
    [SerializeField] private Image timer;
    [SerializeField] private Transform ingredientsPosition;
    [SerializeField] private List<Image> imagesList = new List<Image>();
    [SerializeField] private CanvasGroup orderMessage;
    //[SerializeField] private Canvas canvasOrderMessage;
    
    private List<Image> ingredientsToShow = new List<Image>();

    private void Start()
    {
        //canvasOrderMessage.worldCamera = 
    }

    public void UpdateOrderTimer(float timerCount)
    {
        timer.fillAmount = timerCount;
    }

    public void ShowMessage(Hamburguer hamburguer)
    {
        ShowIngredients(hamburguer);
        orderMessage.alpha = 1.0f;
    }

    private void ShowIngredients(Hamburguer hamburguer)
    {
        foreach(Ingredient ingredient in hamburguer.ingredientList)
        {
            Image desiredIngredient = Instantiate(imagesList[(int)ingredient.ingredientType], ingredientsPosition);
            ingredientsToShow.Add(desiredIngredient);
        }
    } 

    public void HideMessage()
    {
        orderMessage.alpha = 0.0f;
        ingredientsToShow.Clear();
    }
}
