using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
//using Firebase.Extensions;
//using Firebase.RemoteConfig;
using UnityEngine;

namespace Beetles.Helpers
{

    public class FirebaseScript : MonoBehaviour
    {

        public static bool Available = false;

        public Dictionary<string, object> configDefaults = new Dictionary<string, object>
        {
            {"test_value", "default test value"},
            {"money_animation_speed", 1f},
        };

        /*private void Start()
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;

                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    //   app = Firebase.FirebaseApp.DefaultInstance;
                    Available = true;
                    FetchRemoteConfig();
                }
                else
                {
                    Debug.LogError(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                }
            });
        }

        private void FetchRemoteConfig()
        {
            Debug.Log("Fetch Remote Config");

            var settigns = FirebaseRemoteConfig.Settings;
            settigns.IsDeveloperMode = false;
            FirebaseRemoteConfig.Settings = settigns;

            FirebaseRemoteConfig.SetDefaults(configDefaults);

//                TimeSpan timeExpire = TimeSpan.FromHours(0);
            FirebaseRemoteConfig.FetchAsync().ContinueWithOnMainThread(RemoteConfigFetched);
        }

        private void RemoteConfigFetched(Task task)
        {
            Debug.Log("Task IsCanceled: " + task.IsCanceled);
            Debug.Log("Task IsCompleted: " + task.IsCompleted);
            Debug.Log("Task IsFaulted: " + task.IsFaulted);

            FirebaseRemoteConfig.ActivateFetched();
            
            
            Debug.Log("Remote config fetched! with test value: " + FirebaseRemoteConfig.GetValue("test_value").StringValue);
        }

        public static float GetMoneyAnimationSpeed()
        {
            return (float) FirebaseRemoteConfig.GetValue("money_animation_speed").DoubleValue;
        }
*/
    }

}