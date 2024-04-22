using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCtrl : MonoBehaviour
{
    public Image skill1;
    public float skill1CD;
    public bool skill1canUse = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!skill1canUse) {
            skill1.fillAmount += 1 / skill1CD * Time.deltaTime;
        }
        if(skill1.fillAmount >= 1) {
            skill1canUse = true;
            skill1.fillAmount = 0;
        }
    }
    public void useSkill1()
    {
        if(skill1canUse)
        {
            skill1canUse = false;
        }
    }

}
