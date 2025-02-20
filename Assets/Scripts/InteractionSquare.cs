using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InteractionSquare : MonoBehaviour
{
    [SerializeField] InteractionType interactionType;
    [SerializeField] GameObject collectible;
    [SerializeField] GameObject dialogCanvas;
    [SerializeField] TMP_Text dialogText;

    public enum InteractionType
    {
        Collar,
        Fridge,
        FrontDoor,
        Human,
        Plant,
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (interactionType == InteractionType.Collar)
            {
                dialogText.text = "Ooh shiny! Yoink!";
                PausePlayerMovement();
                StartCoroutine(CollectCollectible());
            }
            else if (interactionType == InteractionType.Fridge)
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
                if (FindObjectOfType<Collar>().IsWearingCollar())
                {
                    PausePlayerMovement();
                    StartCoroutine(NextDialog());
                }
            }
            else if (interactionType == InteractionType.Plant)
            {
                dialogText.text = "Hello friend!";
            }
            dialogCanvas.SetActive(true);
        }
    }

    IEnumerator CollectCollectible()
    {
        yield return new WaitForSeconds(2f);

        if (collectible != null)
        {
            Destroy(collectible);
            Destroy(gameObject);
        }

        if (interactionType == InteractionType.Collar)
        {
            FindObjectOfType<Collar>().WearCollar();
        }
        ResumePlayerMovement();
    }

    IEnumerator NextDialog()
    {
        yield return new WaitForSeconds(2.5f);
        dialogText.text = "Well, let's go for a walk then!";

        yield return new WaitForSeconds(2.5f);
        ResumePlayerMovement();

        yield return new WaitForSeconds(1f);
        // TODO: Fade to black?
        SceneManager.LoadScene(2);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            dialogCanvas.SetActive(false);
        }
    }

    void PausePlayerMovement()
    {
        FindObjectOfType<PlayerMovement>().pausePlayerMovement = true;
    }

    void ResumePlayerMovement()
    {
        FindObjectOfType<PlayerMovement>().pausePlayerMovement = false;
    }
}
