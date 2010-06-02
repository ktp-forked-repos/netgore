﻿using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class DeleteItemTemplateQuery : DbQueryNonReader<ItemTemplateID>
    {
        static readonly string _queryStr = FormatQueryString("DELETE FROM `{0}` WHERE `id`=@id", ItemTemplateTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteItemTemplateQuery"/> class.
        /// </summary>
        /// <param name="connectionPool"><see cref="DbConnectionPool"/> to use for creating connections to
        /// execute the query on.</param>
        public DeleteItemTemplateQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
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
            return CreateParameters("id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ItemTemplateID item)
        {
            p["id"] = (int)item;
        }
    }
}