using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicPlayer : MonoBehaviour
{
    public string musicEvent; // FMOD event path
    private EventInstance musicInstance;
    private int currentSongIndex = 1;
    public int totalSongs = 4; // update this count to match the number of songs

    void Update()
    {
        float dpadHorizontal = Input.GetAxis("DPadHorizontal");
        // Remove the playerInVehicle check since the component is enabled only when the player is in the car.

        // Keyboard input
        if (Input.GetKeyDown(KeyCode.Alpha2)) // "2" for next song
        {
            NextSong();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) // "1" for previous song
        {
            PreviousSong();
        }

        if (dpadHorizontal > 0.5f) // D-pad right
        {
            NextSong();
        }
        else if (dpadHorizontal < -0.5f) // D-pad left
        {
            PreviousSong();
        }
    }

    void PlaySong(int index)
    {
        // Create a new instance of the music event
        musicInstance = RuntimeManager.CreateInstance(musicEvent);

        // Set the parameter "SongSelector" to the current index
        musicInstance.setParameterByName("SongSelector", index);

        musicInstance.start();
    }

    public void NextSong()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicInstance.release();
        }
        currentSongIndex = (currentSongIndex + 1) % totalSongs;
        PlaySong(currentSongIndex);
    }

    public void PreviousSong()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicInstance.release();
        }
        currentSongIndex = (currentSongIndex - 1 + totalSongs) % totalSongs;
        PlaySong(currentSongIndex);
    }
}