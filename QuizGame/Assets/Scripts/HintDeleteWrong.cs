using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HintDeleteWrong : HintButton
{
    public int hintPrice;
    private int hintQty;
    private GameController _gc;
    //protected GameController gameController;
    public GameObject activeImage;
    public GameObject inActiveImage;

    public int HintQty
    {
        get { return hintQty; }
        set { hintQty = value; }
    }

    private void Awake()
    {
        _gc = FindObjectOfType<GameController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //gameController = FindObjectOfType<GameController>();
        //HideHintPrice();
    }

    public override void HandleClick()
    {
        _gc.HintButtonClicked(this); ;
    }

    public override void UseHint()
    {
        if (hintQty > 0 && _gc.answerButtonsList.Count > 1)
        {
            Debug.Log("*********Use Hint HintDeleteWrong********");
            hintQty -= 1;
            HideHintPrice();
            _gc.UpdateHintsQtyUI();
            _gc.HintDeleteWrong();
        }
        else
        {
            if (_gc.answerButtonsList.Count > 1)
            {
                Debug.Log("*********You dont have enough qty of hint to use!!!********");
            } else
            {
                Debug.Log("*********the single correct answer is on screen!!!********");
            }
        }
    }
    public void HideHintPrice()
    {
        if (hintQty > 0)
        {

            activeImage.SetActive(true);
            inActiveImage.SetActive(false);
        }
        else
        {
            activeImage.SetActive(false);
            inActiveImage.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //public override int GetHintQty()
    //{
    //    return hintQty;
    //}

    //public override int GetHintPrice()
    //{
    //    return hintPrice;
    //}
}
