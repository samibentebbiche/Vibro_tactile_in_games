using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sound : MonoBehaviour
{
    // TODO parameter les distances 
    // Ajouter une courbe de fréquence 
    // ajouter l'audio onEnter et onExit


    AudioSource soundsource;

    [SerializeField, Range(0, 1)] private float amplitudebase = 1.0f;
    [SerializeField] private float frequencybase = 261.62f;
    private float amplitude;
    private float frequency;
    private float frequency_touched;
    private float frequency_distance;
    private double _phase;
    private int _sampleRate;

    Distance dis;
    DistanceSuivi disS;
    [SerializeField]
    private bool inverse = false;

    AudioSource audio;

    private float freq_min = 100;
    private float freq_max = 400;

    private void Awake()
    {
        _sampleRate = AudioSettings.outputSampleRate;
    }

    private void Start()
    {
        frequency_touched = 0;
        frequency_distance = 0;
        frequency = 0;
        soundsource = GetComponent<AudioSource>();
        dis = GetComponent<Distance>();
        if(dis == null)
        { 
            if(disS != null)
            {
                disS = GetComponent<DistanceSuivi>();
                freq_min = disS.frequence_min;
                freq_max = disS.frequence_max;
            }

        }
        else
        {
            freq_min = dis.frequence_min;
            freq_max = dis.frequence_max;
        }
        // soundsource.Play(0);
        //soundsource.Play();
        audio = gameObject.AddComponent<AudioSource>();
    }

    private void update()
    {

    }
    private void OnAudioFilterRead(float[] data, int channels)
    {

        //Debug.Log((frequency_touched + frequency_distance));
        double phaseIncrement = (frequency_touched + frequency_distance) / _sampleRate;



        for (int sample = 0; sample < data.Length; sample += channels)
        {
            float value = Mathf.Sin((float)_phase * 2 * Mathf.PI) * amplitude;
            _phase = (_phase + phaseIncrement) % 1;
            for (int channel = 0; channel < channels; channel++)
            {
                data[sample + channel] = value;
            }
        }
    }

    public void afficher()
    {
        Debug.Log("The 2 vibration : " + (frequency_touched + frequency_distance));
    }
    public void playSound()
    {
        soundsource.Play(0);
    }

    public void setFrequency(float fr, string className)
    {

        if (string.Equals(className, "touched") || string.Equals(className, "DominosEffect")) frequency_touched = fr;
        else if (string.Equals(className, "Distance") || string.Equals(className, "rotation")) frequency_distance = fr;
        


    }

    public void setToFrequenceBase(string className)
    {
        if (string.Equals(className, "touched") || string.Equals(className, "DominosEffect")) frequency_touched = frequencybase;
        else if (string.Equals(className, "Distance")) frequency_distance = frequencybase;
    }


    public void UpFrerquency(double distance,float scale, string className)
    {
        if (!inverse)
            frequency = (float)(((distance - scale) / (0 - scale)) * (freq_max - freq_min)) + freq_min;
        else
            frequency = (float)(((distance - 0.8) / (6 - 0.8)) * (freq_min - freq_max)) + freq_max;

        if (frequency > freq_max) frequency = freq_max;
        else if (frequency < freq_min) frequency = freq_min;


        if (string.Equals(className, "touched") || string.Equals(className, "DominosEffect")) frequency_touched = frequency;
        else if (string.Equals(className, "Distance"))
        {
            frequency_distance = frequency;
        }

    }

    public void UpIntensity(float distance, string className)
    {
        //amplitude = amplitudebase / distance;
        if (!inverse)
            amplitude = (float)((((distance - 0.8) / (6 - 0.8)) * (0.2 - 0.8)) + 0.8);
        else
            amplitude = (float)((((distance - 0.8) / (6 - 0.8)) * (0.8 - 0.2)) + 0.2);
    }


    public void setIntensity(float intensity, string className)
    {
        amplitude = intensity;
    }

}

