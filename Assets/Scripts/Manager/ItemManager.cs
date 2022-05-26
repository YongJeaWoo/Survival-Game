using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Transform[] itemZones;
    public GameObject[] items;
    public List<int> itemList;

    private void Awake()
    {
        itemList = new List<int>();

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(5f);

        for (int index = 0; index < 20; index++)
        {
            int ranItem = Random.Range(0, 3);
            itemList.Add(ranItem);

            while (itemList.Count > 0)
            {
                int ranZone = Random.Range(0, 8);
                Instantiate(items[itemList[0]], itemZones[ranZone].position, itemZones[ranZone].rotation);

                //if (itemZones[ranZone] != null)
                //    ranZone += 1;

                //if (ranZone > itemZones.Length)
                //    yield return null;

                //if (itemZones[ranZone] )


                itemList.RemoveAt(0);
                yield return new WaitForSeconds(10f);
            }
        }
    }
}
