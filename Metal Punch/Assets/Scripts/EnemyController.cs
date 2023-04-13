
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    private Animator _animator;
    private float _timeLeft = 3.0f;
    public int life;
    public int maxlife;
    public int damage;
    public HealthBar health;
    // Start is called before the first frame update
    void Start()
    {
       var level= DifficultHandler.getDifficultLevel();
       Debug.Log("Level : "+level);
       switch(level){
           case 1:
                life=1200;
                maxlife=1200;
                damage=20;
            break;
           case 2:
                    life=1700;
                    maxlife=1700;
                    damage=50;
            break;
           case 3:
                    life=2000;
                    maxlife=2000;
                    damage=90;
            break;
       }
        
    }

   
	void Update () 
	{

	}
    public void loseLife(int amount){
        life-=amount;
        health.SetHealth(life);
            if(life<=0){
            Debug.Log("Enemigo muerto");
            Dead();
        }
    }
    public int getDamage(){
        return damage;
    }
    public int getLife(){
        return maxlife;
    }
    
       void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision);
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if(player!=null){
            // HERE we know that the other object we collided with is an enemy
            loseLife(player.getDamage());
            
        }
    }

    void Dead(){
        GameManagerController.SetVictory(1);
    }
}
