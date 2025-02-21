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
        Worker,
        ConstructionWater,
        MissingKey,
        BeachDown,
        Sunbathe,
    }

    void Start()
    {
        // Hacky way to make sure the canonical game logic is checked.
        foreach (GameLogic gameLogic in FindObjectsOfType<GameLogic>())
        {
            if (gameLogic.foundWaterValveKey)
            {
                if (interactionType == InteractionType.MissingKey)
                {
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasInteracted) { return; }
        if (collision.tag == "Player")
        {
            if (interactionType == InteractionType.BinCompost)
            {
                dialogText.text = "Compost is yummy!";
                FindObjectOfType<Health>().IncreaseSoil(20);
                PausePlayerMovement();
                StartCoroutine(OneTimeUse());
            }
            else if (interactionType == InteractionType.BinTrash)
            {
                dialogText.text = "Trash is gross!";
                FindObjectOfType<Health>().DecreaseSoil(20);
                PausePlayerMovement();
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
                if (!FindObjectOfType<GameLogic>().waterTurnedOn)
                {
                    dialogText.text =
                    "I'm trying to water my garden but the hose doesn't seem to be working.";
                }
                else
                {
                    dialogText.text = "I'm so happy that my hose is working again. Would you like some water?";
                    PausePlayerMovement();
                    StartCoroutine(NextDialog());
                }
            }
            else if (interactionType == InteractionType.Worker)
            {
                if (!FindObjectOfType<GameLogic>().foundWaterValveKey)
                {
                    dialogText.text = "Sorry Casey, I can't talk right now. I'm in big trouble.";
                    PausePlayerMovement();
                    StartCoroutine(NextDialog());
                }
                else if (!FindObjectOfType<GameLogic>().waterTurnedOn)
                {
                    dialogText.text = "You found the key to the water shut off valve! Now I can turn the water back on!";
                    PausePlayerMovement();
                    StartCoroutine(NextDialog());
                }
                else
                {
                    dialogText.text = "You're my hero! Would you like some soil?";
                    PausePlayerMovement();
                    StartCoroutine(NextDialog());
                }
            }
            else if (interactionType == InteractionType.ConstructionWater)
            {
                dialogText.text = "*spit take* :(";
                FindObjectOfType<Health>().DecreaseWater(20);
                PausePlayerMovement();
                StartCoroutine(OneTimeUse());
            }
            else if (interactionType == InteractionType.MissingKey)
            {
                dialogText.text = "Ooh a key! This looks useful!";
                FindObjectOfType<GameLogic>().foundWaterValveKey = true;
                PausePlayerMovement();
                StartCoroutine(OneTimeUse());
            }
            else if (interactionType == InteractionType.Sunbathe)
            {
                dialogText.text = "Time to take in some sunshine!";
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
                SceneManager.LoadScene(beachSceneIndex);
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
            else if (interactionType == InteractionType.BeachDown)
            {
                dialogCanvas.SetActive(false);
                SceneManager.LoadScene(outsideHouseSceneIndex);
            }
        }
    }

    IEnumerator OneTimeUse()
    {
        hasInteracted = true;
        yield return new WaitForSeconds(1.5f);

        if (collectible != null)
        {
            Destroy(collectible);
        }

        Destroy(gameObject);

        if (interactionType == InteractionType.Collar)
        {
            FindObjectOfType<Collar>().WearCollar();
        }
        else if (interactionType == InteractionType.MissingKey)
        {
            //FindObjectOfType<Collar>().WearCollar();
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

            if (!FindObjectOfType<GameLogic>().happinessFromNeighbor)
            {
                dialogText.text = "It's good to see you outside, Casey! We miss you at book club!";
                FindObjectOfType<Health>().ShowMood();
                yield return new WaitForSeconds(1.5f);
                FindObjectOfType<Health>().IncreaseMood(25);
                FindObjectOfType<GameLogic>().happinessFromNeighbor = true;
                yield return new WaitForSeconds(1.5f);
            }
            ResumePlayerMovement();
            Destroy(gameObject);
        }
        else if (interactionType == InteractionType.Worker)
        {
            if (!FindObjectOfType<GameLogic>().foundWaterValveKey)
            {
                dialogText.text = "I lost the key to the water shut off valve. The whole street doesn't have any water.";
                yield return new WaitForSeconds(3f);
                ResumePlayerMovement();
            }
            else if (!FindObjectOfType<GameLogic>().waterTurnedOn)
            {
                FindObjectOfType<GameLogic>().waterTurnedOn = true;
                dialogText.text = "Thank you so so much!";
                yield return new WaitForSeconds(2f);
                ResumePlayerMovement();
                Destroy(gameObject);
            }
            else
            {
                dialogText.text = "Here you go!";
                FindObjectOfType<Health>().MaxSoil();
                yield return new WaitForSeconds(2f);

                if (!FindObjectOfType<GameLogic>().happinessFromConstruction)
                {
                    dialogText.text = "Hey Casey! It's good to see you!";
                    FindObjectOfType<Health>().ShowMood();
                    yield return new WaitForSeconds(1.5f);
                    FindObjectOfType<Health>().IncreaseMood(25);
                    FindObjectOfType<GameLogic>().happinessFromConstruction = true;
                    yield return new WaitForSeconds(1f);
                }
                ResumePlayerMovement();
                Destroy(gameObject);
            }
        }
        else if (interactionType == InteractionType.Sunbathe)
            {
                dialogText.text = "Mmm that's the stuff!";
                FindObjectOfType<Health>().MaxSun();

                yield return new WaitForSeconds(2f);
                if (!FindObjectOfType<GameLogic>().happinessFromBeach)
                {
                    dialogText.text = "That Vitamin D made me feel a little better too :)";
                    FindObjectOfType<Health>().ShowMood();
                    yield return new WaitForSeconds(1.5f);
                    FindObjectOfType<Health>().IncreaseMood(25);
                    FindObjectOfType<GameLogic>().happinessFromBeach = true;
                    yield return new WaitForSeconds(1f);
                }
                ResumePlayerMovement();
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
