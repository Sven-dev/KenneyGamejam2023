using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPickup : MonoBehaviour, IInteractable
{

    [SerializeField]
    GameObject towerPrefab = null;

    private void Start()
    {
        StartCoroutine(_Rotate());
        StartCoroutine(_Bob());
    }

    private IEnumerator _Rotate()
    {
        while (true)
        {
            transform.Rotate(Vector3.up * 50 * Time.deltaTime, Space.Self);
            yield return null;
        }
    }

    private IEnumerator _Bob()
    {
        Vector3 down = transform.position - Vector3.up * 0.25f;
        Vector3 up = transform.position + Vector3.up * 0.25f;

        while (true)
        {
            float progress = Mathf.PingPong(Time.time, 1);
            transform.position = Vector3.Lerp(down, up, LerpCurves.Instance.EaseInOut(progress));

            yield return null;
        }
    }

    public GameObject GetTower()
    {
        return towerPrefab;
    }

    public void RemovePickUP()
    {
        gameObject.SetActive(false);
    }
}
