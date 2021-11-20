using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

public class GameState : State
{

    public OpenLidState openLidState;
    public PlayState playState;
    public WinState winState;
    public LooseState looseState;
    public UnlockCandyState unlockCandyState;
    public UnlockToolState unlockToolState;
    public Texture2D t;


    public static GameState Build(GameContext context)
    {
        return new GameState(context);
    }

    private GameState(GameContext context) : base(context)
    {
        playState = new PlayState(context, this);
        winState = new WinState(context, this);
        looseState = new LooseState(context, this);
        openLidState = new OpenLidState(context, this);
        unlockCandyState = new UnlockCandyState(context, this);
        unlockToolState = new UnlockToolState(context, this);

        StateChanged += context.OnStateChanged;
        SwitchState(openLidState);
    }

    public abstract class Base : CommonStateBase
    {
        public GameContext context;
        public GameState state;

        public Base(GameContext context, GameState state)
        {
            this.context = context;
            this.state = state;
        }
    }

    public class OpenLidState : Base
    {
        public OpenLidState(GameContext context, GameState state) : base(context, state) { }

        public override void OnEnter()
        {
            context.main_cam.orthographicSize = 11.86f;
            context.main_cam.transform.position = context.cam_p0.position;
            context.outline_out_rotation.gameObject.SetActive(true);

            context.outline_out.gameObject.SetActive(true);

            context.lid.gameObject.SetActive(true);
            Sequence s = DOTween.Sequence();
            s.Append(context.lid.DORotate(new Vector3(0, 0, 360), 3, RotateMode.FastBeyond360));
            s.Append(context.lid.DOLocalMoveX(-20, 2));
            s.OnComplete(() =>
            {
                state.SwitchState(state.playState);
            });
        }
    }

    public class PlayState : Base
    {
        public PlayState(GameContext context, GameState state) : base(context, state) { }

        public long start_pixels;

        public override void OnEnter()
        {
            InitDraw();

            context.pointer.gameObject.SetActive(true);

            context.enlarged.gameObject.SetActive(true);

            context.game_ui.SetActive(true);

            context.btn_skip.SetActive(true);

            context.btn_skip.SetActive(true);

            context.btn_back.SetActive(true);

            context.btn_haptic.SetActive(true);

            context.pressedSkip += OnSkipPressed;

        }

        public override void OnExit()
        {
            context.btn_haptic.SetActive(false);

            context.drawable.enabled = false;

            context.StopAllCoroutines();

            context.pointer.gameObject.SetActive(false);

            context.enlarged.gameObject.SetActive(false);

            context.outline_out_rotation.gameObject.SetActive(false);

            context.outline_out.gameObject.SetActive(false);

            context.game_ui.SetActive(false);

            context.pressedSkip -= OnSkipPressed;

        }


        public void OnSkipPressed()
        {
            context.LD.ShowRewarded(()=> {
                state.SwitchState(state.winState);
            });
        }


        Vector2 hit_0;
        Vector3 pos_0;

        Vector3 prev_pos;

        float vib_time;



        public override void Update()
        {
            context.safe = false;
            context.vib_time = vib_time;

            if (Input.GetMouseButtonDown(0))
            {
                hit_0 = context.main_cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20));
                pos_0 = context.pointer.transform.position;
                prev_pos = context.pointer.transform.position;
            }

