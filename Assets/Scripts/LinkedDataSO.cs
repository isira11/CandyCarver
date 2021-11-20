using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using GoogleMobileAds.Api;
using System;

[CreateAssetMenu(menuName = "LinkedDataSO", order = 1)]
public class LinkedDataSO : ScriptableObject
{

    public ToolsSO toolsSO;
    public ShapesSO shapesSO;

    public string ad_key = "CANDY_ADS:";
    public string candy_item_key = "CANDY_ITEM:";
    public string tool_item_key = "TOOL_ITEM:";
    public string level_key = "LEVEL:";

    public string current_tool_key = "CURRENT_TOOL";

    public string current_shape_key = "CURRENT_SHAPE";

    public string DEFAULT = "DEFAULT";


    public Dictionary<string, GameObject> tools { get; private set; }

    public Dictionary<string, CandyData> shapes { get; private set; }


    public void Init()
    {
        InitAds();

        tools = new Dictionary<string, GameObject>();
        shapes = new Dictionary<string, CandyData>();

        foreach (GameObject item in toolsSO.tools)
        {
            tools.Add(item.name, item);
        }

        foreach (CandyData item in shapesSO.candyDatas)
        {
            shapes.Add(item.id, item);
        }


        PurchaseTool(DEFAULT);
        PurchaseShape(DEFAULT);
        PurchaseTool("tool_1");



        Debug.Log("Init");

    }

    public GameObject GetCurrentTool()
    {
        string key = PlayerPrefs.GetString(current_tool_key, DEFAULT);


        return tools[key];
    }

    public void SetCurrentTool(string id)
    {
        PlayerPrefs.SetString(current_tool_key, id);
    }

    public CandyData GetCurrentShape()
    {
        string key = PlayerPrefs.GetString(current_shape_key, DEFAULT);

        if (shapes.ContainsKey(key))
        {
            return shapes[key];
        }

        return null;
    }

    public void SetCurrentShape(string id)
    {
        PlayerPrefs.SetString(current_shape_key, id);
    }


    public List<GameObject> GetLockedTools()
    {
        List<GameObject> locked_items = new List<GameObject>();

        foreach (KeyValuePair<string, GameObject> item in tools)
        {
            if (!IsToolPurchase(item.Key))
            {
                locked_items.Add(item.Value);
            }
        }

        return locked_items;
    }

    public List<CandyData> GetLockedShapes()
    {
        List<CandyData> locked_items = new List<CandyData>();

        foreach (KeyValuePair<string, CandyData> item in shapes)
        {
            if (!IsShapePurchase(item.Key))
            {
                locked_items.Add(item.Value);
            }
        }

        return locked_items;
    }


    public bool IsShapePurchase(string id)
    {
        return IsPurchased(candy_item_key + id);
    }

    public bool IsToolPurchase(string id)
    {
        return IsPurchased(tool_item_key + id);
    }

    private bool IsPurchased(string id)
    {
        return PlayerPrefs.GetInt(id,0) == 1;
    }

    public void PurchaseShape(string id)
    {
        PurchaseItem(candy_item_key + id);
    }

    public void PurchaseTool(string id)
    {
        PurchaseItem(tool_item_key + id);
    }

    private void PurchaseItem(string id)
    {
        PlayerPrefs.SetInt(id, 1);
    }




    //ADS
    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    //REAL
    string banner_id = "ca-app-pub-2059337027337193/9948421528";
    string interstitial_id = "ca-app-pub-2059337027337193/4934788057";
    string rewarded_id = "ca-app-pub-2059337027337193/6006334750";

    //TEST
    //string banner_id = "ca-app-pub-3940256099942544/6300978111";
    //string interstitial_id = "ca-app-pub-3940256099942544/1033173712";
    //string rewarded_id = "ca-app-pub-3940256099942544/5224354917";

    private Action RewardedAction;

    public void InitAds()
    {
        MobileAds.Initialize((initStatus) => {
            RequestBanner();
            RequestInterstitial();
            RequestRewarded();
        });


    }

    private void RequestBanner()
    {
        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        bannerView = new BannerView(banner_id, adaptiveSize, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();

        bannerView.LoadAd(request);
    }

    private void RequestInterstitial()
    {
        AdRequest request = new AdRequest.Builder().Build();
        interstitialAd = new InterstitialAd(interstitial_id);
        interstitialAd.LoadAd(request);
    }

    private void RequestRewarded()
    {

        rewardedAd = new RewardedAd(rewarded_id);

        AdRequest request = new AdRequest.Builder().Build();

        rewardedAd.LoadAd(request);

        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    }

    private void HandleUserEarnedReward(object sender, Reward e)
    {
        if (RewardedAction != null)
        {
            RewardedAction();
        }
    }

    public void ShowBanner()
    {
        if (bannerView != null)
        {
            bannerView.Show();
        }
    }

    public void ShowRewarded(Action action = null)
    {
        if (rewardedAd.IsLoaded())
        {
            RewardedAction = action;
            rewardedAd.Show();
            RequestRewarded();
        }
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
            RequestInterstitial();

        }
    }


}
