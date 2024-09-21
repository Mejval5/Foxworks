using UnityEngine;

namespace Foxworks.Utils
{
    public static class AudioSourceUtils
    {
        /// <summary>
        ///     Creates a child GameObject with an AudioSource component.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static AudioSource CreateChildAudioSource(this Transform transform, string name = "Player")
        {
            GameObject go = new(name);
            go.transform.SetParent(transform);
            return go.AddComponent<AudioSource>();
        }

        /// <summary>
        ///     Gets an AudioSource from a pool.
        /// </summary>
        /// <param name="audioSource"></param>
        public static void GetPooledAudioSource(AudioSource audioSource)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.volume = 1f;
            audioSource.pitch = 1f;
            audioSource.gameObject.SetActive(true);
        }

        /// <summary>
        ///     Returns an AudioSource to a pool.
        /// </summary>
        /// <param name="audioSource"></param>
        public static void ReturnPooledAudioSource(AudioSource audioSource)
        {
            audioSource.gameObject.SetActive(false);
        }
    }
}