            if (Input.GetMouseButton(0))
            {
 

                Vector2 hit_1 = context.main_cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20));
 
                Vector2 move_delta = (hit_1 - hit_0);



                context.pointer.transform.position = pos_0 + new Vector3(move_delta.x, move_delta.y, 0);

                RaycastHit2D hit2 = Physics2D.Raycast(context.pointer_edge.position, Vector3.forward, 10000);
                float delta_mag = (context.pointer.transform.position - prev_pos).magnitude;
                context.mag = delta_mag;




                if (hit2)
                {
                    if (hit2.transform.tag == "kill_zone")
                    {
                        state.SwitchState(state.looseState);
                    }

                    if (hit2.transform.tag == "safe_zone")
                    {
                        if (delta_mag > 0.0f)
                        {
                            vib_time -= Time.deltaTime * delta_mag * 1000;
                        }

                        if (vib_time <= 0)
                        {
                            context.tool.transform.eulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time*50)*5);

                            MMVibrationManager.Haptic(HapticTypes.Selection, false, true, context);
                            vib_time = 3;
                            context.pointer_particles.Play();
                            Debug.Log("SAF");
                        }

                        context.safe = true;
                    }
                    else
                    {
                        context.pointer_particles.Stop();
                    }
                }
            }

            prev_pos = context.pointer.transform.position;
        }



        IEnumerator Check()
        {
            while (true)
            {

                float percent = (float)GetRemaningPixels() / (float)start_pixels;

                if (percent <= 0.03f)
                {
                    state.SwitchState(state.winState);
                }
                yield return new WaitForSeconds(2);
            }
        } 

        public long GetRemaningPixels()
        {
            Texture2D tex = context.outline_out_rotation.GetComponent<SpriteRenderer>().sprite.texture;

            long number = 0;
            for (int x = 0; x < tex.width; x++)
            {
                for (int y = 0; y < tex.height; y++)
                {

                    if (tex.GetPixel(x, y).a != 0)
                    {
                        number++;
                    }
                }
            }

            return number;
        }

        public void InitDraw()
        {
            Transform outline = context.outline_out_rotation;

            Sprite sprite = outline.GetComponent<SpriteRenderer>().sprite;
            Texture2D texture2 = sprite.texture.DeCompress();

            Texture2D temp = new Texture2D(texture2.width, texture2.height);
            Sprite temp_sprite = Sprite.Create(temp, new Rect(0.0f, 0.0f, temp.width, temp.height), new Vector2(0.5f, 0.5f), 100.0f);
            temp.LoadImage(texture2.EncodeToPNG());

            outline.GetComponent<SpriteMask>().sprite = temp_sprite;
            outline.GetComponent<SpriteRenderer>().sprite = temp_sprite;

            context.drawable.Init(temp_sprite);
            context.drawable.enabled = true;
            start_pixels = GetRemaningPixels();

            context.StartCoroutine(Check());
        }
    }

    public class LooseState : Base
    {
        public LooseState(GameContext context, GameState state) : base(context, state) { }


        public override void OnEnter()
        {
            context.crack.gameObject.SetActive(true);

            context.loose_ui.SetActive(true);

            context.btn_retry.SetActive(true);

            context.outline_out.gameObject.SetActive(true);

            context.pressedNext += OnPressNext;

            context.pressedSkip += OnPressSkip;

            MMVibrationManager.Haptic(HapticTypes.Failure, false, true, context);

        }


        private void OnPressSkip()
        {

            context.LD.ShowRewarded(() => {
                context.ad_shown = true;
                state.SwitchState(state.winState);
            });
        }


        public override void OnExit()
        {
            context.loose_ui.SetActive(false);

            context.pressedNext -= OnPressNext;
        }

        public void OnPressNext()
        {
            context.StartCoroutine(CR_OnPressNext());
        }

        IEnumerator CR_OnPressNext()
        {
            context.LD.ShowInterstitialAd();
            yield return new WaitForSeconds(0.01f);
            SceneManager.LoadScene("Game");
        }
    }

    public class WinState : Base
    {
        public WinState(GameContext context, GameState state) : base(context, state) { }

        public override void OnEnter()
        {
            context.btn_haptic.SetActive(false);
            context.btn_back.SetActive(false);
            List<GameObject> locked_items = context.LD.GetLockedTools();

            if (locked_items.Count > 0)
            {
                GameObject offer_one = locked_items[Random.Range(0, locked_items.Count)];
                locked_items.Remove(offer_one);
                context.offer_1_icon.sprite = offer_one.GetComponentInChildren<SpriteRenderer>().sprite;
                context.offer_1_parent.gameObject.SetActive(true);
                context.offer_1_parent.GetComponent<Button>().onClick.AddListener(() =>
                {
                    context.LD.ShowRewarded(() =>
                    {
                        context.LD.PurchaseTool(offer_one.name);
                        context.ad_shown = true;
                        context.LD.SetCurrentTool(offer_one.name);
                        state.SwitchState(state.unlockToolState);
                    });

                });
                if (locked_items.Count > 1)
                {
                    GameObject offer_two = locked_items[Random.Range(0, locked_items.Count)];
                    locked_items.Remove(offer_two);
                    context.offer_2_icon.sprite = offer_two.GetComponentInChildren<SpriteRenderer>().sprite;
                    context.offer_2_parent.gameObject.SetActive(true);

                    context.offer_2_parent.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        context.LD.ShowRewarded(() =>
                        {
                            context.LD.PurchaseTool(offer_two.name);
                            context.ad_shown = true;
                            context.LD.SetCurrentTool(offer_two.name);
                            state.SwitchState(state.unlockToolState);

                        });
                    });
                }
            }

            


            context.entire.gameObject.SetActive(false);
            context.outter_cut.gameObject.SetActive(true);
            context.inner_cut.gameObject.SetActive(true);
            context.outline_in.gameObject.SetActive(true);
            context.main_cam.DOOrthoSize(15f, 1);
            context.main_cam.transform.DOMove(context.cam_p1.position, 1).OnComplete(() =>
             {
                 context.outter_cut.DOMoveX(-20, 3);
                 context.win_ui.SetActive(true);
                 context.btn_next.SetActive(true);

             });

            context.outline_out.gameObject.SetActive(false);
            context.outline_in.gameObject.SetActive(true);

  
            context.btn_skip.SetActive(false);
            context.btn_retry.SetActive(false);

            context.pressedNext += OnPressNext;


        }

        public override void OnExit()
        {
            context.win_ui.SetActive(false);

            context.pressedNext -= OnPressNext;
        }

        public void OnPressNext()
        {
            state.SwitchState(state.unlockCandyState);
        }
    }

    public class UnlockToolState : Base
    {
        public UnlockToolState(GameContext context, GameState state) : base(context, state) { }

        public override void OnEnter()
        {
            context.btn_back.SetActive(false);
            context.btn_next.gameObject.SetActive(false);
            context.StartCoroutine(CR_Enter());
        }

        public override void OnExit()
        {
            context.unlock_tool_ui.SetActive(false);
        }

        IEnumerator CR_Enter()
        {

            context.unlock_tool_ui.SetActive(true);
            context.tool_icon.sprite = context.LD.GetCurrentTool().GetComponentInChildren<SpriteRenderer>().sprite;
            yield return new WaitForSeconds(0.3f);
            context.tool_unlock_particles.Play();
            context.presetDemoManager.PlayAHAP(2);
            yield return new WaitForSeconds(4);

            state.SwitchState(state.unlockCandyState);
        }
    }

    public class UnlockCandyState : Base
    {
        public UnlockCandyState(GameContext context, GameState state) : base(context, state) { }

        public override void OnEnter()
        {

            context.pressedNext += OnPressNext;

            List<CandyData> locked_items = context.LD.GetLockedShapes();

            if (locked_items.Count > 0)
            {
                context.unlock_ui.SetActive(true);
                context.btn_next.gameObject.SetActive(true);
                context.btn_back.SetActive(false);
                context.candy_unlock_particles.Play();
                context.presetDemoManager.PlayAHAP(2);
                CandyData choosen = locked_items[Random.Range(0, locked_items.Count)];
                PlayerPrefs.SetInt(context.LD.candy_item_key + choosen.id, 1);

                context.LD.SetCurrentShape(choosen.id);

                context.new_unlock_outter.sprite = context.GetSprite(choosen.t1);

                if (choosen.t5)
                {
                    context.new_unlock_inner.sprite = context.GetSprite(choosen.t5);
                }
                else
                {
                    context.new_unlock_inner.gameObject.SetActive(false);
                }

                context.new_unlock_fill.sprite = context.GetSprite(choosen.t4);
                context.presetDemoManager.PlayAHAP(2);
            }
            else
            {
                context.LD.SetCurrentShape("");
                OnPressNext();
            }
        }

        public override void OnExit()
        {
            context.pressedNext -= OnPressNext;
        }

        public void OnPressNext()
        {
            context.StartCoroutine(CR_Next());
        }

        IEnumerator CR_Next()
        {
            if (!context.ad_shown)
            {
                context.LD.ShowInterstitialAd();
            }

            yield return new WaitForSeconds(0.5f);

            SceneManager.LoadScene("Game");
        }

    }

}

