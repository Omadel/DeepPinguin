using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public Sound[] sounds;

    public static AudioManager instance;

    // Start is called before the first frame update
    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }
        this.transform.parent = null;
        DontDestroyOnLoad(this.gameObject);

        foreach(Sound sound in this.sounds) {
            sound.source = this.gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }


    public void Play(string name) {
        Sound s = Array.Find(this.sounds, sound => sound.name == name);
        if(s == null) {
            return;
        }
        s.source.Play();
    }
    public void Stop(string name) {
        Sound s = Array.Find(this.sounds, sound => sound.name == name);
        if(s == null) {
            return;
        }
        s.source.Stop();
    }

}
