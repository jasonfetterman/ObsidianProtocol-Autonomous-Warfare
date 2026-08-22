using System;

namespace ObsidianProtocol.Game.Store
{
    public sealed class StoreUIState
    {
        public bool Visible { get; private set; }

        public string SelectedCategory { get; private set; }
        public string SelectedItemId { get; private set; }

        public int CreditBalance { get; private set; }

        public string StatusMessage { get; private set; }

        public bool PurchaseAvailable
        {
            get;
            private set;
        }

        public StoreUIState()
        {
            SelectedCategory = string.Empty;
            SelectedItemId = string.Empty;
            StatusMessage = string.Empty;

            CreditBalance = 0;
            PurchaseAvailable = false;
        }

        public void Show()
        {
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
        }

        public void SetCreditBalance(
            int balance)
        {
            CreditBalance =
                Math.Max(0, balance);
        }

        public void SelectCategory(
            string category)
        {
            SelectedCategory =
                category ?? string.Empty;

            SelectedItemId =
                string.Empty;

            PurchaseAvailable = false;
        }

        public void SelectItem(
            string itemId)
        {
            SelectedItemId =
                itemId ?? string.Empty;
        }

        public void SetPurchaseAvailable(
            bool available)
        {
            PurchaseAvailable =
                available;
        }

        public void SetStatus(
            string message)
        {
            StatusMessage =
                message ?? string.Empty;
        }

        public void ClearSelection()
        {
            SelectedCategory = string.Empty;
            SelectedItemId = string.Empty;
            PurchaseAvailable = false;
        }

        public void Reset()
        {
            Visible = false;

            SelectedCategory = string.Empty;
            SelectedItemId = string.Empty;

            CreditBalance = 0;

            StatusMessage = string.Empty;
            PurchaseAvailable = false;
        }
    }
}
