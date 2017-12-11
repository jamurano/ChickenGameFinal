using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
    private const string SCORE_FILE = "Scores.dat";
    private const char SCORE_SEPARATOR = ',';
    private const char NEW_SCORE_SEPARATOR = '\n';

    public int maximumTopScores;

    public void LoadLevel()
    {
        SceneManager.LoadScene(0);

    }

    public void Start()
    {
        SetHighScores(ReadScores());

        GameObject.Find("Your Score").GetComponent<Text>().text = ScoreManager.score.ToString();
    }

    public void TakeUserInitials(){
        var textField = GameObject.Find("Initials").GetComponent<InputField>();

        Debug.Log(textField != null);
        Debug.Log(textField.text);

        var scores = ReadScores().OrderByDescending(sd => sd.Score).ToList();

        if(scores.Count() < maximumTopScores || ScoreManager.score > scores.Min(s => s.Score)) {
            scores.Add(new ScoreData() {Initials = textField.text, Score = ScoreManager.score});
        }

        scores = scores.OrderByDescending(s => s.Score).Take(maximumTopScores).ToList();

        using (var fs = new FileStream(SCORE_FILE, FileMode.OpenOrCreate, FileAccess.ReadWrite)) 
        using(var sw = new StreamWriter(fs))
        {
            foreach(var score in scores) {
                sw.WriteLine(string.Format("{0},{1}", score.Initials, score.Score));
            }
        }

        SetHighScores(scores);

        Destroy(GameObject.Find("Enter Button"));
    }

    private void SetHighScores(List<ScoreData> scores) {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        var topScoresUI = GameObject.Find("Top Scores").GetComponent<Text>();

        sb.Append("Top Scores\n\n");

        for (int i = 0; i < scores.Count(); i++) {
            sb.Append(i + 1).Append(".\t").Append(scores[i].Initials).Append('\t').Append(scores[i].Score).Append('\n');
        }

        topScoresUI.text = sb.ToString();
    }

    private List<ScoreData> ReadScores() {
        string fileData;
        using (var fs = new FileStream(SCORE_FILE, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        using (var sr = new StreamReader(fs)) {
            fileData = sr.ReadToEnd();
        }

        var scoreLines = fileData.Split(new char[] { NEW_SCORE_SEPARATOR }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
        List<ScoreData> result = new List<ScoreData>();

        foreach(var line in scoreLines) {
            var scoreData = line.Split(new char[] { SCORE_SEPARATOR }, System.StringSplitOptions.RemoveEmptyEntries);

            result.Add(new ScoreData(){Initials = scoreData[0], Score = int.Parse(scoreData[1])});
        }

        return result;
    }

    struct ScoreData {
        public string Initials { get; set; }
        public int Score { get; set; }
    }
}
