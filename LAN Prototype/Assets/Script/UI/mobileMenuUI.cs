using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mobileMenuUI : MonoBehaviour
{
    [SerializeField]
    private Button openButton, closeButton;

    [SerializeField]
    private GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        openButton.onClick.AddListener(() => OpenMenu());
        closeButton.onClick.AddListener(() => CloseMenu());


        panel.SetActive(false);
        
    }

    private void OpenMenu()
    {
        panel.SetActive(true);
        openButton.gameObject.SetActive(false);
    }

    private void CloseMenu()
    {
        panel.SetActive(false);
        openButton.gameObject.SetActive(true);
    }

}
