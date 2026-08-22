using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Reservations
{
    [CreateAssetMenu(
        fileName = "NavigationReservationDefinition",
        menuName = "Obsidian Protocol/Navigation/Navigation Reservation Definition")]
    public sealed class NavigationReservationDefinition : ScriptableObject
    {
        [SerializeField] private float reservationDuration = 5f;
        [SerializeField] private float reservationRadius = 2f;

        public float ReservationDuration =>
            Mathf.Max(0.1f, reservationDuration);

        public float ReservationRadius =>
            Mathf.Max(0.1f, reservationRadius);
    }
}
