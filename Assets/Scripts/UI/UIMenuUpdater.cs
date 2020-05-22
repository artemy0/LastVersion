using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Slider progressSlider;

    private void Start()
    {
        PlayerAnchor.OnCoinsUpdated += UpdateCoin;

        RaceController.OnUpdateProgress += UpdateProgress; //для заранее созданного уровня
        LevelController.OnUpdateProgress += UpdateProgress; //для процедурно генерируемого уровня
    }

    private void OnDestroy()
    {
        PlayerAnchor.OnCoinsUpdated -= UpdateCoin;

        RaceController.OnUpdateProgress -= UpdateProgress;
        LevelController.OnUpdateProgress -= UpdateProgress;
    }

    private void UpdateCoin(int coin)
    {
        coinText.text = coin.ToString();
    }

    private void UpdateProgress(float progress)
    {
        progressSlider.value = progress;
    }
}
