using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionSquare : MonoBehaviour
{
    [SerializeField] InteractionType interactionType;
    [SerializeField] GameObject dialogCanvas;
    [SerializeField] TMP_Text dialogText;

    public enum InteractionType
    {
        Fridge,
        FrontDoor,
        Human,
        Plant,
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (interactionType == InteractionType.Fridge)
            {
                dialogText.text = "Yuck! That is definitely not what plants crave.";
            }
            else if (interactionType == InteractionType.FrontDoor)
            {
                dialogText.text = "Let me out of here!";
            }
            else if (interactionType == InteractionType.Human)
            {
                dialogText.text = "Aah! You can walk?";
            }
            else if (interactionType == InteractionType.Plant)
            {
                dialogText.text = "Hello friend!";
            }
            dialogCanvas.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            dialogCanvas.SetActive(false);
        }
    }
}
