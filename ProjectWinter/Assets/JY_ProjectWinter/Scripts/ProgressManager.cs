using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public AwakeNoticeCanvas repairScript;
    public AwakeNoticeCanvas callHeliScript;
    public AwakeNoticeCanvas escapeHeliScript;

    // Start is called before the first frame update
    void Start()
    {
        repairScript.enabled = false;
        callHeliScript.enabled = false;
        escapeHeliScript.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenRepairHeliPad()
    {
        repairScript.enabled = true;
    }

    public void OpenCallHeliRadio()
    {
        callHeliScript.enabled = true;
    }

    public void OpenHelicopter()
    {
        escapeHeliScript.enabled = true;
    }
}
