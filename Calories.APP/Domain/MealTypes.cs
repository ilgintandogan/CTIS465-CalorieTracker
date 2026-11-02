namespace Calories.APP.Domain
{
    // Enums are used for predefined values and help not to remember or
    // check each element's value when being used.
    // Getting the value of an enum element:
    // int lunchValue = (int)MealTypes.Lunch;
    // Getting the text of an enum element:
    // string dinnerText = MealTypes.Dinner.ToString();
    public enum MealTypes
    {
        Breakfast, // if no assignment, will start from 0
        Lunch, // will automatically get the next value 1 and other elements
               // will continue to get the next values after 1
        Dinner,
        Snack
    }
}
