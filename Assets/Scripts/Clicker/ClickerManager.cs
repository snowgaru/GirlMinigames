using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClickerManager : MonoBehaviour
{
    public Sprite png;
    public Sprite[] sprite;
    public Sprite[] injurySprite;
    public SpriteRenderer currentSprite;
    public GameObject Plus;
    public GameObject TouchPanel;

    private int hp=10;

    static int random = 0;
    private int _random = -1;

    void Start()
    {
        Make();
    }

    public void OnClickPanel()
    {
        if(hp>0) 
        {
            AudioManager.instance.PlayAttack();
            hp--;
        }
        else
        {
            TouchPanel.GetComponent<Button>().interactable = false;
            Dead();
        }
    }

    public void Dead()
    {
        AudioManager.instance.PlayAttack();
        GameManager.Instance.coinMoney += 25;
        currentSprite.sprite = injurySprite[random];
        Invoke("SpriteDestroy", 1.5f);
    }

    public void SpriteDestroy()
    {
        //MovePlus();
        currentSprite.sprite = null;
        Invoke("Make", 0.65f);
    }

    public void MovePlus()
    {
        Debug.Log("½ÇÇàµÊ");
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(Plus.transform.DOMoveY(10, 0.5f)).Join(Plus.transform.DOScale(new Vector3(0.5f,0.5f,0.5f),0.5f));
    }

    public void Make()
    {
        random = Random.Range(0, sprite.Length);
        if(_random == random)
        {
            Make();
            return;
        }
        _random = random;
        currentSprite.sprite = sprite[random];
        hp = 10;
        TouchPanel.GetComponent<Button>().interactable = true;
    }
}
