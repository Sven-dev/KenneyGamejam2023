using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthGUI : MonoBehaviour
{
    [SerializeField] private Text Label;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(Camera.main.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateHealthStatus(float damage)
    {
        Label.text = damage.ToString();
        StartCoroutine(_DisplayHealthLoss());
    }

    private IEnumerator _DisplayHealthLoss()
    {
        float progress = 0;
        while (progress < 1)
        {
            Vector3 down = transform.position;
            Vector3 up = transform.position + Vector3.up * 0.5f;

            transform.position = Vector3.Lerp(down, up, progress);

            Color c = Label.color;
            c.a = Mathf.Lerp(1, 0, progress);
            Label.color = c;

            progress = Mathf.Clamp01(progress + Time.deltaTime);
            yield return null;
        }
    }
}
