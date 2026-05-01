using UnityEngine;
using GoogleMobileAds.Api; 
using System;

public class AdAutoTester : MonoBehaviour
{
    private RewardedAd rewardedAd;

    string adUnitId = "ca-app-pub-3940256099942544/5224354917"; // TEST ID

    float timer = 0f;
    float adInterval = 10f; // every 10 seconds

    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadAd();
        });
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= adInterval)
        {
            ShowAd();
            timer = 0f;
        }
    }

    public void LoadAd()
    {
        AdRequest request = new AdRequest();

        RewardedAd.Load(adUnitId, request,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.Log("Ad failed to load: " + error);
                    return;
                }

                rewardedAd = ad;
                Debug.Log("Ad loaded");

                rewardedAd.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("Ad closed → loading next");
                    LoadAd(); // reload next ad
                };
            });
    }

    public void ShowAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("User watched ad");
            });

            rewardedAd = null; // prevent reuse
        }
        else
        {
            Debug.Log("Ad not ready yet");
        }
    }
}