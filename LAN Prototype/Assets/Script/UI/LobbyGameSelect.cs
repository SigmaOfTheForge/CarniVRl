using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyGameSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject[] levelPanels;

    private GameObject[] spawnedPanels;

    private Button leftButton, rightButton, playButton;

    private int index;

    

    private void Awake()
    {
        System.Array.Resize(ref spawnedPanels, levelPanels.Length);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.childCount);
        leftButton = transform.GetChild(0).GetComponent<Button>();
        rightButton = transform.GetChild(1).GetComponent<Button>();
        playButton = transform.GetChild(2).GetComponent<Button>();

        leftButton.onClick.AddListener(() => SelectLeft());
        rightButton.onClick.AddListener(() => SelectRight());
        playButton.onClick.AddListener(() => SelectPlay());

        for(int i = 0; i < levelPanels.Length; i++)
        {
            spawnedPanels[i] = Instantiate(levelPanels[i], transform);
            spawnedPanels[i].SetActive(false);
        }
        spawnedPanels[0].SetActive(true);
        index = 0;
    }

    void SelectLeft()
    {
        spawnedPanels[index].SetActive(false );
        index--;
        if(index < 0)
        {
            index = spawnedPanels.Length - 1;
        }
        spawnedPanels[index].SetActive(true);

    }

    void SelectRight()
    {
        spawnedPanels[index].SetActive(false);
        index++;
        if (index >= spawnedPanels.Length)
        {
            index = 0;
        }
        spawnedPanels[index].SetActive(true);

    }

    void SelectPlay()
    {
        LevelDetails details = spawnedPanels[index].gameObject.GetComponent<LevelName>().GetLevelDetails();
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ChangeScene(details.GetLevelName(), details.GetPlayerType());
    }
}
