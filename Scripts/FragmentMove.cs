using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FragmentMove : MonoBehaviour
{
    [SerializeField] private GameObject video, fragments; 
    [SerializeField] private float rotationSpeed, speed, maxRotation;
    private Vector2 direction;
    private Vector3 rotation;
    void OnEnable()
    {
        speed = 0.01f;
        rotationSpeed = 90;
        maxRotation = UnityEngine.Random.Range(-120, 120);
        maxRotation = maxRotation < 0 ? maxRotation-60 : maxRotation+60;
        direction = Vector2.MoveTowards(transform.position, fragments.transform.position, Time.deltaTime);
        rotation = Vector3.MoveTowards(transform.rotation.eulerAngles, new(0, 0, maxRotation), Time.deltaTime);
    }

    void Update()
    {
        if (!video.activeSelf)
        {
            if (!transform.GetChild(0).GetComponent<Renderer>().isVisible)
            {
                Destroy(gameObject);
            }
            else
            {
                //Vector2 direction = Vector2.MoveTowards(transform.position, fragments.transform.position, Time.deltaTime);
                if((transform.rotation.z >= maxRotation && maxRotation > 0) || (transform.rotation.z <= maxRotation && maxRotation < 0))
                {
                    rotation.z = maxRotation;
                    transform.rotation = Quaternion.Euler(rotation);
                }
                else
                {
                    transform.Rotate(rotation * rotationSpeed);
                }
                Vector3 direction3 = new(direction.x, direction.y, 0);
                transform.position += direction3 * speed;
            }
        }
    }
}
