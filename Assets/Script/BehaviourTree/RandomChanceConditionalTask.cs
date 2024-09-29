using UnityEngine;

public class RandomChanceConditionalTask : BTNode
{
    int numberOfDice;
    int numberOfSides;
    int numberToBeat;

    public RandomChanceConditionalTask(int _numberOfDice, int _numberOfSides, int _numberToBeat)
    {
        numberOfDice = _numberOfDice;
        numberOfSides = _numberOfSides;
        numberToBeat = _numberToBeat;
    }
    public override BTNodeStates Evaluate()
    {
        int total = 0;
        for (int i = 0; i < numberOfDice; i++)
        {
            total += Random.Range(1, (numberOfSides + 1));
        }
        if ( total > numberToBeat)
        {
            return BTNodeStates.SUCCESS;
        }
        return BTNodeStates.FAILURE;
    }
}