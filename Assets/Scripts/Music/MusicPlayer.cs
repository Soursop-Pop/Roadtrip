using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class MusicPlayer : MonoBehaviour
{
    [Header("FMOD Music Settings")]
    public EventReference musicEvent; // Use EventReference for compile-time safety
    private EventInstance musicInstance;
    public int currentSongIndex = 0; // Starting at 0, adjust if needed
    public int totalSongs = 4; // Update this count to match the number of songs

    void Update()
    {
        // Keyboard input
        if (Input.GetKeyDown(KeyCode.Alpha2)) // "2" for next song
        {
            NextSong();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) // "1" for previous song
        {
            PreviousSong();
        }
    }

    void PlaySong(int index)
    {
        // Create a new instance of the music event using the EventReference.
        musicInstance = RuntimeManager.CreateInstance(musicEvent);

        // Set the parameter "SongSelector" to the current index.
        musicInstance.setParameterByName("SongSelector", index);

        musicInstance.start();
    }

    public void NextSong()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(STOP_MODE.IMMEDIATE); // Consider using ALLOWFADEOUT if needed.
            musicInstance.release();
        }
        currentSongIndex = (currentSongIndex + 1) % totalSongs;
        PlaySong(currentSongIndex);
    }

    public void PreviousSong()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop((FMOD.Studio.STOP_MODE)FMODUnity.STOP_MODE.Immediate);
            musicInstance.release();
        }
        // Ensure correct wrap-around for negative values.
        currentSongIndex = (currentSongIndex - 1 + totalSongs) % totalSongs;
        PlaySong(currentSongIndex);
    }
}