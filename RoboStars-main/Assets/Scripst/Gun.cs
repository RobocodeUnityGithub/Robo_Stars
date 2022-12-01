using UnityEngine;
public abstract class Gun : Item
{
    public GameObject bulletPrefab;
    public GameObject shootFX;
    public Animator gunAnimator;
    public Transform muzzle;
    public int maxAmmo;
    public abstract override void Use();
}
