using UnityEngine;



public class Player : MonoBehaviour

{

    public float speed;

    private Rigidbody _rb;





    void Start()

    {

        _rb = GetComponent<Rigidbody>();

    }





    void FixedUpdate()

    {

        if (Input.GetKey(KeyCode.W))

        {



            _rb.linearVelocity = Vector3.forward * speed;

        }
        else if (Input.GetKey(KeyCode.S))

        {



            _rb.linearVelocity = Vector3.back * speed;

        }
        else if (Input.GetKey(KeyCode.D))

        {

            _rb.linearVelocity = Vector3.right * speed;

        }
        else if (Input.GetKey(KeyCode.A))

        {

            _rb.linearVelocity = Vector3.left * speed;

        }

        else

        {

            _rb.linearVelocity = Vector3.zero;

        }











    }

}