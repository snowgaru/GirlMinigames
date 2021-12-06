using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

[System.Serializable]
public class CardInformation
{
    public string name;
  //  public Sprite sprite;
    public bool isLock;
   // public float amount;
}

public class CardManager : MonoBehaviour
{
    public GameObject[] GachaCard;
    public CardInformation[] cards;
    public List<int> CardList = new List<int>() {  };
    public Button gachaButton;
    public Button[] galleryCard; //°¶·¯¸® µµ°¨ ¾êµé
    public static int amount = 50; //Ä«µåÀÇ °¹¼ö 
    public Transform galleryCardPos;
    public GameObject galleryShowCardPanel;
    public Image galleryShowCardImage;
    public Sprite galleryShowCard;
    public GameObject gachaOutPanel;
    public TwoManager twoManager;

    public void Save()
    {
        for(int i = 0; i < 50; i ++)
        {
            PlayerPrefs.SetString("isLock" + i, cards[i].isLock.ToString());
        }
    }

    public void Load()
    {
        for (int i = 0; i < 50; i++)
        {
            string value = PlayerPrefs.GetString("isLock" + i, "false");
            cards[i].isLock = System.Convert.ToBoolean(value);
            value = null;
        }
    }

    void Start()
    {
        Load();
    }

    void Update()
    {
        CardLock();
    }
    public void Gacha()
    {
        if(GameManager.Instance.coinMoney < 2000)
        {
            Debug.Log("µ·ÀÌ ºÎÁ·ÇÏ´Ü´Ù");
            return;
        }
        else
        {
            GameManager.Instance.coinMoney -= 2000;
            if (CardList.Count == 0)
            {
                Debug.Log("Ä«µå°¡ ´Ù »ÌÇú´Ü´Ù");
                return;
            }
            AudioManager.instance.PlayGacha();
            int rand = Random.Range(0, CardList.Count);
            int result = (CardList[rand]) - 1;
            GachaCard[result].SetActive(true);
            gachaButton.interactable = false;
            //Invoke("CardSetActive", 0.5f);
            cards[result].isLock = true;
            CardList.RemoveAt(rand);
            gachaOutPanel.SetActive(true);
            //gachaOut.GetComponent<GameObject>().SetActive(true);
        }

    }

    public void GachaOut() // »Ì°í ÀÌ¹ÌÁö ²ô´Â°Å
    {
        CardSetActive();
      //  gachaOut.SetActive(false);
    }

    public void CardSetActive()
    {
        for(int i = 0; i < amount; i++) 
        {
            GachaCard[i].SetActive(false);
        }
        gachaButton.interactable = true;
    }

    public void CardLock()
    {
        for(int i = 0; i < amount; i++)
        {
            if(cards[i].isLock == true)
            {
                galleryCard[i].interactable = true;
            }
            else
            {
                galleryCard[i].interactable = false;
            }
        }
    }
    /*
    public void Destorys()
    {
        Destroy(viewCard);
    }

    public void ClickBtn()
    {
        Destorys();
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        //clickObject.GetComponent<Transform>().DOScale(0.5f, 0.5f);
        viewCard = Instantiate(clickObject, new Vector3(0, 0, 0), Quaternion.identity, galleryCardPos);
        
        viewCard.transform.DOScale(5f, 0f);//.OnComplete(() => viewCard.transform.DOScale(5f, 2f).OnComplete(() => Destroy(viewCard)));
        
    }
    */

    public void GalleryCardButton(Sprite girls)
    {
        galleryShowCardImage.sprite = girls;
        galleryShowCardPanel.SetActive(true);
    }

    public void SetActiveList()
    {
        galleryShowCardPanel.SetActive(false);
    }

    public void SetActiveGalleryCard(bool isOn)
    {
        gachaOutPanel.SetActive(isOn);
        CardSetActive();
    }


    void OnApplicationQuit()
    {
        Save();
    }

}
