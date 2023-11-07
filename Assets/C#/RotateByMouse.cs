using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByMouse : MonoBehaviour
{
    public float rotateSpeedX=50,rotateSpeedY=50;//旋转XY
    public Camera curCamera
        ;//摄像机
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public Vector2 cameraRotateRange=new Vector2(-40,60);
    float curAngle = 0;
    public float getCurAngle
    {
        get
        {
            return curAngle;
        }
    }
    // Update is called once per frame
    void Update()
    {
        ////判断鼠标是否显示
        //if (Cursor.visible)
        //{
        //    return;
        //}
        //获取鼠标旋转
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        transform.Rotate(0,x*rotateSpeedX*Time.deltaTime,0);
       // curCamera.transform.Rotate(-y* rotateSpeedY, 0,0);
        Vector3 cameraRotate = curCamera.transform.localRotation.eulerAngles;
        if (cameraRotate.x > 180)
        {
            cameraRotate.x -= 360;
        }
        else if (cameraRotate.x < -180)
       
        {
            cameraRotate.x += 360;
        }
       // Debug.Log("旋转前"+cameraRotate.x);
        cameraRotate.x += -y * rotateSpeedY*Time.deltaTime;
        //Debug.Log("旋转后"+cameraRotate.x);
        cameraRotate.x = Mathf.Clamp(cameraRotate.x, cameraRotateRange.x, cameraRotateRange.y);
        //Debug.Log("控制后" + cameraRotate.x);
        curAngle = cameraRotate.x;
        curCamera.transform.localRotation = Quaternion.Euler(cameraRotate);
    }
}
