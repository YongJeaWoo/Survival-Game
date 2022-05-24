using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CAm : MonoBehaviour
{
    [SerializeField]
    GameObject m_cam;
    CinemachineFreeLook look;

    private void Awake()
    {
        look = m_cam.GetComponent<CinemachineFreeLook>();
        look.Priority = 7;
        look.Priority = 9;
    }
}
