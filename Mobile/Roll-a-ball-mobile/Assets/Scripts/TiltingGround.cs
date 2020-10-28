using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.InputSystem;

public class TiltingGround : MonoBehaviour
{
    public bool isUseOldInput;
    private Quaternion initialQuaternion;
    public float weight;
    public Text gyroText;

    void Start()
    {
        if (isUseOldInput)
        {

            UnityEngine.Input.gyro.enabled = true;
            initialQuaternion = UnityEngine.Input.gyro.attitude;

        } else
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.AttitudeSensor.current);
            initialQuaternion = UnityEngine.InputSystem.AttitudeSensor.current.attitude.ReadValue();

        }


    }


    void Update()
    {
        Quaternion q_sensor = Quaternion.identity;
        if (isUseOldInput)
        {
            q_sensor = UnityEngine.Input.gyro.attitude;
        } else
        {
           q_sensor = UnityEngine.InputSystem.AttitudeSensor.current.attitude.ReadValue();

        }


        Vector3 newvect = GyroToUnity(Quaternion.Inverse(initialQuaternion) * q_sensor).eulerAngles;
        if (newvect.x > weight) newvect.x = weight;
        else if (newvect.x < -weight) newvect.x = -weight;
        if (newvect.z > weight) newvect.z = weight;
        else if (newvect.z < -weight) newvect.z = -weight;
        newvect.y = 0f;
        this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, newvect, 2.0f);
        gyroText.text = this.transform.rotation.eulerAngles.ToString("F6");
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {

        return new Quaternion(-q.x, -q.z, -q.y, q.w);
    }
}
