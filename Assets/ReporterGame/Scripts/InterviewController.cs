using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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
    [SerializeField] private TMP_InputField inputHeader;
    [SerializeField] private Button continueButtonHeader;
    
    [Header("Article Panel - Description Input")]
    [SerializeField] private GameObject enterDescriptionPanel;
    [SerializeField] private TMP_InputField inputDescription;
    [SerializeField] private Button continueButtonDescription;
    
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
        "Hi! Thanks for having me.",
        "Hello! I'm ready to begin.",
        "Hey! Glad to be here.",
        "Hi there! Excited for this.",
        "Hello! I'm happy to join.",
        "Hey! I've been looking forward.",
        "Hi! Great to be here today.",
        "Hello! Thanks for inviting me.",
        "Hey there! I'm all set.",
        "Hi! Ready when you are.",
        "Hello! Let's get going now.",
        "Hey! I'm ready to talk.",
        "Hi there! It's great to be here.",
        "Hello! Thanks for this chance.",
        "Hey! Nice to meet you too.",
        "Hi! Appreciate the invite.",
        "Hello! I'm feeling good today.",
        "Hey there! Ready to start now.",
        "Hi! Let's do this.",
        "Hello! I'm here and ready.",
        "Hey! Thanks for having me!",
        "Hi there! So glad to join.",
        "Hello! Great to be part of this.",
        "Hey! I'm good to start.",
        "Hi! Excited to talk today."
    };

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
        
        // Пофіксити анімації, пофіксити іконку персонажа, не появляється текст в ResultsPanel
    }

    private void Start()
    {
        PrepareUI();
        HideAllButtons();
        StartCoroutine(InterviewRoutine());
    
        inputHeader.onValueChanged.AddListener(OnHeaderInputChanged);
        continueButtonHeader.onClick.AddListener(OnContinueHeaderClicked);
        inputDescription.onValueChanged.AddListener(OnDescriptionInputChanged);
        continueButtonDescription.onClick.AddListener(OnContinueDescriptionClicked);
    
        nextButton.onClick.AddListener(OnNextIconClicked);
        previousButton.onClick.AddListener(OnPreviousIconClicked);
        continueButtonIcon.onClick.AddListener(OnContinueIconClicked);
        continueButtonIncome.onClick.AddListener(OnContinueIncomeClicked);
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

        SetAlpha(background, 0f);
        SetAlpha(interviewPanel, 0f);
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

        yield return StartCoroutine(ShowPanelWithChildren(enterHeaderPanel));
        
        continueButtonHeader.gameObject.SetActive(false);
        inputHeader.text = "";
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

    private void OnHeaderInputChanged(string text)
    {
        if (!string.IsNullOrEmpty(text) && !continueButtonHeader.gameObject.activeSelf)
        {
            continueButtonHeader.gameObject.SetActive(true);
            continueButtonHeader.transform.localScale = Vector3.zero;
            continueButtonHeader.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
        }
    }

    private void OnContinueHeaderClicked()
    {
        savedHeader = inputHeader.text;
        Debug.Log("Header saved: " + savedHeader);
        StartCoroutine(ShowDescriptionPanel());
    }

    private IEnumerator ShowDescriptionPanel()
    {
        foreach (Transform child in enterHeaderPanel.transform)
        {
            child.DOScale(0f, animationDuration).SetEase(Ease.InBack);
        }
        yield return new WaitForSeconds(animationDuration);
        enterHeaderPanel.SetActive(false);

        yield return StartCoroutine(ShowPanelWithChildren(enterDescriptionPanel));
        
        continueButtonDescription.gameObject.SetActive(false);
        inputDescription.text = "";
    }

    private void OnDescriptionInputChanged(string text)
    {
        if (!string.IsNullOrEmpty(text) && !continueButtonDescription.gameObject.activeSelf)
        {
            continueButtonDescription.gameObject.SetActive(true);
            continueButtonDescription.transform.localScale = Vector3.zero;
            continueButtonDescription.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
        }
    }

    private void OnContinueDescriptionClicked()
    {
        savedDescription = inputDescription.text;
        Debug.Log("Description saved: " + savedDescription);
        StartCoroutine(ShowIconPanel());
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

        foreach (Transform child in resultsPanel.transform)
        {
            child.gameObject.SetActive(false);
        }

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
        else
        {
            Debug.Log("resultsTitleText is null");
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
    }
    else
    {
        Debug.Log("WalletController.Instance is null");
    }

    if (incomeMoneyText != null)
    {
        incomeMoneyText.text = "Money: " + earnedMoney.ToString();
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
        incomeReputationText.text = "Reputation: " + earnedReputation.ToString();
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