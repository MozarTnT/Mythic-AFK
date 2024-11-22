using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Utils
{
    public static SpriteAtlas m_Atlas = Resources.Load<SpriteAtlas>("Atlas");

    public static Sprite Get_Atlas(string temp)
    {
        return m_Atlas.GetSprite(temp);
    }
    public static string String_Color_Rarity(Rarity rare)
    {
        switch(rare)
        {
            case Rarity.Uncommon: return "<color=#FFFFFF>";
            case Rarity.Common: return "<color=#00FF00>";
            case Rarity.Rare: return "<color=#00D8FF>";
            case Rarity.Hero: return "<color=#B750C3>";
            case Rarity.Legendary: return "<color=#FF9000>"; 
        }

        return "<color=#FFFFFF>";
    }
    
}
