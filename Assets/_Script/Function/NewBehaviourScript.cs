using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       print(Camera.main.WorldToScreenPoint(transform.position)); 
    }

    // Update is called once per frame
    void Update()
    {
        while (Input.GetMouseButton(0))
        {
            print(Input.mousePosition);
        }
    }
}
