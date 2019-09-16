using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audioSource;
   [SerializeField] float rcsThrust = 100f;
   [SerializeField] float mainThrust = 100f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();   
    }

    void Thrust(){

        if(Input.GetKey(KeyCode.Space)){
            rigidbody.AddRelativeForce(Vector3.up * mainThrust);
            if(!audioSource.isPlaying){
                audioSource.Play();
            }
        }else{
            audioSource.Stop();
        }
    }


    private void Rotate(){
        rigidbody.freezeRotation = true;
        float rotateThisFrame = rcsThrust * Time.deltaTime;

        if(Input.GetKey(KeyCode.A)){
            transform.Rotate(Vector3.forward * rotateThisFrame);
        }
        if(Input.GetKey(KeyCode.D)){
            transform.Rotate(-Vector3.forward * rotateThisFrame);
        }
        rigidbody.freezeRotation = false;
    }

}
