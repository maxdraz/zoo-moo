using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpMan : MonoBehaviour
{

    [SerializeField] private int _flyTime;
    private Rigidbody rb;

    [SerializeField] private PowerUpManager _powerUpManager;

    [SerializeField] private GameObject _wings;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }





    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "fly")
        {
            StartCoroutine("Fly", _flyTime);
            other.gameObject.SetActive(false);

        }
    }

    IEnumerator Fly(float t)
    {
        _wings.SetActive(true);
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        yield return new WaitForSeconds(t);
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        _wings.SetActive(false);
        Invoke("PowerUpManagerCaller", 10);
    }

    private void PowerUpManagerCaller()
    {
        _powerUpManager.NewPowerUp();
    }
}
