using UnityEngine;
using Proyecto26;
using SimpleJSON;
using System.Linq;
using UnityEngine.SocialPlatforms.Impl;
public class fireBaseRankingManager : MonoBehaviour
{
    public const string url = "https://gi311-leaderboard-default-rtdb.asia-southeast1.firebasedatabase.app/";
    public const string secret = "v2rc5XbhHhBbUPJcq4FRhzmosDlnd87fDg7nyinm";
    [SerializeField] private GamePlay gamePlay;
    public int score;
    private int lastScore = -1;
    public class KeepHighScore
    {
        public int highScore = 0;
    }

    public KeepHighScore keepHighScore = new KeepHighScore();

    public void Start()
    {
        GetData();
    }
    void Update()
    {
        if(score <= gamePlay.highScore)
        {
            score = gamePlay.highScore;
        }
        // ถ้าคะแนนเปลี่ยน → อัปเดต Firebase
        if (score != lastScore)
        {
            keepHighScore.highScore = score;
            SetData();
            lastScore = score;
            
        }
    }


    public void SetData()
    {
        string urlData = $"{url}.json?auth={secret}";

        RestClient.Put<KeepHighScore>(urlData, keepHighScore)
            .Then(response =>
            {
                Debug.Log("Upload Success");
            })
            .Catch(error =>
            {
                Debug.Log(error);
            });
    }
    public void GetData()
    {
        string urlData = $"{url}.json?auth={secret}";

        RestClient.Get<KeepHighScore>(urlData).Then(response =>
        {
            // response = KeepHighScore ที่ Firebase ส่งมา
            gamePlay.highScore = response.highScore;
            score = response.highScore;
            Debug.Log("GET Success! HighScore = " + response.highScore);
        })
        .Catch(error =>
        {
            Debug.LogError(error);
        });
    }
}
