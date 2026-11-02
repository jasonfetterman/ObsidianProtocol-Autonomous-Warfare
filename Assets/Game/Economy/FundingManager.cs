using UnityEngine;

namespace ObsidianProtocol.Game.Economy
{
    public sealed class FundingManager : MonoBehaviour
    {
        private const string FundingKey = "OPAW_SHARED_FUNDING";
        private const int StartingFunding = 84250;

        public static FundingManager Instance { get; private set; }

        public int CurrentFunding { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            CurrentFunding = PlayerPrefs.GetInt(
                FundingKey,
                StartingFunding);
        }

        public bool TrySpend(int amount)
        {
            if (amount <= 0)
                return true;

            if (CurrentFunding < amount)
                return false;

            CurrentFunding -= amount;
            Save();
            return true;
        }

        public void AddFunding(int amount)
        {
            if (amount <= 0)
                return;

            CurrentFunding += amount;
            Save();
        }

        private void Save()
        {
            PlayerPrefs.SetInt(
                FundingKey,
                CurrentFunding);

            PlayerPrefs.Save();
        }
    }
}
