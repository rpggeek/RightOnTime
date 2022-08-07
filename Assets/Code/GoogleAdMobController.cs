using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GoogleAdMobController : MonoBehaviour
{
    public static GoogleAdMobController instance;

    [SerializeField] string banner, interstitial, rewarded;

    private AppOpenAd appOpenAd;
    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private RewardedInterstitialAd rewardedInterstitialAd;

    private bool isShowingAppOpenAd;

    public UnityEvent OnAdLoadedEvent;
    public UnityEvent OnAdFailedToLoadEvent;
    public UnityEvent OnAdOpeningEvent;
    public UnityEvent OnAdFailedToShowEvent;
    public UnityEvent OnUserEarnedRewardEvent;
    public UnityEvent OnAdClosedEvent;


    #region UNITY MONOBEHAVIOR METHODS

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        //MobileAds.SetiOSAppPauseOnBackground(true);
        MobileAds.Initialize(HandleInitCompleteAction);
    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        Debug.Log("Initialization complete.");

        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // the main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            RequestBannerAd();
            RequestAndLoadInterstitialAd();
            //RequestAndLoadRewardedAd();
        });
    }

    #endregion

    #region HELPER METHODS

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();
    }

    public void OnApplicationPause(bool paused)
    {
        // Display the app open ad when the app is foregrounded.
        if (!paused)
        {
            ShowAppOpenAd();
        }
    }

    #endregion

    #region BANNER ADS

    public void RequestBannerAd()
    {
        PrintStatus("Requesting Banner ad.");

        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(banner, AdSize.Banner, AdPosition.Bottom);

        // Add Event Handlers
        bannerView.OnAdLoaded += (sender, args) =>
        {
            PrintStatus("Banner ad loaded.");
            OnAdLoadedEvent.Invoke();
        };
        bannerView.OnAdFailedToLoad += (sender, args) =>
        {
            PrintStatus("Banner ad failed to load with error: " + args.LoadAdError.GetMessage());
            RequestBannerAd();
            OnAdFailedToLoadEvent.Invoke();
        };
        bannerView.OnAdOpening += (sender, args) =>
        {
            PrintStatus("Banner ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        bannerView.OnAdClosed += (sender, args) =>
        {
            PrintStatus("Banner ad closed.");
            OnAdClosedEvent.Invoke();
        };
        bannerView.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Banner ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            PrintStatus(msg);
        };

        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    #endregion

    #region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd()
    {
        PrintStatus("Requesting Interstitial ad.");

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        interstitialAd = new InterstitialAd(interstitial);

        // Add Event Handlers
        interstitialAd.OnAdLoaded += (sender, args) =>
        {
            PrintStatus("Interstitial ad loaded.");
            OnAdLoadedEvent.Invoke();
        };
        interstitialAd.OnAdFailedToLoad += (sender, args) =>
        {
            PrintStatus("Interstitial ad failed to load with error: " + args.LoadAdError.GetMessage());
            RequestAndLoadInterstitialAd();
            OnAdFailedToLoadEvent.Invoke();
        };
        interstitialAd.OnAdOpening += (sender, args) =>
        {
            PrintStatus("Interstitial ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        interstitialAd.OnAdClosed += (sender, args) =>
        {
            PrintStatus("Interstitial ad closed.");
            RequestAndLoadInterstitialAd();
            OnAdClosedEvent.Invoke();
        };
        interstitialAd.OnAdDidRecordImpression += (sender, args) =>
        {
            PrintStatus("Interstitial ad recorded an impression.");
        };
        interstitialAd.OnAdFailedToShow += (sender, args) =>
        {
            PrintStatus("Interstitial ad failed to show.");
        };
        interstitialAd.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Interstitial ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            PrintStatus(msg);
        };

        // Load an interstitial ad
        interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            PrintStatus("Interstitial ad is not ready yet.");
        }
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }

    #endregion

    #region REWARDED ADS

    public void RequestAndLoadRewardedAd()
    {
        PrintStatus("Requesting Rewarded ad.");

        // create new rewarded ad instance
        rewardedAd = new RewardedAd(rewarded);

        // Add Event Handlers
        rewardedAd.OnAdLoaded += (sender, args) =>
        {
            PrintStatus("Reward ad loaded.");
            OnAdLoadedEvent.Invoke();
        };
        rewardedAd.OnAdFailedToLoad += (sender, args) =>
        {
            PrintStatus("Reward ad failed to load.");
            RequestAndLoadRewardedAd();
            OnAdFailedToLoadEvent.Invoke();
        };
        rewardedAd.OnAdOpening += (sender, args) =>
        {
            PrintStatus("Reward ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        rewardedAd.OnAdFailedToShow += (sender, args) =>
        {
            PrintStatus("Reward ad failed to show with error: " + args.AdError.GetMessage());
            OnAdFailedToShowEvent.Invoke();
        };
        rewardedAd.OnAdClosed += (sender, args) =>
        {
            PrintStatus("Reward ad closed.");
            OnAdClosedEvent.Invoke();
        };
        rewardedAd.OnUserEarnedReward += (sender, args) =>
        {
            PrintStatus("User earned Reward ad reward: " + args.Amount);
            OnUserEarnedRewardEvent.Invoke();
        };
        rewardedAd.OnAdDidRecordImpression += (sender, args) =>
        {
            PrintStatus("Reward ad recorded an impression.");
        };
        rewardedAd.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Rewarded ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            PrintStatus(msg);
        };

        // Create empty ad request
        rewardedAd.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Show();
        }
        else
        {
            PrintStatus("Rewarded ad is not ready yet.");
        }
    }

    public void RequestAndLoadRewardedInterstitialAd()
    {
        PrintStatus("Requesting Rewarded Interstitial ad.");

        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/5354046379";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/6978759866";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create an interstitial.
        RewardedInterstitialAd.LoadAd(adUnitId, CreateAdRequest(), (rewardedInterstitialAd, error) =>
        {
            if (error != null)
            {
                PrintStatus("Rewarded Interstitial ad load failed with error: " + error);
                return;
            }

            this.rewardedInterstitialAd = rewardedInterstitialAd;
            PrintStatus("Rewarded Interstitial ad loaded.");

            // Register for ad events.
            this.rewardedInterstitialAd.OnAdDidPresentFullScreenContent += (sender, args) =>
            {
                PrintStatus("Rewarded Interstitial ad presented.");
            };
            this.rewardedInterstitialAd.OnAdDidDismissFullScreenContent += (sender, args) =>
            {
                PrintStatus("Rewarded Interstitial ad dismissed.");
                this.rewardedInterstitialAd = null;
            };
            this.rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
            {
                PrintStatus("Rewarded Interstitial ad failed to present with error: " +
                                                                        args.AdError.GetMessage());
                this.rewardedInterstitialAd = null;
            };
            this.rewardedInterstitialAd.OnPaidEvent += (sender, args) =>
            {
                string msg = string.Format("{0} (currency: {1}, value: {2}",
                                            "Rewarded Interstitial ad received a paid event.",
                                            args.AdValue.CurrencyCode,
                                            args.AdValue.Value);
                PrintStatus(msg);
            };
            this.rewardedInterstitialAd.OnAdDidRecordImpression += (sender, args) =>
            {
                PrintStatus("Rewarded Interstitial ad recorded an impression.");
            };
        });
    }

    public void ShowRewardedInterstitialAd()
    {
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Show((reward) =>
            {
                PrintStatus("Rewarded Interstitial ad Rewarded : " + reward.Amount);
            });
        }
        else
        {
            PrintStatus("Rewarded Interstitial ad is not ready yet.");
        }
    }

    #endregion

    #region APPOPEN ADS

    public void RequestAndLoadAppOpenAd()
    {
        PrintStatus("Requesting App Open ad.");
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/3419835294";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/5662855259";
#else
        string adUnitId = "unexpected_platform";
#endif
        // create new app open ad instance
        AppOpenAd.LoadAd(adUnitId, ScreenOrientation.Portrait, CreateAdRequest(), (appOpenAd, error) =>
        {
            if (error != null)
            {
                PrintStatus("App Open ad failed to load with error: " + error);
                return;
            }

            PrintStatus("App Open ad loaded. Please background the app and return.");
            this.appOpenAd = appOpenAd;
        });
    }

    public void ShowAppOpenAd()
    {
        if (isShowingAppOpenAd)
        {
            return;
        }
        if (appOpenAd == null)
        {
            return;
        }

        // Register for ad events.
        this.appOpenAd.OnAdDidDismissFullScreenContent += (sender, args) =>
        {
            PrintStatus("App Open ad dismissed.");
            isShowingAppOpenAd = false;
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                if (this.appOpenAd != null)
                {
                    this.appOpenAd.Destroy();
                    this.appOpenAd = null;
                }
            });
        };
        this.appOpenAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
        {
            PrintStatus("App Open ad failed to present with error: " + args.AdError.GetMessage());

            isShowingAppOpenAd = false;
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                if (this.appOpenAd != null)
                {
                    this.appOpenAd.Destroy();
                    this.appOpenAd = null;
                }
            });
        };
        this.appOpenAd.OnAdDidPresentFullScreenContent += (sender, args) =>
        {
            PrintStatus("App Open ad opened.");
            isShowingAppOpenAd = true;
        };
        this.appOpenAd.OnAdDidRecordImpression += (sender, args) =>
        {
            PrintStatus("App Open ad recorded an impression.");
        };
        this.appOpenAd.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "App Open ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            PrintStatus(msg);
        };
        appOpenAd.Show();
    }

    #endregion


    #region AD INSPECTOR

    public void OpenAdInspector()
    {
        PrintStatus("Open ad Inspector.");

        MobileAds.OpenAdInspector((error) =>
        {
            if (error != null)
            {
                PrintStatus("ad Inspector failed to open with error: " + error);
            }
            else
            {
                PrintStatus("Ad Inspector opened successfully.");
            }
        });
    }

    #endregion

    #region Utility
    private void PrintStatus(string message)
    {
        Debug.Log(message);
    }
    #endregion
}