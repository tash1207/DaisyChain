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

    bool hasInteracted = false;

    int outsideHouseSceneIndex = 2;
    int neighborYardSceneIndex = 3;
    int constructionSceneIndex = 4;
    int beachSceneIndex = 5;

    public enum InteractionType
    {
        BinCompost,
        BinTrash,
        Collar,
        Fridge,
        FrontDoor,
        Human,
        Plant,
        OutsideHouseLeft,
        OutsideHouseDown,
        OutsideHouseUp,
        NeighborRight,
        NeighborDown,
        Neighbor,
        ConstructionTopLeft,
        ConstructionTopRight,
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasInteracted) { return; }
        if (collision.tag == "Player")
        {
            if (interactionType == InteractionType.BinCompost)
            {
                dialogText.text = "Yum!";
                FindObjectOfType<Health>().IncreaseSoil(20);
                StartCoroutine(OneTimeUse());
            }
            else if (interactionType == InteractionType.BinTrash)
            {
                dialogText.text = "Ew gross!";
                FindObjectOfType<Health>().DecreaseSoil(20);
                StartCoroutine(OneTimeUse());
            }
            else if (interactionType == InteractionType.Collar)
            {
                dialogText.text = "Ooh shiny! Yoink!";
                PausePlayerMovement();
                StartCoroutine(OneTimeUse());
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
            else if (interactionType == InteractionType.Neighbor)
            {
                dialogText.text = "Hi there! Would you like some water?";
                PausePlayerMovement();
                StartCoroutine(NextDialog());
            }

            dialogCanvas.SetActive(true);

            if (interactionType == InteractionType.OutsideHouseLeft)
            {
                dialogCanvas.SetActive(false);
                SceneManager.LoadScene(neighborYardSceneIndex);
            }
            else if (interactionType == InteractionType.OutsideHouseDown)
            {
                dialogCanvas.SetActive(false);
                SceneManager.LoadScene(constructionSceneIndex);
            }
            else if (interactionType == InteractionType.OutsideHouseUp)
            {
                dialogCanvas.SetActive(false);
                Debug.Log("Sorry, not yet");
                // TODO: Load beach scene.
                // SceneManager.LoadScene(beachSceneIndex);
            }
            else if (interactionType == InteractionType.NeighborRight)
            {
                dialogCanvas.SetActive(false);
                SceneManager.LoadScene(outsideHouseSceneIndex);
            }
            else if (interactionType == InteractionType.NeighborDown)
            {
                dialogCanvas.SetActive(false);
                SceneManager.LoadScene(constructionSceneIndex);
            }
            else if (interactionType == InteractionType.ConstructionTopLeft)
            {
                dialogCanvas.SetActive(false);
                SceneManager.LoadScene(neighborYardSceneIndex);
            }
            else if (interactionType == InteractionType.ConstructionTopRight)
            {
                dialogCanvas.SetActive(false);
                SceneManager.LoadScene(outsideHouseSceneIndex);
            }
        }
    }

    IEnumerator OneTimeUse()
    {
        hasInteracted = true;
        yield return new WaitForSeconds(2f);

        if (collectible != null)
        {
            Destroy(collectible);
        }

        Destroy(gameObject);

        if (interactionType == InteractionType.Collar)
        {
            FindObjectOfType<Collar>().WearCollar();
        }
        ResumePlayerMovement();
    }

    IEnumerator NextDialog()
    {
        yield return new WaitForSeconds(2.5f);
        if (interactionType == InteractionType.Human)
        {
            dialogText.text = "Well, I guess we can go for a walk.";
            yield return new WaitForSeconds(2.5f);

            ResumePlayerMovement();
            yield return new WaitForSeconds(1f);
            // TODO: Fade to black?
            SceneManager.LoadScene(2);
        }
        else if (interactionType == InteractionType.Neighbor)
        {
            dialogText.text = "Here you go!";
            FindObjectOfType<Health>().MaxWater();
            yield return new WaitForSeconds(2.5f);

            dialogText.text = "It's good to see you outside, Casey! We miss you at book club!";
            yield return new WaitForSeconds(2.5f);

            ResumePlayerMovement();
            yield return new WaitForSeconds(1f);

            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (dialogCanvas != null)
            {
                dialogCanvas.SetActive(false);
            }
        }
    }

    void PausePlayerMovement()
    {
        FindObjectOfType<PlayerMovement>().PausePlayerMovement();
    }

    void ResumePlayerMovement()
    {
        FindObjectOfType<PlayerMovement>().pausePlayerMovement = false;
    }
}
