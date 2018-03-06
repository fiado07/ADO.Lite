
namespace ADO.Lite.Contracts
{
    /// <summary>
    /// Map data from one to other objects
    /// </summary>
    public interface IMaper
    {

       TMapTo Map<TMapTo>(System.Data.DataTable fromTable) where TMapTo : new();


        TMapTo Map<TMapTo>(System.Data.IDataReader reader, System.Collections.Generic.Dictionary<string, string> dbtypes) where TMapTo : new();

        System.Collections.Generic.IEnumerable<TMapTo> MapToList<TMapTo>(System.Data.IDataReader reader, System.Collections.Generic.Dictionary<string, string> dbtypes) where TMapTo : new();
        
    }
}
