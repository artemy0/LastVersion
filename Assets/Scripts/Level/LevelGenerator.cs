using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int PipeLength = 20;
    public float OneUnitValue { get; private set; }

    [Header("Pipes")]
    [Range(0.0f, 0.5f)]
    [SerializeField] private float TurningProbability = 0.15f; //вероятность возврата в вертикальное положение в 2-а раза больше

    [Space(10)]
    [SerializeField] private GameObject StartPipe;
    [SerializeField] private GameObject EndPipe;
    [Space(10)]
    [SerializeField] private GameObject VerticalPipe;
    [SerializeField] private GameObject HorizontalPipe;
    [Space(10)]
    [SerializeField] private GameObject RotationPipeBottomRight;
    [SerializeField] private GameObject RotationPipeTopRight;
    [SerializeField] private GameObject RotationPipeBottomLeft;
    [SerializeField] private GameObject RotationPipeTopLeft;


    [Header("Obstacles")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float ObstaclesProbability = 0.7f;

    [SerializeField] private GameObject[] Obstacles;


    [Header("Coins")]
    [SerializeField] private GameObject Coin;
    [SerializeField] private GameObject[] CoinsGroups;

    private void Awake()
    {
        //определение размеров объектов
        BoxCollider2D RotationPipeCollider = RotationPipeBottomRight.GetComponent<BoxCollider2D>();
        OneUnitValue = RotationPipeCollider.size.y; //y - это размер всей ячейки, а по x коллайдор обрезан
    }

    private void Start()
    {
        //генерация массива клеток трубы
        Vector2[] pipeCellPositions = GenerateArray();

        //генерация трубы по массиву клеток
        GeneratePipes(pipeCellPositions);
        GenerateObstacles(pipeCellPositions);
    }

    private Vector2[] GenerateArray()
    {
        Vector2[] pipeCellPositions = new Vector2[PipeLength];
        pipeCellPositions[0] = transform.position;

        Vector2 pipeDir = Vector2.up; //-направление в котором будет распологаться следующая труба
        //Y либо 0, либо 1. X лобо 0, если Y = 1, лобо -1 или 1, если Y = 0

        for (int i = 1; i < PipeLength; i++)
        {
            pipeCellPositions[i] = pipeCellPositions[i - 1] + pipeDir * OneUnitValue; //определение расположения следующей ячейки трубы


            bool areTurning = pipeDir == Vector2.up ? Random.Range(0.0f, 1.0f) < TurningProbability : Random.Range(0.0f, 1.0f) < 2 * TurningProbability; //поворот для продолжения движения прямо в 2-а раза вероятнее

            if (areTurning && pipeDir == Vector2.up)
            {
                if (Random.value < 0.5f) //50 на 50 - поворот вправо или влево
                {
                    pipeDir = Vector2.right;
                }
                else
                {
                    pipeDir = Vector2.left;
                }     
            }
            else if(areTurning && (pipeDir == Vector2.left || pipeDir == Vector2.right))
            {
                pipeDir = Vector2.up;
            }
        }

        return pipeCellPositions;
    }

    private void GeneratePipes(Vector2[] pipeCellPositions)
    {
        //вставляем первую ячейку под индексом 0
        var pipe = Instantiate(StartPipe, transform);
        pipe.transform.localPosition = pipeCellPositions[0];

        for (int i = 1; i < PipeLength - 1; i++) //должно быть PipeLength - 1
        {
            if (IsСellTurn(pipeCellPositions, i)) //клетки расположены по диагонали, т.е. между ними поворот
            {
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!Можно вставить switch для красоты!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                if (pipeCellPositions[i - 1].x < pipeCellPositions[i + 1].x && pipeCellPositions[i - 1].y == pipeCellPositions[i].y) //поворот слева наверх
                {
                    pipe = Instantiate(RotationPipeTopLeft, transform);
                }
                else if (pipeCellPositions[i - 1].x > pipeCellPositions[i + 1].x && pipeCellPositions[i - 1].y == pipeCellPositions[i].y) //поворот справа наверх
                {
                    pipe = Instantiate(RotationPipeTopRight, transform);
                }
                else if (pipeCellPositions[i - 1].x > pipeCellPositions[i + 1].x && pipeCellPositions[i - 1].x == pipeCellPositions[i].x) //поворот снизу налево
                {
                    pipe = Instantiate(RotationPipeBottomLeft, transform);
                }
                else if (pipeCellPositions[i - 1].x < pipeCellPositions[i + 1].x && pipeCellPositions[i - 1].x == pipeCellPositions[i].x) //поворот снизу направо
                {
                    pipe = Instantiate(RotationPipeBottomRight, transform);
                }
                pipe.transform.localPosition = pipeCellPositions[i];
            }
            else if (IsCellHorizontal(pipeCellPositions, i))
            {
                pipe = Instantiate(VerticalPipe, transform);
                pipe.transform.localPosition = pipeCellPositions[i];
            }
            else if (IsCellVertical(pipeCellPositions, i))
            {
                pipe = Instantiate(HorizontalPipe, transform);
                pipe.transform.localPosition = pipeCellPositions[i];
            }
        }

        //вставить последнюю ячейку под индексом PipeLength -1!!!
        Quaternion quaternion = pipe.transform.rotation;
        pipe = Instantiate(EndPipe, transform);
        pipe.transform.localPosition = pipeCellPositions[PipeLength - 1];
        pipe.transform.rotation = quaternion;
    }

    private void GenerateObstacles(Vector2[] pipeCellPositions)
    {
        for (int i = 1; i < pipeCellPositions.Length - 2; i++)
        {
            if (!IsСellTurn(pipeCellPositions, i))
            {
                if (Random.value > ObstaclesProbability)
                    continue; //тоесть если рандомное значение больше вероятности появления препятствия то мы даже не рассматриваем появление препятствия


                int randomObstaclesIndex = Random.Range(0, Obstacles.Length);
                var pipe = Instantiate(Obstacles[randomObstaclesIndex], transform);
                pipe.transform.localPosition = pipeCellPositions[i];

                if (IsCellVertical(pipeCellPositions, i))
                {
                    if(pipeCellPositions[i].x > pipeCellPositions[i - 1].x)
                        pipe.transform.localEulerAngles += new Vector3(0, 0, -90); //повернуть объект на 90 градусов
                    else
                        pipe.transform.localEulerAngles += new Vector3(0, 0, 90);

                    i++;

                    //генерацция монет после препятствия
                    if(!IsСellTurn(pipeCellPositions, i))
                        GenerateCoin(pipeCellPositions, i);
                }
                else if(IsCellHorizontal(pipeCellPositions, i))
                {
                    i++;

                    if (!IsСellTurn(pipeCellPositions, i))
                        GenerateCoin(pipeCellPositions, i);
                }
            }
        }
    }

    private void GenerateCoin(Vector2[] pipeCellPositions, int index)
    {
        GameObject CoinsGroup = CoinsGroups[Random.Range(0, CoinsGroups.Length)];

        var pipe = Instantiate(CoinsGroup, transform);
        pipe.transform.localPosition = pipeCellPositions[index];

        if(IsCellVertical(pipeCellPositions, index))
        {
            if (pipeCellPositions[index].x > pipeCellPositions[index - 1].x)
                pipe.transform.localEulerAngles += new Vector3(0, 0, -90); //повернуть объект на 90 градусов
            else
                pipe.transform.localEulerAngles += new Vector3(0, 0, 90);
        }
    }

    private bool IsIndexValid(Vector2[] pipeCells, int index)
    {
        if (index >= 0 && index < pipeCells.Length)
            return true;

        return false;
    }

    //ничего страшног если выскачит сиключение OutOfRangeException сразу будет понятно где ошибка
    private bool IsСellTurn(Vector2[] pipeCells, int index)
    {
        if(IsIndexValid(pipeCells, index) && (pipeCells[index - 1].x != pipeCells[index + 1].x && pipeCells[index - 1].y != pipeCells[index + 1].y))
        {
            return true;
        }

        return false;
    }

    private bool IsCellVertical(Vector2[] pipeCells, int index)
    {
        if(IsIndexValid(pipeCells, index) && pipeCells[index - 1].y == pipeCells[index + 1].y)
        {
            return true;
        }

        return false; //но это может быть не верное утверждение, так как это может быть поворот
    }

    private bool IsCellHorizontal(Vector2[] pipeCells, int index)
    {
        if (IsIndexValid(pipeCells, index) && pipeCells[index - 1].x == pipeCells[index + 1].x)
        {
            return true;
        }

        return false; //но это может быть не верное утверждение, так как это может быть поворот
    }
}
