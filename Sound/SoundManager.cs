using System.Collections;
using System.Collections.Generic;
using Fox.Utils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Fox.Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager shared;

        [SerializeField] private float _fadeoutTimeMusic = 1f;
        [SerializeField] private float _fadeinTimeMusic = 1f;

        [SerializeField] private AudioMixer _sfxMixer;
        [SerializeField] private AudioMixer _musicMixer;

        [SerializeField] private List<AudioClip> _musicList;

        private AudioSource _currentMusic;
        private AudioSource _musicPlayer;
        private IObjectPool<AudioSource> _sfxPlayerPool;

        private void Awake()
        {
            shared = this;
            
            _musicPlayer = transform.CreateChildAudioSource("MusicPlayer");
            
            _sfxPlayerPool = new ObjectPool<AudioSource>(
                () => transform.CreateChildAudioSource("SfxPlayer"), 
                AudioSourceUtils.GetPooledAudioSource, 
                AudioSourceUtils.ReturnPooledAudioSource
                );
        }

        private void Start()
        {
            ToggleSfxPrivate(true);
            ToggleMusicPrivate(true);
        }

        private void Update()
        {
            if (_currentMusic == null || _currentMusic.isPlaying)
            {
                return;
            }

            _currentMusic.gameObject.SetActive(false);

            PlayNewMusic();
        }

        private void OnValidate()
        {
            shared = this;
        }

        private void ToggleSfxPrivate(bool toggle)
        {
            float volume = toggle ? 0f : -80f;
            _sfxMixer.SetFloat("volume", volume);
        }

        private void ToggleMusicPrivate(bool toggle)
        {
            float volume = toggle ? 0f : -80f;
            _musicMixer.SetFloat("volume", volume);
        }

        public bool ToggleSfx(bool toggle)
        {
            ToggleSfxPrivate(toggle);

            return toggle;
        }

        public bool ToggleMusic(bool toggle)
        {
            ToggleMusicPrivate(toggle);

            return toggle;
        }

        public void PlayMusic()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (_currentMusic != null)
            {
                StartCoroutine(FadeoutAudio(_fadeoutTimeMusic, _currentMusic));
                _currentMusic = null;
            }

            PlayNewMusic();
        }

        private void PlayNewMusic()
        {
            AudioClip clip = _musicList.GetRandom(null);
            if (clip == null)
            {
                return;
            }

            _musicPlayer.clip = clip;
            _musicPlayer.loop = false;
            _musicPlayer.Play();
            StartCoroutine(FadeinAudio(_fadeinTimeMusic, _musicPlayer));
        }

        private IEnumerator FadeoutAudio(float fadeoutTime, AudioSource source)
        {
            float currentTime = 0f;
            while (currentTime < fadeoutTime)
            {
                float percent = 1 - currentTime / fadeoutTime;
                source.volume = percent;

                currentTime += Time.deltaTime;
                yield return null;
            }

            source.Stop();
            source.gameObject.SetActive(false);
        }

        private static IEnumerator FadeinAudio(float fadeinTime, AudioSource source)
        {
            float currentTime = 0f;
            while (currentTime < fadeinTime)
            {
                float percent = currentTime / fadeinTime;
                source.volume = percent;

                currentTime += Time.deltaTime;
                yield return null;
            }

            source.volume = 1;
        }

        public static void TryPlaySfx(AudioClip clip)
        {
            if (shared != null)
            {
                shared.PlaySfx(clip);
            }
        }

        public AudioSource PlaySfx(AudioClip clip, float pitch = 1f, float volume = 1f)
        {
            if (clip == null)
            {
                return null;
            }

            AudioSource sfxPlayer = _sfxPlayerPool.Get();

            AudioSource source = sfxPlayer.GetComponent<AudioSource>();
            source.clip = clip;
            source.pitch = pitch;
            source.volume = volume;
            source.Play();
            StartCoroutine(StopSoundAfterTime(clip.length, source));
            return source;
        }

        private IEnumerator StopSoundAfterTime(float time, AudioSource source)
        {
            yield return new WaitForSeconds(time + 0.1f);

            source.Stop();
            source.gameObject.SetActive(false);
            _sfxPlayerPool.Release(source);
        }
    }
}