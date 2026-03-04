using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    //For movement
    [SerializeField]
    private float _speed = 5f;
    private float _gravity = 9.8f;
    private CharacterController _controller;

    //For shooting
    [SerializeField] private GameObject _rifle;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private GameObject _hitMarker;
    private AudioSource _shootingSound;
    private int _currentBullets;
    private int _maxRifleBullets = 60;
    private bool _isReloading = false;

    //For UI
    private UIManager _uiManager;

    //For Inventory
    private bool _hasCoin;

    // Start is called before the first frame update
    void Start()
    {
        _currentBullets = _maxRifleBullets;
        //Hide the mouse cursor at start
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //first we get the required components
        _controller = GetComponent<CharacterController>();
        _shootingSound = GetComponent<AudioSource>();
        _uiManager = FindObjectOfType<Canvas>().GetComponent<UIManager>();

    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButton(0) && _currentBullets > 0 && _rifle.activeSelf)
        {
            Shoot();
        }
        else
        {
            _shootingSound.Stop();
            _muzzleFlash.SetActive(false);
        }


        if(Input.GetKeyDown(KeyCode.R) && _isReloading==false && _rifle.activeSelf)
        {
            StartCoroutine(Reload());
        }
        MovePlayer();
        PauseGame();

    }

    public void BuyRifle()
    {
        if(_hasCoin == true)
        {
            _hasCoin = false;
            if(_uiManager != null)
            {
                _uiManager.HideCoinImage();
                StartCoroutine(_uiManager.BuySuccessful());
                _rifle.SetActive(true);
            }


        }
        else
        {
            StartCoroutine(_uiManager.BuyFailed());

        }
    }
    public void GetCoin()
    {
        _hasCoin = true;
        if(_uiManager != null)
        {
            _uiManager.ShowCoinImage();
        }

    }
    IEnumerator Reload()
    {
        _isReloading = true;
        yield return new WaitForSeconds(1f);
        _currentBullets = _maxRifleBullets;
        UpdateBulletsUI();
        _isReloading = false;
    }

    private static void PauseGame()
    {
        //Show cursor when ESC is clicked
        if(Input.GetKeyDown(KeyCode.Escape))
        {

            if(Cursor.visible == false)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

            }
        }
    }

    private void Shoot()
    {
        _currentBullets--;
        //update the number of bullets in UI
        UpdateBulletsUI();


        //we will use ViewportPointToRay, because it allows us to values from 0-1 for the Vector position
        //0.5 being the center
        Ray originOfShot = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;
        if(Physics.Raycast(originOfShot, out hitInfo))
        {
            //hitInfo.point is the position of the target
            //LookRotation will let us choose the rotations, and using hitInfo.normal we get the normal/perpendicular of the target
            //we will TypeCast it as a GameObject, so we can destroy it later on, without having to make a script for it.
            GameObject hitMarkObj = Instantiate(_hitMarker, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
            GameObject.Destroy(hitMarkObj.gameObject, 1.2f);

            //Shooting a crate
            //hitInfo is just a container for the gameObject, that is why we use hitInfo.transform before Getting component
            Crate crate = hitInfo.transform.GetComponent<Crate>();
            if(crate != null)
            {
                crate.DestroyCrate();
            }
        }

        if(!_shootingSound.isPlaying)
        {
            _shootingSound.Play();
        }

        _muzzleFlash.SetActive(true);
    }

    private void UpdateBulletsUI()
    {
        if(_uiManager != null)
        {
            _uiManager.UpdateNumOfBullets(_currentBullets);
        }
    }
    private void MovePlayer()
    {
        float hMovement = Input.GetAxis("Horizontal");
        float vMovement = Input.GetAxis("Vertical");
        //we will move using the Axis
        Vector3 directionOfMovement = new Vector3(hMovement, 0, vMovement);
        //Velocity is our direction multiplied by the player's speed
        Vector3 velocity = directionOfMovement * _speed;

        //To keep our player on the ground, we always subtract the gravity from his y-axis
        velocity.y -= _gravity;

        //We usually move using through the LocalSpace world, but this time we have to use the GlobalSpace
        //GlobalSpace is where the main camera of the project is looking
        //By using this, our Player will move in GlobalSpace direction, instead of local one.
        velocity = transform.transform.TransformDirection(velocity);

        //Finally we move using the Character Controller's Move function
        _controller.Move(velocity * Time.deltaTime);
    }


}
