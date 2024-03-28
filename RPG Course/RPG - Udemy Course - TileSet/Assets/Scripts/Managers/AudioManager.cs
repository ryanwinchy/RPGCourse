using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;  //Singleton

    [SerializeField] float minDistanceToSound;
    [SerializeField] AudioSource[] sfx;
    [SerializeField] AudioSource[] backgroundMusic;

    public bool playBackgroundMusic;
    int backgroundMusicIndex;

    bool canPlaySfx;    //Without this, you hear lots of sfx on game start.

    private void Awake()        //Singleton
    {
        if (instance != null)    //Check if any instance, if is, destroy it. If none, assign it. This is because we only want one instance of player manager. When change scenes,tries make two.
            Destroy(instance.gameObject);
        else
            instance = this; // first instance assigned, all others destroyed.

        Invoke("AllowSFX", 1);   //For first second cant play sfx, otherwise you hear them all play on load.
    }

    private void Update()
    {
        if (!playBackgroundMusic)
            StopAllBackgroundMusic();
        else
        {
            if (!backgroundMusic[backgroundMusicIndex].isPlaying)
                PlayBackgroundMusic(backgroundMusicIndex);
        }
    }


    public void PlaySFX(int _index, Transform _source)    //If pass null for transform, will be no distance check.
    {
        if (!canPlaySfx)   //This is to for first second cant play sfx, prevents hearing loads on game load.
            return;

        if (sfx[_index].isPlaying)    //If same sound already playing, exit. Dont want to hear 10x skeleton bones.
            return;

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > minDistanceToSound)
            return;

        if (_index < sfx.Length)      //Check its in array of sound effects.
        {
            sfx[_index].pitch = Random.Range(0.7f, 1.2f);       //Adds nice variety to sfx.
            sfx[_index].Play();      //Play given sound effect.
        }
    }

    public void StopSFX(int _sfxIndex) => sfx[_sfxIndex].Stop();

    public void FadeOutVolume(int _index)
    {
        if (this != null)
        {
            StartCoroutine(StopSFXOverTime(sfx[_index]));
        }

    }

    IEnumerator StopSFXOverTime(AudioSource _audio)        //For area sounds. Should make fade in as well, or feels too instant.
    {
        float defaultVolume = _audio.volume;

        while (_audio.volume > 0.1f)
        {
            _audio.volume -= _audio.volume * 0.1f;    //Reduce by 20% every 1/4 of a second.
            yield return new WaitForSeconds(0.75f);  //Can make for this, how quick it fades.

            if (_audio.volume <= 0.1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;                              //Exit loop.
            }
        }
    }

    public void PlayRandomBackgroundMusic()
    {
        backgroundMusicIndex = Random.Range(0, backgroundMusic.Length);
        PlayBackgroundMusic(backgroundMusicIndex);
    }

    public void PlayBackgroundMusic(int _backgroundMusicIndex)
    {
        backgroundMusicIndex = _backgroundMusicIndex;

        StopAllBackgroundMusic();

        backgroundMusic[backgroundMusicIndex].Play();    //Play requested music.
    }

    public void StopAllBackgroundMusic()
    {
        for (int i = 0; i < backgroundMusic.Length; i++)  //Stop all background music.
        {
            backgroundMusic[i].Stop();
        }
    }

    void AllowSFX() => canPlaySfx = true;

}
