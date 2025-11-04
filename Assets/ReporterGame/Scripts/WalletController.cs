using UnityEngine;

public class WalletController : MonoBehaviour
{
    public static WalletController Instance;

    public int Money
    {
        get => _money;

        set
        {
            _money = value;
            PlayerPrefs.SetInt("money", _money);
            PlayerPrefs.Save();
        }
    }
    
    private int _money;
    
    public int Reputation
    {
        get => _reputation;

        set
        {
            _reputation = value;
            PlayerPrefs.SetInt("reputation", _reputation);
            PlayerPrefs.Save();
        }
    }
    
    private int _reputation;
    
    private void Awake()
    {
        Instance = this;
        
        _money = PlayerPrefs.GetInt("money", 100);
        _reputation = PlayerPrefs.GetInt("reputation", 100);
    }

    [ContextMenu("Add Money")]
    public void AddMoney()
    {
        Money += 5000;
    }
}
