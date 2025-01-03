﻿using UnityEngine;
using System.Collections;

class Game_PublicClassVar
{
    //获取XMLScript脚本对象
    private static XMLScript Var_XmlScript = null; 

    public static XMLScript Get_xmlScript {

        get {

            if (Var_XmlScript == null) {

                Var_XmlScript = new XMLScript();
            }
            return Var_XmlScript;
        }
    }
     
    //获取Game_PositionVar脚本对象
    private static GameObject gameStartVar = GameObject.FindWithTag("Tag_GameStartVar");
    private static Game_PositionVar game_PositionVar = gameStartVar.GetComponent<Game_PositionVar>();
    //private static bool ifGet;
    public static Game_PositionVar Get_game_PositionVar {
        get {
            //if (ifGet) {
            if (gameStartVar == null) {
                gameStartVar = GameObject.FindWithTag("Tag_GameStartVar");
            }
                
            game_PositionVar = gameStartVar.GetComponent<Game_PositionVar>();
                //ifGet = true;
            //}

            return game_PositionVar;
        }
    }


    //获取Function_DataSet脚本对象
    private static Function_DataSet function_DataSet;
    public static Function_DataSet Get_function_DataSet
    {
        get
        {
            if (function_DataSet == null)
            {
                function_DataSet = new Function_DataSet();
            }
            return function_DataSet;
        }
    }
    
    //获取Function_UI脚本对象
    private static Function_UI function_UI = null;
    public static Function_UI Get_function_UI
    {
        get
        {
            if (function_UI == null)
            {
                function_UI = new Function_UI();
            }
            return function_UI;
        }
    }
    
    //获取WWWSet脚本对象
    
    private static GameObject gameVar1 = GameObject.FindWithTag("Tag_WWWSet");
    private static WWWSet wwwSet = gameVar1.GetComponent<WWWSet>();
    public static WWWSet Get_wwwSet
    {
        get
        {
            //gameVar1 = GameObject.FindWithTag("Tag_WWWSet");
            //wwwSet = gameVar1.GetComponent<WWWSet>();
            return wwwSet;
        }
    }

    //获取GameLinkServer脚本对象

    private static GameObject gameLinkServerObj = GameObject.FindWithTag("Tag_WWWSet");
    private static GameLinkServer gameLinkServer = gameLinkServerObj.GetComponent<GameLinkServer>();
    public static GameLinkServer Get_gameLinkServerObj
    {
        get
        {
            //gameVar1 = GameObject.FindWithTag("Tag_WWWSet");
            //wwwSet = gameVar1.GetComponent<WWWSet>();
            return gameLinkServer;
        }
    }

    //获取GameLinkServer脚本对象

    private static GameObject gameServerMessageObj = GameObject.FindWithTag("Tag_WWWSet");
    private static GameServerMessage gameServerMessage = gameServerMessageObj.GetComponent<GameServerMessage>();
    public static GameServerMessage Get_GameServerMessage
    {
        get
        {
            //gameVar1 = GameObject.FindWithTag("Tag_WWWSet");
            //wwwSet = gameVar1.GetComponent<WWWSet>();
            return gameServerMessage;
        }
    }

    //获取Fight_Formult脚本对象
    private static Fight_Formult fight_Formult = null;
    public static Fight_Formult Get_fight_Formult
    {
        get
        {
            if (fight_Formult == null)
            {
                fight_Formult = new Fight_Formult();
            }
            return fight_Formult;
        }
    }

    //获取Function_Rose脚本对象
    private static Function_Rose function_Rose = null;
    public static Function_Rose Get_function_Rose
    {

        get
        {

            if (function_Rose == null)
            {

                function_Rose = new Function_Rose();

            }

            return function_Rose;
        }

    }

    //获取Function_Rose脚本对象
    private static Function_AI function_AI = null;
    public static Function_AI Get_function_AI
    {

        get
        {

            if (function_AI == null)
            {

                //function_AI = new Function_AI();
                function_AI = Game_PublicClassVar.wwwSet.GetComponent<Function_AI>();
                //function_AI = 
                //function_AI = function_AI
                
            }

            return function_AI;
        }

    }

    //获取Function_Task脚本对象
    private static Function_Task function_task = null;
    public static Function_Task Get_function_Task
    {
        get
        {
            if (function_task == null)
            {
                function_task = new Function_Task();
            }
            return function_task;
        }
    }

    //获取Function_Skill脚本对象
    private static Function_Skill function_skill = null;
    public static Function_Skill Get_function_Skill
    {
        get
        {
            if (function_skill == null)
            {
                function_skill = new Function_Skill();
            }
            return function_skill;
        }
    }

    //获取Function_MonsterSkill脚本对象
    private static Function_MonsterSkill function_monsterSkill = null;
    public static Function_MonsterSkill Get_function_MonsterSkill
    {
        get
        {
            if (function_monsterSkill == null)
            {
                function_monsterSkill = new Function_MonsterSkill();
            }
            return function_monsterSkill;
        }
    }

    //获取Function_Building脚本对象
    private static Function_Building function_Building = null;
    public static Function_Building Get_function_Building
    {
        get
        {
            if (function_Building == null)
            {
                function_Building = new Function_Building();
            }
            return function_Building;
        }
    }

    //获取Function_Country脚本对象
    private static Function_Country function_Country = null;
    public static Function_Country Get_function_Country
    {
        get
        {
            if (function_Country == null)
            {
                function_Country = new Function_Country();
            }
            return function_Country;
        }
    }
}
