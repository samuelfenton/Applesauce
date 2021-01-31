using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sound : MonoBehaviour
{
    private const string FOOTSTEP = "Footstep";

    private const string PISTOL_FIRE = "Pistol_Fire";
    private const string PISTOL_RELOAD = "Pistol_Reload";
    private const string PISTOL_DRY_FIRE = "Pistol_Empty";

    public void PlayGunshot()
    {
        AkSoundEngine.PostEvent(PISTOL_FIRE, gameObject);
    }

    public void PlayGunReload()
    {
        AkSoundEngine.PostEvent(PISTOL_RELOAD, gameObject);
    }

    public void PlayGunDryFire()
    {
        AkSoundEngine.PostEvent(PISTOL_DRY_FIRE, gameObject);
    }

    public void PlayFootstep()
    {
        AkSoundEngine.SetSwitch("Speed", "Run", gameObject);
        AkSoundEngine.SetSwitch("Terrain", "Stone", gameObject);

        AkSoundEngine.PostEvent(FOOTSTEP, gameObject);
    }
}
