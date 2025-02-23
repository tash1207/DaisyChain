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
    [SerializeField] GameObject dialogCanvas;
    [SerializeField] TMP_Text dialogText;
    [SerializeField] Image dialogImage;

    [SerializeField] Sprite daisySprite;
    [SerializeField] Sprite humanSprite;
    [SerializeField] Sprite neighborSprite;
    [SerializeField] Sprite workerSprite;

    [SerializeField] GameObject collectible;
    [SerializeField] GameObject moveableObject;
    [SerializeField] Sprite moveableObjectSpriteDefault;
    [SerializeField] Sprite moveableObjectSpriteMoved;
    [SerializeField] AudioClip audioClip;

    bool hasInteracted = false;
    bool showingGetTowelSquare = false;
    bool hasEnabledJoke = false;
    bool hasReachedEndGame = false;

    int outsideHouseSceneIndex = 3;
    int neighborYardSceneIndex = 4;
    int constructionSceneIndex = 5;
    int beachSceneIndex = 6;
    int outroHappySceneIndex = 7;
    int suboptimalEndingSceneIndex = 9;

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
        GetTowel,
        DaisyJoke,
        OceanWater,
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
        else if (interactionType == InteractionType.DaisyJoke && !hasEnabledJoke)
            {
                if (FindObjectOfType<Health>().HasMaxedPlantMeters() &&
                    FindObjectOfType<Health>().HasHappyHumanMeters())
                {
                    hasEnabledJoke = true;
                    // GetComponent<SpriteRenderer>().enabled = true;
                    GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        else if (interactionType == InteractionType.GetTowel && !showingGetTowelSquare &&
            FindObjectOfType<GameLogic>().needsTowel &&
            !FindObjectOfType<GameLogic>().hasTowel)
        {
            showingGetTowelSquare = true;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasInteracted) { return; }
        if (collision.tag == "Player")
        {
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
                if (FindObjectOfType<Collar>().IsWearingCollar())
                {
                    if (moveableObject != null)
                    {
                        moveableObject.GetComponent<SpriteRenderer>().sprite = moveableObjectSpriteMoved;
                    }
                    SpeakerHuman();
                    dialogText.text = "Aah! You can walk?";
                    PausePlayerMovement();
                    StartCoroutine(NextDialog());
                }
                else
                {
                    SoundFXManager.instance.PlaySoundFXClipXTimesWithModulation(audioClip, 1f, 3, 0.1f);
                    SpeakerHuman();
                    dialogText.text = "...";
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
                dialogText.text = "Fresh water! Yay!";
                FindObjectOfType<Health>().IncreaseWater(35);
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
                if (FindObjectOfType<GameLogic>().hasTowel)
                {
                    dialogText.text = "Time to take in some sunshine!";
                }
                else
                {
                    dialogText.text = "Let's take in some sunshine!";
                }
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
                    dialogText.text = "It's been an exciting day, Daisy.";
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
            else if (interactionType == InteractionType.GetTowel)
            {
                SpeakerHuman();
                dialogText.text = "I'll just grab a towel while we're here.";
                PausePlayerMovement();
                StartCoroutine(NextDialog());
            }
            else if (interactionType == InteractionType.DaisyJoke)
            {
                SpeakerDaisy();
                dialogText.text = "Hey Casey, what do you call a flower that runs on electricity?";
                PausePlayerMovement();
                StartCoroutine(NextDialog());
            }
            else if (interactionType == InteractionType.OceanWater)
            {
                SpeakerDaisy();
                dialogText.text = "Salt water! Nooo!";
                FindObjectOfType<Health>().DecreaseWater(25);
                PausePlayerMovement();
                StartCoroutine(OneTimeUse());
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
            SoundFXManager.instance.PlaySoundFXClip(audioClip, 0.8f);
            FindObjectOfType<Collar>().WearCollar();
        }
        else if (interactionType == InteractionType.MissingKey)
        {
            SoundFXManager.instance.PlaySoundFXClip(audioClip, 0.8f);
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
            SceneManager.LoadScene(outsideHouseSceneIndex);
        }
        else if (interactionType == InteractionType.Neighbor)
        {
            if (moveableObject != null)
            {
                moveableObject.transform.Rotate(new Vector3(0, 180, 0));
            }
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
            if (moveableObject != null)
            {
                moveableObject.transform.Rotate(new Vector3(0, -180, 0));
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
                if (moveableObject != null)
                {
                    moveableObject.GetComponent<SpriteRenderer>().sprite = moveableObjectSpriteMoved;
                }
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
                if (moveableObject != null)
                {
                    moveableObject.GetComponent<SpriteRenderer>().sprite = moveableObjectSpriteDefault;
                }
                ResumePlayerMovement();
                Destroy(gameObject);
            }
        }
        else if (interactionType == InteractionType.Sunbathe)
        {
            if (FindObjectOfType<GameLogic>().hasTowel)
            {
                if (moveableObject != null)
                {
                    // Hide daisy (and collar) and human (and leash) and show sunbathing sprites.
                    GameObject daisy = FindObjectOfType<PlayerMovement>().gameObject;
                    GameObject human = FindObjectOfType<HumanMovement>().gameObject;
                    daisy.GetComponent<SpriteRenderer>().enabled = false;
                    human.GetComponent<SpriteRenderer>().enabled = false;
                    daisy.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                    human.transform.GetChild(0).GetComponent<LineRenderer>().enabled = false;
                    moveableObject.SetActive(true);
                    GetComponent<SpriteRenderer>().enabled = false;
                }
                SpeakerDaisy();
                dialogText.text = "Mmm that's the stuff!";
                FindObjectOfType<Health>().MaxSun();

                yield return new WaitForSeconds(2.5f);
                if (!FindObjectOfType<GameLogic>().happinessFromBeach)
                {
                    SpeakerHuman();
                    dialogText.text = "This Vitamin D is making me feel a little better too.";
                    FindObjectOfType<Health>().ShowMood();
                    yield return new WaitForSeconds(2f);
                    FindObjectOfType<Health>().IncreaseMood(25);
                    FindObjectOfType<GameLogic>().happinessFromBeach = true;
                    yield return new WaitForSeconds(1f);
                }
                if (moveableObject != null)
                {
                    // Show daisy and human.
                    GameObject daisy = FindObjectOfType<PlayerMovement>().gameObject;
                    GameObject human = FindObjectOfType<HumanMovement>().gameObject;
                    daisy.GetComponent<SpriteRenderer>().enabled = true;
                    human.GetComponent<SpriteRenderer>().enabled = true;
                    daisy.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                    human.transform.GetChild(0).GetComponent<LineRenderer>().enabled = true;
                    moveableObject.SetActive(false);
                }
                Destroy(gameObject);
            }
            else
            {
                SpeakerHuman();
                dialogText.text = "I didn't bring a towel to lay on.";
                FindObjectOfType<GameLogic>().needsTowel = true;
                yield return new WaitForSeconds(1.5f);
            }
            ResumePlayerMovement();
        }
        else if (interactionType == InteractionType.EndGame)
        {
            SpeakerHuman();
            if (!FindObjectOfType<Health>().HasMaxedHumanMeters())
            {
                dialogText.text = "I need to be alone again.";
                FindObjectOfType<Health>().ShowMood();
                yield return new WaitForSeconds(2f);
                FindObjectOfType<Health>().MinMood();
                yield return new WaitForSeconds(2f);
                SceneManager.LoadScene(suboptimalEndingSceneIndex);
            }
            else
            {
                dialogText.text = "Thanks for getting me out of the house.";
                FindObjectOfType<Health>().ShowMood();
                yield return new WaitForSeconds(2.5f);
                FindObjectOfType<Health>().IncreaseMood(25);
                yield return new WaitForSeconds(1.5f);
                SceneManager.LoadScene(outroHappySceneIndex);
            }
        }
        else if (interactionType == InteractionType.GetTowel)
        {
            if (moveableObject != null)
            {
                moveableObject.SetActive(false);
            }
            SpeakerDaisy();
            dialogText.text = "Grab one for me too!";
            yield return new WaitForSeconds(2f);
            if (moveableObject != null)
            {
                moveableObject.SetActive(true);
            }
            Destroy(gameObject);
            FindObjectOfType<GameLogic>().hasTowel = true;
            ResumePlayerMovement();
        }
        else if (interactionType == InteractionType.DaisyJoke)
        {
            SpeakerDaisy();
            dialogText.text = "A power plant!";
            yield return new WaitForSeconds(2f);

            SpeakerHuman();
            dialogText.text = "Hehe I wasn't expecting that.";
            FindObjectOfType<Health>().ShowMood();
            yield return new WaitForSeconds(2f);
            FindObjectOfType<Health>().IncreaseMood(25);
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
            ResumePlayerMovement();
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
            if (moveableObject != null && moveableObjectSpriteDefault != null)
            {
                moveableObject.GetComponent<SpriteRenderer>().sprite = moveableObjectSpriteDefault;
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
