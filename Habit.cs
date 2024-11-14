/*
This is the data structure that represents a habit.
*/
namespace HabitTracker
{
    public class Habit
    {
        private DateTime CreatedOn;

        private DateTime UpdatedOn;

        public string Name;

        public int Streak;

        public Habit(string Name, DateTime CreatedOn, int Streak = 0)
        {
            this.Name = Name;
            this.CreatedOn = CreatedOn;
            this.Streak = Streak;

            this.UpdatedOn = DateTime.Now;
        }

        public void SetName(string Name)
        {
            this.Name = Name;
        }

        public string GetName()
        {
            return this.Name;
        }

        public String GetModifiedName()
        {
            return $"\ud83c\udf1f {this.Name}";
        }

        public DateTime GetCreatedOn()
        {
            return this.CreatedOn;
        }

        public void SetUpdatedOn(DateTime UpdatedOn)
        {
            this.UpdatedOn = UpdatedOn;
        }

        public DateTime GetUpdatedOn()
        {
            return this.UpdatedOn;
        }

        public void SetStreak(int Streak)
        {
            this.Streak = Streak;
        }

        public int GetStreak()
        {
            return this.Streak;
        }

        
    }
}
