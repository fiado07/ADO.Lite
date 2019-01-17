using ADO.Lite.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace ADO.Lite.Contracts
{
    /// <summary>
    /// Interface connection definition
    /// </summary>
    /// <seealso cref="System.Data.IDbConnection" />
    public interface IDbConnectionSql
    {


        IDbConnection DbConnectionBase { get; set; }

        IDbTransaction DbTransaction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can close.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can close; otherwise, <c>false</c>.
        /// </value>
        bool canClose { get; set; }

        /// <summary>
        /// Gets or sets the set adapter.
        /// </summary>
        /// <value>
        /// The set adapter.
        /// </value>
        IDbDataAdapter SetAdapter { get; set; }


        /// <summary>
        /// Gets or sets the database provider.
        /// </summary>
        /// <value>
        /// The database provider.
        /// </value>
        DbProvider DbProvider { get; set; }

    }
}