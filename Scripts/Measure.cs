using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Measure : MonoBehaviour
{
    public bool printData;
    public bool drawRect;
    public List<Vector3> points = new();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (printData)
        {
            printData = false;
            print(transform.position);
            points.Add(transform.position);
        }
        if (drawRect)
        {
            if(points.Count > 1) 
            {
                Debug.DrawLine(points[0], points[points.Count - 1],Color.yellow);
            }
        }
    }
}
