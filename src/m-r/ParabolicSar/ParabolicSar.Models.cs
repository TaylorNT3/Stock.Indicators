namespace Skender.Stock.Indicators;

[Serializable]
public class ParabolicSarResult : ResultBase
{
    public decimal? Sar { get; set; }
    public bool? IsReversal { get; set; }
    
    public bool? IsRising { get; set; }
    public decimal? ExtremePoint { get; set; }
    public decimal? AccelerationFactor { get; set ;}
}
