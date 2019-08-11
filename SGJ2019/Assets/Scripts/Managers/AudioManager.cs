using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public class AudioManager : SimpleSingleton<AudioManager>
	{
		[SerializeField] private AudioSource soundsAudioSource = null;
		[SerializeField] private List<string> audioClipNames = new List<string>();
		[SerializeField] private List<AudioClip> audioClipReferences = new List<AudioClip>();
		private Dictionary<string, AudioClip> audioClipsDictionary = new Dictionary<string, AudioClip>();
		

		private void Awake()
		{
			Assert.IsTrue(audioClipNames.Count == audioClipReferences.Count);
			for (int i = 0; i < audioClipNames.Count; i++)
			{
				Assert.IsNotNull(audioClipNames[i]);
				Assert.IsNotNull(audioClipReferences[i]);
				Assert.IsFalse(audioClipsDictionary.ContainsKey(audioClipNames[i]));
				audioClipsDictionary.Add(audioClipNames[i], audioClipReferences[i]);
			}
			Assert.IsNotNull(soundsAudioSource);
		}

		public void PlaySound(string soundId)
		{
			if (soundId.Length > 0)
			{
				Assert.IsTrue(audioClipsDictionary.ContainsKey(soundId));
				soundsAudioSource.PlayOneShot(audioClipsDictionary[soundId]);
			}
		}
	}
}