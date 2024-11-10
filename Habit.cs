﻿/*
This is the data structure that represents a habit.
*/
namespace HabitTracker
{
    public class Habit
    {
        private int Id;

        public string Name;

        public Habit(string Name)
        {
            this.Name = Name;
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

        public void EditName(String Name)
        {
            if (!String.IsNullOrWhiteSpace(this.Name))
            {
                this.Name = Name;
            }
        }
    }
}