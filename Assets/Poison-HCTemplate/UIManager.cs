using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>, IEventScripts
{
    [SerializeField] Transform _menuPanel;
    [SerializeField] Transform _mainPanel;
    [SerializeField] Transform _winPanel;

    [SerializeField] Transform _playButton;
    [SerializeField] Text _menuLevelCount;
    [SerializeField] Text _mainLevelCount;
    [SerializeField] Text _gemCount;

    public Transform MenuPanel { get { return _menuPanel; } }
    public Transform MainPanel { get { return _mainPanel; } }
    public Transform WinPanel { get { return _winPanel; } }
    public Transform PlayButton { get { return _playButton; } }
    public Text MenuLevelCount { get { return _menuLevelCount; } }
    public Text MainLevelCount { get { return _mainLevelCount; } }
    public Text GemCount { get { return _gemCount; } }

    void Start()
    {
        GameManager.Instance.EventMenu += OnMenu;
        GameManager.Instance.EventPlay += OnPlay;
        GameManager.Instance.EventFinish += OnFinish;
        GameManager.Instance.EventLose += OnLose;
    }
    public void OnMenu()
    {
        MenuPanel.gameObject.SetActive(true);
        WinPanel.gameObject.SetActive(false);
        _menuLevelCount.text = (LevelManager.Instance.CurrentLevel + 1).ToString();
    }

    public void OnPlay()
    {
        MenuPanel.gameObject.SetActive(false);
        MainPanel.gameObject.SetActive(true);
        _mainLevelCount.text = (LevelManager.Instance.CurrentLevel + 1).ToString();
    }

    public void OnFinish()
    {
        MainPanel.gameObject.SetActive(false);
        WinPanel.gameObject.SetActive(true);
    }
    public void OnLose()
    {

    }
    public void RudderLockButton()
    {
        PlayerController.Instance.RudderLock = false;
    }

}
