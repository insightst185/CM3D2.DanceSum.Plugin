using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;

namespace DanceSum.Plugin
{
    [PluginFilter("CM3D2x64"), PluginFilter("CM3D2x86"), PluginFilter("CM3D2VRx64"),
     PluginName("DanceSum"), PluginVersion("0.0.0.1")]

    public class DanceSum : PluginBase
    {
//        private XmlManager xmlManager;
        private int iSceneLevel;
        private DanceMain danceMain = null;
        private FieldInfo field;
        private Boolean m_eModeFlag;
        private Vector3 rotate = new Vector3(0.0f,180.0f,0.0f);

        private enum TargetLevel
        {
            SceneDance_SUM = 37
        }

        private const int MAX_LISTED_MAID = 3;
        private Maid[] maid = new Maid[MAX_LISTED_MAID];
        private Transform cameraTransform;
        private Transform lightTransform;
        public void Awake()
        {
        }

        public void OnDestroy()
        {

        }

        public void OnLevelWasLoaded(int level)
        {
            iSceneLevel = level;
            m_eModeFlag = false;
            danceMain = (DanceMain)FindObjectOfType(typeof(DanceMain));
        }

        public void Start()
        {
        }

        public void Update()
        {

            if (!Enum.IsDefined(typeof(TargetLevel),iSceneLevel)) return;
            if (m_eModeFlag == false){
                field = (typeof(DanceMain)).GetField("m_eMode", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                int m_eMode = (int)field.GetValue(danceMain);
                if(m_eMode == 3){
                    m_eModeFlag = true;
                    for (int i = 0; i < MAX_LISTED_MAID; i++){
                        maid[i] = GameMain.Instance.CharacterMgr.GetMaid(i);
                        maid[i].SetRot(maid[i].GetRot() + rotate);
                    }
                    cameraTransform = GameMain.Instance.MainCamera.transform;
                    lightTransform = GameMain.Instance.MainLight.gameObject.transform;

                if(lightTransform.eulerAngles.y < 180.0f){
                   lightTransform.eulerAngles = lightTransform.eulerAngles + rotate;
                }
                else{
                   lightTransform.eulerAngles = lightTransform.eulerAngles - rotate;
                }
                lightTransform.position = new Vector3(lightTransform.position.x * -1.0f,
                                                      lightTransform.position.y,
                                                      lightTransform.position.z * -1.0f);
                }
            }
        }

        public void LateUpdate()
        {

            if (!Enum.IsDefined(typeof(TargetLevel),iSceneLevel)) return;
            if (m_eModeFlag == true){
                if(cameraTransform.eulerAngles.y < 180.0f){
                   cameraTransform.eulerAngles = cameraTransform.eulerAngles + rotate;
                }
                else{
                   cameraTransform.eulerAngles = cameraTransform.eulerAngles - rotate;
                }
                cameraTransform.position = new Vector3(cameraTransform.position.x * -1.0f,
                                                       cameraTransform.position.y,
                                                       cameraTransform.position.z * -1.0f);
            }
        }

        //------------------------------------------------------xml--------------------------------------------------------------------
    }
}
