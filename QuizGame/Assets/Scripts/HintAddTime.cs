using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintAddTime : HintButton
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
    // Start is called before the first frame update

    private void Awake()
    {
        _gc = FindObjectOfType<GameController>();
    }
    void Start()
    {
        //gameController = FindObjectOfType<GameController>();
        Debug.Log("HintAddTime script starts");
        //HideHintPrice();
    }

    public override void HandleClick()
    {
        _gc.HintButtonClicked(this); ;
    }

    public override void UseHint()
    {
        if (hintQty > 0)
        {
            Debug.Log("*********Use Hint HintAddTime********");
            hintQty -= 1;
            HideHintPrice();
            _gc.UpdateHintsQtyUI();
            _gc.HintAddTime();
        }
        else
        {
            Debug.Log("*********You dont have enough qty of hint to use!!!********");
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
    //    throw new System.NotImplementedException();
    //}

    //public override int GetHintPrice()
    //{
    //    throw new System.NotImplementedException();
    //}
}
