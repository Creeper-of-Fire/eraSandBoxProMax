namespace eraSandBox.Coitus;

public class CoitusScaleLinear : IScale
{
    /// <summary> 基础数值，即只和外部设定值与高度相关的尺寸，除非高度改变，否则这个尺寸不会变 </summary>
    public int baseValueMillimeter;

    protected CoitusAspect parent;

    /// <summary> 尺寸等级，即可以任意改变的尺寸，受多种因素影响 </summary>
    public int scaleLevel;

    /// <param name="baseValueMillimeter">
    ///     <see cref="baseValueMillimeter" />
    /// </param>
    /// <param name="scaleLevel">
    ///     <see cref="scaleLevel" />
    /// </param>
    public CoitusScaleLinear(int baseValueMillimeter, int scaleLevel, CoitusAspect parent)
    {
        this.baseValueMillimeter = baseValueMillimeter;
        this.scaleLevel = scaleLevel;
        this.parent = parent;
    }

    /// <summary> <see cref="baseValueMillimeter" /> * <see cref="scaleLevel" /> </summary>
    private int ValueMillimeter =>
        this.baseValueMillimeter * this.scaleLevel;

    /// <summary> 存储原始的数据值 </summary>
    public int OriginalMillimeter()
    {
        return this.ValueMillimeter;
    }
}