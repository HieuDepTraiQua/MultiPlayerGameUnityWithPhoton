using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSilder;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Map1")){
            PlayerPrefs.SetFloat("Map1", 1);
            Load();
        } else {
            Load();
        }
        if (PlayerPrefs.HasKey("Map 2")){
            PlayerPrefs.SetFloat("Map 2", 1);
            Load();
        } else {
            Load();
        }
    }

    private void Load(){
        volumeSilder.value = PlayerPrefs.GetFloat("Map1");
        volumeSilder.value = PlayerPrefs.GetFloat("Map 2");
    }

    public void ChangeVolume(){
        AudioListener.volume = volumeSilder.value;
        Save();
    }

    public void Save(){
        PlayerPrefs.SetFloat("Map1", volumeSilder.value);
        PlayerPrefs.SetFloat("Map 2", volumeSilder.value);
    }


}
