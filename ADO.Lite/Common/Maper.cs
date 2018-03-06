using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Lite.Common
{
    public class Maper : Contracts.IMaper
    {


        public TMapTo Map<TMapTo>(DataTable fromTable) where TMapTo : new()
        {
            List<string> columns = new List<string>();
            DataTable tableSchemer = fromTable.Clone();
            TMapTo ToObject = new TMapTo();


            // Get columns
            columns = tableSchemer.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();


            // --:: set values

            columns.ForEach(x =>
            {

                // get properties and set new values 

                Array.ForEach(ToObject.GetType().GetProperties(),
                    (_propInfo) =>
                    {

                        if (_propInfo.Name.ToString().StartsWith(x))
                        {
                            var _valor = fromTable.AsEnumerable().FirstOrDefault()[x];


                            if (_propInfo.PropertyType == typeof(DateTime))
                            {

                                if (!string.IsNullOrEmpty(_valor.ToString())) _propInfo.SetValue(ToObject, Convert.ToDateTime(_valor), null);
                            }
                            else
                            {

                                if (!string.IsNullOrEmpty(_valor.ToString())) _propInfo.SetValue(ToObject, _valor, null);
                            }


                        }

                    });

            });

            return ToObject;

        }

        public TMapTo Map<TMapTo>(IDataReader reader, Dictionary<string, string> dbtypes) where TMapTo : new()
        {

            TMapTo newObject = new TMapTo();


            while (reader.Read())
            {


                Array.ForEach(typeof(TMapTo).GetProperties(), (propertInfo) =>
                                                                            {

                                                                                SetMapedObject<TMapTo>(newObject, reader, dbtypes, propertInfo);

                                                                            });
            }

            return newObject;


        }

        public IEnumerable<TMapTo> MapToList<TMapTo>(IDataReader reader, Dictionary<string, string> dbtypes) where TMapTo : new()
        {


            TMapTo newObject;
            List <TMapTo> newObjectList = new List<TMapTo>();

            while (reader.Read())
            {

                newObject = new TMapTo();

                Array.ForEach(typeof(TMapTo).GetProperties(), (propertInfo) =>
                                                                            {

                                                                                SetMapedObject<TMapTo>(newObject, reader, dbtypes, propertInfo);

                                                                            });
                //newObjectList.Add(newObject);
                yield return newObject;

            }

            //return newObjectList;
        }

        private void SetMapedObject<TMapTo>(TMapTo newObject, IDataReader reader, Dictionary<string, string> dbtypes, System.Reflection.PropertyInfo propertyInfo) where TMapTo : new()
        {



            // check if column exists 
            bool columnExists = Enumerable.Range(0, reader.FieldCount).Any(i => reader.GetName(i) == propertyInfo.Name);

            // check 
            if (columnExists)
            {

                // -- Get values :: --
                var _result = Convert.IsDBNull(reader[propertyInfo.Name]) ? "" : reader[propertyInfo.Name];


                // get data type from db
                var dataTypes = dbtypes.FirstOrDefault(p => p.Key == propertyInfo.Name).Value.Trim();


                if (dataTypes.StartsWith("date"))
                {

                    // set data
                    propertyInfo.SetValue(newObject, Convert.ToDateTime(_result), null);

                }
                else if (dataTypes == "int")
                {
                    // set data
                    propertyInfo.SetValue(newObject, Convert.ToInt32(_result), null);

                }
                else if (dataTypes == "decimal")
                {
                    // set data
                    propertyInfo.SetValue(newObject, Convert.ToDecimal(_result), null);

                }
                else if (dataTypes == "Double")
                {
                    // set data
                    propertyInfo.SetValue(newObject, Convert.ToDouble(_result), null);
                }
                else
                {
                    // set data
                    propertyInfo.SetValue(newObject, _result.ToString().Trim(), null);

                }


            }


        }


    }
}
