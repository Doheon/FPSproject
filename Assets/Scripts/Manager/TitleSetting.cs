using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSetting : MonoBehaviour
{
    public static int playernum = -1;
    public static int difficulty = 2;
    public static float bulletSpeed{
        get{
            switch(difficulty){
                case 1:
                    return 30f;
                case 2:
                    return 40f;
                case 3:
                    return 50f;
                case 4:
                    return 65f;
                default:
                    return 50f;
            }
        }
    }

    public static float ballRemoveTime{
        get{
            switch(difficulty){
                case 1:
                    return 4f;
                case 2:
                    return 3f;
                case 3:
                    return 2f;
                case 4:
                    return 1.5f;
                default:
                    return 2f;
            }
        }
    }

    public static float enemyHealth{
        get{
            switch(difficulty){
                case 1:
                    return 0.6f;
                case 2:
                    return 0.8f;
                case 3:
                    return 1f;
                case 4:
                    return 1.5f;
                default:
                    return 1f;
            }
        }
    }
    public static float enemyDamage{
        get{
            switch(difficulty){
                case 1:
                    return 0.6f;
                case 2:
                    return 0.8f;
                case 3:
                    return 1f;
                case 4:
                    return 1.5f;
                default:
                    return 1f;
            }
        }
    }

    public static string diffText{
        get{
            switch(difficulty){
                case 1:
                    return "Easy";
                case 2:
                    return "Normal";
                case 3:
                    return "Hard";
                case 4:
                    return "Hell";
                default:
                    return "Normal";
            }
        }
    }


}
