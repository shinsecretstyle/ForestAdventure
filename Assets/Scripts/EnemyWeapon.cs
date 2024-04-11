using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    EnemyAiLite EnemyAi;


    // Start is called before the first frame update
    void Start()
    {
        EnemyAi = GetComponentInParent<EnemyAiLite>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //private void OnTriggerEnter(Collider collider)
    //{
    //    if (collider.gameObject.CompareTag("Player")&&EnemyAi.isAttacking)
    //    {
    //        collider.gameObject.SendMessage("ApplyDamage");

    //    }
        
    //}


    
}
