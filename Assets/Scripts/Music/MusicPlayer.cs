using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class MusicPlayer : MonoBehaviour
{
    [System.Serializable]
    public class SongInfo
    {
        public string bandName;
        public string songTitle;
        public string publisher;
    }

    public static MusicPlayer Instance;

    [Header("FMOD Music Settings")]
    public EventReference musicEvent; // Use EventReference for compile-time safety
    private EventInstance musicInstance;
    public int currentSongIndex = 0;
    public int totalSongs = 4;

    [Header("Song Information")]
    // An array holding song info that corresponds to your songs (set these values in the Inspector)
    public SongInfo[] songInfos;

    [Header("Car Audio Settings")]
    // Toggle this to simulate the player being in or out of the car.
    // When in the car, the volume is full; when out, the volume is reduced.
    public bool isPlayerInCar = true;

    private void Awake()
    {
        // Singleton pattern to persist between scenes.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            if (musicInstance.isValid())
            {
                musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
                musicInstance.release();
            }
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Removed auto-start. Music will now only play when player presses 1 (previous)
        // or 2 (next).
        // PlaySong(currentSongIndex); <-- removed to delay playback.
    }

    private void Update()
    {
        // Key 2: Next song.
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NextSong();
        }
        // Key 1: Previous song.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PreviousSong();
        }
        // Key 0: Stop music.
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StopSong();
        }

        // Adjust the volume based on the player's car status.
        if (musicInstance.isValid())
        {
            if (isPlayerInCar)
            {
                musicInstance.setVolume(1.0f); // Full volume inside the car.
            }
            else
            {
                musicInstance.setVolume(0.5f); // Lower volume when outside the car.
            }
        }
    }

    void PlaySong(int index)
    {
        // Create a new instance of the music event.
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.setParameterByName("SongSelector", index);
        musicInstance.start();

        // Trigger the UI to display the info for the current song.
        if (SongInfoUI.Instance != null && songInfos != null && index < songInfos.Length)
        {
            SongInfoUI.Instance.DisplaySongInfo(songInfos[index]);
        }
    }

    public void NextSong()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
        }
        currentSongIndex = (currentSongIndex + 1) % totalSongs;
        PlaySong(currentSongIndex);
    }

    public void PreviousSong()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
        }
        currentSongIndex = (currentSongIndex - 1 + totalSongs) % totalSongs;
        PlaySong(currentSongIndex);
    }

    public void StopSong()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
        }
    }
}
