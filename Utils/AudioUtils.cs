using System;
using UnityEngine;

namespace Foxworks.Utils
{
    public static class AudioUtils
    {
        public static AudioClip CreateReversedAudioClip(this AudioClip originalClip)
        {
            // Get the samples from the original clip
            float[] samples = new float[originalClip.samples * originalClip.channels];
            originalClip.GetData(samples, 0);

            // Reverse the samples
            Array.Reverse(samples);

            // Create a new audio clip with the same properties as the original
            AudioClip reversedClip = AudioClip.Create(originalClip.name + "_Reversed", originalClip.samples, originalClip.channels, originalClip.frequency, false);
    
            // Set the reversed samples to the new audio clip
            reversedClip.SetData(samples, 0);
    
            return reversedClip;
        }
    }
}