//This is the alpha version of PowerBI API Client. 
//This version is obsolete, pls use the release one. 
//More informations : https://github.com/Vtek/PowerBI.Api.Client
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using PowerBI.Api.Client.Entity;

namespace PowerBI.Api.Client.Helper
{
    /// <summary>
    /// Helper to deal with data schema
    /// </summary>
    public static class DefinitionHelper
    {
        /// <summary>
        /// Get the json de
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datasetName"></param>
        /// <returns></returns>
        public static DatasetDefinition GetDataset(string datasetName)
        {
            return new DatasetDefinition
            {
                Name = datasetName,
                Tables = new List<TableDefinition>()
            };
        }

        public static TableDefinition GetTable(Type type)
        {
            var table = new TableDefinition()
            {
                Name = type.Name,
                Columns = new List<ColumnDefinition>()
            };

            var properties = type.GetProperties().Where(x => x.CanRead && x.CanWrite).ToList();

            foreach (var propertyInfo in properties)
                table.Columns.Add(GetColumn(propertyInfo));

            return table;
        }

        public static ColumnDefinition GetColumn(PropertyInfo propertyInfo)
        {
            var column = new ColumnDefinition
            {
                Name = typeof (Nullable<>).IsAssignableFrom(propertyInfo.PropertyType)
                    ? propertyInfo.PropertyType.GenericTypeArguments[0].Name
                    : propertyInfo.Name,
                DataType = GetDataType(propertyInfo.PropertyType)
            };

            return column;
        }

        public static string GetDataType(Type type)
        {
            var dataType = string.Empty;

            switch (type.Name)
            {
                case "Int32":
                case "Int64":
                    dataType = "Int64";
                    break;
                case "Double":
                    dataType = "Double";
                    break;
                case "Boolean":
                    dataType = "bool";
                    break;
                case "DateTime":
                    dataType = "DateTime";
                    break;
                case "String":
                    dataType = "string";
                    break;
            }

            return dataType;
        }
    }
}
