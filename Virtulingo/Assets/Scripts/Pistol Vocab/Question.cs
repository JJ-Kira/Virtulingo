using System.Collections.Generic;

namespace Pistol_Vocab
{
    public class Question
    {
        public string QuestionText;
        public List<string> Answers = new List<string>();
        
        public Question(string question, string[] answers)
        {
            QuestionText = question;
            
            foreach (var answer in answers)
            {
                Answers.Add(answer);
            }
        }
    }
}