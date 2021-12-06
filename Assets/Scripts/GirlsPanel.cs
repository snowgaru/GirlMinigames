using UnityEngine.UI;
using UnityEngine;

public class GirlsPanel : MonoBehaviour
{
    [SerializeField]
    private Image girlsImage = null;
    [SerializeField]
    private Sprite[] girlsSprite;

    private Girls girls = null;

    public void Start()
    {
        BuyGirlsUI();
    }

    public void BuyGirlsUI()
    {
        girlsImage.sprite = girlsSprite[girls.girlsNumber];
    }
}
