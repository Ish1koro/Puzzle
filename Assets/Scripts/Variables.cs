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
    public static int _thirty { get; } = 30;

    public static int _drop_Move_Time { get; } = 10;
    #endregion

    #region InputSystemのActionsの名前
    /// <summary>
    /// 入力の位置
    /// </summary>
    public static string _position { get; } = "Position";
    public static string _select { get; } = "Select";
    public static string _drop { get; } = "Drop";
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

    public static string _drop_Name { get; } = "Drop_";

    public static string[] _drop_Tag { get; } = new string[6] { "Fire", "Water", "Wood", "Light", "Dark", "Heal" };
}
