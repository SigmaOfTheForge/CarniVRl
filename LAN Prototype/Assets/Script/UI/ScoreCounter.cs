using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter SharedInstance;
    public int vRScore;
    public int mobileScore;
    public List<TextMeshProUGUI> vRScoreText;
    public List<TextMeshProUGUI> mobileScoreText;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        vRScore = 0;
        mobileScore = 0;
        for (int i = 0; i < vRScoreText.Count; i++)
        {
            vRScoreText[i].text = vRScore.ToString();
        }
        for (int i = 0; i < mobileScoreText.Count; i++)
        {
            mobileScoreText[i].text = mobileScore.ToString();
        }
    }

    //when VR player hits a mobile player off > VR Score ++
    //when Mobile player parries ball back to VR player > Mobile Score ++

    public void VRScored()
    {
        vRScore++;

        for (int i = 0; i < vRScoreText.Count; i++)
        {
            vRScoreText[i].text = vRScore.ToString();
        }
    }

    public void MobileScored()
    {
        mobileScore++;
        for (int i = 0; i < mobileScoreText.Count; i++)
        {
            mobileScoreText[i].text = mobileScore.ToString();
        }
    }

}
