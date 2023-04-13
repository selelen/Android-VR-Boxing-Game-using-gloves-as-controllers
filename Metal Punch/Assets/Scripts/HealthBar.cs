
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float maxlife;
    public PlayerController characterHealth;
    public EnemyController echaracterHealth;
    public Image image;
    public GameObject character;
    public int isPlayer;
    private void Start()
    {
        if(isPlayer==1){
        characterHealth = character.GetComponent<PlayerController>();
        maxlife=characterHealth.getLife();
        }else{

            echaracterHealth = character.GetComponent<EnemyController>();
            maxlife=echaracterHealth.getLife();
  
        }
        
        image.fillAmount = 1.0f;
    }

    public void SetHealth(int hp)
    {
        image.fillAmount = (1/maxlife)*(float)hp;

    }
}

