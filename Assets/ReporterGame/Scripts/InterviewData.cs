using UnityEngine;

[CreateAssetMenu(fileName = "InterviewData", menuName = "Interview/Interview Data")]
public class InterviewData : ScriptableObject
{
    [System.Serializable]
    public class InterviewQuestion
    {
        public int id;
        public string main_question;
        public string short_question;
        public string answer;
    }

    public InterviewQuestion[] questions;
    public Sprite[] personSprites;
}