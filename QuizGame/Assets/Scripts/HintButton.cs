using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HintButton: MonoBehaviour
{
    //public abstract int HintQty { get; set; }
    //protected GameController _gc;
    private void Awake()
    {
        //_gc = FindObjectOfType<GameController>();
    }

    private void Start()
    {
        Debug.Log("HintButton script starts");
    }

    // Start is called before the first frame update

    //public abstract int GetHintQty();
    //public abstract int GetHintPrice();
    public abstract void UseHint();
    public abstract void HandleClick();

}
