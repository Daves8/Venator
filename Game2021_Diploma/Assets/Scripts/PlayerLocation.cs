using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    public Location location;
    private PlayerCharacteristics _playerCharact;

    private void Start()
    {
        _playerCharact = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacteristics>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (location)
            {
                case Location.village:
                    _playerCharact.place = PlayerCharacteristics.Place.village;
                    break;
                case Location.forest:
                    _playerCharact.place = PlayerCharacteristics.Place.forest;
                    break;
                default:
                    break;
            }
        }
    }

    public enum Location
    {
        village,
        forest
    }
}
