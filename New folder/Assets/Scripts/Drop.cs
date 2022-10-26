using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Drop : MonoBehaviour
{
    public Stack<GameObject> StackedDropItems ;
    [SerializeField] float SellTime = 0.5f;

    void Start()
    {
        StackedDropItems = new Stack<GameObject>();
    }

    public IEnumerator SellDropedItems()
    {
        foreach (var item in StackedDropItems)
        {
            yield return new WaitForSeconds(0.05f);
            item.transform.DOScale(1.5f, 0.1f).SetLoops(2, LoopType.Yoyo);
        }

        while (StackedDropItems.Count > 0)
        {
            yield return new WaitForSeconds(SellTime);
            GameObject go = StackedDropItems.Pop();
            go.transform.DOScale(0, 0.3f);
        }
    }
}
