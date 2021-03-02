using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Battle : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private GameObject _camera;
    [SerializeField] private int _countCombo = 0;
    [SerializeField] private Cinemachine.CinemachineBrain _cinemachineBrain;
    private float _previousTime = 0f;
    private float _delay = 0f;

    private float _timeToBattle = 1.5f;

    private bool _firstAttack = true;
    private bool _thirdCombo_1 = false;
    private bool _thirdCombo_2 = false;

    private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;

    [SerializeField] private GameObject _swordOn;
    [SerializeField] private GameObject _swordOff;

    [SerializeField] private GameObject _knifeOn;
    [SerializeField] private GameObject _knifeOff;

    [SerializeField] private GameObject _bowOn;
    [SerializeField] private GameObject _bowOff;



    [SerializeField] private CapsuleCollider _swordCollider;
    [SerializeField] private CapsuleCollider _knifeCollider;

    [SerializeField] private  Animation _bowAnimation;

    private string _changeWeaponAnim;
    private bool _previousWeapon = false;

    static public bool AllowBattle = true;

    private void Awake()
    {
        _swordOn.SetActive(false);
        _swordOff.SetActive(true);

        _bowOn.SetActive(false);
        _bowOff.SetActive(true);

        _knifeOn.SetActive(false);
        _knifeOff.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!AllowBattle)
        {
            KnifeOff();
            SwordOff();
            BowOff();
            return;
        }

        ChangeWeapon();

        _delay = Time.time - _previousTime;
        if (_delay > 100)
        {
            _delay = 100.0f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (_delay >= _timeToBattle)
            {
                _firstAttack = true;
                _thirdCombo_1 = false;
                _thirdCombo_2 = false;
                _countCombo = 0;
            }

            switch (WeaponEnum._selectedWeapon)
            {
                case Weapon.Khife:
                    _knifeCollider.enabled = true;
                    Invoke("WeaponColliderOff", 1.0f);
                    KnifeBattle();
                    break;
                case Weapon.Sword:
                    _swordCollider.enabled = true;
                    Invoke("WeaponColliderOff", 1.0f);
                    SwordBattle();
                    break;
                default:
                    break;
            }
        }
    }
    private void WeaponColliderOff()
    {
        _knifeCollider.enabled = false;
        _swordCollider.enabled = false;
    }
    private void ChangeWeapon_Animation()
    {
        _animator.SetTrigger(_changeWeaponAnim);
    }
    private void ChangeWeapon()
    {
        //if (Input.GetKeyDown(KeyCode.E)) ///////////////////////////////////////////////////////
        //{
        //    int qwe = Random.Range(1, 4);
        //    ChangeClothes.Change(qwe);
        //} //////////////////////////////////////////////////////////////////////////////////////


        if (Input.GetButtonDown("Equip First Item")) // 1 - НОЖ
        {
            _changeWeaponAnim = "KnifeOn";

            if (WeaponEnum._selectedWeapon == Weapon.Khife)
            {
                Invoke("KnifeOff", 0.3f);
                _animator.SetTrigger("KnifeOff");
                WeaponEnum._selectedWeapon = Weapon.None;
                return;
            }
            if (WeaponEnum._selectedWeapon == Weapon.Bow)
            {
                Invoke("BowOff", 0.5f);
                _previousWeapon = true;
                _animator.SetTrigger("BowBack");
            }
            else if (WeaponEnum._selectedWeapon == Weapon.Sword)
            {
                Invoke("SwordOff", 0.5f);
                _previousWeapon = true;
                _animator.SetTrigger("Sheathing");
            }
            if (_previousWeapon)
            {
                Invoke("ChangeWeapon_Animation", 0.4f);
            }
            else
            {
                _animator.SetTrigger("KnifeOn");
            }
            Invoke("KnifeOn", 0.8f);
            _previousWeapon = false;
            WeaponEnum._selectedWeapon = Weapon.Khife;
        }
        else if (Input.GetButtonDown("Equip Second Item")) // 2 - МЕЧ
        {
            _changeWeaponAnim = "Withdrawing";

            if (WeaponEnum._selectedWeapon == Weapon.Sword)
            {
                Invoke("SwordOff", 0.5f);
                _animator.SetTrigger("Sheathing");
                WeaponEnum._selectedWeapon = Weapon.None;
                return;
            }
            if (WeaponEnum._selectedWeapon == Weapon.Bow)
            {
                Invoke("BowOff", 0.5f);
                _previousWeapon = true;
                _animator.SetTrigger("BowBack");
            }
            else if (WeaponEnum._selectedWeapon == Weapon.Khife)
            {
                Invoke("KnifeOff", 0.3f);
                _previousWeapon = true;
                _animator.SetTrigger("KnifeOff");
            }
            if (_previousWeapon)
            {
                Invoke("SwordOn", 0.8f);
                Invoke("ChangeWeapon_Animation", 0.4f);
            }
            else
            {
                Invoke("SwordOn", 0.6f);
                _animator.SetTrigger("Withdrawing");
            }

            _previousWeapon = false;
            WeaponEnum._selectedWeapon = Weapon.Sword;
        }
        else if (Input.GetButtonDown("Equip Third Item")) // 3 - ЛУК
        {
            _changeWeaponAnim = "BowOut";

            if (WeaponEnum._selectedWeapon == Weapon.Bow)
            {
                Invoke("BowOff", 0.5f);
                _animator.SetTrigger("BowBack");
                WeaponEnum._selectedWeapon = Weapon.None;
                return;
            }
            if (WeaponEnum._selectedWeapon == Weapon.Sword)
            {
                Invoke("SwordOff", 0.9f);
                _previousWeapon = true;
                _animator.SetTrigger("Sheathing");
            }
            else if (WeaponEnum._selectedWeapon == Weapon.Khife)
            {
                Invoke("KnifeOff", 0.3f);
                _previousWeapon = true;
                _animator.SetTrigger("KnifeOff");
            }
            if (_previousWeapon)
            {
                Invoke("BowOn", 1.5f);
                Invoke("ChangeWeapon_Animation", 0.9f);
            }
            else
            {
                Invoke("BowOn", 0.65f);
                _animator.SetTrigger("BowOut");
            }

            _previousWeapon = false;
            WeaponEnum._selectedWeapon = Weapon.Bow;
        }
    }
    private void KnifeOff()
    {
        _knifeOn.SetActive(false);
        _knifeOff.SetActive(true);
    }
    private void KnifeOn()
    {
        _knifeOff.SetActive(false);
        _knifeOn.SetActive(true);
    }
    private void SwordOff()
    {
        _swordOn.SetActive(false);
        _swordOff.SetActive(true);
    }
    private void SwordOn()
    {
        _swordOff.SetActive(false);
        _swordOn.SetActive(true);
    }
    private void BowOff()
    {
        _bowOn.SetActive(false);
        _bowOff.SetActive(true);
    }
    private void BowOn()
    {
        _bowOff.SetActive(false);
        _bowOn.SetActive(true);
    }
    public IEnumerator RotateToCamera()
    {
        while (true)
        {
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _cameraTransform.eulerAngles.y, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //Vector3 direction = (_target.position - transform.position).normalized;
            //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            yield return null;
        }
    }
    private void DestroyCoroutine()
    {
        StopCoroutine("RotateToCamera");
    }
    private void KnifeBattle()
    {
        if (CharacterMoving.moveDirection.magnitude <= 0.1f) { StartCoroutine("RotateToCamera"); }
        Invoke("DestroyCoroutine", 0.8f);

        if (_delay >= 0.3f)
        {
            _previousTime = Time.time;

            if (_firstAttack)
            {
                _animator.SetTrigger("Knife1");
                _firstAttack = false;
            }
            else
            {
                _animator.SetTrigger("Knife2");
                _firstAttack = true;
            }
        }
    }
    private void SwordBattle()
    {
        if (CharacterMoving.moveDirection.magnitude <= 0.1f) { StartCoroutine("RotateToCamera"); }
        Invoke("DestroyCoroutine", 1.0f);

        ++_countCombo;
        _countCombo = _countCombo % 3;
        if (_countCombo == 0)
        {
            _countCombo = 3;
        }
        if (_delay >= 0.5f)
        {
            _previousTime = Time.time;
            if (_countCombo == 3 && _thirdCombo_1 && _thirdCombo_2)
            {
                _animator.SetTrigger("Sword3");
                _thirdCombo_1 = false;
                _thirdCombo_2 = false;
            }
            else
            {
                if (_firstAttack)
                {
                    _animator.SetTrigger("Sword1");
                    _firstAttack = false;
                    if (_countCombo == 1)
                    {
                        _thirdCombo_1 = true;
                    }
                }
                else
                {
                    _animator.SetTrigger("Sword2");
                    _firstAttack = true;
                    if (_countCombo == 2 && _thirdCombo_1)
                    {
                        _thirdCombo_2 = true;
                    }
                    else
                    {
                        _thirdCombo_1 = false;
                    }
                }
            }
        }
    }
}