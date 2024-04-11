using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //PlayerCtrl playerCtrl;
    int atk;
    // Start is called before the first frame update
    void Start()
    {
        atk = 10;
        //playerCtrl = GetComponentInParent<PlayerCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            collider.gameObject.SendMessage("ApplyDamage",atk);

        }
    }


}
