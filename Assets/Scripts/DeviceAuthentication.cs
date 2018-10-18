namespace Mapbox.Unity.Ar
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class DeviceAuthentication : MonoBehaviour
    {
        /// <summary>
        /// Authenticates our device with GameSparks. Once authenticated, we show the UI
        /// and load initial messages. 
        /// </summary>
        void Start()
        {

            StartCoroutine(DelayAuthenticateRoutine());
        }

        IEnumerator DelayAuthenticateRoutine()
        {

            yield return new WaitForSeconds(1f);
            Unity.Utilities.Console.Instance.Log("here", "lightblue");
            new GameSparks.Api.Requests.DeviceAuthenticationRequest().Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Unity.Utilities.Console.Instance.Log("Device Authenticated...", "lightblue");
                    Debug.Log("Device Authenticated...");

                    //tell message provider we have been authenticated
                    ARMessageProvider.Instance.deviceAuthenticated = true;
                    StartCoroutine(DelayLoadAllMessage());
                }
                else
                {
                    Unity.Utilities.Console.Instance.Log("Error Authenticating Device...", "lightblue");
                    Debug.Log("Error Authenticating Device...");
                }
            });
        }

        IEnumerator DelayLoadAllMessage()
        {
            yield return new WaitForSeconds(1f);
            MessageService.Instance.LoadAllMessageNotSet();
        }
    }
}
