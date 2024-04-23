public class Condition
{
    public virtual bool Test()
    {
        return true;
    }
}

public class FloatCondition : Condition
{
    private float minValue;
    private float testValue;
    private float maxValue;

    public float TestValue()
    {
        return testValue;
    }

    public override bool Test()
    {
        return minValue <= testValue && testValue <= maxValue;
    }
}

public class AndCondition : Condition
{
    Condition conditionA;
    Condition conditionB;

    public override bool Test()
    {
        return conditionA.Test() && conditionB.Test();
    }
}