using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;

    private PlaneController _plane;
    void Start()
    {
        _plane = GameObject.Find("Player").GetComponent<PlaneController>();
        if(_plane == null)
        {
            Debug.Log("The PlaneController is Null");
        }
    }
    void Update()
    {
        EnemyState();
    }
    private void EnemyState()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if(transform.position.y <= -5f)
        {
            RespawnEnemy();
        }
    }
    private void RespawnEnemy()
    {
        float randomX = Random.Range(-8f, 8f);
        transform.position = new Vector3(randomX, 4, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlaneController plane = other.transform.GetComponent<PlaneController>();
            if(plane != null)
            {
                plane.PlaneDamage();
                Debug.Log("Destroyed: " + gameObject.name);
            }
            Destroy(this.gameObject);
        }
        else if(other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Debug.Log("Destroyed: " + gameObject.name);

            if(_plane != null)
            {
                _plane.AddScore(10);
            }

            Destroy(this.gameObject);
        }
    }
}
