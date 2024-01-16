using UnityEngine;
using System.Collections;

public class CameraAI : MonoBehaviour
{

    public GameObject FlowObj;  //跟随移动的物体
    //private float cushionSpeed;     //缓冲移动速度
    //private RoseStatus roseObj;
    private bool storyStatus;       //进入故事状态,镜头拉近
    private bool storyExitStatus;   //退出故事状态,镜头拉远
    public bool BuildEnterStatus;   //进入建筑状态
    public bool BuildExitStatus;   //退出建筑状态
    public GameObject BuildMoveObj;       //移动到的建筑点
    private float cameraMoveTime;
    private float cameraMoveExitTime;
    private Rose_Status roseStatus;
    private Vector3 EnterGameCameraPosition;
    // Use this for initialization
    void Start()
    {
        roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        //EnterGameCameraPosition = new Vector3(-5f, 40f, -21f);
        EnterGameCameraPosition = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //获取当前角色是否进入故事模式
        storyStatus = roseStatus.RoseStorySpeakStatus;
        //故事模式,镜头移动
        if (storyStatus)
        {
            cameraMoveTime = cameraMoveTime + Time.deltaTime*2;
            if (cameraMoveTime <= 1)
            {
                transform.localPosition = new Vector3(FlowObj.transform.position.x, 37.0f - 5 * cameraMoveTime, FlowObj.transform.position.z - 7f + 2 * cameraMoveTime);
                transform.localRotation = Quaternion.Euler(new Vector3(45 - 30 * cameraMoveTime, 0, 0));
                storyExitStatus = true;
                //清空镜头移动的计数器
                cameraMoveExitTime = 0;
            }
        }
        else {
            //退出故事模式,镜头移动
            if (storyExitStatus)
            {
                cameraMoveExitTime = cameraMoveExitTime + Time.deltaTime*2;
                if (cameraMoveExitTime <= 1)
                {
                    transform.localPosition = new Vector3(FlowObj.transform.position.x, 32.0f + 5 * cameraMoveExitTime, FlowObj.transform.position.z - 5f - 2 * cameraMoveExitTime);
                    transform.localRotation = Quaternion.Euler(new Vector3(15 + 30 * cameraMoveExitTime, 0, 0));
                    //storyExitStatus = true;
                    //清空镜头移动的计数器
                    cameraMoveTime = 0;
                }
                else {
                    storyExitStatus = false;
                }
            }
            else {
                //普通进入游戏摄像机状态
                if (Game_PublicClassVar.Get_game_PositionVar.EnterGameStatus)
                {
                    //进入游戏登陆界面
                    /*   Old
                    transform.localPosition = new Vector3(1.5f, 30.3f, -15.66f);
                    transform.localRotation = Quaternion.Euler(new Vector3(6.9159f, 0, 0));
                     */
                    //建筑模式,镜头移动
                    if (BuildEnterStatus)
                    {
                        cameraMoveTime = cameraMoveTime + Time.deltaTime * 2;
                        if (cameraMoveTime <= 1)
                        {
                            //Debug.Log("开始移动");
                            Vector3 chaV3 = BuildMoveObj.transform.position - EnterGameCameraPosition;
                            //Debug.Log("chaV3 : " + chaV3);
                            transform.localPosition = new Vector3(EnterGameCameraPosition.x + chaV3.x * cameraMoveTime, EnterGameCameraPosition.y + chaV3.y * cameraMoveTime, EnterGameCameraPosition.z + chaV3.z * cameraMoveTime);
                            transform.localRotation = Quaternion.Euler(new Vector3(45 - 45 * cameraMoveTime, 45 - 45 * cameraMoveTime, 0));

                            //清空镜头移动的计数器
                            //cameraMoveExitTime = 0;
                        }
                    }
                    else {

                        if (BuildExitStatus)
                        {
                            cameraMoveExitTime = cameraMoveExitTime + Time.deltaTime * 2;
                            if (cameraMoveExitTime <= 1)
                            {
                                Vector3 chaV3 = BuildMoveObj.transform.position - EnterGameCameraPosition;
                                //Debug.Log("chaV3 : " + chaV3);
                                transform.localPosition = new Vector3(BuildMoveObj.transform.position.x - chaV3.x * cameraMoveExitTime, BuildMoveObj.transform.position.y - chaV3.y * cameraMoveExitTime, BuildMoveObj.transform.position.z - chaV3.z * cameraMoveExitTime);
                                transform.localRotation = Quaternion.Euler(new Vector3(0 + 45 * cameraMoveExitTime, 0 + 45 * cameraMoveExitTime, 0));

                            }
                            else {
                                BuildExitStatus = false;
                                BuildEnterStatus = false;
                                cameraMoveTime = 0;
                                cameraMoveExitTime = 0;
                                //Debug.Log("值清空");
                            }
                        }

                        else {
                            transform.localPosition = EnterGameCameraPosition;
                            transform.localRotation = Quaternion.Euler(new Vector3(45, 45, 0));
                        }
                    }
                }
                else
                {
                    //进入实际游戏界面
                    //transform.localPosition = new Vector3(FlowObj.transform.position.x, 37.0f, FlowObj.transform.position.z - 7f);
                    transform.localPosition = new Vector3(FlowObj.transform.position.x, FlowObj.transform.position.y+9.0f, FlowObj.transform.position.z - 7f);
                    transform.localRotation = Quaternion.Euler(new Vector3(45, 0, 0));
                }
            }
        }
    }
}
