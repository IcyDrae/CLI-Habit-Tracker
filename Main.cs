/*
This class is where everything comes together, the menu is built with the
necessary options and submenus, and the usage methods for the Habit class are defined here.
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

        Menu MarkAsDoneSubMenu;

        Menu EditHabitSubMenu;

        Menu DeleteHabitSubMenu;

        private void BuildMenu()
        {
            this.RootMenu = new Menu(new List<Option> {
                new Option("Add a new habit", this.AddHabit),
                new Option("Show my habits", this.ShowAllHabits),
                new Option("Mark as done", this.MarkAsDoneMenu),
                new Option("Edit a habit", this.EditHabitMenu),
                new Option("Delete a habit", this.DeleteHabitMenu),
                new Option("Exit", () => RootMenu.Exit()),
            });

            HabitsListMenu = new Menu(new List<Option>());
            RootMenu.AddSubMenu(HabitsListMenu);

            MarkAsDoneSubMenu = new Menu(new List<Option>());
            RootMenu.AddSubMenu(MarkAsDoneSubMenu);

            EditHabitSubMenu = new Menu(new List<Option>());
            RootMenu.AddSubMenu(EditHabitSubMenu);

            DeleteHabitSubMenu = new Menu(new List<Option>());
            RootMenu.AddSubMenu(DeleteHabitSubMenu);

            RootMenu.Run();
        }

        public void Run()
        {
            Json Json = new Json();
            Json.Create();

            //this.TestAddHabits();

            //Json.InsertHabits(this.HabitsList);

            this.BuildMenu();
        }

        private void TestAddHabits()
        {
            Habit Habit1 = new Habit("Training", DateTime.Now);
            Habit Habit2 = new Habit("Stretching", DateTime.Now);
            Habit Habit3 = new Habit("Coding", DateTime.Now);
            Habit Habit4 = new Habit("Reading", DateTime.Now);

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
            Json Json = new Json();
            List<Habit> HabitsFromFile = Json.GetAllHabits();

            Console.Write("Enter habit name: ");
            string Name = Convert.ToString(Console.ReadLine());

            if (!string.IsNullOrEmpty(Name))
            {
                if (HabitsFromFile.Any(Habit => Habit.GetName() == Name))
                {
                    Console.WriteLine("That habit already exists. Please create another one.");
                } else
                {
                    Habit Habit = new Habit(Name, DateTime.Now);
                    this.HabitsList.Add(Habit);
                    Json.AddNewHabit(Habit);

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
                    string FormattedName = $"{PositionIndex}) {Habit.GetModifiedName()}" +
                            $"\n\tCreated on: {Habit.GetCreatedOn()}" +
                            $"\n\tStreak: {Habit.GetStreak().Last().GetValue()} on {Habit.GetStreak().Last().GetMarkedOn()}" +
                            $"\n\tUpdated on: {Habit.GetUpdatedOn()}" +
                            $"\n\tLast streak: {Habit.GetLastStreak()}";

                    HabitsListMenu.Options.Add(
                        new Option(FormattedName, () => ShowHabitDetail(Habit))
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

        private void ShowHabitDetail(Habit Habit) {
            Console.Clear();

            string FormattedName = $"{Habit.GetModifiedName()}" +
                    $"\n\tCreated on: {Habit.GetCreatedOn()}" +
                    $"\n\tStreak: {Habit.GetStreak().Last().GetValue()} on {Habit.GetStreak().Last().GetMarkedOn()}" +
                    $"\n\tUpdated on: {Habit.GetUpdatedOn()}" +
                    $"\n\tLast streak: {Habit.GetLastStreak()}";
            Console.WriteLine(FormattedName);

            Console.WriteLine("This is your streak history for this habit.");            
            foreach (Streak Streak in Habit.GetStreak())
            {
                Console.WriteLine($"-----------------------------------------");
                Console.WriteLine($"#: {Streak.GetValue()}");
                Console.WriteLine($"Date: {Streak.GetMarkedOn()}");
                Console.WriteLine($"-----------------------------------------");
            }

            Console.ReadKey();
        }

        private void MarkAsDoneMenu()
        {
            Json Json = new Json();
            List<Habit> HabitsFromFile = Json.GetAllHabits();

            if (HabitsFromFile.Count > 0)
            {
                MarkAsDoneSubMenu.Options.Clear();
                int PositionIndex = 1;

                foreach (Habit Habit in HabitsFromFile)
                {
                    MarkAsDoneSubMenu.Options.Add(new Option(
                        $"{PositionIndex}) {Habit.GetModifiedName()}", () => MarkAsDone(Habit)));
                
                    PositionIndex++;
                }

                // Push the current menu to the stack and navigate to the HabitsListMenu
                RootMenu.Stack.Push(RootMenu.CurrentMenu);
                RootMenu.CurrentMenu = MarkAsDoneSubMenu;
                // Reset index for new menu
                MarkAsDoneSubMenu.Index = 0;
            }
            else
            {
                Console.WriteLine("No habits to edit.");
                Console.ReadKey();
            }
        }

        private void MarkAsDone(Habit Habit)
        {
            Json Json = new Json();
            int CurrentStreak = Habit.GetStreak().Last().GetValue();
            bool IsNextDay = DateTime.Now.Date == Habit.GetUpdatedOn().Date.AddDays(1);
            bool IsAfterNextDay = DateTime.Now > Habit.GetUpdatedOn().Date.AddDays(1);

            Console.WriteLine($"\nMarking as done: {Habit.GetName()}");

            Console.Write($"Current streak is: {CurrentStreak}");
            Console.Write("\nMark as done? ");
            string MarkAsDone = Console.ReadLine();
            if (!string.IsNullOrEmpty(MarkAsDone) && MarkAsDone.ToLower().Equals("y"))
            {
                if (CurrentStreak == 0 || IsNextDay)
                {
                    Console.WriteLine($"Marked as done: {Habit.GetName()}! Check back in again tomorrow!");
                    Habit.SetStreak(CurrentStreak + 1, DateTime.Now);
                    Habit.SetUpdatedOn(DateTime.Now);

                    Json.MarkAsDone(Habit);
                }
                else if (IsAfterNextDay)
                {
                    Console.WriteLine($"Marked as done: {Habit.GetName()}! " +
                    "Your streak has been reset. Check back in again tomorrow!");
                    Habit.SetLastStreak(CurrentStreak);
                    Habit.SetStreak(1, DateTime.Now);
                    Habit.SetUpdatedOn(DateTime.Now);

                    Json.MarkAsDone(Habit);
                }
                else if (!IsAfterNextDay)
                {
                    Console.WriteLine("You can only mark this habit as done on the next day!");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Habit not updated.");
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
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

                // Push the current menu to the stack and navigate to the HabitsListMenu
                RootMenu.Stack.Push(RootMenu.CurrentMenu);
                RootMenu.CurrentMenu = EditHabitSubMenu;
                // Reset index for new menu
                EditHabitSubMenu.Index = 0;
            }
            else
            {
                Console.WriteLine("No habits to edit.");
                Console.ReadKey();
            }
        }

        private void EditHabit(Habit Habit)
        {
            Json Json = new Json();

            Console.WriteLine($"\nEditing: {Habit.GetName()}");

            Console.Write("Enter new name: ");
            string OldName = Habit.GetName();

            var NewName = Console.ReadLine();
            if (!string.IsNullOrEmpty(NewName))
            {
                Habit.SetName(NewName);
                Console.WriteLine($"Habit updated to: {NewName}");
            }
            else
            {
                Console.WriteLine("Invalid name. Habit not updated.");
            }

            if (!string.IsNullOrEmpty(NewName))
            {
                Habit.SetUpdatedOn(DateTime.Now);
                Json.UpdateHabit(OldName, Habit);
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        private void DeleteHabitMenu()
        {
            Json Json = new Json();
            List<Habit> HabitsFromFile = Json.GetAllHabits();

            if (HabitsFromFile.Count > 0)
            {
                DeleteHabitSubMenu.Options.Clear();
                int PositionIndex = 1;

                foreach (Habit Habit in HabitsFromFile)
                {
                    Option Option = new Option(
                        $"{PositionIndex}) {Habit.GetModifiedName()}");
                    Option.Selected = () => DeleteHabit(Habit);

                    DeleteHabitSubMenu.Options.Add(Option);
                    
                    PositionIndex++;
                }

                // Push the current menu to the stack and navigate to the HabitsListMenu
                RootMenu.Stack.Push(RootMenu.CurrentMenu);
                RootMenu.CurrentMenu = DeleteHabitSubMenu;
                // Reset index for new menu
                DeleteHabitSubMenu.Index = 0;
            }
            else
            {
                Console.WriteLine("No habits to delete.");
                Console.ReadKey();
            }
        }

        private void DeleteHabit(Habit Habit) {
            Json Json = new Json();

            Console.WriteLine($"\nDeleting: {Habit.GetName()}");

            Console.Write("Do you really want to delete it? ");
            string DeleteInput = Console.ReadLine();

            if (!string.IsNullOrEmpty(DeleteInput) && DeleteInput.ToLower().Equals("y"))
            {
                Json.DeleteHabit(Habit);
                DeleteHabitSubMenu.Options.RemoveAll(option => {
                    return option.Name == Habit.GetName();
                });
                Console.WriteLine("The habit was deleted.");
            }
            else
            {
                Console.WriteLine("Invalid input. Habit not deleted.");
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();            
        }
    }
}
