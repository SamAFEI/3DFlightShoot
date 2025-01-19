using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public static AudioSource BGMSource { get; private set; }
    public static AudioSource SFXSource { get; private set; }

    public AudioClip bgmClip;
    public AudioClip noFireSFXClip;
    public AudioClip noEPSFXClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
        BGMSource = transform.Find("BGMSource").GetComponent<AudioSource>();
        SFXSource = transform.Find("SFXSource").GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (bgmClip != null)
        {
            BGMSource.clip = bgmClip;
            BGMSource.loop = true;
            BGMSource.Play();
        }
    }

    public static void PlaySFXOnPoint(AudioClip clip, Vector3 point, float volume = 1f)
    {
        GameObject obj = new GameObject(clip.name);
        obj.transform.position = point;
        AudioSource audio = obj.AddComponent<AudioSource>();
        audio.outputAudioMixerGroup = SFXSource.outputAudioMixerGroup;
        audio.clip = clip;
        audio.volume = volume;
        audio.Play();
        Destroy(obj, clip.length);
    }

    public static void NoFireSFX()
    {
        if (SFXSource.isPlaying) { return; }
        SFXSource.clip = Instance.noFireSFXClip;
        SFXSource.loop = false;
        SFXSource.volume = 0.8f;
        SFXSource.Play();
    }
    public static void NoEPSFX()
    {
        if (SFXSource.isPlaying) { return; }
        SFXSource.clip = Instance.noEPSFXClip;
        SFXSource.loop = false;
        SFXSource.volume = 0.8f;
        SFXSource.Play();
    }
}
