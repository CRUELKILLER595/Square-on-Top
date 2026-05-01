using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("AdMob Initialized");
        });
    }
}