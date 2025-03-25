using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTraps : MonoBehaviour
{
    public Tilemap tilemap;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            // �浹�� ��ġ�� Ÿ�ϸ� Ÿ�� �߽� ���
            Vector3 collisionPosition = collision.contacts[0].point; // �浹 ��ġ (���� ��ǥ)
            Vector3Int cellPosition = tilemap.WorldToCell(collisionPosition); // �׸��� ��ǥ
            Vector3 tileWorldPosition = tilemap.GetCellCenterWorld(cellPosition); // Ÿ�� �߽� ��ǥ

            GameObject tempTransform = new GameObject("TempTransform");
            tempTransform.transform.position = tileWorldPosition;

            // �÷��̾� ��ũ��Ʈ���� playerKnock ȣ��
            Playrer player = collision.gameObject.GetComponent<Playrer>();
            if (player != null) {
                player.StartCoroutine(player.playerKnock(tempTransform.transform, 5f)); // �� ����
            }
        }
    }
}
