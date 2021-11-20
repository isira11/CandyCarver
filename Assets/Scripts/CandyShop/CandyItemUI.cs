using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CandyItemUI : MonoBehaviour
{
    public Image icon;
    public Image inner;
    public Transform ad;
    public TextMeshProUGUI adamount;
    public Transform padlock;
    public Transform play;
    public Button button;


    public void Reset()
    {
        play.gameObject.SetActive(false);
        ad.gameObject.SetActive(false);
        adamount.gameObject.SetActive(false);
        padlock.gameObject.SetActive(false);
    }

}
