using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private int _maxRifleBullets;
    private bool _isReloading = false;

    //For Inventory
    //private bool _hasCoin;

    private int hasCoin;//拥有的金币

    [SerializeField]
    private LayerMask enemyMask;

    [SerializeField]
    private float bulletLen;//子弹的射击长度


    private int maxHp;
    private int currHp;

    [SerializeField]
    private PlayerInitData playerInfo;


    private List<GunInfo> hasGuns = new List<GunInfo>();

    private int currWeaponIndex;//当前武器索引
    private void Awake()
    {
        EventCenter.Instance.AddEventListener<GunInfo>("BuyGunEvent", BuyRifle);
        EventCenter.Instance.AddEventListener<int>("PlayerKillEnemyGetGold", PlayerKillEnemyGetGold);
    }
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<GunInfo>("BuyGunEvent", BuyRifle);
        EventCenter.Instance.RemoveEventListener<int>("PlayerKillEnemyGetGold", PlayerKillEnemyGetGold);
    }
    // Start is called before the first frame update
    void Start()
    {
        //_currentBullets = _maxRifleBullets;
        //Hide the mouse cursor at start
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //first we get the required components
        _controller = GetComponent<CharacterController>();
        _shootingSound = GetComponent<AudioSource>();
       
        InitPlayer();
    }

    private void InitPlayer() 
    {
        maxHp = playerInfo.cherkPlayerInfos[Tool.currCherk].initHp;
        currHp = maxHp;
        hasCoin = playerInfo.cherkPlayerInfos[Tool.currCherk].initGold;
        hasGuns.Clear();
        EventCenter.Instance.EventTrigger<int>("UpdateGoldNumEvent", hasCoin);
    }

    private void PlayerKillEnemyGetGold(int gold) 
    {
        hasCoin += gold;
        EventCenter.Instance.EventTrigger<int>("UpdateGoldNumEvent", hasCoin);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetMouseButtonDown(0) && _currentBullets > 0 && _rifle.activeSelf)
        {
            Shoot();
        }
        else
        {
            _shootingSound.Stop();
            //_muzzleFlash.SetActive(false);
        }


        if(Input.GetKeyDown(KeyCode.R) && _isReloading==false && _rifle.activeSelf)
        {
            StartCoroutine(Reload());
        }

        //MovePlayer();
        PauseGame();

        //测试
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            //切换场景   
            UnityEngine.SceneManagement.SceneManager.LoadScene("Demo1");
        }

        if (Input.GetKeyDown(KeyCode.P)) 
        {
            EventCenter.Instance.EventTrigger("OpenShopPanel");
        }
        int index;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            index = 0;
            ChangeWeapon(index);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            index = 1;
            ChangeWeapon(index);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            index = 2;
            ChangeWeapon(index);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            index = 3;
            ChangeWeapon(index);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            index = 4;
            ChangeWeapon(index);
        }
    }
    private void ChangeWeapon(int index) 
    {
        if (index >= hasGuns.Count) return;
        currWeaponIndex = index;
        _maxRifleBullets = hasGuns[currWeaponIndex].bulletNum;
        _currentBullets = _maxRifleBullets;
        UpdateBulletsUI();
        EventCenter.Instance.EventTrigger<Sprite>("UpdateWeaponIcon", hasGuns[currWeaponIndex].icon);
    }
    public void BuyRifle(GunInfo info)
    {
        if (hasGuns.Find((a) => { return a.gunId == info.gunId; }) != null) return;
        if (hasCoin >= info.price)
        {
            hasCoin -= info.price;
            //StartCoroutine(_uiManager.BuySuccessful());
            EventCenter.Instance.EventTrigger("BuySuccessful");
            _rifle.SetActive(true);
            _maxRifleBullets = info.bulletNum;
            _currentBullets = _maxRifleBullets;
            UpdateBulletsUI();
            EventCenter.Instance.EventTrigger<int>("UpdateGoldNumEvent", hasCoin);
            hasGuns.Add(info);
            currWeaponIndex = hasGuns.FindIndex((a)=> { return a.gunId == info.gunId; });
            EventCenter.Instance.EventTrigger<Sprite>("UpdateWeaponIcon", hasGuns[currWeaponIndex].icon);
            
        }
        else
        {
            //StartCoroutine(_uiManager.BuyFailed());
            EventCenter.Instance.EventTrigger("BuyFailed");
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
        if (EventSystem.current.IsPointerOverGameObject()) return;

        _currentBullets--;
        //update the number of bullets in UI
        UpdateBulletsUI();


        //we will use ViewportPointToRay, because it allows us to values from 0-1 for the Vector position
        //0.5 being the center
        Ray originOfShot = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;
        if(Physics.Raycast(originOfShot, out hitInfo, bulletLen, enemyMask))
        {
            //hitInfo.point is the position of the target
            //LookRotation will let us choose the rotations, and using hitInfo.normal we get the normal/perpendicular of the target
            //we will TypeCast it as a GameObject, so we can destroy it later on, without having to make a script for it.

            //射击到怪物
            Fsm enemy = hitInfo.collider.gameObject.GetComponent<Fsm>();
            if (enemy != null)
            {
                //Debug.LogError("射击到怪物");
                enemy.Damage(hasGuns[currWeaponIndex].attack);
            }
            else
            {
                Debug.LogError($"射击检测错误提示，射线打到了物体_____{hitInfo.collider.gameObject.name}，但是没有找到脚本FSM");
            }

            GameObject hitMarkObj = Instantiate(_hitMarker, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
            GameObject.Destroy(hitMarkObj.gameObject, 1.2f);

            ////Shooting a crate
            ////hitInfo is just a container for the gameObject, that is why we use hitInfo.transform before Getting component
            //Crate crate = hitInfo.transform.GetComponent<Crate>();
            //if(crate != null)
            //{
            //    crate.DestroyCrate();
            //}
        }

        if(!_shootingSound.isPlaying)
        {
            _shootingSound.Play();
        }

        _muzzleFlash.SetActive(true);
        CancelInvoke("CanelMuzzleFlash");
        Invoke("CanelMuzzleFlash",0.1f);

        //RaycastHit enemyHitInfo;
        //if (Physics.Raycast(originOfShot, out enemyHitInfo, bulletLen, enemyMask)) 
        //{
        //    //射击到怪物
        //    Fsm enemy = enemyHitInfo.collider.gameObject.GetComponent<Fsm>();
        //    if (enemy != null)
        //    {
        //        //Debug.LogError("射击到怪物");
        //        enemy.Transititionstate(StateType.Hit);
        //    }
        //    else 
        //    {
        //        Debug.LogError($"射击检测错误提示，射线打到了物体_____{enemyHitInfo.collider.gameObject.name}，但是没有找到脚本FSM");
        //    }
        //}
    }

    private void CanelMuzzleFlash() 
    {
        _muzzleFlash.SetActive(false);
    }

    private void UpdateBulletsUI()
    {
        //if(_uiManager != null)
        //{
        //    _uiManager.UpdateNumOfBullets(_currentBullets);
        //}
        int[] bullets = new int[2] { _maxRifleBullets, _currentBullets };
        EventCenter.Instance.EventTrigger<int[]>("UpdateNumOfBullets", bullets);
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

    public void Damage(int attack)
    {
        int _currHp = currHp - attack;
        currHp = Mathf.Clamp(_currHp, 0, maxHp);
        if (currHp <= 0)
        {
            //玩家死亡状态   游戏结束
            Debug.LogError("玩家死亡状态   游戏结束");
        }
        float[] hps = new float[2] { (float)maxHp, (float)currHp };
        EventCenter.Instance.EventTrigger<float[]>("UpdatePlayerHp", hps);
    }
}
