using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

[System.Serializable]
public class ArticleData
{
    public string header;
    public string description;
    public string iconName;
    public int earnedMoney;
}

public class ArticleManager : MonoBehaviour
{
    public static ArticleManager Instance;

    [Header("Article Card")]
    [SerializeField] private GameObject articleCardPrefab;
    [SerializeField] private Transform content;

    [Header("Article View Panel")]
    [SerializeField] private GameObject articleViewPanel;
    [SerializeField] private TextMeshProUGUI viewPanelHeader;
    [SerializeField] private TextMeshProUGUI viewPanelDescription;
    [SerializeField] private Image viewPanelIcon;

    private List<ArticleData> articles = new List<ArticleData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("ArticleManager instance already exists");
        }
    }

    private void Start()
    {
        LoadArticles();
    }

    public void AddArticle(string header, string description, Sprite icon, int earnedMoney)
    {
        ArticleData newArticle = new ArticleData
        {
            header = header,
            description = description,
            iconName = icon.name,
            earnedMoney = earnedMoney
        };

        articles.Add(newArticle);
        SaveArticles();
        CreateArticleCard(newArticle);
    }

    private void CreateArticleCard(ArticleData articleData)
    {
        if (articleCardPrefab != null && content != null)
        {
            GameObject cardObject = Instantiate(articleCardPrefab, content);

            Image iconImage = null;
            Image[] images = cardObject.GetComponentsInChildren<Image>();
            foreach (Image img in images)
            {
                if (img.gameObject.name == "Icon")
                {
                    iconImage = img;
                    break;
                }
            }

            if (iconImage != null)
            {
                Sprite foundSprite = GetSpriteByName(articleData.iconName);
                if (foundSprite != null)
                {
                    iconImage.sprite = foundSprite;
                }
                else
                {
                    Debug.Log("Icon sprite not found: " + articleData.iconName);
                }
            }
            else
            {
                Debug.Log("Icon Image not found in ArticleCard");
            }

            TextMeshProUGUI moneyText = null;
            TextMeshProUGUI[] texts = cardObject.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI txt in texts)
            {
                moneyText = txt;
                break;
            }

            if (moneyText != null)
            {
                moneyText.text = "$" + articleData.earnedMoney.ToString();
            }
            else
            {
                Debug.Log("TextMeshProUGUI not found in ArticleCard");
            }

            Button cardButton = cardObject.GetComponent<Button>();
            if (cardButton != null)
            {
                cardButton.onClick.AddListener(() => OpenArticleView(articleData));
            }
            else
            {
                Debug.Log("Button component not found on ArticleCard");
            }
        }
        else
        {
            Debug.Log("articleCardPrefab or content is null");
        }
    }

    private void OpenArticleView(ArticleData articleData)
    {
        if (articleViewPanel != null)
        {
            articleViewPanel.SetActive(true);

            if (viewPanelHeader != null)
            {
                viewPanelHeader.text = articleData.header;
            }
            else
            {
                Debug.Log("viewPanelHeader is null");
            }

            if (viewPanelDescription != null)
            {
                viewPanelDescription.text = articleData.description;
            }
            else
            {
                Debug.Log("viewPanelDescription is null");
            }

            if (viewPanelIcon != null)
            {
                Sprite foundSprite = GetSpriteByName(articleData.iconName);
                if (foundSprite != null)
                {
                    viewPanelIcon.sprite = foundSprite;
                }
                else
                {
                    Debug.Log("Icon sprite not found for view panel: " + articleData.iconName);
                }
            }
            else
            {
                Debug.Log("viewPanelIcon is null");
            }
        }
        else
        {
            Debug.Log("articleViewPanel is null");
        }
    }

    private Sprite GetSpriteByName(string spriteName)
    {
        if (InterviewController.Instance != null)
        {
            Sprite[] iconSprites = InterviewController.Instance.GetIconSprites();
            if (iconSprites != null)
            {
                foreach (Sprite sprite in iconSprites)
                {
                    if (sprite.name == spriteName)
                    {
                        return sprite;
                    }
                }
            }
            else
            {
                Debug.Log("iconSprites array is null in InterviewController");
            }
        }
        else
        {
            Debug.Log("InterviewController.Instance is null");
        }
        return null;
    }

    private void SaveArticles()
    {
        string json = JsonUtility.ToJson(new ArticleListWrapper { articles = articles }, true);
        PlayerPrefs.SetString("SavedArticles", json);
        PlayerPrefs.Save();
    }

    private void LoadArticles()
    {
        if (PlayerPrefs.HasKey("SavedArticles"))
        {
            string json = PlayerPrefs.GetString("SavedArticles");
            ArticleListWrapper wrapper = JsonUtility.FromJson<ArticleListWrapper>(json);
            articles = wrapper.articles;

            foreach (ArticleData article in articles)
            {
                CreateArticleCard(article);
            }
        }
        else
        {
            Debug.Log("No saved articles found");
        }
    }

    [System.Serializable]
    private class ArticleListWrapper
    {
        public List<ArticleData> articles;
    }
}