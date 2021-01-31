using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sound : MonoBehaviour
{
    private const string PISTOL_FIRE = "Pistol_Fire";

    public void PlayGunshot()
    {
        AkSoundEngine.PostEvent(PISTOL_FIRE, gameObject);
    }
}
