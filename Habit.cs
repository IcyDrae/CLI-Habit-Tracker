/*
This is the data structure that represents a habit.
*/
using System.Text.Json.Serialization;

namespace HabitTracker
{
    public class Streak
    {
        [JsonInclude]
        private int Value;

        [JsonInclude]
        private DateTime MarkedOn;

        public Streak(int Value, DateTime MarkedOn) {
            this.Value = Value;
            this.MarkedOn = MarkedOn;
        }

        public void SetValue(int Value) {
            this.Value = Value;
        }

        public int GetValue() {
            return this.Value;
        }

        public void SetMarkedOn(DateTime MarkedOn) {
            this.MarkedOn = MarkedOn;
        }

        public DateTime GetMarkedOn() {
            return this.MarkedOn;
        }
    }

    public class Habit
    {
        private DateTime CreatedOn;

        private DateTime UpdatedOn;

        public string Name;

        public List<Streak> Streak;

        public int LastStreak;

        public Habit(string Name,
                    DateTime CreatedOn,
                    int Streak = 0,
                    int LastStreak = 0)
        {
            this.Name = Name;
            this.CreatedOn = CreatedOn;
            this.Streak = new List<Streak>() {
                new Streak(Streak, DateTime.Now)
            };
            this.LastStreak = LastStreak;
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

        public void SetStreak(int Value, DateTime MarkedOn)
        {
            this.Streak.Add(
                new Streak(Value, MarkedOn)
            );
        }

        public List<Streak> GetStreak()
        {
            return this.Streak;
        }

        public void SetLastStreak(int LastStreak)
        {
            this.LastStreak = LastStreak;
        }

        public int GetLastStreak()
        {
            return this.LastStreak;
        }
    }
}
