/*
This is the interactive menu, it can contains submenus. It gets redrawn on
each movement.
*/
namespace HabitTracker.InteractiveMenu
{
    class Menu
    {
        public int Index;

        public List<Option> Options;

        public Menu CurrentMenu;

        public Stack<Menu> Stack = new Stack<Menu>();

        public List<Menu> SubMenus { get; set; } = new List<Menu>();

        public Menu(List<Option> Options)
        {
            this.Index = 0;
            this.Options = Options;
            this.CurrentMenu = this;
        }

        public void Run()
        {
            while (true)
            {
                WriteMenu(CurrentMenu.Options,
                          CurrentMenu.Options[CurrentMenu.Index]);
                ConsoleKey Key = Console.ReadKey(true).Key;

                if (Key == ConsoleKey.UpArrow)
                {
                    if (CurrentMenu.Index - 1 >= 0)
                    {
                        CurrentMenu.Index--;
                        WriteMenu(CurrentMenu.Options,
                                  CurrentMenu.Options[CurrentMenu.Index]);
                    }
                }
                else if (Key == ConsoleKey.DownArrow)
                {
                    if (CurrentMenu.Index + 1 < CurrentMenu.Options.Count)
                    {
                        CurrentMenu.Index++;
                        WriteMenu(CurrentMenu.Options,
                                  CurrentMenu.Options[CurrentMenu.Index]);
                    }
                }
                else if (Key == ConsoleKey.Enter)
                {
                    var SelectedOption = CurrentMenu.Options[CurrentMenu.Index];
                    SelectedOption.Selected?.Invoke();
                }
                else if (Key == ConsoleKey.Backspace)
                {
                    if (Stack.Count > 0)
                    {
                        CurrentMenu = Stack.Pop();
                        CurrentMenu.Index = 0;
                    }
                }
                else if (Key == ConsoleKey.Escape)
                {
                    Exit();
                }
            }
        }

        public void WriteMenu(List<Option> Options, Option selectedOption)
        {
            Console.Clear();

            foreach (Option Option in Options)
            {
                if (Option == selectedOption)
                {
                    Console.Write("> ");
                }
                else
                {
                    Console.Write(" ");
                }

                Console.WriteLine(Option.Name);
            }

            Console.WriteLine("\nPress Backspace to go back, Escape to exit.");
        }

        public void Exit()
        {
            Console.WriteLine("Habit Tracker exited");
            Environment.Exit(0);
        }

        public void AddSubMenu(Menu SubMenu)
        {
            this.SubMenus.Add(SubMenu);
        }
    }
}
