using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `account_ips`.
/// </summary>
public class AccountIpsTable : IAccountIpsTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"account_id", "ip", "time" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumns;
}
}
/// <summary>
/// Array of the database column names for columns that are primary keys.
/// </summary>
 static  readonly System.String[] _dbColumnsKeys = new string[] {"account_id", "time" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbKeyColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumnsKeys;
}
}
/// <summary>
/// Array of the database column names for columns that are not primary keys.
/// </summary>
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"ip" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbNonKeyColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumnsNonKey;
}
}
/// <summary>
/// The name of the database table that this class represents.
/// </summary>
public const System.String TableName = "account_ips";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 3;
/// <summary>
/// The field that maps onto the database column `account_id`.
/// </summary>
System.Int32 _accountID;
/// <summary>
/// The field that maps onto the database column `ip`.
/// </summary>
System.UInt32 _ip;
/// <summary>
/// The field that maps onto the database column `time`.
/// </summary>
System.DateTime _time;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `account_id`.
/// The underlying database type is `int(11)`. The database column contains the comment: 
/// "The ID of the account.".
/// </summary>
public DemoGame.Server.AccountID AccountID
{
get
{
return (DemoGame.Server.AccountID)_accountID;
}
set
{
this._accountID = (System.Int32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `ip`.
/// The underlying database type is `int(10) unsigned`. The database column contains the comment: 
/// "The IP that logged into the account.".
/// </summary>
public System.UInt32 Ip
{
get
{
return (System.UInt32)_ip;
}
set
{
this._ip = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `time`.
/// The underlying database type is `datetime`. The database column contains the comment: 
/// "When this IP last logged into this account.".
/// </summary>
public System.DateTime Time
{
get
{
return (System.DateTime)_time;
}
set
{
this._time = (System.DateTime)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public IAccountIpsTable DeepCopy()
{
return new AccountIpsTable(this);
}
/// <summary>
/// AccountIpsTable constructor.
/// </summary>
public AccountIpsTable()
{
}
/// <summary>
/// AccountIpsTable constructor.
/// </summary>
/// <param name="accountID">The initial value for the corresponding property.</param>
/// <param name="ip">The initial value for the corresponding property.</param>
/// <param name="time">The initial value for the corresponding property.</param>
public AccountIpsTable(DemoGame.Server.AccountID @accountID, System.UInt32 @ip, System.DateTime @time)
{
this.AccountID = (DemoGame.Server.AccountID)@accountID;
this.Ip = (System.UInt32)@ip;
this.Time = (System.DateTime)@time;
}
/// <summary>
/// AccountIpsTable constructor.
/// </summary>
/// <param name="source">IAccountIpsTable to copy the initial values from.</param>
public AccountIpsTable(IAccountIpsTable source)
{
CopyValuesFrom(source);
}
/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
/// this method will not create them if they are missing.
/// </summary>
/// <param name="dic">The Dictionary to copy the values into.</param>
public void CopyValues(System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
CopyValues(this, dic);
}
/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
/// this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="dic">The Dictionary to copy the values into.</param>
public static void CopyValues(IAccountIpsTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@account_id"] = (DemoGame.Server.AccountID)source.AccountID;
dic["@ip"] = (System.UInt32)source.Ip;
dic["@time"] = (System.DateTime)source.Time;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this AccountIpsTable.
/// </summary>
/// <param name="source">The IAccountIpsTable to copy the values from.</param>
public void CopyValuesFrom(IAccountIpsTable source)
{
this.AccountID = (DemoGame.Server.AccountID)source.AccountID;
this.Ip = (System.UInt32)source.Ip;
this.Time = (System.DateTime)source.Time;
}

/// <summary>
/// Gets the value of a column by the database column's name.
/// </summary>
/// <param name="columnName">The database name of the column to get the value for.</param>
/// <returns>
/// The value of the column with the name <paramref name="columnName"/>.
/// </returns>
public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "account_id":
return AccountID;

case "ip":
return Ip;

case "time":
return Time;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Sets the <paramref name="value"/> of a column by the database column's name.
/// </summary>
/// <param name="columnName">The database name of the column to get the <paramref name="value"/> for.</param>
/// <param name="value">Value to assign to the column.</param>
public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "account_id":
this.AccountID = (DemoGame.Server.AccountID)value;
break;

case "ip":
this.Ip = (System.UInt32)value;
break;

case "time":
this.Time = (System.DateTime)value;
break;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Gets the data for the database column that this table represents.
/// </summary>
/// <param name="columnName">The database name of the column to get the data for.</param>
/// <returns>
/// The data for the database column with the name <paramref name="columnName"/>.
/// </returns>
public static ColumnMetadata GetColumnData(System.String columnName)
{
switch (columnName)
{
case "account_id":
return new ColumnMetadata("account_id", "The ID of the account.", "int(11)", null, typeof(System.Int32), false, true, false);

case "ip":
return new ColumnMetadata("ip", "The IP that logged into the account.", "int(10) unsigned", null, typeof(System.UInt32), false, false, false);

case "time":
return new ColumnMetadata("time", "When this IP last logged into this account.", "datetime", null, typeof(System.DateTime), false, true, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

}

}