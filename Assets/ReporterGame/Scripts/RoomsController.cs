using TMPro;
using UnityEngine;
using System.Collections;

public class RoomsController : MonoBehaviour
{
    public static RoomsController Instance;

    [SerializeField] private GameObject[] rooms;
    [SerializeField] private int[] roomPrices = { 100, 250, 500, 1000 };

    [SerializeField] private GameObject _buyPanel;
    [SerializeField] private TextMeshProUGUI _buyText;
    
    [SerializeField] private float panelDisplayDuration = 2f;
    
    private void Awake()
    {
        Instance = this;
        
        if (PlayerPrefs.GetInt("room_0", -1) == -1)
        {
            PlayerPrefs.SetInt("room_0", 1);
            PlayerPrefs.Save();
        }
        
        LoadRooms();
        
        if (_buyPanel != null)
        {
            _buyPanel.SetActive(false);
        }
        else
        {
            Debug.Log("_buyPanel is null");
        }
    }

    private void LoadRooms()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            bool isPurchased = PlayerPrefs.GetInt($"room_{i}", 0) == 1;
            rooms[i].SetActive(isPurchased);
        }
    }

    public void BuyRoom0()
    {
        BuyRoom(0);
    }

    public void BuyRoom1()
    {
        BuyRoom(1);
    }

    public void BuyRoom2()
    {
        BuyRoom(2);
    }

    public void BuyRoom3()
    {
        BuyRoom(3);
    }

    private void BuyRoom(int roomIndex)
    {
        if (roomIndex < 0 || roomIndex >= rooms.Length)
        {
            Debug.Log("Invalid room index");
            return;
        }

        if (PlayerPrefs.GetInt($"room_{roomIndex}", 0) == 1)
        {
            ShowBuyPanel("Room already purchased!");
            return;
        }

        if (WalletController.Instance != null)
        {
            if (WalletController.Instance.Money >= roomPrices[roomIndex])
            {
                WalletController.Instance.Money -= roomPrices[roomIndex];
                PlayerPrefs.SetInt($"room_{roomIndex}", 1);
                PlayerPrefs.Save();
                rooms[roomIndex].SetActive(true);
                
                if (InterviewController.Instance != null)
                {
                    InterviewController.Instance.HideButtonInRoom(rooms[roomIndex]);
                }
                else
                {
                    Debug.Log("InterviewController.Instance is null");
                }
                
                ShowBuyPanel("Room purchased successfully!");
            }
            else
            {
                ShowBuyPanel("Not enough money!");
            }
        }
        else
        {
            Debug.Log("WalletController.Instance is null");
        }
    }

    private void ShowBuyPanel(string message)
    {
        if (_buyPanel != null && _buyText != null)
        {
            _buyText.text = message;
            _buyPanel.SetActive(true);
            StartCoroutine(HideBuyPanelAfterDelay());
        }
        else
        {
            Debug.Log("_buyPanel or _buyText is null");
        }
    }

    private IEnumerator HideBuyPanelAfterDelay()
    {
        yield return new WaitForSeconds(panelDisplayDuration);
        
        if (_buyPanel != null)
        {
            _buyPanel.SetActive(false);
        }
        else
        {
            Debug.Log("_buyPanel is null");
        }
    }

    public GameObject[] GetPurchasedRooms()
    {
        int count = 0;
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].activeSelf)
                count++;
        }

        GameObject[] purchasedRooms = new GameObject[count];
        int index = 0;
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].activeSelf)
            {
                purchasedRooms[index] = rooms[i];
                index++;
            }
        }

        return purchasedRooms;
    }
}