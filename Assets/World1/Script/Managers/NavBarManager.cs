using TMPro;
using UnityEngine;

public class NavBarManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stageLevelLabel;
    [SerializeField] GameObject HelpPanel;

    private void Start()
    {
    }
    public void ChangeLevelIndicator(int stageLvl)
    {
        stageLevelLabel.text = "Stage " + stageLvl;
    }
    public void ChangeToTutorial()
    {
        stageLevelLabel.text = "Tutorial";
    }
}
