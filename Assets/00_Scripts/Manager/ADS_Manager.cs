using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class ADS_Manager 
{
    private bool TestMode = true;
    public readonly string banner_Android_ID = "ca-app-pub-8778606611149783/2018576135";
    public readonly string interstitial_Android_ID = "ca-app-pub-8778606611149783/2321828462";
    public readonly string reward_Android_ID = "ca-app-pub-8778606611149783/4756420114";


    public readonly string banner_Android_sample = "ca-app-pub-3940256099942544/6300978111";
    public readonly string interstitial_Android_sample = "ca-app-pub-3940256099942544/1033173712";
    public readonly string reward_Android_sample = "ca-app-pub-3940256099942544/5224354917";


    BannerView _banner; // 배너 광고
    InterstitialAd _interstitialAD; // 전면 광고
    RewardedAd _rewardedAD; // 보상형 광고
    AdRequest _adRequest;


    public void Init()
    {
        MobileAds.Initialize(initStatus => { });
        PrepareADS();
    }

    private void PrepareADS()
    {
        string banner;
        string interstitial;
        string reward;

        if(TestMode)
        {
            banner = banner_Android_sample;
            interstitial = interstitial_Android_sample;
            reward = reward_Android_sample;
        }
        else
        {
            banner = banner_Android_ID;
            interstitial = interstitial_Android_ID;
            reward = reward_Android_ID;
        }

        _adRequest = new AdRequest();
        _adRequest.Keywords.Add("unity-admob-sample");

        BannerView(banner);
    }

    public void BannerView(string banner_id)
    {
        if(_banner != null)
        {
            _banner.Destroy();
            _banner = null;
        }

        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth); // 배너 사이즈 설정

        _banner = new BannerView(banner_id, adaptiveSize, AdPosition.Bottom);

        _banner.LoadAd(_adRequest);

    }

   
}
