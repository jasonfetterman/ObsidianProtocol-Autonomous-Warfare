using UnityEngine;
using UnityEngine.UI;

public class AcheronPlanetOverviewController : MonoBehaviour
{
    public GameObject overview;
    public GameObject information;
    public GameObject location;
    public GameObject actions;

    public void ShowOverview()
    {
        Set(overview,true);
        Set(information,false);
        Set(location,false);
        Set(actions,true);
    }

    public void ShowInformation()
    {
        Set(overview,true);
        Set(information,true);
        Set(location,false);
        Set(actions,true);
    }

    public void ShowLocation()
    {
        Set(overview,true);
        Set(information,false);
        Set(location,true);
        Set(actions,true);
    }

    public void CloseOverview()
    {
        Set(overview,false);
        Set(information,false);
        Set(location,false);
        Set(actions,false);
    }

    void Set(GameObject o,bool state)
    {
        if(o) o.SetActive(state);
    }
}
