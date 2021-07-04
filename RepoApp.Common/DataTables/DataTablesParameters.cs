using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace RepoApp.Common.DataTables
{
    [Serializable]
    [DataContract]
    public class DataTablesParameters
    {
        [DataMember(Name = "totalCount")]
        public int TotalCount { get; set; }

        [DataMember(Name = "draw")]
        public int Draw { get; set; }

        [DataMember(Name = "start")]
        public int Start { get; set; }

        [DataMember(Name = "length")]
        public int Length { get; set; }

        [DataMember(Name = "columns")]
        public List<DataTablesColumn> Columns { get; set; }

        [DataMember(Name = "search")]
        public DataTablesSearch Search { get; set; }

        [DataMember(Name = "order")]
        public List<DataTablesOrder> Order { get; set; }


        /// <summary>
        /// Used for sorting
        /// </summary>
        public void SetColumnName()
        {
            foreach (var item in Order)
            {
                item.Name = Columns[item.Column].Data;
            }
        }

        /// <summary>
        /// Gets the <see cref="DataTablesColumn"/> with the specified column name.
        /// </summary>
        /// <value>
        /// The <see cref="DataTablesColumn"/>.
        /// </value>
        /// <param name="columnName">The column name.</param>
        /// <returns></returns>
        public DataTablesColumn this[string columnName] => this.Columns.FirstOrDefault(x => x.Data == columnName);
    }
}