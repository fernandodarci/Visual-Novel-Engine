using UnityEngine;

[CreateAssetMenu(menuName = "Story/Float Operator")]
public class GSC_FloatObjectOperator : ScriptableObject
{
    [SerializeField] private GSC_FloatOperator Operator;
    public void Operate() => Operator.Operate();
}
