using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _speedMultiplier = 2f;
    [SerializeField]
    private float _fireRate = 0.5f;
    
    private float _canFire = -1f;
    [SerializeField]
    private int _planeLives = 3;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _doubleShot;
    [SerializeField]
    private bool _isDoubleShotActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;

    private EnemySpawnManager _enemySpawnManager;
    private PowerUpSpawnManager _powerUpSpawnManager;

    [SerializeField]
    private int _score;

    private UIManager _uIManager;

    private Vector2 bounds = new Vector2(10.5f, 4f);
    void Start()
    {
        InitializeReferences();
    }
    void Update()
    {
        //PlayerBound();
        PlaneMovement();
        PlaneBounds();
        if (Time.time > _canFire)
        {
            PlaneShooting();
        }
    }
    private void InitializeReferences()
    {
        transform.position = new Vector3(0, 0, 0);
        _enemySpawnManager = GameObject.FindObjectOfType<EnemySpawnManager>();
        _powerUpSpawnManager = GameObject.FindObjectOfType<PowerUpSpawnManager>();
        _uIManager = GameObject.Find("Canvas")?.GetComponent<UIManager>();

        if (_uIManager == null)
        {
            Debug.Log("The UI Manager is Null");
        }
        if (_enemySpawnManager == null)
        {
            Debug.LogError("The EnemySpawnManager is Null");
        }
        if (_powerUpSpawnManager == null)
        {
            Debug.LogError("The PowerUpSpawnManager is Null");
        }

    }
    public void PlaneMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0).normalized;

        transform.Translate(direction * _speed * Time.deltaTime);
    }
    public void PlaneBound()
    {
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -4f)
        {
            transform.position = new Vector3(transform.position.x, -4f, 0);
        }
        if (transform.position.x > 10.5f)
        {
            transform.position = new Vector3(-10.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -10.5f)
        {
            transform.position = new Vector3(10.5f, transform.position.y, 0);
        }
    }
    private void PlaneBounds()
    {
        var position = transform.position;
        if(position.x > bounds.x)
        {
            position.x = -bounds.x;
        }
        else if(position.x < -bounds.x)
        {
            position.x = bounds.x;
        }
        if(position.y > bounds.y)
        {
            position.y = bounds.y;
        }
        else if(position.y < -bounds.y)
        {
            position.y = -bounds.y;
        }

        transform.position = position;
    }
    private void PlaneShooting()
    {
        _canFire = Time.time + _fireRate;

        //Instantiate(_isDoubleShotActive ? _doubleShot : _laserPrefab, transform.position, Quaternion.identity);
        if (_isDoubleShotActive == true)
        {
            Instantiate(_doubleShot, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        }

    }
    public void PlaneDamage()
    {
        _planeLives--;
        Debug.Log("One lives lost: "+gameObject.name);
        if(_planeLives < 1)
        {
            _enemySpawnManager.OnPlaneDestroyed();
            Destroy(this.gameObject);
        }
    }
    public void DoubleShotActive()
    {
        _isDoubleShotActive = true;
        StartCoroutine(PowerupPowerDownRoutine(() => _isDoubleShotActive = false));
    }
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(PowerupPowerDownRoutine(() =>
        {
            _isSpeedBoostActive = false;
            _speed /= _speedMultiplier;
        }));
    }
    private IEnumerator PowerupPowerDownRoutine(System.Action callback)
    {
        yield return new WaitForSeconds(5.0f);
        callback?.Invoke();
    }
    public void AddScore(int points)
    {
        _score += points;
        _uIManager?.UpdateScore(_score);
    }
}