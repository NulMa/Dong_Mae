using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTraps : MonoBehaviour
{
    public Tilemap tilemap;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            // 충돌한 위치와 타일맵 타일 중심 계산
            Vector3 collisionPosition = collision.contacts[0].point; // 충돌 위치 (월드 좌표)
            Vector3Int cellPosition = tilemap.WorldToCell(collisionPosition); // 그리드 좌표
            Vector3 tileWorldPosition = tilemap.GetCellCenterWorld(cellPosition); // 타일 중심 좌표

            GameObject tempTransform = new GameObject("TempTransform");
            tempTransform.transform.position = tileWorldPosition;

            // 플레이어 스크립트에서 playerKnock 호출
            Playrer player = collision.gameObject.GetComponent<Playrer>();
            if (player != null) {
                player.StartCoroutine(player.playerKnock(tempTransform.transform, 5f)); // 힘 전달
            }
        }
    }
}
