using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScroll : MonoBehaviour
{
    public float slot = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        if (scrollInput > 0f)
        {
            float verticalMovement = scrollInput * 10;

            slot += verticalMovement;
            invenCheck();
        }
        else if (scrollInput < 0f)
        {
            float verticalMovement = scrollInput * 10;

            slot += verticalMovement;
            invenCheck();
        }
    }

    private void invenCheck()
    {
        if (slot < 1)
        {
            slot = 4;
        }
        else if (slot > 4)
        {
            slot = 1;
        }
    }
}
