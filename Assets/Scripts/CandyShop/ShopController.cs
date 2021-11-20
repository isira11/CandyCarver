using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopController : MonoBehaviour
{
    public LinkedDataSO LD;

    public Transform content;

    public GameObject UIPrefabs;



    Hashtable items = new Hashtable();

    private void Start()
    {

        LoadShop();
    }

    public void LoadShop()
    {
        foreach (Transform item in content)
        {
            Destroy(item.gameObject);
        }

        foreach (KeyValuePair<string,CandyData> key_value in LD.shapes)
        {
            CandyData item = key_value.Value;
            GameObject obj = Instantiate(UIPrefabs);
            obj.transform.SetParent(content);
            obj.transform.localScale = Vector3.one;
            CandyItemUI candyItemUI = obj.GetComponent<CandyItemUI>();
            candyItemUI.icon.sprite = Sprite.Create(item.t1, new Rect(0.0f, 0.0f, item.t1.width, item.t1.height), new Vector2(0.5f, 0.5f));
            if (item.t5)
            {
                candyItemUI.inner.sprite = Sprite.Create(item.t5, new Rect(0.0f, 0.0f, item.t5.width, item.t5.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                candyItemUI.inner.color = new Color(0, 0, 0, 0);
            }

            items.Add(item.id, candyItemUI);

            candyItemUI.Reset();

            if (LD.IsShapePurchase(item.id))
            {
                candyItemUI.play.gameObject.SetActive(true);
                candyItemUI.button.onClick.AddListener(() =>
                {
                    LD.SetCurrentShape(item.id);
                    SceneManager.LoadScene("Game");
                });
            }
            else
            {
                candyItemUI.padlock.gameObject.SetActive(true);
            }


        }
    }






}
