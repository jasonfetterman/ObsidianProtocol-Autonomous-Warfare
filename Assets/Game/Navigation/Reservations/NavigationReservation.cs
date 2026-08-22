using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Reservations
{
    public sealed class NavigationReservation : MonoBehaviour
    {
        [SerializeField] private NavigationReservationDefinition definition;

        private float expirationTime;
        private bool isReserved;

        public NavigationReservationDefinition Definition => definition;
        public bool IsReserved => isReserved;

        private void Update()
        {
            if (isReserved && UnityEngine.Time.time >= expirationTime)
            {
                Release();
            }
        }

        public bool TryReserve()
        {
            if (isReserved)
            {
                return false;
            }

            float duration =
                definition != null
                    ? definition.ReservationDuration
                    : 5f;

            expirationTime = UnityEngine.Time.time + duration;
            isReserved = true;

            return true;
        }

        public void Release()
        {
            isReserved = false;
            expirationTime = 0f;
        }
    }
}
