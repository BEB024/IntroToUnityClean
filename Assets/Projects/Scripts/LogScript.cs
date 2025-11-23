using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public string message = "Hello, Unity!";
    public int x;


    [HideInInspector] public float y = 6;

    public Color color;

    [SerializeField] private GameObject obj;

    void Start()
    {
        Debug.Log(message);
    }
}
