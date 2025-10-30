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

    [SerializeField] private Button[] optionButtons;

    [SerializeField] private float animationDuration = 0.5f;

    private List<InterviewData.InterviewQuestion> selectedQuestions;
    private int selectedQuestionIndex;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PrepareUI();
        HideAllButtons();
        StartCoroutine(InterviewRoutine());
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

        dialoguePlayer1.SetActive(true);
        dialoguePlayer1.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(animationDuration);

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

        TextMeshProUGUI player2Text = dialoguePlayer2.GetComponentInChildren<TextMeshProUGUI>();
        if (player2Text != null && selectedQuestionIndex < selectedQuestions.Count)
        {
            player2Text.text = selectedQuestions[selectedQuestionIndex].main_question;
        }

        dialoguePlayer2.SetActive(true);
        dialoguePlayer2.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(animationDuration);

        TextMeshProUGUI opponent2Text = dialogueOpponent2.GetComponentInChildren<TextMeshProUGUI>();
        if (opponent2Text != null && selectedQuestionIndex < selectedQuestions.Count)
        {
            opponent2Text.text = selectedQuestions[selectedQuestionIndex].answer;
        }

        dialogueOpponent2.SetActive(true);
        dialogueOpponent2.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(animationDuration);

        continueButton.SetActive(true);
        continueButton.transform.DOScale(1f, animationDuration).SetEase(Ease.OutBack);
    }

    private void OnContinueClicked()
    {
        Debug.Log("Continue button clicked!");
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