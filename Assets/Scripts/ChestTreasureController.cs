using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestTreasureController : MonoBehaviour
{

    private Animator _animator;

    public float ColliderRadius;

    public bool IsOpened;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        ColliderRadius = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayer();
    }

    void GetPlayer()
    {
        foreach (Collider collider in Physics.OverlapSphere((transform.localPosition - transform.forward * ColliderRadius), ColliderRadius))
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    OpenChest(); 
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.localPosition - transform.forward, ColliderRadius);
        //Action cube for player
        //Gizmos.DrawWireCube(new Vector3(transform.localPosition.x, transform.localPosition.y + 1, transform.localPosition.z) - transform.forward, new Vector3(1, 1, 1)); 
    }

    void OpenChest()
    {
        IsOpened = true;
        _animator.SetTrigger("isOpened");
    }
}
