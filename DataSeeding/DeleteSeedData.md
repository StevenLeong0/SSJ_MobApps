const {MongoClient} = require("mongodb");

const uri = "mongodb://localhost:27017";
const dbName = "SLearnMobApp_db";

deleteDatabase();

async function deleteDatabase() {
    const client = new MongoClient(uri);
  
    try {
      await client.connect();
      const db = client.db(dbName);
      const result = await db.dropDatabase();
  
      if (result) {
        console.log(`Database '${dbName}' deleted successfully.`);
      } else {
        console.log(`Database '${dbName}' did not exist or failed to delete.`);
      }
    } catch (err) {
      console.error("Error deleting database:", err);
    } finally {
      await client.close();
    }
  }

