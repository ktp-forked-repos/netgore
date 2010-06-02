﻿using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Features.Shops;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class ReplaceShopItemQuery : DbQueryNonReader<IShopItemTable>
    {
        static readonly string _queryStr = FormatQueryString("REPLACE INTO `{0}` {1}", ShopItemTable.TableName,
                                                         FormatParametersIntoValuesString(ShopItemTable.DbColumns));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceShopItemQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public ReplaceShopItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
        }

        public int Execute(ShopID shopID, ItemTemplateID itemID)
        {
            return Execute(new ShopItemTable(itemID, shopID));
        }

        public int Execute(ShopID shopID, IEnumerable<ItemTemplateID> itemIDs)
        {
            var sum = 0;
            foreach (var itemID in itemIDs)
            {
                sum += Execute(shopID, itemID);
            }
            return sum;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>
        /// IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.
        /// </returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(ShopItemTable.DbColumns);
        }

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="item">The item.</param>
        protected override void SetParameters(DbParameterValues p, IShopItemTable item)
        {
            item.CopyValues(p);
        }
    }
}