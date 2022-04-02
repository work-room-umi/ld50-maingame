using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrifterManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ship;
    [SerializeField]
    private float deleteDist;
    [SerializeField]
    private float spawnDist;
    [SerializeField]
    private int maxDrifterNum;
    [SerializeField]
    private List<string> drifterPrefabs;

    private List<GameObject> managedDrifters;

    // Start is called before the first frame update
    private void Start()
    {
        managedDrifters = new List<GameObject>();
    }

    private Vector3 generateRandomBasePosition()
    {
        float generateComponent()
        {
            float c = Random.Range(spawnDist, deleteDist);
            if (Random.value < .5)
            {
                return c;
            }
            else
            {
                return -c;
            }
        };
        return new Vector3(generateComponent(), 0, generateComponent());
    }

    void GC(Vector3 shipPosition)
    {
        foreach(GameObject drifter in managedDrifters)
        {
                Vector3 position = drifter.transform.position;
                float dist = Vector3.Distance(position, shipPosition);
                if( deleteDist < dist )
                {
                    managedDrifters.Remove(drifter);
                    Destroy(drifter);
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 shipPosition = ship.transform.position;
        GC(shipPosition);
        
        if (managedDrifters.Count<maxDrifterNum)
        {
            // 漂流物の初期位置を計算する
            Vector3 basePosition = generateRandomBasePosition();
            Vector3 position = shipPosition + basePosition;

            int index = Random.Range(0, drifterPrefabs.Count);
            GameObject prefab = (GameObject)Resources.Load(drifterPrefabs[index]);
            GameObject drifter = Instantiate (prefab, position, Quaternion.identity);
            managedDrifters.Add(drifter);
        }
    }
}
