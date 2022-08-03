using UnityEngine;

public class Pin : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
          
          transform.localRotation = Quaternion.Euler(Random.Range(90, 270), Random.Range(90, 270), Random.Range(90, 270));
        }
    }
}
