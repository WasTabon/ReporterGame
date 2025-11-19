using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class HeaderOption
{
    public string shortText;
    public string fullText;
}

[System.Serializable]
public class DescriptionOption
{
    public string shortText;
    public string fullText;
}

public class InterviewController : MonoBehaviour
{
    public static InterviewController Instance;

    [Header("Data")]
    [SerializeField] private InterviewData interviewData;
    
    [Header("Main UI")]
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject interviewPanel;
    
    [Header("Interview UI - Person & Dialogue")]
    [SerializeField] private GameObject personBackground;
    [SerializeField] private Image personIcon;
    [SerializeField] private GameObject dialogueBackground;
    [SerializeField] private GameObject dialogueOpponent1;
    [SerializeField] private GameObject dialoguePlayer1;
    [SerializeField] private GameObject dialogueOpponent2;
    [SerializeField] private GameObject dialoguePlayer2;
    
    [Header("Interview UI - Options & Continue")]
    [SerializeField] private GameObject optionsBackground;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private GameObject continueButton;

    [Header("Article Panel - Main")]
    [SerializeField] private GameObject articlePanel;
    
    [Header("Article Panel - Header Input")]
    [SerializeField] private GameObject enterHeaderPanel;
    [SerializeField] private Button[] headerButtons;

    [Header("Article Panel - Description Input")]
    [SerializeField] private GameObject enterDescriptionPanel;
    [SerializeField] private Button[] descriptionButtons;
    
    [Header("Article Panel - Icon Selection")]
    [SerializeField] private GameObject enterIconPanel;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button continueButtonIcon;
    [SerializeField] private Sprite[] iconSprites;

    [Header("Results Panel - Main")]
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private TextMeshProUGUI resultsTitleText;
    
    [Header("Results Panel - Article Display")]
    [SerializeField] private TextMeshProUGUI articleHeader;
    [SerializeField] private TextMeshProUGUI articleDescription;
    [SerializeField] private Image articleIcon;
    [SerializeField] private RectTransform article;
    
    [Header("Results Panel - Statistics")]
    [SerializeField] private GameObject statisticsPanel;
    [SerializeField] private RectTransform viewHandler;
    [SerializeField] private RectTransform likesHandler;
    [SerializeField] private RectTransform dislikesHandler;
    
    [Header("Results Panel - Income")]
    [SerializeField] private GameObject incomePanel;
    [SerializeField] private TextMeshProUGUI incomeMoneyText;
    [SerializeField] private TextMeshProUGUI incomeReputationText;
    [SerializeField] private Button continueButtonIncome;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float typewriterSpeed = 0.05f;

    [Header("Dialogue Text")]
    private string[] player1Texts = new string[]
    {
        "Hello! Thanks for joining us.",
        "Hi there! Great to see you.",
        "Hey! Welcome to our show.",
        "Hello! Glad you could make it.",
        "Hi! Thanks for being here.",
        "Hey there! How are you today?",
        "Hello! Let's get started, shall we?",
        "Hi! Appreciate your time today.",
        "Hey! Thanks for taking part.",
        "Hello! Great to have you here.",
        "Hi there! Welcome to the talk.",
        "Hey! Nice to meet you again.",
        "Hello! Hope you're doing well.",
        "Hi! Thanks for stopping by.",
        "Hey there! Let's begin now.",
        "Hello! Excited to talk today.",
        "Hi there! How are things going?",
        "Hey! Let's get going then.",
        "Hello! Ready to dive in?",
        "Hi! Thanks for coming today.",
        "Hey there! Thanks for joining.",
        "Hello! Great to see you again.",
        "Hi! Appreciate your presence.",
        "Hey! Let's start the interview.",
        "Hello! How are you feeling?"
    };

    private string[] opponent1Texts = new string[]
    {
        "Hello, the match was intense today.",
        "Hi, that was a tight game out there.",
        "Hello, the match felt very balanced.",
        "Hi, the pace today was really high.",
        "Hello, that game demanded full focus.",
        "Hi, both sides pushed hard today.",
        "Hello, the match was tough but fair.",
        "Hi, the tempo today was impressive.",
        "Hello, that game required precision.",
        "Hi, the match showed strong effort."
    };

