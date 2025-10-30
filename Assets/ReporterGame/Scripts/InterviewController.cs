using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class InterviewController : MonoBehaviour
{
    public static InterviewController Instance;

    [SerializeField] private InterviewData interviewData;
    
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject interviewPanel;
    [SerializeField] private GameObject personBackground;
    [SerializeField] private GameObject dialogueBackground;
    [SerializeField] private GameObject dialogueOpponent1;
    [SerializeField] private GameObject dialoguePlayer1;
    [SerializeField] private GameObject optionsBackground;
    [SerializeField] private GameObject dialoguePlayer2;
    [SerializeField] private GameObject dialogueOpponent2;
    [SerializeField] private GameObject continueButton;

    [SerializeField] private GameObject articlePanel;
    [SerializeField] private GameObject enterHeaderPanel;
    [SerializeField] private GameObject enterDescriptionPanel;
    [SerializeField] private GameObject enterIconPanel;
    
    [SerializeField] private TMP_InputField inputHeader;
    [SerializeField] private Button continueButtonHeader;
    [SerializeField] private TMP_InputField inputDescription;
    [SerializeField] private Button continueButtonDescription;

    [SerializeField] private Button[] optionButtons;

    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float typewriterSpeed = 0.05f;

    [SerializeField] private string opponent1Text = "Hello! I'm ready for the interview.";
    [SerializeField] private string player1Text = "Great! Let's begin.";

    private List<InterviewData.InterviewQuestion> selectedQuestions;
    private int selectedQuestionIndex;
    
    private string savedHeader;
    private string savedDescription;

    private void Awake()
    {
        Instance = this;
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

        SetAlpha(background, 0f);
        SetAlpha(interviewPanel, 0f);
        SetAlpha(articlePanel, 0f);
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
                Image personIcon = personBackground.GetComponentInChildren<Image>();
                if (personIcon != null)
                {
                    personIcon.sprite = randomSprite;
                }
            }
        }
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
        yield return StartCoroutine(TypewriterEffect(dialogueOpponent1, opponent1Text));

        dialoguePlayer1.SetActive(true);
        dialoguePlayer1.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(animationDuration);
        yield return StartCoroutine(TypewriterEffect(dialoguePlayer1, player1Text));

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

        yield return StartCoroutine(ShowPanelWithChildren(enterIconPanel));
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