using UnityEngine;

public class HintPlayersHelp : HintButton
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
        Debug.Log("HintPlayersHelp script starts");
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
            Debug.Log("*********Use Hint HintPlayersHelp********");
            hintQty -= 1;
            HideHintPrice();
            _gc.UpdateHintsQtyUI();
            _gc.HintPlayersHelp();
        } else
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
}
