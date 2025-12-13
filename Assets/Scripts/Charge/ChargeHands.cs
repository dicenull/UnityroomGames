
public enum ChargeHands
{
    Charge,
    Guard,
    Beam,
}

public static class ChargeHandsExtend
{
    public static ChargeHands ToChargeHands(this string str)
    {
        return str switch
        {
            "Charge" => ChargeHands.Charge,
            "Guard" => ChargeHands.Guard,
            "Beam" => ChargeHands.Beam,
            _ => ChargeHands.Charge,
        };
    }
}