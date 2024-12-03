namespace HabitTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SetOutputEncodingForEmojis();

            Main Main = new Main();
            Main.Run();
        }

        static void SetOutputEncodingForEmojis()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
    }
}
