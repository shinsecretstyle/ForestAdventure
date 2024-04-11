using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    public Transform enemyTransform;
    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        Vector3 targetPosition = new Vector3(enemyTransform.position.x, enemyTransform.position.y + 4.5f, enemyTransform.position.z);
        transform.position = targetPosition;

    }
}
