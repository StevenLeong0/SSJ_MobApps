
db = connect ("mongodb://localhost:9887/SLearnMobApp_db")

const collections = ["User","MemberBulletin", "OfficialBulletin", "UserSetting"];
collections.forEach(collection => {
    db.getCollection(collection).drop();
})
//delete in mongodbshell in js