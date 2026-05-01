using UnityEngine;

public class CloudMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  

    public float speed = 0.3f;

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

}
