using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class GameManager : MonoBehaviour
{
    [Inject]
    private readonly UIManager _uiManager;
    [Inject]
    private readonly FloatingJoystick _floatingJoystick;
    [Inject]
    private readonly ZombieSpawnerManager _zombieSpawnerManager;
    [Inject]
    private readonly Player _player;
    private bool _isEndLevel = false;
    private void Start()
    {
        _uiManager.GetPanel(IngamePanel.PanelId).Show();
    }
    public void OnWin()
    {
        if (_isEndLevel) return;
        _isEndLevel = true;
        _uiManager.GetPanel(WinPanel.PanelId).Show(1.5f);
        _floatingJoystick.enabled = false;
        _zombieSpawnerManager.enabled = false;
    }
    public void OnLose()
    {
        if (_isEndLevel) return;
        _isEndLevel = true;
        _uiManager.GetPanel(LosePanel.PanelId).Show(1.5f);
        _floatingJoystick.enabled = false;
        _zombieSpawnerManager.enabled = false;
    }
    private void Update()
    {
        if (_isEndLevel) return;
        if (_zombieSpawnerManager.HasClearLevel)
            OnWin();
        if (!_player.IsAlive()) 
            OnLose();
    }
}
