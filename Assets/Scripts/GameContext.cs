using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EasyButtons;
using FreeDraw;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using MoreMountains.NiceVibrations;


public class GameContext : MonoBehaviour
{
    public float mag;
    public float vib_time;
    public bool safe;

    public int load_n;

    public string state_indicator;

    public LinkedDataSO LD;

    State state;

    public Camera main_cam;
    public Transform cam_p0;
    public Transform cam_p1;

    [Header("Candy")]
    public Transform entire;
    public Transform inner_cut;
    public Transform outter_cut;
    public Transform outline_out_rotation;
    public Transform inner_design;
    public Transform outline_out;
    public Transform outline_in;
    public Transform shapefill;
    public Transform pointer_edge;
    public Transform lid;
    public Transform crack;
    public Transform enlarged;

    public Transform pointer;
    public ToolsSO tools;
    public GameObject tool;
    public PresetDemoManager presetDemoManager;


    public Transform offer_1_parent;
    public Image offer_1_icon;

    public Transform offer_2_parent;
    public Image offer_2_icon;

    public Image new_unlock_outter;
    public Image new_unlock_inner;
    public Image new_unlock_fill;

    [Header("GUI")]

    public GameObject game_ui;
    public GameObject win_ui;
    public GameObject loose_ui;

    public GameObject btn_back;
    public GameObject btn_next;
    public GameObject btn_skip;
    public GameObject btn_retry;

    public GameObject btn_haptic;

    public GameObject unlock_ui;

    public GameObject unlock_tool_ui;
    public Image tool_icon;

    public ParticleSystem tool_unlock_particles;
    public ParticleSystem candy_unlock_particles;
    public ParticleSystem pointer_particles;

    public CandyData curr_candy;

    public Drawable drawable;

    public Action pressedNext = delegate {};
    public Action pressedSkip = delegate {};
    public Action pressedBack = delegate { };

    public bool ad_shown;

    public bool isHaptic;

    public void Start()
    {
        LD.ShowBanner();
        ResetScene();

        tool = Instantiate(LD.GetCurrentTool());
        tool.transform.parent = pointer;
        tool.transform.localPosition = Vector3.zero;


        curr_candy = LD.GetCurrentShape();

        if (curr_candy == null)
        {
            curr_candy = LD.shapesSO.candyDatas[Random.Range(0, LD.shapesSO.candyDatas.Count)];
        }



        outline_out.GetComponent<SpriteRenderer>().sprite = GetSprite(curr_candy.t1);

        outline_in.GetComponent<SpriteRenderer>().sprite = GetSprite(curr_candy.t3);

        outline_out_rotation.GetComponent<SpriteRenderer>().sprite = GetSprite(curr_candy.t2);
        outline_out_rotation.GetComponent<SpriteMask>().sprite = GetSprite(curr_candy.t2);
        outline_out_rotation.gameObject.AddComponent<PolygonCollider2D>();

        inner_cut.GetComponent<SpriteMask>().sprite = GetSprite(curr_candy.t4);
        outter_cut.GetComponent<SpriteMask>().sprite = GetSprite(curr_candy.t4);
        shapefill.GetComponent<SpriteRenderer>().sprite = GetSprite(curr_candy.t4);

        if (curr_candy.t5)
        {
            inner_design.gameObject.SetActive(true);
            inner_design.GetComponent<SpriteRenderer>().sprite = GetSprite(curr_candy.t5);

        }

        shapefill.gameObject.AddComponent<PolygonCollider2D>();

        outline_in.transform.gameObject.SetActive(false);

        enlarged.gameObject.SetActive(false);
        pointer.gameObject.SetActive(false);

        state = GameState.Build(this);
    }

    public Sprite GetSprite(Texture2D tex)
    {
        return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    internal void OnStateChanged(CommonStateBase obj)
    {
        state_indicator = obj.ToString();
    }

    public void Update()
    {
        if (state != null)
        {
            state.current_state.Update();
        }
    }

    public void PressNext()
    {
        pressedNext();
    }

    public void ToggleVibrations()
    {
        isHaptic = !isHaptic;

        btn_haptic.GetComponent<Image>().enabled = !isHaptic;

        MMVibrationManager.SetHapticsActive(isHaptic);

    }

    public void PressSkip()
    {
        pressedSkip();
    }

    public void PressedBack()
    {
        OnBackPressed();
    }

    public void OnBackPressed()
    {
        StartCoroutine(CR_OnBackNext());

    }

    IEnumerator CR_OnBackNext()
    {
        LD.ShowInterstitialAd();
        yield return new WaitForSeconds(0.01f);
        SceneManager.LoadScene("Menu");
    }

    public void ResetScene()
    {
        main_cam.orthographicSize = 11.86f;
        main_cam.transform.position = cam_p0.position;
        pointer.gameObject.SetActive(false);

        inner_design.gameObject.SetActive(false);
        outline_in.gameObject.SetActive(false);
        outline_out.gameObject.SetActive(false);
        outter_cut.gameObject.SetActive(false);
        inner_cut.gameObject.SetActive(false);
        crack.gameObject.SetActive(false);
        unlock_tool_ui.gameObject.SetActive(false);

        enlarged.gameObject.SetActive(false);
        lid.gameObject.SetActive(true);

        game_ui.SetActive(false);
        win_ui.SetActive(false);
        loose_ui.SetActive(false);
        unlock_ui.SetActive(false);
        btn_haptic.SetActive(false);

        btn_next.SetActive(false);
        btn_retry.SetActive(false);
        btn_skip.SetActive(false);
        btn_back.SetActive(false);

    }



}
