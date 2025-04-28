using Unity.VisualScripting;
using UnityEngine;

public class MosquitePositionSetter : MonoBehaviour
{
    public Transform obj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(obj == null){
            Debug.Log(name);
            Debug.Break();
            obj= new GameObject($"{name}_follower").transform;
            obj.SetParent(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        obj.transform.position=this.transform.position;
        obj.transform.rotation=this.transform.rotation;
    }
}
