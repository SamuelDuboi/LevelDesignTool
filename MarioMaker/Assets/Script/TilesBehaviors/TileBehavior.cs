using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    public virtual void Start()
    {

    }

    private  void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollision(collision);
    }
    public virtual void OnCollision(Collision2D collision)
    {

    }

    public virtual void OnInputPlayer(KeyCode code)
    {

    }
}
