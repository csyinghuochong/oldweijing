﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Map : MonoBehaviour {

    private Camera MapCamera;
    public GameObject RoseImgObj;
    public GameObject Obj_DoorWaySet;       //场景传送点
    public GameObject Obj_MapNameSet;       //场景名称父级
    public GameObject Obj_MapName;          //场景名称源文件

    public GameObject Obj_RoseNameText;
    // Use this for initialization
    void Start () {

        //Debug.Log("Application.loadedLevelName = " + Application.loadedLevelName);
        string MapCameraPositionStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapCameraPosition", "ID", Application.loadedLevelName, "Scene_Template");
        //MapCameraPositionStr = "15;120;-50;0";
        if (MapCameraPositionStr != null && MapCameraPositionStr != "0")
        {
            string[] MapCameraPosition = MapCameraPositionStr.Split(';');
            Game_PublicClassVar.Get_game_PositionVar.Obj_MapCamera.SetActive(true);
            MapCamera = Game_PublicClassVar.Get_game_PositionVar.Obj_MapCamera.GetComponent<Camera>();
            MapCamera.transform.localPosition = new Vector3(float.Parse(MapCameraPosition[0]), float.Parse(MapCameraPosition[1]), float.Parse(MapCameraPosition[2]));
            MapCamera.transform.localRotation = Quaternion.Euler(90, float.Parse(MapCameraPosition[3]), 0);
        }

        //获得当前场景传送的物体
        Obj_DoorWaySet = GameObject.Find("DoorWaySet").gameObject;
        for (int i = 0; i < Obj_DoorWaySet.transform.childCount; i++)
        {
            GameObject doorWayObj = Obj_DoorWaySet.transform.GetChild(i).gameObject;

            if (doorWayObj.active == true)
            {
                Debug.Log(doorWayObj.name + "子物体");
                string scenceID = doorWayObj.GetComponent<DoorWay>().DoorWayID;
                string mapID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", scenceID, "SceneTransfer_Template");
                string mapName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", mapID, "Scene_Template");
                Debug.Log("MapName = " + mapName);

                //设置地图的位置
                Vector3 vec3_MapName = MapCamera.WorldToViewportPoint(doorWayObj.transform.position);
                vec3_MapName = ToScreenV2(vec3_MapName);
                GameObject objMapName = (GameObject)Instantiate(Obj_MapName);
                objMapName.transform.SetParent(Obj_MapNameSet.transform);
                objMapName.transform.localScale = new Vector3(1, 1, 1);
                objMapName.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(vec3_MapName.x, vec3_MapName.y, 0);
                objMapName.GetComponent<UI_MapName>().Obj_MapNameLab.GetComponent<Text>().text = "前往：" + mapName;

            }
        }

        if (RenderSettings.fog)
        {
            RenderSettings.fog = false;
        }

        //读取玩家名称显示
        string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_RoseNameText.GetComponent<Text>().text = roseName;

    }
	
	// Update is called once per frame
	void Update () {

        //设置角色的当前位置
        Vector3 vec3_BuildingName = MapCamera.WorldToViewportPoint(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
        vec3_BuildingName = ToScreenV2(vec3_BuildingName);
        RoseImgObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(vec3_BuildingName.x, vec3_BuildingName.y, 0);





	}


    //传入屏幕的百分比位置，显示当前对应的位置
    public Vector3 ToScreenV2(Vector3 v3)
    {
        //获取当前屏幕的坐标
        int screen_X = 800;
        int screen_Y = 500;
        //Debug.Log("screen_X = " + screen_X + ";" + screen_Y);

        Vector3 UI_V3 = new Vector3();

        UI_V3.x = v3.x * 800;
        UI_V3.y = v3.y * 800 * screen_Y / screen_X;

        return UI_V3;
    }

    public void close_Map() {
        //Game_PublicClassVar.Get_game_PositionVar.Obj_MapCamera.SetActive(false);
        //Destroy(this.gameObject);
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_Map();
        
    }

    //自身销毁时调用
    void OnDestroy()
    {
        Debug.Log("隐藏地图");
        RenderSettings.fog = true;
        Game_PublicClassVar.Get_game_PositionVar.Obj_MapCamera.SetActive(false);
    }


    //获取点击地图的位置
    public void ClickMap() {

        Vector3 click = Input.mousePosition;
        float scene_pro_x = Screen.width / 1366.0f;
        float scene_pro_y = Screen.height / 768.0f;
        //点击坐标修正
        click = new Vector3(Input.mousePosition.x - 283.1f * scene_pro_x, Input.mousePosition.y - 98.2f * scene_pro_y, 100);
        click = new Vector3(click.x / (800.0f * scene_pro_x), click.y / (500.0f* scene_pro_y), 100);
        Vector3 worldPosi = MapCamera.ViewportToWorldPoint(click);

        
        //设置深度
       //从摄像机处向点击的目标处发送一条射线
        Ray Move_Target_Ray = MapCamera.ScreenPointToRay(worldPosi);

        //声明一个光线触碰器
        RaycastHit Move_Target_Hit;
        //LayerMask mask = (1 << 8) | (1 << 9) | (1 << 10) | (1 << 12) | (1 << 13);
        LayerMask mask = (1 << 10);
        Debug.Log("0000");
        //检测射线是否碰触到对象
        //标记OUT变量在传入参数进去后可能发生改变，和ref类似，但是ref需要给他一个初始值
        //第一个参数射线变量  第二个参数光线触碰器的反馈变量
        if (Physics.Raycast(Move_Target_Ray, out Move_Target_Hit, 10000, mask.value))
        {
            Debug.Log("1111");
            //获取碰撞地面
            if (Move_Target_Hit.collider.gameObject.layer == 10)
            {
                Debug.Log("2222");

                //将碰触的三维坐标进行赋值
                //Move_Target_Position = Move_Target_Hit.point;
                Debug.Log("Move_Target_Hit.point = " + Move_Target_Hit.point);
                float dis = Vector3.Distance(MapCamera.transform.position, Move_Target_Hit.point);
                dis = dis - 20.0f;
                //Debug.Log("dis = " + dis);
                click = new Vector3(click.x, click.y, dis);
                worldPosi = MapCamera.ViewportToWorldPoint(click);
                //Debug.Log("worldPosi = " + worldPosi);
                worldPosi.y = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position.y;

            }
        }

        //移动坐标
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseMapMoveStatus = true;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseMapMoveVec3 = worldPosi;

        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("小地图移动开启！");
        
        /*
        click = new Vector3(330, 365, 0);
        worldPosi = MapCamera.ViewportToWorldPoint(click);
        Debug.Log("我点击了地图worldPosi = " + worldPosi);
        */
    }

}
