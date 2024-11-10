using System.IO;
using System.Text.Json;

namespace HabitTracker.File
{
    public class Json
    {
        private string Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\HabitTracker.json";

        public async void CreateFile(List<Habit> HabitsList)
        {
            JsonSerializerOptions Options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            await using FileStream CreateStream = System.IO.File.Create(Path);
            List<HabitData> HabitDataList = new List<HabitData>();

            foreach (var Habit in HabitsList)
            {
                HabitDataList.Add(new HabitData
                {
                    Name = Habit.Name
                });
            }

            await JsonSerializer.SerializeAsync(CreateStream, HabitDataList, Options);
        }
    }
}
