using System;
using System.Security.Cryptography;
using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public bool isFiring;
    public float fireRate;
    public int magazineSize;
    public float reloadTime;
    public bool isReloading;
    private float _timeBetweenShots;
    private float _counter;
    private float _reloadTimer;
    private int _currentAmmo;

    public BulletController bullet;
    
    public Collider weaponCollider;
    
    public Collider bulletCollider;
    
    public Transform bulletPoint;
    
    public GameObject muzzleFlash;
    
    public Transform muzzlePoint;
    
    public GameObject muzzleLight;

    private void Start() {
        // calculate the time to fire
        _timeBetweenShots = 1 / fireRate;
        _counter = _timeBetweenShots;
        _currentAmmo = magazineSize;
    }

    private void Update() {
        // Decrement the counter by the amount of time that has passed
        _counter -= Time.deltaTime;

        if (_currentAmmo <= 0 || Input.GetKey(KeyCode.R)) {
            isFiring = false;
            Reload();
            return;
        }

        // If the counter is 0 or less, the player can shoot again
        if (!isFiring ) return;
        if(!(_counter <= 0)) return;


        // Reset the counter to the time between shots
        _counter = _timeBetweenShots;
        if (_reloadTimer > 0) {
            return;
        }

        _currentAmmo--;

        // Instantiate a new bullet
        Instantiate(bullet, bulletPoint.transform.position, bulletPoint.rotation);
        GameObject flash = Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
        GameObject light = Instantiate(muzzleLight, muzzlePoint.position, muzzlePoint.rotation);
        Destroy(flash, 0.05f);
        Destroy(light, 0.05f);

        // disable collisions
        Physics.IgnoreCollision(bulletCollider, weaponCollider);
    }

    private void Reload() {
        if (isReloading) return;
        isReloading = true;
        _reloadTimer = reloadTime;
        StartCoroutine(TextTracker.instance.Reload(_reloadTimer));
        StartCoroutine(ReloadTimer());
    }

    private IEnumerator ReloadTimer() {
        while (_reloadTimer > 0) {
            _reloadTimer -= Time.deltaTime;
            yield return null;
        }

        _currentAmmo = magazineSize;
        isReloading = false;
    }

    public void CancelReload() {
        isReloading = false;
        _reloadTimer = 0;
        TextTracker.instance.CancelReload();
        StopCoroutine(ReloadTimer());
    }
}

