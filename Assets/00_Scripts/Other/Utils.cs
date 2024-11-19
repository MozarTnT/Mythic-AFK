using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static string String_Color_Rarity(Rarity rare)
    {
        switch(rare)
        {
            case Rarity.Common: return "<color=#FFFFFF>";
            case Rarity.Rare: return "<color=#00FF00>";
            case Rarity.Hero: return "<color=#00D8FF>";
            case Rarity.Legendary: return "<color=#B750C3>"; 
        }

        return "<color=#FFFFFF>";
    }
    
}
