const bcrypt = require ('bcrypt');


db = connect ("mongodb://localhost:9887/SLearnMobApp_db")
//ObjectId() for MongoDBShell, JS, and Node.js
//GenerateNewId() used in C# with MongoDB.Driver
//GenerateNewId().ToString()
async function HashPassword(plainPassword){
    const saltRounds = 10;
    const hashPassword = await bcrypt.hash(plainPassword, saltRounds);
    return hashPassword;
}
//PasswordHash: HashPassword("User123");
/*
https://www.reddit.com/r/node/comments/17m8b4p/best_node_hashing_algorithm_option/
*/

const userIds= [];
for (let i = 0;i<8;i++){
    userIds.push(new ObjectId());
}  
if (db.User.countDocuments({})===0){

    db.User.insertMany([
        {
            Id: userIds[0],
            FirstName: "Admin",
            LastName:"One",
            Email: "seniorlearnAdmin@slearn.org.au",
            Username:"Admin",
            PasswordHash:HashPassword("user123"),
            MemberSince: new Date(2020, 4, 9, 15,30,45)
        },
        {
            Id: userIds[1],
            FirstName: "AnonUser123",
            LastName:"One",
            Email: "anonUser123@slearn.org.au",
            Username:"AnonUser123",
            PasswordHash:HashPassword("user123"),
            MemberSince: new Date(2022, 4, 9, 15,30,45)
        },
        {
            Id: userIds[2],
            FirstName: "User101",
            LastName:"One",
            Email: "User101@slearn.org.au",
            Username:"User101",
            PasswordHash:HashPassword("user123"),
            MemberSince: new Date(2023, 9, 4, 15,30,45)
        },
        {
            Id: userIds[3],
            FirstName: "TheatreSenior",
            LastName:"One",
            Email: "TheatreSenior@slearn.org.au",
            Username:"TheatreSenior",
            PasswordHash:HashPassword("user123"),
            MemberSince: new Date(2024, 7, 3, 15,30,45)
        },
        {
            Id: userIds[4],
            FirstName: "Zhonghua",
            LastName:"Chen",
            Email: "chenZhonghua@slearn.org.au",
            Username:"ChenZhonghua",
            PasswordHash:HashPassword("user123"),
            MemberSince: new Date(2021, 12, 20, 15,30,45)
        },
        {
            Id: userIds[5],
            FirstName: "Peter",
            LastName:"One",
            Email: "program@slearn.org.au",
            Username:"Peter",
            PasswordHash:HashPassword("user123"),
            MemberSince: new Date(2023, 4, 4, 15,30,45)
        },
        {
            Id: userIds[6],
            FirstName: "Scammer",
            LastName:"Senior",
            Email: "seancemagic@slearn.org.au",
            Username:"SeniorScammer",
            PasswordHash:HashPassword("user123"),
            MemberSince: new Date(2025, 4, 4, 15,30,45)
        },
        {
            Id: userIds[7],
            FirstName: "SeniorLearnFan",
            LastName:"Fan",
            Email: "seniorLearnFanClub@slearn.org.au",
            Username:"SeniorLearnFan",
            PasswordHash:HashPassword("user123"),
            MemberSince: new Date(2025, 4, 4, 15,30,45)
        }
    ])
};
if (db.UserSetting.countDocuments({})===0){
    db.UserSetting.insertMany([
        {
            Id: userIds[0],
            FontSize: 36,
            DarkMode: false,
            EnableNotification: false,
        },
        {
            Id: userIds[1],
            FontSize: 36,
            DarkMode: true,
            EnableNotification: false,
        },
        {
            Id: userIds[2],
            FontSize: 32,
            DarkMode: true,
            EnableNotification: false,
        },
        {
            Id: userIds[3],
            FontSize: 24,
            DarkMode: true,
            EnableNotification: false,
        },
        {
            Id: userIds[4],
            FontSize: 36,
            DarkMode: true,
            EnableNotification: false,
        },
        {
            Id: userIds[5],
            FontSize: 36,
            DarkMode: true,
            EnableNotification: false,
        },
        {
            Id: userIds[6],
            FontSize: 36,
            DarkMode: true,
            EnableNotification: false,
        },
        {
            Id: userIds[7],
            FontSize: 36,
            DarkMode: true,
            EnableNotification: false,
        }
    ])
}



