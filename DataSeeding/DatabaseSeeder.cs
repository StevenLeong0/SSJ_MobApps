using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt.Net;
using SeniorLearnApi.Models;
using SeniorLearnApi.Enums;

namespace SeniorLearnApi.DataSeeding;

public class DatabaseSeeder
{
    private readonly IMongoDatabase _database;
    private readonly string _connectionString = "mongodb://localhost:27017";
    private readonly string _databaseName = "SeniorLearnBulletin";
    private List<string> _userIds;

    public DatabaseSeeder()
    {
        var client = new MongoClient(_connectionString);
        _database = client.GetDatabase(_databaseName);
    }

    public DatabaseSeeder(string connectionString, string databaseName)
    {
        _connectionString = connectionString;
        _databaseName = databaseName;
        var client = new MongoClient(_connectionString);
        _database = client.GetDatabase(_databaseName);
    }

    public async Task SeedDataAsync()
    {
        try
        {
            Console.WriteLine("Seeding SeniorLearnDb...");
            await SeedUsersAsync();
            await SeedUserSettingsAsync();
            await SeedMemberBulletinsAsync();
            await SeedOfficialBulletinsAsync();
            Console.WriteLine("Seeds Planted");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding database: {ex.Message}");
            throw;
        }
    }

    private string HashPassword(string plainPassword)
    {
        return BCrypt.Net.BCrypt.HashPassword(plainPassword, 10);
    }

    private List<string> GenerateUserIds()
    {
        var userIds = new List<string>();
        for (int i = 0; i < 8; i++)
        {
            userIds.Add(ObjectId.GenerateNewId().ToString());
        }
        return userIds;
    }

    private async Task SeedUsersAsync()
    {
        var userCollection = _database.GetCollection<User>("User");
        if (await userCollection.CountDocumentsAsync(FilterDefinition<User>.Empty) == 0)
        {
            _userIds = GenerateUserIds();
            var users = new List<User>
            {
                new User
                {
                    Id = _userIds[0],
                    FirstName = "Admin",
                    LastName = "One",
                    Email = "seniorlearnAdmin@slearn.org.au",
                    Username = "Admin",
                    PasswordHash = HashPassword("user123"),
                    MemberSince = new DateTime(2020, 5, 9, 15, 30, 45)
                },
                new User
                {
                    Id = _userIds[1],
                    FirstName = "AnonUser123",
                    LastName = "One",
                    Email = "anonUser123@slearn.org.au",
                    Username = "AnonUser123",
                    PasswordHash = HashPassword("user123"),
                    MemberSince = new DateTime(2022, 5, 9, 15, 30, 45)
                },
                new User
                {
                    Id = _userIds[2],
                    FirstName = "User101",
                    LastName = "One",
                    Email = "User101@slearn.org.au",
                    Username = "User101",
                    PasswordHash = HashPassword("user123"),
                    MemberSince = new DateTime(2023, 10, 4, 15, 30, 45)
                },
                new User
                {
                    Id = _userIds[3],
                    FirstName = "TheatreSenior",
                    LastName = "One",
                    Email = "TheatreSenior@slearn.org.au",
                    Username = "TheatreSenior",
                    PasswordHash = HashPassword("user123"),
                    MemberSince = new DateTime(2024, 8, 3, 15, 30, 45)
                },
                new User
                {
                    Id = _userIds[4],
                    FirstName = "Zhonghua",
                    LastName = "Chen",
                    Email = "chenZhonghua@slearn.org.au",
                    Username = "ChenZhonghua",
                    PasswordHash = HashPassword("user123"),
                    MemberSince = new DateTime(2022, 1, 20, 15, 30, 45)
                },
                new User
                {
                    Id = _userIds[5],
                    FirstName = "Peter",
                    LastName = "One",
                    Email = "program@slearn.org.au",
                    Username = "Peter",
                    PasswordHash = HashPassword("user123"),
                    MemberSince = new DateTime(2023, 5, 4, 15, 30, 45)
                },
                new User
                {
                    Id = _userIds[6],
                    FirstName = "Scammer",
                    LastName = "Senior",
                    Email = "seancemagic@slearn.org.au",
                    Username = "SeniorScammer",
                    PasswordHash = HashPassword("user123"),
                    MemberSince = new DateTime(2025, 5, 4, 15, 30, 45)
                },
                new User
                {
                    Id = _userIds[7],
                    FirstName = "SeniorLearnFan",
                    LastName = "Fan",
                    Email = "seniorLearnFanClub@slearn.org.au",
                    Username = "SeniorLearnFan",
                    PasswordHash = HashPassword("user123"),
                    MemberSince = new DateTime(2025, 5, 4, 15, 30, 45)
                }
            };
            await userCollection.InsertManyAsync(users);
        }
        else
        {
            // If users already exist, get their IDs for other collections
            var existingUsers = await userCollection.Find(FilterDefinition<User>.Empty).ToListAsync();
            _userIds = existingUsers.Take(8).Select(u => u.Id).ToList();
        }
    }

