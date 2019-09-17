using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audioSource;
    enum State {Alive, Dying, Transending}
    State state = State.Alive;
   [SerializeField] float rcsThrust = 100f;
   [SerializeField] float mainThrust = 100f;
   [SerializeField] AudioClip mainEngine;
   [SerializeField] AudioClip collisionSound;
   [SerializeField] AudioClip completionSound;

   [SerializeField] ParticleSystem successParticles;
   [SerializeField] ParticleSystem deathParticles;
   [SerializeField] ParticleSystem mainEngineParticles;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive){
            ResponeToThrustInput();
            Rotate();
        }
    }

    void ResponeToThrustInput(){

        if(Input.GetKey(KeyCode.Space)){
            rigidbody.AddRelativeForce(Vector3.up * mainThrust);
            if(!audioSource.isPlaying){
                audioSource.PlayOneShot(mainEngine);
            }
            print("thrust here");
            mainEngineParticles.Play();
        }else{
            mainEngineParticles.Stop();
            audioSource.Stop();
        }
    }

    void LoadNextScene(){
        SceneManager.LoadScene(1);
        if(audioSource.isPlaying){
            audioSource.Stop();
        }  
    }

    void resetCurrentScene(){
           Scene activeScene = SceneManager.GetActiveScene();
            if(activeScene.buildIndex == 0){
                SceneManager.LoadScene(0);
            } else {
                SceneManager.LoadScene(1);
            }
    }

    void OnCollisionEnter(Collision collision){
        if(state != State.Alive) return;
        switch (collision.gameObject.tag)
        {   
            case "Friendly":
                break;
            case "Finish":
                state = State.Transending;
                if(audioSource.isPlaying){
                    audioSource.Stop();
                }       
                audioSource.PlayOneShot(completionSound);
                successParticles.Play();
                Invoke("LoadNextScene",2f);
                break;
            default:
                state = State.Dying;
                if(audioSource.isPlaying){
                    audioSource.Stop();
                }       

                audioSource.PlayOneShot(collisionSound);
                deathParticles.Play();
                Invoke("resetCurrentScene",2f);
                break;
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
