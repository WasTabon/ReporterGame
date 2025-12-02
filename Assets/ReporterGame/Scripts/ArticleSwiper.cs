using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class ArticleSwiper : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform cardPanel;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI descriptionText;
    public Image iconImage;
    public TextMeshProUGUI earningsText;
    public Button nextButton;
    public Button prevButton;

    [Header("Settings")]
    public Sprite[] icons;
    public float swipeDuration = 0.4f;
    public float swipeDistance = 2000f;

    private List<ArticleData> history = new List<ArticleData>();
    private int currentIndex = -1;
    private bool isAnimating = false;

    private string[] headers = new string[]
    {
        "Sharpening competitive mindset",
        "High-intensity drills",
        "Game tempo control",
        "Focus under fatigue",
        "Reading opponent patterns",
        "Capturing key moments",
        "Adapting mid-game",
        "Strength progression",
        "Matchday confidence",
        "Team communication",
        "Precision movements",
        "Situational awareness",
        "Pre-game analysis",
        "Conditioning balance",
        "Handling setbacks",
        "Motivational boost",
        "Warm-up focus",
        "Long-term discipline",
        "Breaking pressure cycles",
        "Anticipation training",
        "Footwork refinement",
        "Weather adaptation",
        "Squad unity",
        "Energy management",
        "Goal-setting clarity",
        "Post-game reflection",
        "Offensive creativity",
        "Defensive mindset",
        "Hydration strategy",
        "Injury prevention",
        "Momentum shifts",
        "Leadership moments",
        "Smart positioning",
        "Conflict resolution",
        "Role adaptation",
        "Fueling for performance",
        "Sharpening instincts",
        "Tactical versatility",
        "Mid-season adjustments",
        "Confidence routines",
        "Body control",
        "Game awareness",
        "Refocus methods",
        "Technical polishing",
        "Strategic patience",
        "Handling noise",
        "Effort distribution",
        "Impact moments",
        "Pre-event visualization",
        "Season recalibration"
    };

    private string[] descriptions = new string[]
    {
        "Mental resilience exercises helped maintain confidence throughout the match.",
        "Fast-paced training sessions improved reaction time during crucial moments.",
        "Athlete analyzed how pacing adjustments shifted momentum in key phases.",
        "Maintaining concentration while tired proved essential for consistent decisions.",
        "Studying rival tendencies allowed more effective anticipation on the field.",
        "Critical turning points were identified to refine strategy for future matches.",
        "Quick tactical adjustments helped respond effectively to unexpected challenges.",
        "Improved power output came from months of targeted conditioning work.",
        "Positive mindset and calm preparation boosted performance from the start.",
        "Clear calls and coordination allowed smoother execution of complex plays.",
        "Fine motor control training enhanced accuracy during fast exchanges.",
        "Constant scanning of the field supported smarter decision-making under pressure.",
        "Reviewing opponent footage provided vital insights for tailoring tactics.",
        "Mixing endurance and speed work ensured all-around physical readiness.",
        "Quick emotional recovery after errors helped maintain steady performance.",
        "Encouragement from teammates created a strong competitive drive.",
        "Specific activation exercises optimized muscle response before action.",
        "Daily routine consistency contributed to gradual but clear performance growth.",
        "Breathing techniques prevented stress from affecting gameplay decisions.",
        "Pattern recognition drills improved prediction of opponent actions.",
        "Agility exercises enhanced stability during rapid directional changes.",
        "Player adjusted tactics to handle wind variations during key plays.",
        "Shared preparation routines strengthened trust during competitive moments.",
        "Controlled pacing allowed the athlete to stay sharp for decisive plays.",
        "Defined objectives made training sessions more structured and productive.",
        "Thoughtful evaluation highlighted improvement areas for upcoming matches.",
        "Unpredictable attacking choices created new scoring opportunities.",
        "Focused positioning and strong anticipation limited opponent advances.",
        "Proper fluid intake maintained performance consistency in warm conditions.",
        "Stability routines and stretching reduced risk during high-intensity actions.",
        "Recognizing momentum changes allowed smarter play adjustments.",
        "Taking initiative in tense situations guided the team toward stability.",
        "Spatial awareness helped the athlete stay prepared for sudden transitions.",
        "Quick communication prevented misunderstandings during fast plays.",
        "Player adjusted responsibilities depending on match scenarios.",
        "Well-planned nutrition boosted energy levels during demanding moments.",
        "Repetitive scenario practice strengthened immediate reaction skills.",
        "Flexibility in formation roles added unpredictability to the team's strategy.",
        "Updated training plans addressed weaknesses revealed in recent matches.",
        "Pre-competition rituals helped build a stable emotional state.",
        "Core training improved balance during physical duels.",
        "Constant monitoring of movement flows guided smarter positioning.",
        "Short mental resets helped regain clarity after stressful exchanges.",
        "Detailed repetition refined execution of key skill elements.",
        "Waiting for the right moment created more effective opportunities.",
        "Concentration drills helped the athlete ignore distracting crowd sounds.",
        "Careful energy use ensured consistency across all match periods.",
        "Player focused on delivering strong performances in decisive situations.",
        "Imagining match scenarios prepared the mind for complex decisions.",
        "Revised goals aligned training focus with long-term ambitions."
    };

    void Start()
    {
        nextButton.onClick.AddListener(OnNextClicked);
        prevButton.onClick.AddListener(OnPrevClicked);
        
        GenerateNewArticle();
    }

    void OnNextClicked()
    {
        if (isAnimating) return;

        if (currentIndex < history.Count - 1)
        {
            currentIndex++;
            ShowArticle(history[currentIndex], true);
        }
        else
        {
            GenerateNewArticle();
        }
    }

    void OnPrevClicked()
    {
        if (isAnimating || currentIndex <= 0) return;
        
        currentIndex--;
        ShowArticle(history[currentIndex], false);
    }

    void GenerateNewArticle()
    {
        ArticleData newArticle = new ArticleData
        {
            header = headers[Random.Range(0, headers.Length)],
            description = descriptions[Random.Range(0, descriptions.Length)],
            icon = icons[Random.Range(0, icons.Length)],
            earnings = Random.Range(100, 10000)
        };

        history.Add(newArticle);
        currentIndex = history.Count - 1;
        
        if (history.Count > 5)
        {
            history.RemoveAt(0);
            currentIndex = 4;
        }

        if (history.Count == 1)
        {
            SetArticleData(newArticle);
        }
        else
        {
            ShowArticle(newArticle, true);
        }
    }

    void SetArticleData(ArticleData article)
    {
        headerText.text = article.header;
        descriptionText.text = article.description;
        iconImage.sprite = article.icon;
        earningsText.text = "$" + article.earnings;
    }

    void ShowArticle(ArticleData article, bool isNext)
    {
        isAnimating = true;

        float startY = isNext ? -swipeDistance : swipeDistance;
        float endY = isNext ? swipeDistance : -swipeDistance;
        
        Vector2 originalPos = cardPanel.anchoredPosition;
        
        headerText.text = article.header;
        descriptionText.text = article.description;
        iconImage.sprite = article.icon;
        earningsText.text = "$" + article.earnings;
        
        cardPanel.anchoredPosition = new Vector2(originalPos.x, startY);

        cardPanel.DOAnchorPosY(originalPos.y, swipeDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                isAnimating = false;
            });
    }

    [System.Serializable]
    public class ArticleData
    {
        public string header;
        public string description;
        public Sprite icon;
        public int earnings;
    }
}