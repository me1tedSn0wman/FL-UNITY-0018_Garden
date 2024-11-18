using UnityEngine;
#if PLATFORM_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

namespace GPControl
{
    public class GPAuthentication : MonoBehaviour
    {
#if PLATFORM_ANDROID
        public static bool Authenticated { get; private set; }

        /// Uncomment when app would have been connected to google services
        /*
        public static PlayGamesPlatform Platform { get; private set; }
        */

        private void Start()
        {
            Login();
        }

        /// <summary>
        /// Try login in Google Play
        /// </summary>
        private void Login()
        {
            Authenticated = false;
            OnAuthenticationSucceded();

            /// Uncomment when app would have been connected to google services
            /*
            
            /// Create Google Play platform 
            if (Platform == null)
            {
                Platform = buildPlatform();
            }

            /// Try to login in Google play
            PlayGamesPlatform.Instance.Authenticate(success =>
            {
                Authenticated = success;
                OnAuthenticationSucceded();
            });
            */
        }

        /// <summary>
        /// Behaviour, when we login (or not)
        /// </summary>
        private void OnAuthenticationSucceded()
        {

            if (Authenticated)
            {
                GPCloudSaveManager.Instance.Load();

            }
            else
            {
                GPCloudSaveManager.Instance.LoadLocal();
                GPCloudSaveManager.Instance.isOnlyLocal = true;
            }

        }

        /// Uncomment when app would have been connected to google services
        /*
        /// Create Google Play Platform
        private PlayGamesPlatform buildPlatform() {
            var builder = new PlayGamesClientConfiguration.Builder();
            /// Turns on Google Play cloud Saves
            builder.EnableSavedGames();                             

            PlayGamesPlatform.InitializeInstance(builder.Build());
            PlayGamesPlatform.DebugLogEnabled = true;

            return PlayGamesPlatform.Activate();
        }
        */
#endif
    }
}
