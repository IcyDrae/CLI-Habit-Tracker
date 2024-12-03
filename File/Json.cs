/*
This class is used to create and maintain a JSON file, which the
app uses to maintain state.
*/
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HabitTracker.File
{
    public class Json
    {
        private readonly string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HabitTracker.json");

        private readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public async void Create()
        {
            if (!System.IO.File.Exists(Path))
            {
                await using FileStream CreateStream = System.IO.File.Create(Path);
                List<HabitData> HabitDataList = new List<HabitData>();

                await JsonSerializer.SerializeAsync(CreateStream, HabitDataList, Options);
            }
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
                        Streak = Habit.GetStreak(),
                        LastStreak = Habit.GetLastStreak()
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
                    List<Streak> Streaks = HabitData.Streak;

                    Habit Habit = new Habit(HabitData.Name,
                                            HabitData.CreatedOn,
                                            Streaks.Last().GetValue(),
                                            HabitData.LastStreak);

                    Habit.Streak = Streaks;
                    Habit.SetUpdatedOn(HabitData.UpdatedOn);
                    Result.Add(Habit);
                }

                return Result;
            }

            return new List<Habit>();
        }

        public async void AddNewHabit(Habit Habit) {
            if (System.IO.File.Exists(Path))
            {
                using FileStream ReadStream = System.IO.File.Open(Path, FileMode.Open, FileAccess.Read);
                List<HabitData>? Contents = await JsonSerializer.DeserializeAsync<List<HabitData>>(ReadStream);
                ReadStream.Close();

                Contents.Add(new HabitData
                {
                    Name = Habit.GetName(),
                    CreatedOn = Habit.GetCreatedOn(),
                    UpdatedOn = Habit.GetUpdatedOn(),
                    Streak = Habit.GetStreak(),
                    LastStreak = Habit.GetLastStreak()
                });

                using FileStream WriteStream = System.IO.File.Open(Path, FileMode.Create, FileAccess.Write);
                await JsonSerializer.SerializeAsync(WriteStream, Contents, Options);
            }
        }

        public async void MarkAsDone(Habit Habit)
        {
            if (System.IO.File.Exists(Path))
            {
                using FileStream ReadStream = System.IO.File.Open(Path, FileMode.Open, FileAccess.Read);
                List<HabitData>? Contents = await JsonSerializer.DeserializeAsync<List<HabitData>>(ReadStream);
                ReadStream.Close();

                HabitData? Result = Contents.Find((HabitData) =>
                {
                    return HabitData.Name == Habit.GetName();
                });

                Result.Streak.Add(Habit.GetStreak().Last());
                Result.LastStreak = Habit.GetLastStreak();
                Result.UpdatedOn = Habit.GetUpdatedOn();

                using FileStream WriteStream = System.IO.File.Open(Path, FileMode.Create, FileAccess.Write);
                await JsonSerializer.SerializeAsync(WriteStream, Contents, Options);
            }
        }

        public async void UpdateHabit(string OldName, Habit NewHabit)
        {
            if (System.IO.File.Exists(Path))
            {
                using FileStream ReadStream = System.IO.File.Open(Path, FileMode.Open, FileAccess.Read);
                List<HabitData>? Contents = await JsonSerializer.DeserializeAsync<List<HabitData>>(ReadStream);
                ReadStream.Close();

                HabitData? Result = Contents.Find((HabitData) =>
                {
                    return HabitData.Name == OldName;
                });

                Result.Name = NewHabit.GetName();
                Result.UpdatedOn = NewHabit.GetUpdatedOn();

                using FileStream WriteStream = System.IO.File.Open(Path, FileMode.Create, FileAccess.Write);
                await JsonSerializer.SerializeAsync(WriteStream, Contents, Options);
            }
        }

        public async void DeleteHabit(Habit Habit)
        {
            if (System.IO.File.Exists(Path))
            {
                using FileStream ReadStream = System.IO.File.Open(Path, FileMode.Open, FileAccess.Read);
                List<HabitData>? Contents = await JsonSerializer.DeserializeAsync<List<HabitData>>(ReadStream);
                ReadStream.Close();

                HabitData? Result = Contents.Find((HabitData) =>
                {
                    return HabitData.Name == Habit.GetName();
                });

                Contents.Remove(Result);

                using FileStream WriteStream = System.IO.File.Open(Path, FileMode.Create, FileAccess.Write);
                await JsonSerializer.SerializeAsync(WriteStream, Contents, Options);
            }
        }
    }
}
