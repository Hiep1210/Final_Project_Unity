using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    public List<GameObject> gunWeaponList;

    private bool _isPistolGunActive;
    private bool _isMachineGunActive;
    private bool _isShotGunActive;

    public bool IsPistolGunActive { get => _isPistolGunActive; set => _isPistolGunActive = value; }
    public bool IsMachineGunActive { get => _isMachineGunActive; set => _isMachineGunActive = value; }
    public bool IsShotGunActive { get => _isShotGunActive; set => _isShotGunActive = value; }

    public override void Awake()
    {
        MakeSingleton(false);

        _isPistolGunActive = true;
        _isMachineGunActive = false;
        _isShotGunActive = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (gunWeaponList == null || gunWeaponList.Count < 0) return;

        if (GamepadController.Ins.IsPistolGun && !GamepadController.Ins.IsMachineGun && !GamepadController.Ins.IsShotGun)
        {
            if (_isPistolGunActive)
            {
                gunWeaponList[0].gameObject.SetActive(true);
                gunWeaponList[1].gameObject.SetActive(false);
                gunWeaponList[2].gameObject.SetActive(false);
            }
        }
        else if (!GamepadController.Ins.IsPistolGun && GamepadController.Ins.IsMachineGun && !GamepadController.Ins.IsShotGun)
        {
            if (_isMachineGunActive)
            {
                gunWeaponList[0].gameObject.SetActive(false);
                gunWeaponList[1].gameObject.SetActive(true);
                gunWeaponList[2].gameObject.SetActive(false);
            }
        }
        else if (!GamepadController.Ins.IsPistolGun && !GamepadController.Ins.IsMachineGun && GamepadController.Ins.IsShotGun)
        {
            if (_isShotGunActive)
            {
                gunWeaponList[0].gameObject.SetActive(false);
                gunWeaponList[1].gameObject.SetActive(false);
                gunWeaponList[2].gameObject.SetActive(true);
            }
        }
    }
}
