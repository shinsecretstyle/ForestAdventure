using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashObj : MonoBehaviour
{
    public float lifeTime = 0.4f;
    public string target;
    public int power;
    public bool hasLifeTime = false;
    public bool isLimit = false;

    SphereCollider sc;
    float waitLimit = 1.1f;
    // Start is called before the first frame update
    void Start()
    {
        sc = GetComponent<SphereCollider>();
        if(isLimit)
        {
            sc.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLimit)
        {
            waitLimit -= Time.deltaTime;
        }
        if(waitLimit < 0)
        {
            sc.enabled = true;
        }
        if(hasLifeTime) lifeTime -= Time.deltaTime;
        if(lifeTime < 0 ) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(target))
        {
            collider.gameObject.SendMessage("ApplyDamage", power);

        }

    }
}
