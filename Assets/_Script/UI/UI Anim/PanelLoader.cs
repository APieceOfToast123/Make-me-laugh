using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PanelLoader : MonoBehaviour
{
    public GameObject LoadedPanel;
    private Animator LoadedPanelAnimator;
    public bool PanelOn = false;

    void Start()
    {
        LoadedPanelAnimator = LoadedPanel.GetComponent<Animator>();

        if(PanelOn){
            ClosePanel();
        }
    }

    public void TogglePanel()
    {
        if (PanelOn)
        {
            ClosePanel();
        }
        else
        {
            LoadPanel();
        }
    }

    public void LoadPanel()
    {
        if(!PanelOn){
            LoadedPanelAnimator.SetTrigger("LoadPanel");
            PanelOn = true;
        }
    }

    public void ClosePanel()
    {
        if(PanelOn){
            LoadedPanelAnimator.SetTrigger("ClosePanel");
            PanelOn = false;
        }
    }

}
