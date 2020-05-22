using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAnchor"))
        {
            PlayerAnchor playerAnchor = collision.gameObject.GetComponentInParent<PlayerAnchor>(); //нет смысла кэшировать данные так как игрок сталкнётся с препятствием только 1 раз
            playerAnchor.Victory();
        }
    }
}
