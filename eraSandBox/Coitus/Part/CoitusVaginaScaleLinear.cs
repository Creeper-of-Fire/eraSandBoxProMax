namespace eraSandBox.Coitus.Part;

public class CoitusVaginaScaleLinear(int baseValueMillimeter, int scaleLevel, CoitusAspect parent)
    : CoitusScaleLinear(baseValueMillimeter, scaleLevel, parent), IVaginaScale
{
    private const float LevelToScale = 0.5f;


    private CoitusVaginaAspect Parent =>
        (CoitusVaginaAspect)this.parent;

    public MinusOneToOneRatio ExpansionOrContractionRatio { get; } = new();

    public int PerceptMillimeter() =>
        (int)(this.OriginalMillimeter() / LevelToScale / this.Parent.tighticityLevel);

    /// <summary> 用于“结点”类型的计算 </summary>
    public int ComfortMillimeter() =>
        this.OriginalMillimeter();

    public int UnComfortMillimeter() =>
        (int)(this.OriginalMillimeter() * this.Parent.elasticityLevel / LevelToScale);
}