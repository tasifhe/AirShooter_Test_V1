using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerupID;

    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlaneController plane = other.transform.GetComponent<PlaneController>();
            if (plane != null)
            {
                ActivatePowerup(plane);
            }
            Destroy(gameObject);
        }
    }

    private void ActivatePowerup(PlaneController plane)
    {
        switch (_powerupID)
        {
            case 0:
                plane.DoubleShotActive();
                break;
            case 1:
                plane.SpeedBoostActive();
                break;
            case 2:
                Debug.Log("Collected Shield boost");
                break;
            default:
                Debug.Log("Default value");
                break;
        }
    }
}
