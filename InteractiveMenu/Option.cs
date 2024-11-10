/*
This is a menu option. It contains an action, which runs when the option
is selected.
*/
namespace HabitTracker.InteractiveMenu
{
    public class Option
    {
        public string Name { get; }

        public Action? Selected {
            set;
            get;
        }

        public Option(string Name, Action? Selected = null)
        {
            this.Name = Name;
            this.Selected = Selected;
        }
    }
}
