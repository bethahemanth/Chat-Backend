using ChatAppBackend.Models;
using ChatApplication.Data.ChatAppBackend.Data;
using ChatApplication.Repositories.Service_Contracts;


namespace ChatApplication.Repositories
{
    public class Repo:IRepo
    {
        private readonly IDatabaseContext _databaseContext;
        public Repo(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public string CreateTable(Query query)
        {
            string qry = "CREATE TABLE IF NOT EXISTS " + query.TableName + " (";
            int i = 0;
            List<string> splitKeywords;
            for (i = 0; i < query.Attributes.Count - 1; i++)
            {
                splitKeywords = query.Attributes[i].Split(",").ToList();
                qry += splitKeywords[1] + " " + splitKeywords[0] + " , ";
            }
            splitKeywords = query.Attributes[query.Attributes.Count - 1].Split(",").ToList();
            qry += splitKeywords[1] + " " + splitKeywords[0];

            qry += ");";
            return _databaseContext.ExecuteQuery(qry);
        }

        public string InsertRecord(Query query)
        {
            if (query.Values == null || query.Values.Count == 0)
            {
                return "No values provided.";
            }

            List<string> columns = new List<string>();
            List<string> rows = new List<string>();

            foreach (var row in query.Values)
            {
                List<string> columnValues = new List<string>();
                foreach (var colValue in row)
                {
                    var split = colValue.Split('=');
                    if (split.Length == 2)
                    {
                        string column = split[0];
                        string value = split[1];

                        if (!columns.Contains(column))
                        {
                            columns.Add(column);
                        }

                        columnValues.Add(value);
                    }
                }
                if (columnValues.Count > 0)
                {
                    rows.Add($"({string.Join(", ", columnValues)})");
                }
            }

            if (columns.Count == 0 || rows.Count == 0)
            {
                return "Invalid data format.";
            }

            string qry = $"INSERT INTO {query.TableName} ({string.Join(", ", columns)}) VALUES {string.Join(", ", rows)};";

            return _databaseContext.ExecuteQuery(qry);
        }

        public string DeleteRecord(Query query)
        {
            string qry = $"DELETE FROM {query.TableName}";
            if (query.Conditions != null && query.Conditions.Count > 0)
            {
                qry += " WHERE " + string.Join(" AND ", query.Conditions);
            }
            return _databaseContext.ExecuteQuery(qry);
        }

        public List<T> GetRecords<T>(Query query)
        {
            string qry = "SELECT * FROM " + query.TableName + " ";
            if (query.Conditions != null && query.Conditions.Count > 0)
            {
                qry += " WHERE " + string.Join(" AND ", query.Conditions);
            }
            List<T> records = _databaseContext.GetQuery<T>(qry);
            return records;
        }

        public string UpdateRecord(Query query)
        {
            if (string.IsNullOrEmpty(query.TableName) || query.Values == null || query.Values.Count == 0)
                return "Invalid input parameters";

            List<string> updateStatements = new List<string>();

            // Assuming query.Values is a List<List<string>>, where each sub-list contains ["column=value"]
            foreach (var row in query.Values)
            {
                if (row.Count > 0)
                {
                    updateStatements.AddRange(row);
                }
            }

            if (updateStatements.Count == 0)
                return "No valid update values provided.";

            string qry = $"UPDATE {query.TableName} SET ";
            qry += string.Join(", ", updateStatements);

            if (query.Conditions != null && query.Conditions.Count > 0)
            {
                qry += " WHERE " + string.Join(" AND ", query.Conditions);
            }
            qry += ";";
            return _databaseContext.ExecuteQuery(qry);
        }
    }
}
