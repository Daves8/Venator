using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BowShot : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Animation _bowAnimation;

    [SerializeField] private Rig _rigBow;
    [SerializeField] private Rig _rigBowHand;
    [SerializeField] private Transform _target;

    [SerializeField] private GameObject _arrowInHand;
    [SerializeField] private GameObject _arrowPrefab;

    [SerializeField] private GameObject _mainCamera;

    [SerializeField] private GameObject _spine03;

    [SerializeField] private GameObject _freeLook;
    [SerializeField] private GameObject _cameraBow;

    [SerializeField] private Cinemachine.CinemachineFreeLook _freeLookCinemachine;
    [SerializeField] private Cinemachine.CinemachineFreeLook _cameraBowCinemachine;

    [SerializeField] private GameObject _aim;

    private Player _playerScript;
    public TextMeshProUGUI showPickedItem;

    private bool _canShoot = false;
    private bool _bow;
    private bool _beginBow;
    private float _timeToShoot;

    private Battle _battle;

    void Start()
    {
        _rigBow.weight = 0;
        _arrowInHand.SetActive(false);

        _freeLook.SetActive(true);
        _cameraBow.SetActive(false);

        _aim.SetActive(false);
        _battle = GetComponent<Battle>();

        _beginBow = false;
        _bow = false;

        _playerScript = GetComponent<Player>();
    }

    void Update()
    {
        if (!_battle.AllowBattle)
        {
            ExitBow();
            return;
        }

        #region Старое
        //if (Input.GetButton("Fire1") && WeaponEnum._selectedWeapon == Weapon.Bow)
        //{
        //    if (_beginBow == false)
        //    {
        //        _canShoot = false;
        //        _rigBow.weight = 1;
        //        StartCoroutine("RotateToCamera");
        //        _animator.SetTrigger("BeginShootBow");
        //        _beginBow = true;
        //        Invoke("CanShoot", 1.0f);

        //        CharacterMoving.rotateCharacter = false;
        //        _freeLook.SetActive(false);
        //        _cameraBow.SetActive(true);
        //        _cameraBowCinemachine.m_XAxis = _freeLookCinemachine.m_XAxis; // даем X новой камеры значения старой 
        //        _cameraBowCinemachine.m_YAxis = _freeLookCinemachine.m_YAxis; // даем Y новой камеры значения старой
        //        _aim.SetActive(true);
        //    }
        //    CharacterMoving.IsReadyToRun = false;
        //    //print("Держим"); //
        //}
        //else if (Input.GetButtonUp("Fire1") && WeaponEnum._selectedWeapon == Weapon.Bow && _canShoot)
        //{
        //    _animator.SetTrigger("ShootBow");
        //    _beginBow = false;
        //    _canShoot = false;
        //    CharacterMoving.IsReadyToRun = true;
        //    //print("Отпустили"); //
        //}
        //else if (Input.GetButtonUp("Fire1") && WeaponEnum._selectedWeapon == Weapon.Bow && !_canShoot)
        //{
        //    _rigBow.weight = 0;
        //    StopCoroutine("RotateToCamera");
        //    _animator.SetTrigger("ExitBow");
        //    _beginBow = false;
        //    _canShoot = false;
        //    //print("Не держим"); //

        //    CharacterMoving.rotateCharacter = true;
        //    _cameraBow.SetActive(false);
        //    _freeLook.SetActive(true);
        //    _freeLookCinemachine.m_XAxis = _cameraBowCinemachine.m_XAxis; // даем X новой камеры значения старой 
        //    _freeLookCinemachine.m_YAxis = _cameraBowCinemachine.m_YAxis; // даем Y новой камеры значения старой
        //    _aim.SetActive(false);
        //}
        //else
        //{
        //    _rigBow.weight = 0;
        //}
        //Debug.DrawRay(_mainCamera.transform.position, _mainCamera.transform.forward * 40, Color.red); // Луч идёт как надо, правильно
        #endregion

        if (WeaponEnum._selectedWeapon != Weapon.Bow)
        {
            //ExitBow();
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            // если нету стрел в инвентаре, вывод сообщения в "подбор предметов" "Остутствуют стрелы!" и return -------------------------------------------------------------------
            // начало анимации заряжания, начало стрельбы из лука
            if(_playerScript.equipment.FindItemOnInventory(22) <= 0)
            {
                return;
            }
            if (_playerScript.inventory.FindItemOnInventory(21) <= 0)
            {
                showPickedItem.gameObject.SetActive(true);
                Invoke("ShowAttention", 1f);
                showPickedItem.text = "Отсутствуют стрелы!";
                return;
            }

            --_playerScript.inventory.FindItemOnInventory(new Item(_playerScript.dbVenator.ItemObjects[21])).amount;
            _bow = true;
            _rigBow.weight = 1;
            StartCoroutine("RotateToCamera");
            _animator.SetTrigger("BeginShootBow");
            CharacterMoving.rotateCharacter = false;
            _freeLook.SetActive(false);
            _cameraBow.SetActive(true);
            _cameraBowCinemachine.m_XAxis = _freeLookCinemachine.m_XAxis;
            _cameraBowCinemachine.m_YAxis = _freeLookCinemachine.m_YAxis;
            _aim.SetActive(true);
            CharacterMoving.IsReadyToRun = false;
            _timeToShoot = Time.time;
            //Invoke("BowChargingAnim", 0.01f);
            BowChargingAnim();
        }
        if (Input.GetButton("Fire1"))
        {
            // таймер, проверяющий достаточно ли держим, сбрасывается при отпускании

        }
        if (Input.GetButtonUp("Fire1"))
        {
            // отпустили, проверка: если достаточно долго держали, то выстрел, иначе ExitBow
            if (Time.time - _timeToShoot >= 1.0f)
            {
                // выстрел
                _animator.SetTrigger("ShootBow");
            }
            else
            {
                ExitBow();
            }
            _bow = false;
            _rigBow.weight = 0;
            _arrowInHand.SetActive(false);
        }

        if (!_bow)
        {
            _rigBow.weight = 0;
            _arrowInHand.SetActive(false);
        }
    }

    private void ShowAttention()
    {
        showPickedItem.gameObject.SetActive(false);
    }

    private void ExitBow()
    {
        _rigBow.weight = 0;
        _arrowInHand.SetActive(false);
        _animator.SetTrigger("ExitBow");
        _freeLook.SetActive(true);
        _cameraBow.SetActive(false);
        StopCoroutine("RotateToCamera");
        _aim.SetActive(false);
        CharacterMoving.rotateCharacter = true;
        CharacterMoving.IsReadyToRun = true;
    }

    private void Shoot()
    {
        GameObject newArrow = Instantiate(_arrowPrefab, _mainCamera.transform.position + _mainCamera.transform.forward * 2, _mainCamera.transform.rotation);
        newArrow.transform.LookAt(_target);
        newArrow.GetComponent<Rigidbody>().velocity = newArrow.transform.forward * 100;

        _bowAnimation.Play("MECANIM");
        _rigBow.weight = 0;
        StopCoroutine("RotateToCamera");

        CharacterMoving.rotateCharacter = true;
        CharacterMoving.IsReadyToRun = true;
        _cameraBow.SetActive(false);
        _freeLook.SetActive(true);
        _freeLookCinemachine.m_XAxis = _cameraBowCinemachine.m_XAxis; // даем X новой камеры значения старой 
        _freeLookCinemachine.m_YAxis = _cameraBowCinemachine.m_YAxis; // даем Y новой камеры значения старой
        _aim.SetActive(false);
    }

    private void BowBattle() // НЕ актуально
    {
        if (!_beginBow)
        {
            _rigBow.weight = 1;



            //_freeLookCinemachine.m_Orbits = new Cinemachine.CinemachineFreeLook.Orbit[] { new Cinemachine.CinemachineFreeLook.Orbit(2.52f, 1.51f), new Cinemachine.CinemachineFreeLook.Orbit(1.98f, 1.95f), new Cinemachine.CinemachineFreeLook.Orbit(1.34f, 1.51f) };

            //_freeLook.SetActive(false);
            //_cameraBow.SetActive(true); //

            StartCoroutine("RotateToCamera");
            _animator.SetTrigger("BeginShootBow");
            Invoke("BowChargingAnim", 1.03f);
            _beginBow = true;
        }

        if (Input.GetButtonUp("Fire1")) // тут по задумке нужно использовать GetButtonDown(), но это не работает, вообще не понимаю из-за чего, просто условие не выполняется
        {
            _animator.SetTrigger("ShootBow");
            _bowAnimation.Play("Archer_shoting");
            _beginBow = false;

            GameObject newArrow = Instantiate(_arrowPrefab, _mainCamera.transform.position + transform.forward * 4, _arrowPrefab.transform.rotation);//_arrowInHand.transform.position, _arrowInHand.transform.rotation);
            newArrow.transform.LookAt(_target);
            newArrow.GetComponent<Rigidbody>().velocity = newArrow.transform.forward * 1;
            //newArrow.transform.rotation = _arrowPrefab.transform.rotation;  //.x += 90;// x = 90
        }
    }

    private void BowChargingAnim()
    {
        _bowAnimation.Play("Archer_charging");
    }
    public IEnumerator RotateToCamera()
    {
        while (true)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            yield return null;
        }
    }

    private void CanShoot()
    {
        _canShoot = true;
    }

    private void ArrowEnable(int enable)
    {
        if (enable == 1)
        {
            _arrowInHand.SetActive(true);
        }
        else if (enable == 0)
        {
            _arrowInHand.SetActive(false);
        }
    }
}