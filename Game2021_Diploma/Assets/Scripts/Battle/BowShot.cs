using System.Collections;
using System.Collections.Generic;
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

    private bool _canShoot = false;
    private bool _bow = false;
    private bool _beginBow = false;

    private Battle _battle;

    void Start()
    {
        _rigBow.weight = 0;
        _arrowInHand.SetActive(false);

        _freeLook.SetActive(true);
        _cameraBow.SetActive(false);

        _aim.SetActive(false);
        _battle = GetComponent<Battle>();
    }

    void Update()
    {
        if (!_battle.AllowBattle)
        {
            _rigBow.weight = 0;
            _arrowInHand.SetActive(false);

            _freeLook.SetActive(true);
            _cameraBow.SetActive(false);

            _aim.SetActive(false);
            return;
        }

        if (Input.GetButton("Fire1") && WeaponEnum._selectedWeapon == Weapon.Bow)
        {
            if (_beginBow == false)
            {
                _canShoot = false;
                _rigBow.weight = 1;
                StartCoroutine("RotateToCamera");
                _animator.SetTrigger("BeginShootBow");
                _beginBow = true;
                Invoke("CanShoot", 1.0f);

                CharacterMoving.rotateCharacter = false;
                _freeLook.SetActive(false);
                _cameraBow.SetActive(true);
                _cameraBowCinemachine.m_XAxis = _freeLookCinemachine.m_XAxis; // даем X новой камеры значения старой 
                _cameraBowCinemachine.m_YAxis = _freeLookCinemachine.m_YAxis; // даем Y новой камеры значения старой
                _aim.SetActive(true);
            }
            CharacterMoving.IsReadyToRun = false;
            //print("Держим"); //
        }
        else if (Input.GetButtonUp("Fire1") && WeaponEnum._selectedWeapon == Weapon.Bow && _canShoot)
        {
            _animator.SetTrigger("ShootBow");
            _beginBow = false;
            _canShoot = false;
            CharacterMoving.IsReadyToRun = true;
            //print("Отпустили"); //
        }
        else if (Input.GetButtonUp("Fire1") && WeaponEnum._selectedWeapon == Weapon.Bow && !_canShoot)
        {
            _rigBow.weight = 0;
            StopCoroutine("RotateToCamera");
            _animator.SetTrigger("ExitBow");
            _beginBow = false;
            _canShoot = false;
            //print("Не держим"); //

            CharacterMoving.rotateCharacter = true;
            _cameraBow.SetActive(false);
            _freeLook.SetActive(true);
            _freeLookCinemachine.m_XAxis = _cameraBowCinemachine.m_XAxis; // даем X новой камеры значения старой 
            _freeLookCinemachine.m_YAxis = _cameraBowCinemachine.m_YAxis; // даем Y новой камеры значения старой
            _aim.SetActive(false);
        }
        else
        {
            _rigBow.weight = 0;
        }
        Debug.DrawRay(_mainCamera.transform.position, _mainCamera.transform.forward * 40, Color.red); // Луч идёт как надо, правильно
    }

    private void Shoot()
    {
        GameObject newArrow = Instantiate(_arrowPrefab, _mainCamera.transform.position + _mainCamera.transform.forward * 2, _mainCamera.transform.rotation);
        newArrow.transform.LookAt(_target);
        newArrow.GetComponent<Rigidbody>().velocity = newArrow.transform.forward * 100;


        _rigBow.weight = 0;
        StopCoroutine("RotateToCamera");

        CharacterMoving.rotateCharacter = true;
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