using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;
using JPTest;

namespace JPTest
{
    public class JP_DAC
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog_DAC" /> class.
        /// </summary>
        public JP_DAC()
        {
            if (this.SqlHelper == null)
            {
                this.SqlHelper = new SqlHelper();
            }
        }

        /// <summary>
        /// Gets or sets the SQL Helper class
        /// </summary>
        public SqlHelper SqlHelper { get; set; }

        /// <summary>
        /// Convert List Collection to Data Table
        /// </summary>
        /// <typeparam name="T">List Collection</typeparam>
        /// <param name="data"> List data</param>
        /// <returns>Data table</returns>
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;
        }
        internal DataTable Get_OtherUserTestDetails(int moduleId, DateTime startDate, DateTime endDate)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Get_OtherUserTestDetails_Proc")
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 3600
            };

            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            cmd.Parameters.AddWithValue("@ModuleId", moduleId);
            SqlDataReader dr = this.SqlHelper.ExecuteDataReader(cmd);

            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }

            this.SqlHelper.Close();
            return dt;
        }

        internal int UpdateEvaluatorDetails(long userId, long evaluatorId, int moduleId, int marks)
        {
            int res = 0;
            SqlCommand cmd = new SqlCommand();
            string strQuery = "update Other_UserDetails "
                + " set EvaluatorId = @EvaluatorId, EvaluatorStatus = 1, EvaluationDt = GETDATE(), total_marks_obtained = @Marks "
                + " where UserId = @UserId and ModuleId = @ModuleId ";

            cmd.CommandText = strQuery;
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@EvaluatorId", evaluatorId);
            cmd.Parameters.AddWithValue("@Marks", marks);
            cmd.Parameters.AddWithValue("@ModuleId", moduleId);
            res = this.SqlHelper.ExecuteNonQuery(cmd);

            cmd.CommandText = strQuery;

            return res;
        }

        internal int UpdateUserDetails(long userId, int moduleId)
        {
            int insertCnt = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                string query = " update Other_UserDetails set ended_time=getdate(),submitted_type='FINISH',tot_Sec=DATEDIFF(SECOND,started_time,getdate()) where userid=@userId and roleid=3 and ModuleId=@moduleId ";
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@moduleId", moduleId);
                insertCnt = this.SqlHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                insertCnt = 0;
            }
            finally
            {
                this.SqlHelper.Close();
            }

            return insertCnt;
        }
    }
}