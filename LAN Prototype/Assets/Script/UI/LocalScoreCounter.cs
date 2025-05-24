using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class LocalScoreCounter : NetworkBehaviour
{
    [SerializeField]
    public TextMeshProUGUI vrScore, mobileScore;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;
        GameScoreManager.Instance.OnScoreChanged += ScoreUpdate;
    }


    private void ScoreUpdate(object sender, System.EventArgs e)
    {
        if(!IsOwner) return;
        vrScore.text = GameScoreManager.Instance.GetVRScore().ToString();
        mobileScore.text = GameScoreManager.Instance.GetMobileScore().ToString();
    }
}
