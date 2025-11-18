using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _moneyText2;
    [SerializeField] private TextMeshProUGUI _reputationText;

    private void Update()
    {
        _moneyText.text = $"{WalletController.Instance.Money}";
        _moneyText2.text = $"{WalletController.Instance.Money}";
        _reputationText.text = $"{WalletController.Instance.Reputation}";
    }
}
