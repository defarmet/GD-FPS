using UnityEngine;
using UnityEngine.UI;

public class UIElementController : MonoBehaviour
{
    [SerializeField] GameObject hints;
    [SerializeField] Slider slider;
    [SerializeField] Light[] lights;

    float lightCurrentValue;

    bool toggle = true;

    private void Start()
    {
        foreach(Light light in lights)
        {
            slider.value = light.intensity;
        }

        slider.onValueChanged.AddListener(delegate { CheckLightIntensityValues(); });

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            toggle = !toggle;
            hints.SetActive(toggle);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void CheckLightIntensityValues()
    {
        foreach (Light light in lights)
        {
            light.intensity = lightCurrentValue + slider.value;
        }

    }
}
