using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisslovePlayerTest : MonoBehaviour
{
    [Range(0, 1)] public float _dissolve = 0.0f;
    GameObject _playerBody;
    // Start is called before the first frame update
    void Start()
    {
        _playerBody = transform.Find("girl_body").gameObject;
        _playerBody.GetComponent<Renderer>().material.SetFloat("_alphaClipThreshold", _dissolve);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("push");
            _dissolve += 0.1f;
            _playerBody.GetComponent<Renderer>().material.SetFloat("_alphaClipThreshold", _dissolve);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Box")
        {
            Debug.Log("hit");
            _dissolve += 0.1f;
            _playerBody.GetComponent<Renderer>().material.SetFloat("_alphaClipThreshold", _dissolve);

        }
    }
}
