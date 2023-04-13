using UnityEngine;
using System.Collections;

public class FighterAnimationDemoFREE : MonoBehaviour {
	
	public Animator _animator;

	private Transform defaultCamTransform;
	private Vector3 resetPos;
	private Quaternion resetRot;
	private GameObject cam;
	private GameObject fighter;
    private float _timeLeft;

	void Start()
	{
		cam = GameObject.FindWithTag("MainCamera");
		defaultCamTransform = cam.transform;
		resetPos = defaultCamTransform.position;
		resetRot = defaultCamTransform.rotation;
		fighter = GameObject.FindWithTag("Enemy");
		fighter.transform.position = new Vector3(0,0,0);
		_timeLeft=2.0f;
	}

	void Update () 
	{
		_timeLeft -= Time.deltaTime;

    //When time is 0
        if (_timeLeft <=0){

            // perform action
           System.Random rnd = new System.Random();

           int action = rnd.Next(1,4);
          Debug.Log("Acción"+action);

             int _timeDelay= rnd.Next(2000, 6000);

            switch(action){
				
                case 1:
				
                //number of times the enemy is going to attack
                Debug.Log("Attack");
               
                    //perform the attack
                    _animator.SetTrigger("Attack");
         
                //return to standing position

                    break;

                case 2:
                Debug.Log("Attack while defending");
                //number of seconds the enemy is going to defend itself
               
                //enter in defending position
                    _animator.SetTrigger("AttackDefend");
               
                //return to standing position
                    break;
                case 3:
                Debug.Log("Strong attack");
                   //number of seconds the enemy is going to defend itself
               
                //enter in defending position
                    _animator.SetTrigger("StrongAttack");


                //return to standing position
                    break;

            }
               

			_timeLeft=4.0f;
        }

	}
}