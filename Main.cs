/*
This class is where everything comes together, the menu is built with the
necessary options and submenus...continue
*/
using HabitTracker.InteractiveMenu;
using HabitTracker.File;

namespace HabitTracker
{
    public class Main
    {
        List<Habit> HabitsList = new List<Habit>();

        Menu RootMenu;

        Menu HabitsListMenu;

        Menu EditHabitSubMenu;

        private void BuildMenu()
        {
            this.RootMenu = new Menu(new List<Option> {
                new Option("Add a new habit", this.AddHabit),
                new Option("Show my habits", this.ShowAllHabits),
                new Option("Edit a habit", this.EditHabitMenu),
                new Option("Exit", () => RootMenu.Exit()),
            });

            HabitsListMenu = new Menu(new List<Option>());
            RootMenu.AddSubMenu(HabitsListMenu);

            EditHabitSubMenu = new Menu(new List<Option>());
            RootMenu.AddSubMenu(EditHabitSubMenu);

            RootMenu.Run();
        }

        public void Run()
        {
            Json Json = new Json();
            Json.Create();

            this.TestAddHabits();

            Json.InsertHabits(this.HabitsList);

            this.BuildMenu();
        }

        private async void TestAddHabits()
        {
            Habit Habit1 = new Habit("Train every day", DateTime.Now);
            Habit Habit2 = new Habit("Stretch every day", DateTime.Now);
            Habit Habit3 = new Habit("Code every day", DateTime.Now);
            Habit Habit4 = new Habit("Read a book", DateTime.Now);

            this.HabitsList.Add(Habit1);
            this.HabitsList.Add(Habit2);
            this.HabitsList.Add(Habit3);
            this.HabitsList.Add(Habit4);
        }

        private void TestShowAllHabits()
        {
            this.ShowAllHabits();
        }

        private void AddHabit()
        {
            Console.Write("Enter habit name: ");
            String Name = Console.ReadLine();

            if (!String.IsNullOrEmpty(Name))
            {
                if (this.HabitsList.Any(Habit => Habit.GetName() == Name))
                {
                    Console.WriteLine("That habit already exists. Please create another one.");
                } else
                {
                    Habit Habit = new Habit(Name, DateTime.Now);
                    this.HabitsList.Add(Habit);

                    Console.WriteLine($"Added: {Habit.GetModifiedName()}");
                }
            } else
            {
                Console.Write("Invalid input. Habit name cannot be empty.");
            }

            Console.ReadKey();
        }

        private void ShowAllHabits()
        {
            Json Json = new Json();
            List<Habit> HabitsFromFile = Json.GetAllHabits();

            if (HabitsFromFile.Count > 0)
            {
                HabitsListMenu.Options.Clear();
                int PositionIndex = 1;

                foreach (Habit Habit in HabitsFromFile)
                {
                    HabitsListMenu.Options.Add(
                        new Option(
                            $"{PositionIndex}) {Habit.GetModifiedName()}" +
                            $"\n\tCreated on: {Habit.GetCreatedOn()}" +
                            $"\n\tStreak: {Habit.GetStreak()}" +
                            $"\n\tUpdated on: {Habit.GetUpdatedOn()}")
                    );

                    PositionIndex++;
                }

                // Push the current menu to the stack and navigate to the HabitsListMenu
                RootMenu.Stack.Push(RootMenu.CurrentMenu);
                RootMenu.CurrentMenu = HabitsListMenu;
                // Reset index for new menu
                HabitsListMenu.Index = 0;
            }
            else
            {
                Console.WriteLine("There are no habits in your list. " +
                    "Create some!");

                Console.ReadKey();
            }
        }

        private void EditHabitMenu()
        {
            Json Json = new Json();
            List<Habit> HabitsFromFile = Json.GetAllHabits();

            if (HabitsFromFile.Count > 0)
            {
                EditHabitSubMenu.Options.Clear();
                int PositionIndex = 1;

                foreach (Habit Habit in HabitsFromFile)
                {
                    EditHabitSubMenu.Options.Add(new Option(
                        $"{PositionIndex}) {Habit.GetModifiedName()}", () => EditHabit(Habit)));
                
                    PositionIndex++;
                }
            }
            else
            {
                Console.WriteLine("No habits to edit.");
                Console.ReadKey();
            }

            // Push the current menu to the stack and navigate to the HabitsListMenu
            RootMenu.Stack.Push(RootMenu.CurrentMenu);
            RootMenu.CurrentMenu = EditHabitSubMenu;
            // Reset index for new menu
            EditHabitSubMenu.Index = 0;
        }

        private void EditHabit(Habit Habit)
        {
            Console.WriteLine($"Editing: {Habit.GetName()}");
            Console.Write("Enter new name: ");

            string OldHabit = Habit.GetName();
            Json Json = new Json();

            var NewName = Console.ReadLine();
            if (!string.IsNullOrEmpty(NewName))
            {
                Habit.SetName(NewName);
                Habit.SetUpdatedOn(DateTime.Now);
                Console.WriteLine($"Habit updated to: {NewName}");
            }
            else
            {
                Console.WriteLine("Invalid name. Habit not updated.");
            }

            Console.Write("Mark as done? ");
            string MarkAsDone = Console.ReadLine();
            if (!string.IsNullOrEmpty(MarkAsDone) && MarkAsDone.Equals("Y"))
            {
                bool HasDayPassed = DateTime.Now >= Habit.GetUpdatedOn().AddHours(24);

                if (Habit.GetStreak() == 0 || HasDayPassed)
                {
                    Habit.SetStreak(Habit.GetStreak() + 1);
                    Habit.SetUpdatedOn(DateTime.Now);
                    Console.WriteLine($"Marked as done: {Habit.GetName()}. Check back in again tomorrow!");
                }
                else if (!HasDayPassed)
                {
                    Console.WriteLine("You cannot mark a habit as done before 24 hours have passed. Please " +
                        "try again tomorrow.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Habit not updated.");
            }

            if (!string.IsNullOrEmpty(NewName) ||
                (!string.IsNullOrEmpty(MarkAsDone) && MarkAsDone.Equals("Y")))
            {
                Json.UpdateHabit(OldHabit, Habit);
            }

            Console.WriteLine("Press ENTER to continue.");
            Console.ReadKey();
        }
    }
}
