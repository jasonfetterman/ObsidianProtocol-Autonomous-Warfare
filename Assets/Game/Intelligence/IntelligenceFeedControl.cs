using UnityEngine;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class IntelligenceFeedControl : MonoBehaviour
    {
        [SerializeField]
        private GameObject surveillanceFeedDisplay;

        private void Awake()
        {
            if (surveillanceFeedDisplay == null)
                surveillanceFeedDisplay = FindDisplay();
        }

        public void ToggleFeed()
        {
            IntelligenceRuntime runtime = IntelligenceRuntime.Instance;

            if (runtime == null || runtime.SurveillanceFeeds == null)
            {
                Debug.LogWarning("[INTELLIGENCE] VIEW FEED: Surveillance feed system unavailable.");
                return;
            }

            bool anyActive = false;

            foreach (SurveillanceFeed feed in runtime.SurveillanceFeeds.GetFeeds())
            {
                if (feed.Active)
                {
                    anyActive = true;
                    break;
                }
            }

            foreach (SurveillanceFeed feed in runtime.SurveillanceFeeds.GetFeeds())
            {
                if (anyActive)
                    runtime.SurveillanceFeeds.DeactivateFeed(feed.FeedId);
                else
                    runtime.SurveillanceFeeds.ActivateFeed(feed.FeedId);
            }

            if (surveillanceFeedDisplay != null)
                surveillanceFeedDisplay.SetActive(!anyActive);

            Debug.Log(
                anyActive
                    ? "[INTELLIGENCE] VIEW FEED: Surveillance feeds CLOSED."
                    : "[INTELLIGENCE] VIEW FEED: Surveillance feeds OPEN.");
        }

        private GameObject FindDisplay()
        {
            GameObject display = GameObject.Find("SURVEILLANCE FEEDS DISPLAY");

            if (display != null)
                return display;

            display = GameObject.Find("SURVEILLANCE FEEDS FRAME");

            if (display != null)
                return display;

            return GameObject.Find("SURVEILLANCE CENTER");
        }
    }
}