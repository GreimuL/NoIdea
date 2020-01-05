using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public int init_pool_size = 20;
    public int[] pool_size;
    public int type_size = 2;
    public GameObject needle_instant;
    public GameObject narrow_needle_instant;
    private List<List<GameObject>> pool_list;
    static public List<Queue<GameObject>> inactive_needle_queue;

    // Start is called before the first frame update
    void Start()
    {
        pool_size = new int[type_size];
        pool_list = new List<List<GameObject>>();
        inactive_needle_queue = new List<Queue<GameObject>>();
        for(int i = 0; i < type_size; i++)
        {
            pool_size[i] = init_pool_size;
            pool_list.Add(new List<GameObject>());
            inactive_needle_queue.Add(new Queue<GameObject>());
        }

        for(int i = 0; i < init_pool_size; i++)
        {
            GameObject createdNeedle = Instantiate(needle_instant);
            createdNeedle.SetActive(false);
            pool_list[0].Add(createdNeedle);
            inactive_needle_queue[0].Enqueue(createdNeedle);

            createdNeedle = Instantiate(narrow_needle_instant);
            createdNeedle.SetActive(false);
            pool_list[1].Add(createdNeedle);
            inactive_needle_queue[1].Enqueue(createdNeedle);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject getNeedle(int type)
    {
        GameObject retNeedle;
        if (inactive_needle_queue.Count > 0)
        {
            retNeedle = inactive_needle_queue[type].Peek();
            inactive_needle_queue[type].Dequeue();
        }
        else
        {
            extendPoolSize(type);
            retNeedle = inactive_needle_queue[type].Peek();
            inactive_needle_queue[type].Dequeue();
        }
        return retNeedle;
    }
    void extendPoolSize(int type)
    {
        int pre_pool_size = pool_size[type];
        pool_size[type] *= 2;
        for(int i= pre_pool_size; i < pool_size[type]; i++)
        {
            GameObject createdNeedle = Instantiate(needle_instant);
            createdNeedle.SetActive(false);
            pool_list[type].Add(createdNeedle);
            inactive_needle_queue[type].Enqueue(createdNeedle);
        }
    }
}
