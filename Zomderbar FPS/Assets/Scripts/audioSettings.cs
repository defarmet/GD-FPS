using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class audioSettings : MonoBehaviour
{
	[SerializeField] AudioMixer  mixer;
	
	Slider      slider;
	AudioSource testSound;

	void Start()
	{
		if (gameManager.instance.currentMenuOpen)
			gameManager.instance.currentMenuOpen.SetActive(false);

		slider = GetComponent<Slider>();
		testSound = GetComponent<AudioSource>();

		mixer.GetFloat(name, out float vol);
		slider.value = Mathf.Pow(10, vol / 20);
	}

	public void volume_change()
	{
		if (slider.value != 0)
			mixer.SetFloat(name, Mathf.Log10(slider.value) * 20);
		else
			mixer.SetFloat(name, -80);
	}

	public void play_sound()
	{
		testSound.Play();
	}
}
