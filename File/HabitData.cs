/*
This class is used as a data represantion for the JSON file.
*/
namespace HabitTracker.File
{
    public class HabitData
    {
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public int Streak { get; set; }
    }
}
