using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSound : MonoBehaviour
{
    public AudioSource _audiosource;
    public AudioClip _clip;
    public bool _hit;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("pin") && _hit == false)
        {
            UpdateSound(_clip);
            _hit = true;
        }
    }
    public void UpdateSound(AudioClip _clip)
    {
        _audiosource.PlayOneShot(_clip);
    }
}
