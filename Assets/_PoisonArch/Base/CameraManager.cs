using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoisonArch
{
    public class CameraManager : AbstractSingleton<CameraManager>, IEventScripts
    {
        [SerializeField] List<Transform> _cameras;
        [SerializeField] Transform _playCam;
        [SerializeField] Transform _menuCam;

        public Camera MainUsedCamera;
        void Start()
        {
            GameManager.Instance.EventMenu += OnMenu;
            GameManager.Instance.EventPlay += OnPlay;
            GameManager.Instance.EventFinish += OnFinish;
            GameManager.Instance.EventLose += OnLose;

            //getCamera
            for (int i = 0; i < transform.childCount; i++)
            {
                _cameras.Add(transform.GetChild(i));
            }
        }

        void SetActiveCamera(Transform activeCamera)
        {
            int _priority = _cameras.Count;
            for (int i = 0; i < _cameras.Count; i++)
            {
                if (_cameras[i] == transform)
                    continue;

                if (_cameras[i] == activeCamera)
                {
                    _cameras[i].GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = _cameras.Count;
                    _priority--;
                }
                else
                {
                    _priority--;
                    _cameras[i].GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = _priority;
                }
            }

            MainUsedCamera = _cameras[_priority].GetComponent<Camera>();
        }

        public void OnMenu()
        {
            //SetActiveCamera(_menuCam);
        }

        public void OnPlay()
        {
            SetActiveCamera(_playCam);
        }

        public void OnFinish()
        {
            SetActiveCamera(_menuCam);
        }
        public void OnLose()
        {
            SetActiveCamera(_menuCam);
        }
    }
}
