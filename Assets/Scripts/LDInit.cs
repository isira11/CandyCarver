using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

public class LDInit : MonoBehaviour
{
    public LinkedDataSO LD;

    public Image load_fill;

    float fill_value = 0;

    bool stop;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LD.Init();
    }

    private void Update()
    {
        if (stop)
        {
            return;
        }

        fill_value += Time.deltaTime * 0.5f;

        if (fill_value > 1 && !stop)
        {
            stop = true;
            SceneManager.LoadSceneAsync("Menu");
        }
        else
        {
            load_fill.fillAmount = fill_value;
        }
    }
}