    private async Task SeedUserSettingsAsync()
    {
        var userSettingCollection = _database.GetCollection<UserSetting>("UserSetting");
        if (await userSettingCollection.CountDocumentsAsync(FilterDefinition<UserSetting>.Empty) == 0)
        {
            var userSettings = new List<UserSetting>
            {
                new UserSetting { Id = _userIds[0], TextSize = 36, DarkMode = false, EnableNotifications = false },
                new UserSetting { Id = _userIds[1], TextSize = 36, DarkMode = true, EnableNotifications = false },
                new UserSetting { Id = _userIds[2], TextSize = 32, DarkMode = true, EnableNotifications = false },
                new UserSetting { Id = _userIds[3], TextSize = 24, DarkMode = true, EnableNotifications = false },
                new UserSetting { Id = _userIds[4], TextSize = 36, DarkMode = true, EnableNotifications = false },
                new UserSetting { Id = _userIds[5], TextSize = 36, DarkMode = true, EnableNotifications = false },
                new UserSetting { Id = _userIds[6], TextSize = 36, DarkMode = true, EnableNotifications = false },
                new UserSetting { Id = _userIds[7], TextSize = 36, DarkMode = true, EnableNotifications = false }
            };
            await userSettingCollection.InsertManyAsync(userSettings);
        }
    }

