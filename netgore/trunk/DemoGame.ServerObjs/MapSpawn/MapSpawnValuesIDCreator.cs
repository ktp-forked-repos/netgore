using System.Linq;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using NetGore.Db;

namespace DemoGame.Server
{
    [DBControllerQuery]
    public class MapSpawnValuesIDCreator : IDCreatorBase
    {
        public MapSpawnValuesIDCreator(DbConnectionPool connectionPool)
            : base(connectionPool, MapSpawnTable.TableName, "id", 1, 0)
        {
        }
    }
}