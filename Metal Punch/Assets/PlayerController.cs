
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public int life=1000;
    public int maxlife=1000;
    public HealthBar health;
    int damage=25;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       }

    void OnTriggerEnter(Collider collision)
    {
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        Debug.Log(collision.gameObject);
  
        if(enemy!=null){
            // HERE we know that the other object we collided with is an enemy
            loseLife(enemy.getDamage());
        }
    }
   public int getDamage(){
       return damage;
   }
   public int getLife(){
       return maxlife;
   }
    void loseLife(int amount){
        Debug.Log("vida"+amount);
        life-=amount;
        health.SetHealth(life);
         if (life<=0){
            Debug.Log("Dead");
            Dead();
        }
    }
    void Dead(){
        GameManagerController.SetVictory(0);
    }
}
