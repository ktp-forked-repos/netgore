using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `alliance_attackable`.
/// </summary>
public class AllianceAttackableTable : IAllianceAttackableTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"alliance_id", "attackable_id", "placeholder" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] {"alliance_id", "attackable_id" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"placeholder" };
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
public const System.String TableName = "alliance_attackable";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 3;
/// <summary>
/// The field that maps onto the database column `alliance_id`.
/// </summary>
System.Byte _allianceID;
/// <summary>
/// The field that maps onto the database column `attackable_id`.
/// </summary>
System.Byte _attackableID;
/// <summary>
/// The field that maps onto the database column `placeholder`.
/// </summary>
System.Nullable<System.Byte> _placeholder;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `alliance_id`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public DemoGame.Server.AllianceID AllianceID
{
get
{
return (DemoGame.Server.AllianceID)_allianceID;
}
set
{
this._allianceID = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `attackable_id`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public DemoGame.Server.AllianceID AttackableID
{
get
{
return (DemoGame.Server.AllianceID)_attackableID;
}
set
{
this._attackableID = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `placeholder`.
/// The underlying database type is `tinyint(3) unsigned`. The database column contains the comment: 
/// "Unused placeholder column - please do not remove".
/// </summary>
public System.Nullable<System.Byte> Placeholder
{
get
{
return (System.Nullable<System.Byte>)_placeholder;
}
set
{
this._placeholder = (System.Nullable<System.Byte>)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public IAllianceAttackableTable DeepCopy()
{
return new AllianceAttackableTable(this);
}
/// <summary>
/// AllianceAttackableTable constructor.
/// </summary>
public AllianceAttackableTable()
{
}
/// <summary>
/// AllianceAttackableTable constructor.
/// </summary>
/// <param name="allianceID">The initial value for the corresponding property.</param>
/// <param name="attackableID">The initial value for the corresponding property.</param>
/// <param name="placeholder">The initial value for the corresponding property.</param>
public AllianceAttackableTable(DemoGame.Server.AllianceID @allianceID, DemoGame.Server.AllianceID @attackableID, System.Nullable<System.Byte> @placeholder)
{
this.AllianceID = (DemoGame.Server.AllianceID)@allianceID;
this.AttackableID = (DemoGame.Server.AllianceID)@attackableID;
this.Placeholder = (System.Nullable<System.Byte>)@placeholder;
}
public AllianceAttackableTable(IAllianceAttackableTable source)
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
public static void CopyValues(IAllianceAttackableTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@alliance_id"] = (DemoGame.Server.AllianceID)source.AllianceID;
dic["@attackable_id"] = (DemoGame.Server.AllianceID)source.AttackableID;
dic["@placeholder"] = (System.Nullable<System.Byte>)source.Placeholder;
}

public void CopyValuesFrom(IAllianceAttackableTable source)
{
this.AllianceID = (DemoGame.Server.AllianceID)source.AllianceID;
this.AttackableID = (DemoGame.Server.AllianceID)source.AttackableID;
this.Placeholder = (System.Nullable<System.Byte>)source.Placeholder;
}

public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "alliance_id":
return AllianceID;

case "attackable_id":
return AttackableID;

case "placeholder":
return Placeholder;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "alliance_id":
this.AllianceID = (DemoGame.Server.AllianceID)value;
break;

case "attackable_id":
this.AttackableID = (DemoGame.Server.AllianceID)value;
break;

case "placeholder":
this.Placeholder = (System.Nullable<System.Byte>)value;
break;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

public static ColumnMetadata GetColumnData(System.String fieldName)
{
switch (fieldName)
{
case "alliance_id":
return new ColumnMetadata("alliance_id", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, true, false);

case "attackable_id":
return new ColumnMetadata("attackable_id", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, true, false);

case "placeholder":
return new ColumnMetadata("placeholder", "Unused placeholder column - please do not remove", "tinyint(3) unsigned", null, typeof(System.Nullable<System.Byte>), true, false, false);

default:
throw new ArgumentException("Field not found.","fieldName");
}
}

}

}
