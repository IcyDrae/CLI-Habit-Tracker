/*
This class is used to create and maintain a JSON file, which the
app uses to maintain state.
*/
using System.Text.Json;

namespace HabitTracker.File
{
    public class Json
    {
        private string Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\HabitTracker.json";

        private JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public async void Create()
        {
            await using FileStream CreateStream = System.IO.File.Create(Path);
            List<HabitData> HabitDataList = new List<HabitData>();

            await JsonSerializer.SerializeAsync(CreateStream, HabitDataList, Options);
        }

        public async void InsertHabits(List<Habit> Habits)
        {
            if (System.IO.File.Exists(Path))
            {
                using FileStream LocalFile = System.IO.File.Open(Path, FileMode.Truncate, FileAccess.ReadWrite);
                List<HabitData>? HabitDataList = new List<HabitData>();

                foreach (Habit Habit in Habits)
                {
                    HabitDataList.Add(new HabitData
                    {
                        Name = Habit.GetName(),
                        CreatedOn = Habit.GetCreatedOn(),
                        UpdatedOn = Habit.GetUpdatedOn(),
                        Streak = Habit.GetStreak()
                    });
                }

                await JsonSerializer.SerializeAsync(LocalFile, HabitDataList, Options);
            }
        }

        public List<Habit> GetAllHabits()
        {
            List<Habit> Result = new List<Habit>();

            if (System.IO.File.Exists(Path))
            {
                using FileStream LocalFile = System.IO.File.Open(Path, FileMode.Open, FileAccess.Read);
                List<HabitData>? Contents = JsonSerializer.Deserialize<List<HabitData>>(LocalFile);

                foreach (HabitData HabitData in Contents)
                {
                    Habit Habit = new Habit(HabitData.Name, HabitData.CreatedOn, HabitData.Streak);
                    Habit.SetUpdatedOn(HabitData.UpdatedOn);
                    Result.Add(Habit);
                }

                return Result;
            }

            return new List<Habit>();
        }

        public Habit GetHabit(string Name)
        {
            if (System.IO.File.Exists(Path))
            {
                List<HabitData>? HabitDataList = new List<HabitData>();
                using FileStream LocalFile = System.IO.File.Open(Path, FileMode.Open, FileAccess.Read);
                List<HabitData>? Contents = JsonSerializer.Deserialize<List<HabitData>>(LocalFile);

                HabitData? Result = HabitDataList.Find((HabitData) =>
                {
                    return HabitData.Name.Equals(Name);
                });

                return new Habit(
                    Result.Name,
                    Result.CreatedOn,
                    Result.Streak
                );
            }

            return new Habit("", DateTime.Now);
        }

        public async void UpdateHabit(string OldHabit, Habit NewHabit)
        {
            if (System.IO.File.Exists(Path))
            {
                using FileStream ReadStream = System.IO.File.Open(Path, FileMode.Open, FileAccess.Read);
                List<HabitData>? Contents = await JsonSerializer.DeserializeAsync<List<HabitData>>(ReadStream);
                ReadStream.Close();

                HabitData? Result = Contents.Find((HabitData) =>
                {
                    return HabitData.Name == OldHabit;
                });

                Result.Name = NewHabit.GetName();
                Result.UpdatedOn = NewHabit.GetUpdatedOn();
                Result.Streak = NewHabit.GetStreak();

                using FileStream WriteStream = System.IO.File.Open(Path, FileMode.Create, FileAccess.Write);
                await JsonSerializer.SerializeAsync(WriteStream, Contents, Options);
            }
        }
    }
}
