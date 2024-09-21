using UnityEngine;

namespace Fox.Utils
{
    public static class AudioSourceUtils
    {
        public static AudioSource CreateChildAudioSource(this Transform transform, string name = "Player")
        {
            GameObject go = new(name);
            go.transform.SetParent(transform);
            return go.AddComponent<AudioSource>();
        }

        public static void GetPooledAudioSource(AudioSource audioSource)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.volume = 1f;
            audioSource.pitch = 1f;
            audioSource.gameObject.SetActive(true);
        }

        public static void ReturnPooledAudioSource(AudioSource audioSource)
        {
            audioSource.gameObject.SetActive(false);
        }
        
    }
}