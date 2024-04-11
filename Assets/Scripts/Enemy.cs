using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;

    public int hp;
    int max_hp;
    bool isDie;
    float dieTime = 1.2f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isDie = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
           
            animator.SetBool("Die",true);
            dieTime-= Time.deltaTime;
            if (dieTime < 0)
            {
                Destroy(gameObject);
            }
            
            
            
            
        }
    }

    void ApplyDamage()
    {
        hp -= 2;

        //Destroy(gameObject);
    }
}
