using UnityEngine;

public class tree : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mainmap")
        {
            Destroy(gameObject);
        }
    }
}
