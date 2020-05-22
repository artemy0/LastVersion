using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static System.Action<float> OnUpdateProgress;

    [SerializeField] private Transform PlayerAnchor;

    private LevelGenerator LevelGenerator;
    private int levelLength;
    private float oneUnitLength;

    private float distanceToFinish;
    private float currentDistanceToFinish;

    private Vector3 priviousPos;
    private Vector3 currentPos;

    private void Awake()
    {
        LevelGenerator = GetComponent<LevelGenerator>();

        levelLength = LevelGenerator.PipeLength;
        oneUnitLength = LevelGenerator.OneUnitValue;
        distanceToFinish = oneUnitLength * (levelLength - 1); //levelLength - 1 так как мы начинаем на краю старта и заканчиваем на краю финиша, а отсчёт производится с середины старта и финиша (2-е половины = целая клетка)

        currentDistanceToFinish = distanceToFinish;
        priviousPos = PlayerAnchor.position;
    }

    //Скрипт для трекинга прогресса в рандомно генерируемой труюе
    private void Update()
    {
        currentPos = PlayerAnchor.position;
        currentDistanceToFinish -= Vector3.Distance(priviousPos, currentPos);
        priviousPos = currentPos;

        float progress = 1 - (currentDistanceToFinish / distanceToFinish);

        OnUpdateProgress?.Invoke(progress);
    }

}
