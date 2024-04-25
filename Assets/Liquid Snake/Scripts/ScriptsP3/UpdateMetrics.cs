using LiquidSnake.Character;
using TMPro;
using UnityEngine;

public class UpdateMetrics : MonoBehaviour
{
    public TextMeshProUGUI fpsValueText;
    public TextMeshProUGUI interactedValueText;
    public TextMeshProUGUI healthValueText;
    public TextMeshProUGUI timeValueText;
    public TextMeshProUGUI successTasksValueText;
    public TextMeshProUGUI failedTasksValueText;
    public TextMeshProUGUI numberTasksValueText;
    public TextMeshProUGUI numberStatesValueText;

    private float deltaTime;
    private float fpsValue;
    private int interactedValue;
    public Health health;
    public int successTasksValue;
    public int failedTasksValue;
    public int numberTasksValue;
    public int numberStatesValue;

    void Start()
    {
        deltaTime = 0.0f;
        fpsValue = 0.0f;
        interactedValue = 0;
        successTasksValue = 0;
        failedTasksValue = 0;
        numberTasksValue = 0;
        numberStatesValue = 0;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        // FPS
        fpsValue = 1.0f / deltaTime;
        fpsValueText.text = fpsValue.ToString("F0");
        // Interacted
        interactedValueText.text = interactedValue.ToString();
        // Health
        if (health != null)
        {
            float healthValue = health.CurrentValue();
            healthValueText.text = healthValue.ToString("F0");
        }
        // Time
        timeValueText.text = Time.time.ToString("F2");
        // Success Tasks
        //successTasksValueText.text = successTasksValue.ToString();
        // Failed Tasks
        //failedTasksValueText.text = failedTasksValue.ToString();
        // Number Tasks
        //numberTasksValueText.text = numberTasksValue.ToString();
        // Number States
        //numberStatesValueText.text = numberStatesValue.ToString();
    }

    public void OnInteracted()
    {
        ++interactedValue;
    }
}
