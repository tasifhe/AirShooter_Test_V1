using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneShoterComp : MonoBehaviour
{
    [SerializeField]
    private float _fireRate = 0.5f;

    private float _canFire = -1f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _doubleShot;
    [SerializeField]
    private bool _isDoubleShotActive = false;

    private PlaneController _planeController;
    private void Start()
    {
        _planeController = transform.parent.GetComponent<PlaneController>();
    }
    private void Update()
    {
        if (Time.time > _canFire)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        _canFire = Time.time + _fireRate;

        if (_isDoubleShotActive == true)
        {
            Instantiate(_doubleShot, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    public void ActivateDoubleShot()
    {
        _isDoubleShotActive = true;
        StartCoroutine(DoubleShotPowerDownRoutine());
    }

    IEnumerator DoubleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isDoubleShotActive = false;
    }
}
