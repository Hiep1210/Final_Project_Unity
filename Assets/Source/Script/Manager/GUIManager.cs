using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager>
{
    [Header("Info User - UI - Component: ")]
    public GameObject pistolGunUI;
    public GameObject machineGunUI;
    public GameObject shotGunUI;
    public Text pistolCountBulletTxt;
    public Text machineCountBulletTxt;
    public Text shotCountBulletTxt;
    public Image hpBar;
    public Image expBar;
    public Text playTimeTxt;
    public Text scoreTxt;
    public Text coinTxt;
    public Text countKillEnemyTxt;


    [Header("Component Referent: ")]
    public GameObject pistolGun;
    public GameObject machineGun;
    public GameObject shotGun;


    public override void Awake()
    {
        MakeSingleton(false);
    }

    private void Update()
    {
        InfoUserUI();
    }

    private void InfoUserUI()
    {
        if(pistolGun != null && machineGun != null !&& shotGun != null)
        {
            if(pistolGun.activeSelf)
            {
                pistolGunUI.SetActive(true);
                machineGunUI.SetActive(false);
                shotGunUI.SetActive(false);

                PistolGun pistolG = pistolGun.GetComponent<PistolGun>();
                pistolCountBulletTxt.text = "x" + pistolG.CountBullet.ToString();
            }
            else if (machineGun.activeSelf)
            {
                machineGunUI.SetActive(true);
                pistolGunUI.SetActive(false);
                shotGunUI.SetActive(false); 
                
                MachineGun machineG = machineGun.GetComponent<MachineGun>();
                machineCountBulletTxt.text = "x" + machineG.CountBullet.ToString();
            }
            else if (shotGun.activeSelf)
            {
                shotGunUI.SetActive(true);
                machineGunUI.SetActive(false);
                pistolGunUI.SetActive(false);

                ShotGun shotG = shotGun.GetComponent<ShotGun>();
                shotCountBulletTxt.text = "x" + shotG.CountBullet.ToString();
            }
        }


        TimeSpan timeSpan = TimeSpan.FromSeconds(GameManager.Ins.TimePlaying);
        string playTimeFormatted = "";
        if (GameManager.Ins.TimePlaying >= 3600f)
        {
            playTimeFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
        else
        {
            playTimeFormatted = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
        playTimeTxt.text = playTimeFormatted;


        scoreTxt.text = GameManager.Ins.Score.ToString();

        countKillEnemyTxt.text = "X" + GameManager.Ins.CountKillEnemy.ToString();

        coinTxt.text = "X" + Math.Ceiling((double)GameManager.Ins.Coin).ToString();

        float curHp = GameManager.Ins.Player.CurrentHp / GameManager.Ins.Player.MaxHp;
        hpBar.fillAmount = curHp;

        float curLevel = GameManager.Ins.CurExpressInLevels / GameManager.Ins.RequireExpressToNextLevel;
        expBar.fillAmount = curLevel;         
    }
}
