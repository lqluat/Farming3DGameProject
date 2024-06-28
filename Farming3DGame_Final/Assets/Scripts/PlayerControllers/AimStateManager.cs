using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TreeEditor;
using Unity.VisualScripting;

public class AimStateManager : MonoBehaviour
{
    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] 
    Transform cameraFollowPosition;
    public GameObject inventoryPanel;

    // Start is called before the first frame update
    void Start()
    {
        xAxis.m_MaxSpeed = 300f;
        xAxis.m_MaxValue = 180f;
        xAxis.m_MinValue = -180f;
        xAxis.m_InputAxisName = "Mouse X";
        xAxis.m_Wrap = true;

        yAxis.m_MaxSpeed = 300f;
        yAxis.m_MaxValue = 60f;
        yAxis.m_MinValue = -60f;
        yAxis.m_InputAxisName = "Mouse Y";
        yAxis.m_InvertInput = true;

    }

    // Update is called once per frame
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime); 
    }
    private void LateUpdate()
    {
        if (inventoryPanel.activeInHierarchy == false)
        {
            cameraFollowPosition.localEulerAngles = new Vector3(yAxis.Value, cameraFollowPosition.localEulerAngles.y, cameraFollowPosition.localEulerAngles.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
        }
        if(xAxis.Value == xAxis.m_MaxValue)
        {
            xAxis.Value = xAxis.m_MinValue;
        } else if(xAxis.Value == xAxis.m_MinValue)
        {
            xAxis.Value = xAxis.m_MaxValue;
        }
    }
}
