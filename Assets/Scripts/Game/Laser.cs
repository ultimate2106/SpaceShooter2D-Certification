using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private float _speed = 10;
    #endregion

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 8f || transform.position.y <= -8f)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(gameObject);
        }
    }
}
