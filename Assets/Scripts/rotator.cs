using UnityEngine;

public class rotator : MonoBehaviour
{ 

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime * 2 );

        //angle += 2 * Mathf.PI * Time.deltaTime * this.speed;
        //float x = Mathf.Cos(angle * Time.deltaTime) * radius;
        //float y = Mathf.Sin(angle * Time.deltaTime) * radius;

        //transform.position = pos + new Vector3(x, y, 0);

        //float x = Mathf.Cos(2 * Mathf.PI * Time.deltaTime * speed) * 10;
        //float y = Mathf.Sin(2 * Mathf.PI * Time.deltaTime * speed) * 10;


    }
}