    private async Task SeedMemberBulletinsAsync()
    {
        var memberBulletinCollection = _database.GetCollection<MemberBulletin>("MemberBulletin");
        if (await memberBulletinCollection.CountDocumentsAsync(FilterDefinition<MemberBulletin>.Empty) == 0)
        {
            var memberBulletins = new List<MemberBulletin>
            {
                new MemberBulletin
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "Ice-Cream Party",
                    Category = MemberBulletinCategory.Event,
                    Content = "Ice Cream Party to celebrate the 1st June for all SeniorLearn Members. Buy one get one free",
                    DateCreated = new DateTime(2025, 5, 9, 15, 30, 45),
                    DateUpdated = new DateTime(2025, 5, 9, 15, 30, 45),
                    AuthorId = _userIds[1],
                    AuthorUsername = "AnonUser123"
                },
                new MemberBulletin
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "Free Dental Seniors Only",
                    Category = MemberBulletinCategory.Event,
                    Content = "Come to Ha Loo Loo Dental for your free dental check-up. Offer lasts for the next 3 days!",
                    DateCreated = new DateTime(2025, 5, 4, 15, 30, 45),
                    DateUpdated = new DateTime(2025, 5, 4, 15, 30, 45),
                    AuthorId = _userIds[2],
                    AuthorUsername = "User101"
                },
                new MemberBulletin
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "Theatre Night: Matilda",
                    Category = MemberBulletinCategory.Event,
                    Content = "She felt fairly confident that with a great deal of practice and effort, she would succeed in the end.",
                    DateCreated = new DateTime(2025, 5, 3, 15, 30, 45),
                    DateUpdated = new DateTime(2025, 5, 3, 15, 30, 45),
                    AuthorId = _userIds[3],
                    AuthorUsername = "TheatreSenior"
                },
                new MemberBulletin
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "Martial Arts Lessons",
                    Category = MemberBulletinCategory.Interest,
                    Content = "Whenever you can separate Yin and Yang, one part doesn't move, the other part moves, there has to be a relationship between them.",
                    DateCreated = new DateTime(2025, 5, 9, 15, 30, 45),
                    DateUpdated = new DateTime(2025, 5, 9, 15, 30, 45),
                    AuthorId = _userIds[4],
                    AuthorUsername = "ChenZhonghua"
                },
                new MemberBulletin
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "Boogie Woogie",
                    Category = MemberBulletinCategory.Interest,
                    Content = "Just joking, I'm gonna teach programming instead",
                    DateCreated = new DateTime(2025, 5, 4, 15, 30, 45),
                    DateUpdated = new DateTime(2025, 5, 4, 15, 30, 45),
                    AuthorId = _userIds[5],
                    AuthorUsername = "Peter"
                },
                new MemberBulletin
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "Seance Magic Talk to your Wife Today!!",
                    Category = MemberBulletinCategory.Interest,
                    Content = "For the cheap price of $99, talk to the dead!!",
                    DateCreated = new DateTime(2025, 5, 3, 15, 30, 45),
                    DateUpdated = new DateTime(2025, 5, 3, 15, 30, 45),
                    AuthorId = _userIds[6],
                    AuthorUsername = "SeniorScammer"
                },
                new MemberBulletin
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "CelebratingSeniorLearnFan!!",
                    Category = MemberBulletinCategory.Update,
                    Content = "Woohoo",
                    DateCreated = new DateTime(2025, 5, 9, 15, 30, 45),
                    DateUpdated = new DateTime(2025, 5, 9, 15, 30, 45),
                    AuthorId = _userIds[7],
                    AuthorUsername = "SeniorLearnFan"
                },
                new MemberBulletin
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "CelebratingSeniorLearnFan!!",
                    Category = MemberBulletinCategory.Update,
                    Content = "Woohoo",
                    DateCreated = new DateTime(2025, 5, 4, 15, 30, 45),
                    DateUpdated = new DateTime(2025, 5, 4, 15, 30, 45),
                    AuthorId = _userIds[7],
                    AuthorUsername = "SeniorLearnFan"
                },
                new MemberBulletin
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "SeniorLearn App Out Now!",
                    Category = MemberBulletinCategory.Update,
                    Content = "Seniors need to learn technology now!! WOO HOO",
                    DateCreated = new DateTime(2025, 5, 3, 15, 30, 45),
                    DateUpdated = new DateTime(2025, 5, 3, 15, 30, 45),
                    AuthorId = _userIds[7],
                    AuthorUsername = "SeniorLearnFan"
                }
            };
            await memberBulletinCollection.InsertManyAsync(memberBulletins);
        }
    }

    private async Task SeedOfficialBulletinsAsync()
    {
        var officialBulletinCollection = _database.GetCollection<OfficialBulletin>("OfficialBulletin");
        if (await officialBulletinCollection.CountDocumentsAsync(FilterDefinition<OfficialBulletin>.Empty) == 0)
        {
            var officialBulletins = new List<OfficialBulletin>
            {
                new OfficialBulletin
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "Important Organisation Update",
                    Type = OfficialType.Update,
                    Content = "Dear SeniorLearn members,We are pleased to announce some important updates to our organisation that we believe will enhance your learning experience.Sincerely, The SeniorLearnTeam",
                    DateCreated = new DateTime(2025, 5, 9, 15, 30, 45),
                    DateUpdated = new DateTime(2025, 5, 9, 15, 30, 45),
                    AuthorId = _userIds[0],
                    AuthorName = "Admin"
                },
                new OfficialBulletin
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "Monthly Meeting Cancelled",
                    Type = OfficialType.News,
                    Content = "Someone got sick, its cancelled folks",
                    DateCreated = new DateTime(2025, 5, 9, 15, 30, 45),
                    DateUpdated = new DateTime(2025, 5, 9, 15, 30, 45),
                    AuthorId = _userIds[0],
                    AuthorName = "Admin"
                },
                new OfficialBulletin
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = "SeniorLearn App Out Now!",
                    Type = OfficialType.Announcement,
                    Content = "The SeniorLearn App is out now! It took a long time to make but its finally done. We are also gonna hold lessons on how to use it so stay tune!",
                    DateCreated = new DateTime(2025, 5, 9, 15, 30, 45),
                    DateUpdated = new DateTime(2025, 5, 9, 15, 30, 45),
                    AuthorId = _userIds[0],
                    AuthorName = "Admin"
                }
            };
            await officialBulletinCollection.InsertManyAsync(officialBulletins);
        }
    }

    public async Task DeleteAllDataAsync()
    {
        try
        {
            Console.WriteLine("Deleting Data in SeniorLearnDb...");
            var collections = new[] { "User", "MemberBulletin", "OfficialBulletin", "UserSetting" };
            foreach (var collectionName in collections)
            {
                await _database.DropCollectionAsync(collectionName);
            }
            Console.WriteLine("Data Deleted");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting data: {ex.Message}");
            throw;
        }
    }
}