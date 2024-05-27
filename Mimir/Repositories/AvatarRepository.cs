using Libplanet.Crypto;
using Mimir.Models.Agent;
using Mimir.Models.Avatar;
using Mimir.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mimir.Repositories;

public class AvatarRepository : BaseRepository<BsonDocument>
{
    public AvatarRepository(MongoDBCollectionService mongoDBCollectionService)
        : base(mongoDBCollectionService)
    {
    }

    protected override string GetCollectionName()
    {
        return "avatars";
    }
    
    public Avatar? GetAvatar(string network, Address avatarAddress)
    {
        var collection = GetCollection(network);
        var filter = Builders<BsonDocument>.Filter.Eq("Avatar.address", avatarAddress.ToHex());
        var document = collection.Find(filter).FirstOrDefault();
        if (document is null)
        {
            return null;
        }

        try
        {
            return new Avatar(
                document["Avatar"]["agentAddress"].AsString,
                document["Avatar"]["address"].AsString,
                document["Avatar"]["name"].AsString,
                document["Avatar"]["level"].AsInt32,
                document["Avatar"]["actionPoint"].AsInt32,
                document["Avatar"]["dailyRewardReceivedIndex"].ToInt64()
            );
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    public Inventory? GetInventory(string network, Address avatarAddress)
    {
        var collection = GetCollection(network);
        var filter = Builders<BsonDocument>.Filter.Eq("Avatar.address", avatarAddress.ToHex());
        var projection = Builders<BsonDocument>.Projection.Include("Avatar.inventory.Equipments");
        var document = collection.Find(filter).Project(projection).FirstOrDefault();
        if (document is null)
        {
            return null;
        }

        try
        {
            return new Inventory(document["Avatar"]["inventory"].AsBsonDocument);
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }
}