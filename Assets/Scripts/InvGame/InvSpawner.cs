using UnityEngine;

public class InvSpawner : MonoBehaviour
{
    [SerializeField] private GameObject inv;
    void Start()
    {
        for (var y = 0; y < 3; y++)
        {
            for (var x = 0; x < 10; x++)
            {
                var instance = Instantiate(inv, transform.position + new Vector3(x, y), Quaternion.identity);
                instance.transform.localScale = Vector3.one * 0.3f;
                instance.transform.parent = transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
