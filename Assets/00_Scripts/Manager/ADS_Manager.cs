using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class ADS_Manager 
{
    private bool TestMode = true;
    //public readonly string banner_Android_ID = "ca-app-pub-8778606611149783/2018576135";
    //public readonly string interstitial_Android_ID = "ca-app-pub-8778606611149783/2321828462";
    public readonly string reward_Android_ID = "ca-app-pub-8778606611149783/4756420114";


    //public readonly string banner_Android_sample = "ca-app-pub-3940256099942544/6300978111";
    //public readonly string interstitial_Android_sample = "ca-app-pub-3940256099942544/1033173712";
    public readonly string reward_Android_sample = "ca-app-pub-3940256099942544/5224354917";


    BannerView _banner; // 배너 광고
    InterstitialAd _interstitialAD; // 전면 광고
    RewardedAd _rewardedAD; // 보상형 광고
    AdRequest _adRequest;
    Action _rewardedCallBack;


    public void Init()
    {
        MobileAds.Initialize(initStatus => { });
        PrepareADS();
    }

    private void PrepareADS()
    {
        string reward;

        if(TestMode)
        {
            reward = reward_Android_sample;
        }
        else
        {

            reward = reward_Android_ID;
        }

        _adRequest = new AdRequest();
        _adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(reward, _adRequest, OnAdRewardCallBack);
    }

    private void OnAdRewardCallBack(RewardedAd ad, LoadAdError error)
    {
        if(error != null || ad == null)
        {
            Debug.LogError("보상형 광고 준비 실패 : " + error);
            return;
        }
        
        Debug.Log("보상형 광고 준비 성공" + ad.GetResponseInfo());
        _rewardedAD = ad;
        RegisterEventHandlers(_rewardedAD);
        
    }

    private void RegisterEventHandlers(RewardedAd ad) // 광고 세팅
    {
        ad.OnAdFullScreenContentClosed += () => 
        {
            Debug.Log("보상형 광고 닫힘");
            PrepareADS();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("보상형 광고 실패 : " + error);
            PrepareADS();
        };

        ad.OnAdPaid += (AdValue adValue) => 
        {
            if(_rewardedCallBack != null)
            {
                _rewardedCallBack?.Invoke();
                _rewardedCallBack = null;
            }
        };
    }

    public void ShowRewardedAds(Action rewardCallBack)
    {
        _rewardedCallBack = rewardCallBack;
        if(_rewardedAD != null && _rewardedAD.CanShowAd())
        {
            _rewardedAD.Show((Reward reward) =>
            {
                if(_rewardedCallBack != null)
                {
                    Debug.Log(reward.Type + " : " + reward.Amount);

                    if(_rewardedCallBack != null) // 보상 오류 있는 경우 일단 지급 판정 -> 출시 후에 빌드 테스트시 제거하기
                    {
                        _rewardedCallBack?.Invoke();
                        _rewardedCallBack = null;
                    }
                }
            });
        }
        else 
        {
            Debug.Log("준비된 광고가 없음"); // 추후 팝업창 여기 구현
            PrepareADS();
        }
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

