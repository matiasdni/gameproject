using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedWeaponManager : MonoBehaviour {
    
    public WeaponController[] weapons;
    public int currentWeaponIndex = 0;

    public SPlayerController playerController;
    // Start is called before the first frame update
    private void Start() {
        weapons = GetComponentsInChildren<WeaponController>();
        weapons[1].gameObject.SetActive(false);
    }

    public WeaponController SwitchWeapon(int index) {
        if (index == currentWeaponIndex) {
            return weapons[currentWeaponIndex];
        }
        
        if(weapons[currentWeaponIndex].isReloading) {
            weapons[currentWeaponIndex].CancelReload();
        }
        
        weapons[currentWeaponIndex].gameObject.SetActive(false);
        weapons[index].gameObject.SetActive(true);
        currentWeaponIndex = index;

        return weapons[index];
    }

    public WeaponController GetWeapon() {
        return weapons[currentWeaponIndex];
    }
}
