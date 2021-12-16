using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public Image DiceImage;
    public void InsertImage(int num)
    {
        DiceImage.sprite = HolJackManager.Instance.DiceImages[num];
    }

    public void ResetImage()
    {
        DiceImage.sprite = null;
    }
}