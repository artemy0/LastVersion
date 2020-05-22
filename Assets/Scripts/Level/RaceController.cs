using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RaceController : MonoBehaviour
{
    public static System.Action<float> OnUpdateProgress;

    [SerializeField] private Transform PlayerAnchor;
    [SerializeField] private Transform StartPipe, EndPipe;
    //[SerializeField] private Transform IntermediatePoint;

    private Vector3 PlayerAnchorPos;
    private Vector3 StartPipePos, EndPipePos;
    //private Vector3 IntermediatePos;

    private void Start()
    {
        PlayerAnchorPos = PlayerAnchor.position;

        StartPipePos = StartPipe.position;
        EndPipePos = EndPipe.position;
        //IntermediatePos = IntermediatePoint.position;
    }

    private void Update()
    {
        PlayerAnchorPos = PlayerAnchor.position;

        float progress = Vector3.Distance(PlayerAnchorPos, StartPipePos) / (Vector3.Distance(PlayerAnchorPos, StartPipePos) + Vector3.Distance(PlayerAnchorPos, EndPipePos));

        //float progress = (Vector3.Distance(PlayerAnchorPos, EndPipePos)) / (Vector3.Distance(StartPipePos, IntermediatePos) + Vector3.Distance(IntermediatePos, EndPipePos) - Vector3.Distance(StartPipePos, PlayerAnchorPos));

        OnUpdateProgress?.Invoke(progress);
    }
}
