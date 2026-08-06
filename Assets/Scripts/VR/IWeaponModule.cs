namespace Obsidian.VR
{
    public interface IWeaponModule
    {
        bool IsFiring { get; }
        bool IsReloading { get; }
        bool IsJammed { get; }

        int GetAmmoCount();
        float GetWeaponHeat();

        void FirePrimary();
        void FireSecondary();
        void Reload();
        void CeaseFire();

        bool CanFire();
        void Fire();
    }
}
