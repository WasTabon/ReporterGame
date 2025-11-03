using System;
using TMPro;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    [SerializeField] private GameObject[] _levels;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private GameObject _buyPanel;
    [SerializeField] private TextMeshProUGUI _buyText;
    
    private int _currentLevel = 1;

    private void Start()
    {
        _currentLevel = PlayerPrefs.GetInt("level", 1);
        for (int i = 0; i < _currentLevel; i++)
        {
            _levels[_currentLevel - 1].SetActive(true);
        }
    }

    private void Update()
    {
        _levelText.text = $"OFFICE LEVEL - {_currentLevel}";
    }

    public void Upgrade()
    {
        if (WalletController.Instance.Money >= 1500)
        {
            if (_levels[_currentLevel - 1] != null)
            {
                _levels[_currentLevel - 1].SetActive(true);
                _currentLevel++;
                PlayerPrefs.SetInt("level", _currentLevel);
            }
        }
    }
}