if (db.MemberBulletin.countDocuments({})===0){
    db.MemberBulletin.insertMany([
        {
            Id:new ObjectId(),
            Title: "Ice-Cream Party",
            Category: 2,
            Content:"Ice Cream Party to celebrate the 1st June for all SeniorLearn Members. Buy one get one free",
            DateCreated:new Date(2025, 4, 9, 15,30,45),
            DateUpdated:new Date(2025, 4, 9, 15,30,45),
            AuthorId:userIds[1],
            AuthorUsername:"AnonUser123"
        },
        {
            Id:new ObjectId(),
            Title: "Free Dental Seniors Only",
            Category: 2,
            Content: "Come to Ha Loo Loo Dental for your free dental check-up. Offer lasts for the next 3 days!",
            DateCreated:new Date(2025,4,4, 15,30,45),
            DateUpdated: new Date(2025,4,4, 15,30,45),
            AuthorId:userIds[2],
            AuthorUsername:"User101"
        },
        {
            Id:new ObjectId(),
            Title: "Theatre Night: Matilda",
            Category: 2,
            Content: "She felt fairly confident that with a great deal of practice and effort, she would succeed in the end.",
            DateCreated:new Date(2025,4,3, 15,30,45),
            DateUpdated: new Date(2025,4,3, 15,30,45),
            AuthorId:userIds[3],
            AuthorUsername:"TheatreSenior"
        },
        {
            Id:new ObjectId(),
            Title: "Martial Arts Lessons",
            Category: 1,
            Content: "Whenever you can separate Yin and Yang, one part doesn’t move, the other part moves, there has to be a relationship between them.",
            DateCreated:new Date(2025,4,9, 15,30,45),
            DateUpdated: new Date(2025,4,9, 15,30,45),
            AuthorId:userIds[4],
            AuthorUsername:"ChenZhonghua"
        },
        {
            Id:new ObjectId(),
            Title: "Boogie Woogie",
            Category: 1,
            Content: "Just joking, I'm gonna teach programming instead",
            DateCreated:new Date(2025,4,4, 15,30,45),
            DateUpdated: new Date(2025,4,4, 15,30,45),
            AuthorId:userIds[5],
            AuthorUsername:"Peter"
        },
        {
            Id: new ObjectId(),
            Title:"Seance Magic Talk to your Wife Today!!",
            Category:1,
            Content: "For the cheap price of $99, talk to the dead!!",
            DateCreated:new Date(2025,4,3, 15,30,45),
            DateUpdated: new Date(2025,4,3, 15,30,45),
            AuthorId:userIds[6],
            AuthorUsername:"SeniorScammer"
        },
        {
            Id: new ObjectId(),
            Title:"CelebratingSeniorLearnFan!!",
            Category:3,
            Content: "Woohoo",
            DateCreated:new Date(2025,4,9, 15,30,45),
            DateUpdated: new Date(2025,4,9, 15,30,45),
            AuthorId:userIds[7],
            AuthorUsername:"SeniorLearnFan"
        },
        {
            Id: new ObjectId(),
            Title:"CelebratingSeniorLearnFan!!",
            Category:3,
            Content: "Woohoo",
            DateCreated:new Date(2025,4,4, 15,30,45),
            DateUpdated: new Date(2025,4,4, 15,30,45),
            AuthorId:userIds[7],
            AuthorUsername:"SeniorLearnFan"
        },
        {
            Id: new ObjectId(),
            Title:"SeniorLearn App Out Now!",
            Category:3,
            Content: "Seniors need to learn technology now!! WOO HOO",
            DateCreated:new Date(2025,4,3, 15,30,45),
            DateUpdated: new Date(2025,4,3, 15,30,45),
            AuthorId:userIds[7],
            AuthorUsername:"SeniorLearnFan"
        }
    ])
};
if (db.OfficialBulletin.countDocuments({})===0){
    db.OfficialBulletin.insertMany([
        {
            Id: new ObjectId(),
            Title:"Important Organisation Update",
            Type:1,
            Content: "Dear SeniorLearn members,We are pleased to announce some important updates to our organisation that we believe will enhance your learning experience.Sincerely, The SeniorLearnTeam",
            DateCreated:new Date(2025,4,9, 15,30,45),
            DateUpdated: new Date(2025,4,9, 15,30,45),
            Author:"Admin"
        },
        {
            Id: new ObjectId(),
            Title:"Monthly Meeting Cancelled",
            Type:3,
            Content: "Someone got sick, its cancelled folks",
            DateCreated:new Date(2025,4,9, 15,30,45),
            DateUpdated: new Date(2025,4,9, 15,30,45),
            Author:"Admin"
        },
        {
            Id: new ObjectId(),
            Title:"SeniorLearn App Out Now!",
            Type:2,
            Content: "The SeniorLearn App is out now! It took a long time to make but its finally done. We are also gonna hold lessons on how to use it so stay tune!",
            DateCreated:new Date(2025,4,9, 15,30,45),
            DateUpdated:new Date(2025,4,9, 15,30,45),
            Author:"Admin"
        }
    ])
};