    private HeaderOption[] allHeaders = new HeaderOption[]
    {
        new HeaderOption { shortText = "Performance review", fullText = "Athlete discusses today's performance" },
        new HeaderOption { shortText = "Match analysis", fullText = "Breaking down the game strategy" },
        new HeaderOption { shortText = "Training insights", fullText = "Behind the scenes preparation" },
        new HeaderOption { shortText = "Team dynamics", fullText = "How teamwork shaped the outcome" },
        new HeaderOption { shortText = "Mental preparation", fullText = "The psychology of competition" },
        new HeaderOption { shortText = "Equipment matters", fullText = "Gear choices and their impact" },
        new HeaderOption { shortText = "Coach influence", fullText = "Guidance that made the difference" },
        new HeaderOption { shortText = "Recovery methods", fullText = "Staying sharp after intense play" },
        new HeaderOption { shortText = "Pressure handling", fullText = "Managing expectations and stress" },
        new HeaderOption { shortText = "Tactical decisions", fullText = "Key moments that changed everything" },
        new HeaderOption { shortText = "Physical condition", fullText = "Fitness levels and stamina" },
        new HeaderOption { shortText = "Fan support", fullText = "Crowd energy and motivation" },
        new HeaderOption { shortText = "Opponent respect", fullText = "Acknowledging the competition" },
        new HeaderOption { shortText = "Future goals", fullText = "What comes next in training" },
        new HeaderOption { shortText = "Mistake lessons", fullText = "Learning from today's errors" },
        new HeaderOption { shortText = "Victory factors", fullText = "What led to success today" },
        new HeaderOption { shortText = "Routine importance", fullText = "Pre-game rituals and habits" },
        new HeaderOption { shortText = "Injury concerns", fullText = "Dealing with physical challenges" },
        new HeaderOption { shortText = "Weather impact", fullText = "How conditions affected play" },
        new HeaderOption { shortText = "Season outlook", fullText = "Long-term goals and planning" }
    };

    private DescriptionOption[] allDescriptions = new DescriptionOption[]
    {
        new DescriptionOption { shortText = "Detailed performance", fullText = "Player breaks down every decision and movement during the crucial moments of play." },
        new DescriptionOption { shortText = "Strategic approach", fullText = "Analysis reveals the tactical choices that shaped both offensive and defensive efforts." },
        new DescriptionOption { shortText = "Preparation routine", fullText = "Hours of focused training and mental conditioning led to today's performance level." },
        new DescriptionOption { shortText = "Team coordination", fullText = "Communication and trust between teammates proved essential for executing the game plan." },
        new DescriptionOption { shortText = "Focus techniques", fullText = "Mental clarity and concentration methods helped maintain composure under pressure." },
        new DescriptionOption { shortText = "Gear selection", fullText = "Equipment choices were carefully made to optimize comfort and performance standards." },
        new DescriptionOption { shortText = "Coaching impact", fullText = "Strategic advice and motivational support from coaching staff made critical difference." },
        new DescriptionOption { shortText = "Recovery process", fullText = "Post-match recovery includes proper rest, nutrition, and physical therapy sessions." },
        new DescriptionOption { shortText = "Stress management", fullText = "Handling expectations requires mental strength and confidence in preparation work." },
        new DescriptionOption { shortText = "Critical choices", fullText = "Split-second decisions during key moments determined the final outcome today." },
        new DescriptionOption { shortText = "Fitness level", fullText = "Consistent training regimen maintained endurance and strength throughout the match." },
        new DescriptionOption { shortText = "Crowd energy", fullText = "Supporter enthusiasm provided extra motivation during challenging periods of play." },
        new DescriptionOption { shortText = "Competitor skill", fullText = "Opponent's abilities pushed performance levels higher and demanded complete focus." },
        new DescriptionOption { shortText = "Next steps", fullText = "Future training will address weaknesses and build on strengths shown today." },
        new DescriptionOption { shortText = "Error analysis", fullText = "Reviewing mistakes provides valuable lessons for improvement in upcoming matches." },
        new DescriptionOption { shortText = "Success elements", fullText = "Combination of preparation, execution, and teamwork created winning conditions." },
        new DescriptionOption { shortText = "Ritual benefits", fullText = "Consistent pre-game habits help establish mental readiness and physical warmup." },
        new DescriptionOption { shortText = "Physical issues", fullText = "Managing minor injuries requires careful attention while maintaining performance quality." },
        new DescriptionOption { shortText = "Condition effects", fullText = "Environmental factors influenced strategy and required adaptations during play." },
        new DescriptionOption { shortText = "Season planning", fullText = "Long-term objectives guide daily training focus and competitive preparation approach." }
    };

