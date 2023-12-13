using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class ArduinoReceiver : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM6", 9600);

    // Start is called before the first frame update
    void Start()
    {
        sp.Open();
        /*
            Set the read timeout low so unity doesn't freeze,
            and catch the exception below in update that unity will throw
            when the port isn't open and unity tries to check it
        */
        sp.ReadTimeout = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sp.IsOpen)
        {
            try
            {
                ButtonPressed(sp.ReadLine());
            }
            catch (System.Exception)
            {

            }
        }
    }

    private void ButtonPressed(string dist)
    {
        int distance = int.Parse(dist);
        float adjustedDistance = distance / 1000;
        adjustedDistance = Mathf.Abs(adjustedDistance);
        if (adjustedDistance <= 1)
        {

        }
        else
        {
            Debug.Log(adjustedDistance);
        }
    }
}
