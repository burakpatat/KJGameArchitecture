using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoisonArch;
using System;

namespace PoisonArch
{
    /// <summary>
    /// Handles playing sounds and music based on their sound ID
    /// </summary>
    public enum SoundID
    {
        None,
        CoinSound,
        KeySound,
        ButtonSound,
        MenuMusic
    }
    public enum SoureID
    {
        None,
    }
    public class AudioManager : AbstractSingleton<AudioManager>
    {
        [Serializable]
        class SoundIDClipPair
        {
            public SoundID m_SoundID;
            public AudioClip m_AudioClip;
        }
        [Serializable]
        class SourceIDPair
        {
            public SoureID m_SoundID;
            public List<AudioSource> m_Sources;
        }

        [SerializeField]
        List<AudioSource> m_MusicSources;
        [SerializeField]
        List<AudioSource> m_EffectSources;

        [SerializeField, Min(0f)]
        float m_MinSoundInterval = 0.1f;
        [SerializeField]
        SoundIDClipPair[] m_Sounds;

        float m_LastSoundPlayTime;
        readonly Dictionary<SoundID, AudioClip> m_Clips = new();
        readonly Dictionary<SoureID, List<AudioSource>> m_Sources = new();

        AudioSettings m_AudioSettings = new();

        /// <summary>
        /// Unmute/mute the music
        /// </summary>
        public bool EnableMusic
        {
            get => m_AudioSettings.EnableMusic;
            set
            {
                m_AudioSettings.EnableMusic = value;
                for (int i = 0; i < m_MusicSources.Count; i++)
                {
                    m_MusicSources[i].mute = !value;
                }
            }
        }

        /// <summary>
        /// Unmute/mute all sound effects
        /// </summary>
        public bool EnableSfx
        {
            get => m_AudioSettings.EnableSfx;
            set
            {
                m_AudioSettings.EnableSfx = value;
                for (int i = 0; i < m_EffectSources.Count; i++)
                {
                    m_EffectSources[i].mute = !value;
                }
            }
        }

        /// <summary>
        /// The Master volume of the audio listener
        /// </summary>
        public float MasterVolume
        {
            get => m_AudioSettings.MasterVolume;
            set
            {
                m_AudioSettings.MasterVolume = value;
                AudioListener.volume = value;
            }
        }

        void Start()
        {
            foreach (var sound in m_Sounds)
            {
                m_Clips.Add(sound.m_SoundID, sound.m_AudioClip);
            }
        }

        void OnEnable()
        {
            if (SaveManager.Instance == null)
            {
                // Disable music, enable sfx, and 
                // set volume to a very low amount
                // in the LevelEditor
                EnableMusic = true;
                EnableSfx = true;
                MasterVolume = 0.2f;
                return;
            }

            var audioSettings = SaveManager.Instance.LoadAudioSettings();
            EnableMusic = audioSettings.EnableMusic;
            EnableSfx = audioSettings.EnableSfx;
            MasterVolume = audioSettings.MasterVolume;
        }

        void OnDisable()
        {
            if (SaveManager.Instance == null)
            {
                return;
            }

            SaveManager.Instance.SaveAudioSettings(m_AudioSettings);
        }

        void PlayMusic(AudioClip audioClip, bool looping = true)
        {
            if (m_MusicSource.isPlaying)
                return;

            m_MusicSource.clip = audioClip;
            m_MusicSource.loop = looping;
            m_MusicSource.Play();
        }
        void PlayMusicForStart(AudioClip audioClip, bool looping = true)
        {
            m_MusicSource.clip = audioClip;
            m_MusicSource.loop = looping;
            m_MusicSource.Play();
        }

        /// <summary>
        /// Play a music based on its sound ID
        /// </summary>
        /// <param name="soundID">The ID of the music</param>
        /// <param name="looping">Is music looping?</param>
        public void PlayMusic(SoundID soundID, bool looping = true)
        {
            PlayMusic(m_Clips[soundID], looping);
        }
        public void PlayMusicForStart(SoundID soundID, bool looping = true)
        {
            PlayMusicForStart(m_Clips[soundID], looping);
        }

        /// <summary>
        /// Stop the current music
        /// </summary>
        public void StopMusic()
        {
            m_MusicSource.Stop();
        }

        void PlayEffect(AudioClip audioClip)
        {
            if (Time.time - m_LastSoundPlayTime >= m_MinSoundInterval)
            {
                m_EffectSource.PlayOneShot(audioClip);
                m_LastSoundPlayTime = Time.time;
            }
        }

        /// <summary>
        /// Play a sound effect based on its sound ID
        /// </summary>
        /// <param name="soundID">The ID of the sound effect</param>
        public void PlayEffect(SoundID soundID)
        {
            if (soundID == SoundID.None)
                return;

            PlayEffect(m_Clips[soundID]);
        }
    }
    public class AudioSettings
    {
        public bool EnableMusic;
        public bool EnableSfx;
        public float MasterVolume;

        public AudioSettings()
        {
            EnableMusic = true;
            EnableSfx = true;
            MasterVolume = 0.25f;
        }
    }
}


