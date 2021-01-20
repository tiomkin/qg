using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject chestOpened;
    public GameObject chestClosed;
    public GameObject prizePanel;
    private GameController _gc;
    // Start is called before the first frame update
    void Start()
    {
        _gc = FindObjectOfType<GameController>();
        //CloseChest();
    }

    public void HandleClick()
    {
        _gc.ChestClicked(this);
    }
    public void OpenChest(int i)
    {
        switch (i)
        {
            case 0:
                Debug.Log("None");
                break;
            case 1:
                Debug.Log("You won 50 coins!");
                break;
            case 2:
                Debug.Log("You won hint!");
                break;
            case 3:
                Debug.Log("You won a live");
                break;
            default:
                Debug.Log("None");
                break;
        }
        chestOpened.SetActive(true);
        chestClosed.SetActive(false);
    }

    public void CloseChest()
    {
        chestClosed.SetActive(true);
        chestOpened.SetActive(false);
    }

}
