using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStart : MonoBehaviour
{
    [SerializeField] GameObject daisy;
    [SerializeField] GameObject human;

    Vector2 constructionTopLeftDaisy = new Vector2(-4.3f, 4f);
    Vector2 constructionTopLeftHuman = new Vector2(-3.3f, 4f);

    Vector2 constructionTopRightDaisy = new Vector2(2f, 4f);
    Vector2 constructionTopRightHuman = new Vector2(3f, 4f);

    Vector2 neighborYardBottomDaisy = new Vector2(-4f, -3.4f);
    Vector2 neighborYardBottomHuman = new Vector2(-5f, -3f);

    Vector2 neighborYardRightDaisy = new Vector2(7f, -2.5f);
    Vector2 neighborYardRightHuman = new Vector2(8f, -2.5f);

    Vector2 outsideHouseBottomDaisy = new Vector2(2f, -3.1f);
    Vector2 outsideHouseBottomHuman = new Vector2(3f, -3.1f);

    Vector2 outsideHouseLeftDaisy = new Vector2(-7f, -2.7f);
    Vector2 outsideHouseLeftHuman = new Vector2(-8f, -2.7f);

    void Start()
    {
        // Hacky way to make sure the canonical game session is updated.
        foreach (GameSession gameSession in FindObjectsOfType<GameSession>())
        {
            gameSession.SetSceneSettings();
        }
    }

    public void ConstructionToNeighborYard()
    {
        // Set position to the bottom left of screen.
        daisy.transform.position = neighborYardBottomDaisy;
        human.transform.position = neighborYardBottomHuman;
    }

    public void ConstructionToOutsideHouse()
    {
        // Set position to the bottom right of screen.
        daisy.transform.position = outsideHouseBottomDaisy;
        human.transform.position = outsideHouseBottomHuman;
    }

    public void OutsideHouseToNeighborYard()
    {
        // Set position to the right of screen.
        daisy.transform.position = neighborYardRightDaisy;
        human.transform.position = neighborYardRightHuman;
    }

    public void OutsideHouseToConstruction()
    {
        // Set position to the top right of screen.
        daisy.transform.position = constructionTopRightDaisy;
        human.transform.position = constructionTopRightHuman;
    }

    public void NeighborYardToConstruction()
    {
        // Set position to the top left of screen.
        daisy.transform.position = constructionTopLeftDaisy;
        human.transform.position = constructionTopLeftHuman;
    }

    public void NeighborYardToOutsideHouse()
    {
        // Set position to the left of screen.
        daisy.transform.position = outsideHouseLeftDaisy;
        human.transform.position = outsideHouseLeftHuman;
    }
}
