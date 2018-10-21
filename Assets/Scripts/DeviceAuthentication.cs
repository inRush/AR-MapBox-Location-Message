namespace Mapbox.Unity.Ar
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class DeviceAuthentication : MonoBehaviour
    {
        public static DeviceAuthentication Instance { get; set; }

        public bool DeviceAuthenticated { get; private set; }

        /// <summary>
        /// Authenticates our device with GameSparks. Once authenticated, we show the UI
        /// and load initial messages. 
        /// </summary>
        void Start()
        {
            Instance = this;
            StartCoroutine(DelayAuthenticateRoutine());
        }

        IEnumerator DelayAuthenticateRoutine()
        {

            yield return new WaitForSeconds(1f);
            XLogger.Info("开始授权....");
            new GameSparks.Api.Requests.DeviceAuthenticationRequest().Send((response) =>
            {
                if (!response.HasErrors)
                {
                    XLogger.Info("授权成功!");
                    //tell message provider we have been authenticated
                    DeviceAuthenticated = true;
                }
                else
                {
                    XLogger.Info("授权失败!");

                }
            });
        }
    }
}
