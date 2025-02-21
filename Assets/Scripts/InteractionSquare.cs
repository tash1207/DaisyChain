using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class InteractionSquare : MonoBehaviour
{
    [SerializeField] InteractionType interactionType;
    [SerializeField] GameObject collectible;
    [SerializeField] GameObject dialogCanvas;
    [SerializeField] TMP_Text dialogText;
    [SerializeField] Image dialogImage;

    [SerializeField] Sprite daisySprite;
    [SerializeField] Sprite humanSprite;
    [SerializeField] Sprite neighborSprite;
    [SerializeField] Sprite workerSprite;

    bool hasInteracted = false;
    bool hasReachedEndGame = false;

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
        EndGame,
        Sink,
        Bookcase,
        Lamp,
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

    void Update()
    {
        if (interactionType == InteractionType.EndGame && !hasReachedEndGame)
        {
            if (FindObjectOfType<Health>().HasMaxedPlantMeters())
            {
                hasReachedEndGame = true;
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasInteracted) { return; }
        if (collision.tag == "Player")
        {
            Debug.Log("OnTriggerEnter2D");
            if (interactionType == InteractionType.BinCompost)
            {
                SpeakerDaisy();
                dialogText.text = "Compost is yummy!";
                FindObjectOfType<Health>().IncreaseSoil(20);
                PausePlayerMovement();
                StartCoroutine(OneTimeUse());
            }
            else if (interactionType == InteractionType.BinTrash)
            {
                SpeakerDaisy();
                dialogText.text = "Trash is gross!";
                FindObjectOfType<Health>().DecreaseSoil(20);
                PausePlayerMovement();
                StartCoroutine(OneTimeUse());
            }
            else if (interactionType == InteractionType.Collar)
            {
                SpeakerDaisy();
                dialogText.text = "Ooh shiny! Yoink!";
                PausePlayerMovement();
                StartCoroutine(OneTimeUse());
            }
            else if (interactionType == InteractionType.Fridge)
            {
                SpeakerDaisy();
                dialogText.text = "Yuck! That is definitely not what plants crave. I need soil.";
            }
            else if (interactionType == InteractionType.FrontDoor)
            {
                SpeakerDaisy();
                dialogText.text = "Let me out of here! I need air!";
            }
            else if (interactionType == InteractionType.Human)
            {
                SpeakerHuman();
                dialogText.text = "Aah! You can walk?";
                if (FindObjectOfType<Collar>().IsWearingCollar())
                {
                    PausePlayerMovement();
                    StartCoroutine(NextDialog());
                }
            }
            else if (interactionType == InteractionType.Plant)
            {
                SpeakerDaisy();
                dialogText.text = "Hello friend!";
            }
            else if (interactionType == InteractionType.Neighbor)
            {
                SpeakerNeighbor();
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
                SpeakerWorker();
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
                SpeakerDaisy();
                dialogText.text = "*spit take*\nPretty sure that was sewer water :(";
                FindObjectOfType<Health>().DecreaseWater(20);
                PausePlayerMovement();
                StartCoroutine(OneTimeUse());
            }
            else if (interactionType == InteractionType.MissingKey)
            {
                SpeakerDaisy();
                dialogText.text = "Ooh a key! This looks useful!";
                FindObjectOfType<GameLogic>().foundWaterValveKey = true;
                PausePlayerMovement();
                StartCoroutine(OneTimeUse());
            }
            else if (interactionType == InteractionType.Sunbathe)
            {
                SpeakerDaisy();
                dialogText.text = "Time to take in some sunshine!";
                PausePlayerMovement();
                StartCoroutine(NextDialog());
            }
            else if (interactionType == InteractionType.EndGame)
            {
                if (!FindObjectOfType<Health>().HasMaxedPlantMeters())
                {
                    SpeakerDaisy();
                    dialogText.text = "I'm not ready to go inside yet.";
                }
                else if (!FindObjectOfType<Health>().HasMaxedHumanMeters())
                {
                    SpeakerHuman();
                    dialogText.text = "You seem like you have everything you need. Let's go inside.";
                    PausePlayerMovement();
                    StartCoroutine(NextDialog());
                }
                else
                {
                    SpeakerHuman();
                    dialogText.text = "It's been an eventful day, Daisy.";
                    PausePlayerMovement();
                    StartCoroutine(NextDialog());
                }
            }
            else if (interactionType == InteractionType.Sink)
            {
                SpeakerDaisy();
                dialogText.text = "Why isn't the sink working? I need water!";
            }
            else if (interactionType == InteractionType.Bookcase)
            {
                SpeakerDaisy();
                dialogText.text = "I wish I knew how to read!";
            }
            else if (interactionType == InteractionType.Lamp)
            {
                SpeakerDaisy();
                dialogText.text = "This lamp ain't cutting it. I need real sunlight.";
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
        if (interactionType == InteractionType.ConstructionWater)
        {
            yield return new WaitForSeconds(2.5f);
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
        }

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
            // TODO: Maybe wear the key. Or show it in inventory somehow.
            //FindObjectOfType<Collar>().WearCollar();
        }
        ResumePlayerMovement();
    }

    IEnumerator NextDialog()
    {
        yield return new WaitForSeconds(2.5f);
        if (interactionType == InteractionType.Human)
        {
            SpeakerHuman();
            dialogText.text = "Well, I guess we can go for a walk.";
            yield return new WaitForSeconds(2.5f);

            ResumePlayerMovement();
            yield return new WaitForSeconds(1f);
            // TODO: Fade to black?
            SceneManager.LoadScene(2);
        }
        else if (interactionType == InteractionType.Neighbor)
        {
            SpeakerNeighbor();
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
            SpeakerWorker();
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
            SpeakerDaisy();
            dialogText.text = "Mmm that's the stuff!";
            FindObjectOfType<Health>().MaxSun();

            yield return new WaitForSeconds(2f);
            if (!FindObjectOfType<GameLogic>().happinessFromBeach)
            {
                SpeakerHuman();
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
        else if (interactionType == InteractionType.EndGame)
        {
            SpeakerHuman();
            if (!FindObjectOfType<Health>().HasMaxedHumanMeters())
            {
                dialogText.text = "I need to be alone again.";
                ResumePlayerMovement();
                // TODO: Trigger suboptimal game ending.
            }
            else
            {
                dialogText.text = "Thanks for getting me out of the house.";
                FindObjectOfType<Health>().ShowMood();
                yield return new WaitForSeconds(2.5f);
                FindObjectOfType<Health>().IncreaseMood(25);
                ResumePlayerMovement();
                // TODO: Load happy end cutscene.
            }
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

    void SpeakerDaisy()
    {
        dialogImage.sprite = daisySprite;
    }

    void SpeakerHuman()
    {
        dialogImage.sprite = humanSprite;
    }

    void SpeakerNeighbor()
    {
        dialogImage.sprite = neighborSprite;
    }

    void SpeakerWorker()
    {
        dialogImage.sprite = workerSprite;
    }
}
