using JetBrains.Annotations;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] AudioClip intro;
    [SerializeField] AudioClip sound1;
    [SerializeField] AudioClip sound2;
    [SerializeField] AudioClip sound3;
    [SerializeField] AudioClip sound4;
    [SerializeField] AudioClip sound5;

    AudioClip[] sounds;
    public static SoundManager sharedInstance;
    void Awake(){
        sharedInstance = this;
    }
    void Start()
    {
        sounds = new AudioClip[5];
        sounds[0] = sound1;
        sounds[1] = sound2;
        sounds[2] = sound3;
        sounds[3] = sound4;
        sounds[4] = sound5;
        //gameObject.GetComponent<AudioSource>().PlayOneShot(intro);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSound(int value){
        if(value>-1 && value<sounds.Length)
        gameObject.GetComponent<AudioSource>().PlayOneShot(sounds[value]);
    }
}
