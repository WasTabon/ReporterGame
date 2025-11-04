using TMPro;
using UnityEngine;

public class ReputationController : MonoBehaviour
{
    [SerializeField] private int _level;
    [SerializeField] private GameObject _buyPanel;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _reputationText;

    private float _timer;
    
    private void Update()
    {
        _reputationText.text = $"{WalletController.Instance.Reputation}";
        
        _timer += Time.deltaTime;
        
        if (_timer >= 1f)
        {
            _timer = 0;
            
            WalletController.Instance.Money += _level;
        }
    }

    public void Upgrade()
    {
        if (WalletController.Instance.Reputation >= 3000)
        {
            WalletController.Instance.Reputation -= 3000;
            _level++;
            _text.text = "YOU SPENT REPUTATION";
            _buyPanel.SetActive(true);
        }
        else
        {
            _text.text = "YOU DON'T HAVE ENOUGH REPUTATION";
            _buyPanel.SetActive(true);
        }
    }
}