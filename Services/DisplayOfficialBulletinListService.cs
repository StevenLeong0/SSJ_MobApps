using System.Collections.ObjectModel;
using System.Data.Common;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbSettingsabc;

public class DisplayOfficialBulletinListService : IDisplayBulletinListService<OfficialBulletin>
{
    private readonly IMongoCollection<OfficialBulletin> _officialBulletinList;
    public DisplayOfficialBulletinListService(IOptions<MongoDbSettings> dbSettings)
    {
        var client = new MongoClient(dbSettings.Value.ConnectionString);
        var database = client.GetDatabase(dbSettings.Value.DatabaseName);
        _officialBulletinList = database.GetCollection<OfficialBulletin>("OfficialBulletin");   
    }
    public Task<List<OfficialBulletin>> GetBulletinListByDate<OfficialType>(OfficialType NoType)
    {
        /* MongoDb's built-in way to say “no filter”
        var result = _officialBulletinList.Find(FilterDefinition<OfficialBulletin>.Empty).SortByDescending(b => b.DateCreated).ToListAsync();
        */
        //C#  A predicate (lambda) that returns true for every document
        var result = _officialBulletinList
        .Find(doc => true)
        .SortByDescending(b => b.DateCreated)
        .ToListAsync();

        //change author from Id to username
        return result;
        
        /*Won't there be conflict between the 2 discard variables- one in parameter and the other in the lambda?
        No, because they are in different scopes and they are not discard variables (just identifiers) 

        Types of discard variables:
        deconstruction:
        var (_, lastname) = GetName(); [Discard first part]
        pattern matching
        if (obj is string _) {} [just check type, don't use it]
        out parameters
        int.TryParse("123", out _); [We don’t care about the parsed result]

        Not all unused variables are discard variables, unused variables are still allocated a named variable.
        A true discard variable is a compiler supported feature:
        the compiler does not assign a value to it
        doesn't create a named variable
        knows to deliberately ignore the value

        Example of not a discard variable
        public void hi (string _)
        {
        Console.Writeline("Hello");
        }



        Revise on tuple-returning methods, tuples which are just lightweight unnamed objects that hold values
        */
    }
}