    private HeaderOption[] selectedHeaders = new HeaderOption[3];
    private DescriptionOption[] selectedDescriptions = new DescriptionOption[3];
    
    private string selectedOpponent1Text;
    private string selectedPlayer1Text;

    private List<InterviewData.InterviewQuestion> selectedQuestions;
    private int selectedQuestionIndex;
    
    private string savedHeader;
    private string savedDescription;
    private Sprite savedIcon;
    
    private int savedViews;
    private int savedLikes;
    private int savedDislikes;
    
    private int currentIconIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PrepareUI();
        HideAllButtons();
        StartCoroutine(InterviewRoutine());

        nextButton.onClick.AddListener(OnNextIconClicked);
        previousButton.onClick.AddListener(OnPreviousIconClicked);
        continueButtonIcon.onClick.AddListener(OnContinueIconClicked);
        continueButtonIncome.onClick.AddListener(OnContinueIncomeClicked);
    
        for (int i = 0; i < headerButtons.Length; i++)
        {
            int index = i;
            if (headerButtons[i] != null)
            {
                headerButtons[i].onClick.AddListener(() => OnHeaderButtonClicked(index));
            }
            else
            {
                Debug.Log($"headerButtons[{i}] is null");
            }
        }
    
        for (int i = 0; i < descriptionButtons.Length; i++)
        {
            int index = i;
            if (descriptionButtons[i] != null)
            {
                descriptionButtons[i].onClick.AddListener(() => OnDescriptionButtonClicked(index));
            }
            else
            {
                Debug.Log($"descriptionButtons[{i}] is null");
            }
        }
    }

    private void PrepareUI()
    {
        background.SetActive(false);
        interviewPanel.SetActive(false);
        personBackground.SetActive(false);
        dialogueBackground.SetActive(false);
        dialogueOpponent1.SetActive(false);
        dialoguePlayer1.SetActive(false);
        optionsBackground.SetActive(false);
        dialoguePlayer2.SetActive(false);
        dialogueOpponent2.SetActive(false);
        continueButton.SetActive(false);
        
        articlePanel.SetActive(false);
        enterHeaderPanel.SetActive(false);
        enterDescriptionPanel.SetActive(false);
        enterIconPanel.SetActive(false);
        
        resultsPanel.SetActive(false);
        statisticsPanel.SetActive(false);
        incomePanel.SetActive(false);

        SetAlpha(background, 0f);
        SetAlpha(interviewPanel, 0f);
        SetAlpha(articlePanel, 0f);
        SetAlpha(resultsPanel, 0f);
        SetAlpha(incomePanel, 0f);
        
        personBackground.transform.localScale = Vector3.zero;
        dialogueBackground.transform.localScale = Vector3.zero;
        dialogueOpponent1.transform.localScale = Vector3.zero;
        dialoguePlayer1.transform.localScale = Vector3.zero;
        optionsBackground.transform.localScale = Vector3.zero;
        dialoguePlayer2.transform.localScale = Vector3.zero;
        dialogueOpponent2.transform.localScale = Vector3.zero;
        continueButton.transform.localScale = Vector3.zero;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }

        Button contButton = continueButton.GetComponent<Button>();
        if (contButton != null)
        {
            contButton.onClick.AddListener(OnContinueClicked);
        }
    }

    private void HideAllButtons()
    {
        GameObject[] purchasedRooms = RoomsController.Instance.GetPurchasedRooms();
        foreach (GameObject room in purchasedRooms)
        {
            HideButtonInRoom(room);
        }
    }

    public void HideButtonInRoom(GameObject room)
    {
        Button button = GetButtonFromRoom(room);
        if (button != null)
        {
            button.transform.localScale = Vector3.zero;
        }
    }

    private IEnumerator InterviewRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(1f, 10f);
            yield return new WaitForSeconds(waitTime);

            GameObject[] purchasedRooms = RoomsController.Instance.GetPurchasedRooms();
            if (purchasedRooms.Length > 0)
            {
                GameObject randomRoom = purchasedRooms[Random.Range(0, purchasedRooms.Length)];
                Button button = GetButtonFromRoom(randomRoom);

                if (button != null)
                {
                    button.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
                    
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => OnButtonClicked(button));
                }
            }
        }
    }

    public Sprite[] GetIconSprites()
    {
        return iconSprites;
    }
    
    private Button GetButtonFromRoom(GameObject room)
    {
        Canvas canvas = room.GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            Button button = canvas.GetComponentInChildren<Button>();
            return button;
        }
        return null;
    }

    private void OnButtonClicked(Button button)
    {
        button.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack);
        Debug.Log("Interview button clicked!");
        
        PrepareInterview();
        StartCoroutine(ShowInterviewSequence());
    }

    private void PrepareInterview()
    {
        selectedQuestions = new List<InterviewData.InterviewQuestion>();
        List<InterviewData.InterviewQuestion> availableQuestions = new List<InterviewData.InterviewQuestion>(interviewData.questions);

        for (int i = 0; i < 3 && availableQuestions.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableQuestions.Count);
            selectedQuestions.Add(availableQuestions[randomIndex]);
            availableQuestions.RemoveAt(randomIndex);
        }

        for (int i = 0; i < optionButtons.Length && i < selectedQuestions.Count; i++)
        {
            TextMeshProUGUI buttonText = optionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = selectedQuestions[i].short_question;
            }
        }

        if (interviewData.personSprites != null && interviewData.personSprites.Length > 0)
        {
            Sprite randomSprite = interviewData.personSprites[Random.Range(0, interviewData.personSprites.Length)];
            if (randomSprite != null)
            {
                personIcon.sprite = randomSprite;
            }
        }
    
        selectedOpponent1Text = opponent1Texts[Random.Range(0, opponent1Texts.Length)];
        selectedPlayer1Text = player1Texts[Random.Range(0, player1Texts.Length)];
    }

    private IEnumerator ShowInterviewSequence()
    {
        ResetUI();
    
        background.SetActive(true);
        DOTween.To(() => GetAlpha(background), x => SetAlpha(background, x), 1f, animationDuration);
        yield return new WaitForSeconds(animationDuration);

        interviewPanel.SetActive(true);
        DOTween.To(() => GetAlpha(interviewPanel), x => SetAlpha(interviewPanel, x), 1f, animationDuration);
        yield return new WaitForSeconds(animationDuration);

        personBackground.SetActive(true);
        personBackground.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(animationDuration);

        dialogueBackground.SetActive(true);
        dialogueBackground.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(animationDuration);

        dialogueOpponent1.SetActive(true);
        dialogueOpponent1.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(animationDuration);
        yield return StartCoroutine(TypewriterEffect(dialogueOpponent1, selectedOpponent1Text));

        dialoguePlayer1.SetActive(true);
        dialoguePlayer1.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(animationDuration);
        yield return StartCoroutine(TypewriterEffect(dialoguePlayer1, selectedPlayer1Text));

        optionsBackground.SetActive(true);
        optionsBackground.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
    }

    private void OnOptionSelected(int index)
    {
        selectedQuestionIndex = index;
        StartCoroutine(ShowResponseSequence());
    }

    private IEnumerator ShowResponseSequence()
    {
        optionsBackground.transform.DOScale(0f, animationDuration).SetEase(Ease.InBack);
        yield return new WaitForSeconds(animationDuration);
        optionsBackground.SetActive(false);

        string player2FullText = "";
        if (selectedQuestionIndex < selectedQuestions.Count)
        {
            player2FullText = selectedQuestions[selectedQuestionIndex].main_question;
        }

        dialoguePlayer2.SetActive(true);
        dialoguePlayer2.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(animationDuration);
        yield return StartCoroutine(TypewriterEffect(dialoguePlayer2, player2FullText));

        string opponent2FullText = "";
        if (selectedQuestionIndex < selectedQuestions.Count)
        {
            opponent2FullText = selectedQuestions[selectedQuestionIndex].answer;
        }

        dialogueOpponent2.SetActive(true);
        dialogueOpponent2.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(animationDuration);
        yield return StartCoroutine(TypewriterEffect(dialogueOpponent2, opponent2FullText));

        continueButton.SetActive(true);
        continueButton.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
    }

    private IEnumerator TypewriterEffect(GameObject dialogueObject, string textToShow)
    {
        TextMeshProUGUI textComponent = dialogueObject.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = "";

            foreach (char character in textToShow)
            {
                textComponent.text += character;
                yield return new WaitForSeconds(typewriterSpeed);
            }
        }
    }

    private void ResetUI()
    {
        background.SetActive(false);
        interviewPanel.SetActive(false);
        personBackground.SetActive(false);
        dialogueBackground.SetActive(false);
        dialogueOpponent1.SetActive(false);
        dialoguePlayer1.SetActive(false);
        optionsBackground.SetActive(false);
        dialoguePlayer2.SetActive(false);
        dialogueOpponent2.SetActive(false);
        continueButton.SetActive(false);
        statisticsPanel.SetActive(false);
        incomePanel.SetActive(false);

        SetAlpha(background, 0f);
        SetAlpha(interviewPanel, 0f);
        SetAlpha(incomePanel, 0f);
        personBackground.transform.localScale = Vector3.zero;
        dialogueBackground.transform.localScale = Vector3.zero;
        dialogueOpponent1.transform.localScale = Vector3.zero;
        dialoguePlayer1.transform.localScale = Vector3.zero;
        optionsBackground.transform.localScale = Vector3.zero;
        dialoguePlayer2.transform.localScale = Vector3.zero;
        dialogueOpponent2.transform.localScale = Vector3.zero;
        continueButton.transform.localScale = Vector3.zero;

        TextMeshProUGUI text1 = dialogueOpponent1.GetComponentInChildren<TextMeshProUGUI>();
        if (text1 != null) text1.text = "";
    
        TextMeshProUGUI text2 = dialoguePlayer1.GetComponentInChildren<TextMeshProUGUI>();
        if (text2 != null) text2.text = "";
    
        TextMeshProUGUI text3 = dialoguePlayer2.GetComponentInChildren<TextMeshProUGUI>();
        if (text3 != null) text3.text = "";
    
        TextMeshProUGUI text4 = dialogueOpponent2.GetComponentInChildren<TextMeshProUGUI>();
        if (text4 != null) text4.text = "";
    }

    private void OnContinueClicked()
    {
        Debug.Log("Continue button clicked!");
        StartCoroutine(ShowArticlePanel());
    }

    private IEnumerator ShowArticlePanel()
    {
        DOTween.To(() => GetAlpha(interviewPanel), x => SetAlpha(interviewPanel, x), 0f, animationDuration);
        yield return new WaitForSeconds(animationDuration);
        interviewPanel.SetActive(false);

        enterHeaderPanel.SetActive(false);
        enterDescriptionPanel.SetActive(false);
        enterIconPanel.SetActive(false);

        articlePanel.SetActive(true);
        DOTween.To(() => GetAlpha(articlePanel), x => SetAlpha(articlePanel, x), 1f, animationDuration);
        yield return new WaitForSeconds(animationDuration);

        PrepareRandomHeaders();
        yield return StartCoroutine(ShowPanelWithChildren(enterHeaderPanel));
    }

    private void PrepareRandomHeaders()
    {
        List<HeaderOption> availableHeaders = new List<HeaderOption>(allHeaders);
        
        for (int i = 0; i < 3 && i < availableHeaders.Count; i++)
        {
            int randomIndex = Random.Range(0, availableHeaders.Count);
            selectedHeaders[i] = availableHeaders[randomIndex];
            availableHeaders.RemoveAt(randomIndex);
            
            if (headerButtons[i] != null)
            {
                TextMeshProUGUI buttonText = headerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = selectedHeaders[i].shortText;
                }
                else
                {
                    Debug.Log($"TextMeshProUGUI not found in headerButtons[{i}]");
                }
            }
            else
            {
                Debug.Log($"headerButtons[{i}] is null");
            }
        }
    }

    private void PrepareRandomDescriptions()
    {
        List<DescriptionOption> availableDescriptions = new List<DescriptionOption>(allDescriptions);
        
        for (int i = 0; i < 3 && i < availableDescriptions.Count; i++)
        {
            int randomIndex = Random.Range(0, availableDescriptions.Count);
            selectedDescriptions[i] = availableDescriptions[randomIndex];
            availableDescriptions.RemoveAt(randomIndex);
            
            if (descriptionButtons[i] != null)
            {
                TextMeshProUGUI buttonText = descriptionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = selectedDescriptions[i].shortText;
                }
                else
                {
                    Debug.Log($"TextMeshProUGUI not found in descriptionButtons[{i}]");
                }
            }
            else
            {
                Debug.Log($"descriptionButtons[{i}] is null");
            }
        }
    }

    private void OnHeaderButtonClicked(int index)
    {
        savedHeader = selectedHeaders[index].fullText;
        Debug.Log("Header saved: " + savedHeader);
        StartCoroutine(ShowDescriptionPanel());
    }

    private void OnDescriptionButtonClicked(int index)
    {
        savedDescription = selectedDescriptions[index].fullText;
        Debug.Log("Description saved: " + savedDescription);
        StartCoroutine(ShowIconPanel());
    }

    private IEnumerator ShowPanelWithChildren(GameObject panel)
    {
        foreach (Transform child in panel.transform)
        {
            child.localScale = Vector3.zero;
        }

        panel.SetActive(true);

        foreach (Transform child in panel.transform)
        {
            child.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(animationDuration * 0.2f);
        }
    }

    private IEnumerator ShowDescriptionPanel()
    {
        foreach (Transform child in enterHeaderPanel.transform)
        {
            child.DOScale(0f, animationDuration).SetEase(Ease.InBack);
        }
        yield return new WaitForSeconds(animationDuration);
        enterHeaderPanel.SetActive(false);

        PrepareRandomDescriptions();
        yield return StartCoroutine(ShowPanelWithChildren(enterDescriptionPanel));
    }
    
    private IEnumerator ShowIconPanel()
    {
        foreach (Transform child in enterDescriptionPanel.transform)
        {
            child.DOScale(0f, animationDuration).SetEase(Ease.InBack);
        }
        yield return new WaitForSeconds(animationDuration);
        enterDescriptionPanel.SetActive(false);

        currentIconIndex = 0;
        UpdateIconImage();

        yield return StartCoroutine(ShowPanelWithChildren(enterIconPanel));
    }

    private void OnNextIconClicked()
    {
        if (iconSprites.Length > 0)
        {
            currentIconIndex = (currentIconIndex + 1) % iconSprites.Length;
            UpdateIconImage();
        }
    }

    private void OnPreviousIconClicked()
    {
        if (iconSprites.Length > 0)
        {
            currentIconIndex--;
            if (currentIconIndex < 0)
            {
                currentIconIndex = iconSprites.Length - 1;
            }
            UpdateIconImage();
        }
    }

    private void UpdateIconImage()
    {
        if (iconSprites.Length > 0 && iconImage != null)
        {
            iconImage.sprite = iconSprites[currentIconIndex];
        }
    }

    private void OnContinueIconClicked()
    {
        savedIcon = iconImage.sprite;
        Debug.Log("Icon saved: " + savedIcon.name);
        Debug.Log("All data saved - Header: " + savedHeader + ", Description: " + savedDescription + ", Icon: " + savedIcon.name);
        
        StartCoroutine(ShowResultsPanel());
    }

    private IEnumerator ShowResultsPanel()
    {
        DOTween.To(() => GetAlpha(articlePanel), x => SetAlpha(articlePanel, x), 0f, animationDuration);
        yield return new WaitForSeconds(animationDuration);
        articlePanel.SetActive(false);

        statisticsPanel.SetActive(false);
        statisticsPanel.transform.localScale = Vector3.zero;
    
        incomePanel.SetActive(false);
        SetAlpha(incomePanel, 0f);

        resultsPanel.SetActive(true);
        DOTween.To(() => GetAlpha(resultsPanel), x => SetAlpha(resultsPanel, x), 1f, animationDuration);
        yield return new WaitForSeconds(animationDuration);

        if (resultsTitleText != null)
        {
            resultsTitleText.gameObject.SetActive(true);
            resultsTitleText.transform.localScale = Vector3.zero;
            resultsTitleText.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(animationDuration);
        }

        articleHeader.text = savedHeader;
        articleDescription.text = savedDescription;
        articleIcon.sprite = savedIcon;

        Vector3 originalScale = article.localScale;
        Vector3 originalPosition = article.localPosition;

        article.localScale = originalScale * 3f;
        article.gameObject.SetActive(true);

        article.DOScale(originalScale, 0.3f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.3f);

        if (statisticsPanel != null)
        {
            statisticsPanel.SetActive(true);
            statisticsPanel.transform.localScale = Vector3.zero;
            statisticsPanel.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(animationDuration);
        }

        yield return StartCoroutine(ShowStatHandler(viewHandler, 1, 9999));
        yield return StartCoroutine(ShowStatHandler(likesHandler, 0, savedViews));
        yield return StartCoroutine(ShowStatHandler(dislikesHandler, 0, savedViews));

        yield return StartCoroutine(ShowIncomePanel());
    }

    private IEnumerator ShowStatHandler(RectTransform handler, int minValue, int maxValue)
    {
        TextMeshProUGUI textComponent = handler.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = "";
        }

        handler.gameObject.SetActive(true);
        handler.localScale = Vector3.zero;
        handler.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(animationDuration);

        if (textComponent != null)
        {
            int targetValue = Random.Range(minValue, maxValue + 1);
            
            float duration = 1f;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                int currentValue = Mathf.RoundToInt(Mathf.Lerp(0, targetValue, elapsed / duration));
                textComponent.text = currentValue.ToString();
                yield return null;
            }
            
            textComponent.text = targetValue.ToString();
            
            if (handler == viewHandler)
            {
                savedViews = targetValue;
            }
            else if (handler == likesHandler)
            {
                savedLikes = targetValue;
            }
            else if (handler == dislikesHandler)
            {
                savedDislikes = targetValue;
            }
        }
    }

    private IEnumerator ShowIncomePanel()
    {
        foreach (Transform child in incomePanel.transform)
        {
            child.gameObject.SetActive(false);
        }

        incomePanel.SetActive(true);
        DOTween.To(() => GetAlpha(incomePanel), x => SetAlpha(incomePanel, x), 1f, animationDuration);
        yield return new WaitForSeconds(animationDuration);

        Transform panelChild = incomePanel.transform.Find("Panel");
        if (panelChild != null)
        {
            RectTransform panelRect = panelChild.GetComponent<RectTransform>();
            Vector3 originalPosition = panelRect.localPosition;
            
            panelRect.localPosition = new Vector3(originalPosition.x + 500f, originalPosition.y, originalPosition.z);
            panelRect.gameObject.SetActive(true);
            
            panelRect.DOLocalMoveX(originalPosition.x, 0.3f).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            Debug.Log("Panel child not found in incomePanel");
        }

        int earnedMoney = Random.Range(100, 1001);
        int earnedReputation = Random.Range(100, 501);
        
        if (WalletController.Instance != null)
        {
            WalletController.Instance.Money += earnedMoney;
            WalletController.Instance.Reputation += earnedReputation;
        }
        else
        {
            Debug.Log("WalletController.Instance is null");
        }

        if (incomeMoneyText != null)
        {
            incomeMoneyText.text = earnedMoney.ToString();
            incomeMoneyText.gameObject.SetActive(true);
            incomeMoneyText.transform.localScale = Vector3.zero;
            incomeMoneyText.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(animationDuration);
        }
        else
        {
            Debug.Log("incomeMoneyText is null");
        }

        if (incomeReputationText != null)
        {
            incomeReputationText.text = earnedReputation.ToString();
            incomeReputationText.gameObject.SetActive(true);
            incomeReputationText.transform.localScale = Vector3.zero;
            incomeReputationText.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(animationDuration);
        }
        else
        {
            Debug.Log("incomeReputationText is null");
        }

        if (continueButtonIncome != null)
        {
            continueButtonIncome.gameObject.SetActive(true);
            continueButtonIncome.transform.localScale = Vector3.zero;
            continueButtonIncome.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
        }
        else
        {
            Debug.Log("continueButtonIncome is null");
        }
        
        if (ArticleManager.Instance != null)
        {
            ArticleManager.Instance.AddArticle(savedHeader, savedDescription, savedIcon, earnedMoney);
        }
        else
        {
            Debug.Log("ArticleManager.Instance is null");
        }
    }
    
    private void OnContinueIncomeClicked()
    {
        StartCoroutine(CloseAllPanels());
    }

    private IEnumerator CloseAllPanels()
    {
        DOTween.To(() => GetAlpha(resultsPanel), x => SetAlpha(resultsPanel, x), 0f, animationDuration);
        yield return new WaitForSeconds(animationDuration);
        resultsPanel.SetActive(false);

        DOTween.To(() => GetAlpha(background), x => SetAlpha(background, x), 0f, animationDuration);
        yield return new WaitForSeconds(animationDuration);
        background.SetActive(false);
    }

    private void SetAlpha(GameObject obj, float alpha)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = obj.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = alpha;
    }

    private float GetAlpha(GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = obj.AddComponent<CanvasGroup>();
        }
        return canvasGroup.alpha;
    }
}