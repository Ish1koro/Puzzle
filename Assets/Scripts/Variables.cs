using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 読み取り専用の変数を定義するクラス
// マジックナンバー防止用
public static class Variables
{
    #region int 
    public static int _zero { get; } = 0;
    public static int _one { get; } = 1;
    public static int _two { get; } = 2;
    public static int _three { get; } = 3;
    public static int _four { get; } = 4;
    public static int _five { get; } = 5;
    public static int _six { get; } = 6;

    public static int _drop_Move_Time { get; } = 4;
    #endregion

    #region InputSystemのActionsの名前
    /// <summary>
    /// 入力の位置
    /// </summary>
    public static string _position { get; } = "Position";
    public static string _select { get; } = "Select";
    #endregion

    #region 配列関係
    public static int _cant_Move_Area { get; } = 1 << 1 | 1 << 2;
    public static int _air { get; } = 0;
    public static int _wall { get; } = 1;
    public static int _old_Mino { get; } = 2;
    public static int _now_Mino { get; } = 3;
    public static int _mino_Center { get; } = 4;
    public static int _can_Fall_Position { get; } = 5;
    #endregion

    /// <summary>
    /// ドロップの種類
    /// </summary>
    public enum _drop_Type
    {
        Fire,
        Water,
        Wood,
        Light,
        Dark,
        Heal,
        Null
    }

    public static string[] _drop_Tag { get; } = new string[6] { "Fire", "Water", "Wood", "Light", "Dark", "Heal" };
}
