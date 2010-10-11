﻿using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Interface for a root query builder object.
    /// </summary>
    public interface IQueryBuilder
    {
        /// <summary>
        /// Gets the <see cref="IQueryBuilderSettings"/> for this <see cref="IQueryBuilder"/>.
        /// </summary>
        IQueryBuilderSettings Settings { get; }

        /// <summary>
        /// Gets the <see cref="IQueryBuilderKeywords"/> for this <see cref="IQueryBuilder"/>.
        /// </summary>
        IQueryBuilderKeywords Keywords { get; }

        /// <summary>
        /// Creates an <see cref="IDeleteQuery"/>.
        /// </summary>
        /// <param name="tableName">The name of the table to delete from.</param>
        /// <returns>The <see cref="IDeleteQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="tableName"/> is an invalid table name.</exception>
        IDeleteQuery Delete(string tableName);

        /// <summary>
        /// Creates an <see cref="IInsertQuery"/>.
        /// </summary>
        /// <param name="tableName">The name of the table to insert on.</param>
        /// <returns>The <see cref="IInsertQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="tableName"/> is an invalid table name.</exception>
        IInsertQuery Insert(string tableName);

        /// <summary>
        /// Creates an <see cref="ISelectQuery"/>.
        /// </summary>
        /// <param name="tableName">The name of the table to select from.</param>
        /// <param name="alias">The alias to give the <paramref name="tableName"/>. Set as null to not use an alias.</param>
        /// <returns>The <see cref="ISelectQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="tableName"/> is an invalid table name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="alias"/> is an invalid table alias.</exception>
        ISelectQuery Select(string tableName, string alias = null);

        /// <summary>
        /// Creates an <see cref="IUpdateQuery"/>.
        /// </summary>
        /// <param name="tableName">The name of the table to update.</param>
        /// <returns>The <see cref="IUpdateQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="tableName"/> is an invalid table name.</exception>
        IUpdateQuery Update(string tableName);
    }
}