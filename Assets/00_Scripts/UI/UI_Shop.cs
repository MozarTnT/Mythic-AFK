using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Shop : UI_Base
{
    public override void DisableOBJ()
    {
        Main_UI.instance.LayerCheck(-1);
        Main_UI.instance.FadeInOut(true, false, null);
        base.DisableOBJ();       

    }
    
}
