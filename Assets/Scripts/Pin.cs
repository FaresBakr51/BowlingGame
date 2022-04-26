using UnityEngine;

public class Pin : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
           this.transform.rotation = Quaternion.Euler(Random.Range(90, 180), this.transform.rotation.y, Random.Range(90, 180));
        }
    }
}
