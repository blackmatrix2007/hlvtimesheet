using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HLVTimeSheet.AcsessData
{
    public class ExecuteData
    {
        #region Properties

        public static string ConnectionString
        {
            get
            {
                return ConnectionDatabase.ConStringHLVTimeSheet;
            }
        }

        public int TotalRows
        {
            get;
            set;
        }

        #endregion Properties

        public const int Xem = 0;
        public const int Xoa = 1;
        public const int TaoMoi = 2;
        public const int CapNhat = 3;
        public const int Duyet = 4;
        public const int HuyChot = 5;

        private string ConStringHLVTimeSheet = "";

        public ExecuteData()
        {
            ConnectionDatabase conn = new ConnectionDatabase();

            ConStringHLVTimeSheet = conn.ReturnConnectionDatabase();
        }

        public ExecuteData(string server, string databasename, string username, string password)
        {
            ConnectionDatabase conn = new ConnectionDatabase();

            ConStringHLVTimeSheet = conn.ReturnConnectionDatabase(server, databasename, username, password);
        }

        public DataTable ReadTable(string sql)
        {
            using (SqlDataAdapter reader = new SqlDataAdapter(sql, ConStringHLVTimeSheet))
            {
                reader.SelectCommand.CommandTimeout = int.MaxValue;
                DataTable result = new DataTable();
                reader.Fill(result);
                return result;
            }
        }

        public DataTable ReadTable(string sql, out SqlDataAdapter da)
        {
            da = new SqlDataAdapter(sql, ConStringHLVTimeSheet);
            da.SelectCommand.CommandTimeout = int.MaxValue;

            DataTable result = new DataTable();
            da.Fill(result);
            SqlCommandBuilder sqlcommand = new SqlCommandBuilder(da);
            return result;
        }

        public bool ExecuteNonQuerySQL(string sql)
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(ConStringHLVTimeSheet))
                {
                    connectionSQL.Open();
                    SqlCommand cm = new SqlCommand(sql, connectionSQL);
                    if (cm.ExecuteNonQuery() > 0)
                        return true;
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public object ExecuteScalarSQL(string sql)
        {
            using (SqlConnection connectionSQL = new SqlConnection(ConStringHLVTimeSheet))
            {
                connectionSQL.Open();
                SqlCommand cm = new SqlCommand(sql, connectionSQL);

                return cm.ExecuteScalar();
            }
        }

        public SqlDataReader getData(SqlCommand comand)
        {
            SqlDataReader reader = null;

            return reader;
        }

        public DataTable getTableFromProc(string procName, string[] paraName, string[] paraValue)
        {
            DataTable KQ = new DataTable();

            using (SqlConnection ketNoi = new SqlConnection(ConStringHLVTimeSheet))
            {
                ketNoi.Open();
                SqlCommand command = new SqlCommand(procName, ketNoi);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = int.MaxValue;

                if (paraName.Length > 0)
                {
                    for (int i = 0; i < paraName.Length; i++)
                    {
                        command.Parameters.Add(paraName[i], SqlDbType.NVarChar);
                        command.Parameters[paraName[i]].Value = paraValue[i];
                        command.Parameters[paraName[i]].Direction = ParameterDirection.Input;
                    }
                }

                SqlDataReader dr = command.ExecuteReader();
                if (dr != null)
                {
                    KQ.Load(dr);
                }

                ketNoi.Close();
                command.Dispose();
            }

            return KQ;
        }

        public string[] getRole(string userId, string function_fk)
        {
            //Theo thu tu: XEM - XOA - TAO MOI - CHINH SUA - DUYET - Mở chốt
            string[] roles = new string[] { "0", "0", "0", "0", "0", "0" };

            try
            {
                string query = "SELECT max(xem) as xem, max(xoa) as xoa, max(taomoi) as taomoi, max(sua) as sua, max(chot) as chot, max(Huychot) as mochot " +
                               " FROM NhomQuyen_ChucNang_ChiTiet  " +
                               " WHERE nhomquyen_fk in ( SELECT nhomquyen_fk FROM NhanVien_Quyen_NhomQuyen WHERE nhanvien_fk = '" + userId + "' ) and chucnang_fk = '" + function_fk + "'";

                DataTable dt = this.ReadTable(query);
                if (dt.Rows.Count > 0)
                {
                    roles[0] = dt.Rows[0]["xem"].ToString();
                    roles[1] = dt.Rows[0]["xoa"].ToString();
                    roles[2] = dt.Rows[0]["taomoi"].ToString();
                    roles[3] = dt.Rows[0]["sua"].ToString();
                    roles[4] = dt.Rows[0]["chot"].ToString();
                    roles[5] = dt.Rows[0]["mochot"].ToString();
                }
            }
            catch { }

            return roles;
        }
       
        public string Standardized(string input)
        {
            string result = Regex.Replace(input, "'", "''");
            return result;
        }

        public string ReplaceComma(string input)
        {
            string result = Regex.Replace(input, ",", "");
            return result;
        }
       
        public string ForMatNumberHOADON(string input)
        {
            try
            {
                if (input.Trim().Length <= 0)
                    return "0";

                string kq = double.Parse(input).ToString("#,#", CultureInfo.InvariantCulture);
                if (kq.Trim().Length <= 0)
                    kq = "0";

                if (kq.Contains("."))
                {
                    string[] arr = Regex.Split(kq, ".");
                    kq = Regex.Replace(arr[0], ",", "/.") + ", " + arr[1];
                }
                else
                    kq = Regex.Replace(kq, ",", ".");

                return kq;
            }
            catch (Exception e)
            {
                return "0";
            }

        }
        
        public string ForMatNumber(string input)
        {
            if (input.Trim().Length <= 0 || input.Equals("0"))
                return "0";

            if (!input.Contains("."))
                return double.Parse(input).ToString("#,#", CultureInfo.InvariantCulture);

            try
            {
                string[] data = Regex.Split(input, @"\.");

                string phanNGUYEN = double.Parse(data[0]).ToString("#,#", CultureInfo.InvariantCulture);
                if (phanNGUYEN.Trim().Length <= 0)
                    phanNGUYEN = "0";

                string phanle = "0";
                if (data[1].Trim().Length >= 3)
                    phanle = data[1].Substring(0, 3);
                else
                    phanle = data[1];

                while (phanle.EndsWith("0"))
                    phanle = phanle.Substring(0, phanle.Length - 1);

                if (phanle.Trim().Length > 0 && double.Parse(phanle) > 0)
                    return phanNGUYEN + "." + phanle;
                else
                    return phanNGUYEN;
            }
            catch (Exception e)
            {
                return "0";
            }
        }

        public string Change_AV(string ip_str_change)
        {
            ip_str_change = ip_str_change.Trim();

            Regex v_reg_regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string v_str_FormD = ip_str_change.Normalize(NormalizationForm.FormD);
            string kq = v_reg_regex.Replace(v_str_FormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

            return Regex.Replace(kq.Trim(), " ", "-");
        }

        public bool checkKeyData(string keyData)
        {
            string sql = "SELECT COUNT(*) FROM SanPham WHERE keyData = '" + keyData + "' ";
            object obj = this.ExecuteScalarSQL(sql);
            if (obj != null)
            {
                if (int.Parse(obj.ToString()) > 0)
                    return false;
            }
            else
                return false;
            return true;
        }

        //THỂ HIỆN TRẠNG THÁI
        public string GetStatus(string language, string tableName, string status)
        {
            string result = "";

            if (tableName.Equals("DONHANG"))
            {
                if (language.Equals("1"))
                {
                    if (status.Equals("0"))
                        result = " <span class='label label-info'>Processing</span> ";
                    else if (status.Equals("1"))
                        result = " <span class='label label-success'>Approved</span> ";
                    else
                        result = " <span class='label label-danger'>Cancelled</span> ";
                }
                else if (language.Equals("2"))
                {
                    if (status.Equals("0"))
                        result = " <span class='label label-info'>Đang xử lý</span> ";
                    else if (status.Equals("1"))
                        result = " <span class='label label-success'>Đã duyệt</span> ";
                    else
                        result = " <span class='label label-danger'>Đã hủy</span> ";
                }
            }
            else if (tableName.Equals("DATHANG"))
            {
                if (status.Equals("0"))
                    result = " <span class='label label-info'>Chờ xử lý</span> ";
                else if (status.Equals("1"))
                    result = " <span class='label label-primary'>Đã duyệt</span> ";
                else if (status.Equals("2"))
                    result = " <span class='label label-danger'>Cancelled</span> ";
                else if (status.Equals("3"))
                    result = " <span class='label label-default'>Đã lập phiếu</span> ";
                else if (status.Equals("4"))
                    result = " <span class='label label-warning'>Đã chuyển SO</span> ";
                else if (status.Equals("5"))
                    result = " <span class='label label-success'>Hoàn thành</span> ";
                else
                    result = " <span class='label label-danger'>Không xác định</span> ";
            }
            else if (tableName.Equals("NHAPHANG") || tableName.Equals("DONTRAHANG") || tableName.Equals("TRAHANG") || tableName.Equals("KiemKho"))
            {
                if(language.Equals("1"))
                {
                    if (status.Equals("0"))
                        result = " <span class='label label-info'>Processing</span> ";
                    else if (status.Equals("1"))
                        result = " <span class='label label-success'>Approved</span> ";
                    else
                        result = " <span class='label label-danger'>Cancelled</span> ";
                }
                else if (language.Equals("2"))
                {
                    if (status.Equals("0"))
                        result = " <span class='label label-info'>Đang xử lý</span> ";
                    else if (status.Equals("1"))
                        result = " <span class='label label-success'>Đã duyệt</span> ";
                    else
                        result = " <span class='label label-danger'>Đã hủy</span> ";
                }
            }
            else if (tableName.Equals("MuaHang"))
            {
                if (status.Equals("0"))
                    result = " <span class='label label-info'>Processing</span> ";
                else if (status.Equals("1"))
                    result = " <span class='label label-primary'>Approved</span> ";
                else if (status.Equals("2"))
                    result = " <span class='label label-success'>Cancelled</span> ";
                else if (status.Equals("4"))
                    result = " <span class='label label-success'>Đã nhập</span> ";
                else if (status.Equals("5"))
                    result = " <span class='label label-success'>Hoàn thành</span> ";
            }
            else if (tableName.Equals("DaSuDung"))
            {
                if (language.Equals("1"))
                {
                    if (status.Equals("0"))
                        result = " <span class='label label-info'>Empty</span> ";
                    else if (status.Equals("1"))
                        result = " <span class='label label-primary'>At the supplier</span> ";
                    else if (status.Equals("2"))
                        result = " <span class='label label-warning'>On the way</span> ";
                    else if (status.Equals("3"))
                        result = " <span class='label label-success'>Using</span> ";
                    else
                        result = " <span class='label label-danger'>Unknow</span> ";
                }
                else if (language.Equals("2"))
                {
                    if (status.Equals("0"))
                        result = " <span class='label label-info'>Còn trống</span> ";
                    else if (status.Equals("1"))
                        result = " <span class='label label-primary'>Đang nhà cung cấp</span> ";
                    else if (status.Equals("2"))
                        result = " <span class='label label-warning'>Đang trên đường</span> ";
                    else if (status.Equals("3"))
                        result = " <span class='label label-success'>Đang sử dụng</span> ";
                    else
                        result = " <span class='label label-danger'>Không xác định</span> ";
                }
            }            
            else 
            {
                if(language.Equals("1"))
                {
                    if (status.Equals("0"))
                        result = " <span class='label label-danger'>Off</span> ";
                    else if (status.Equals("1"))
                        result = " <span class='label label-success'>On</span> ";
                    else
                        result = " <span class='label label-danger'>Off</span> ";
                }
                else if (language.Equals("2"))
                {
                    if (status.Equals("0"))
                        result = " <span class='label label-danger'>Ngưng hoạt động</span> ";
                    else if (status.Equals("1"))
                        result = " <span class='label label-success'>Hoạt động</span> ";
                    else
                        result = " <span class='label label-danger'>Ngưng hoạt động</span> ";
                }
            }

            return result;
        }

        public string GetAspect(string location, string aspect)
        {
            
            if (aspect.Equals("0"))
                return "";
            else if (aspect.Equals("1"))
                return "; background-color:yellow";
            else if (aspect.Equals("2"))
                return "; background-color:orange";
            else if (aspect.Equals("3"))
                return "; background-color:greenyellow";

            return "; background-color:red";
        }

        public string getRole_Branch(string userId)
        {
            return " SELECT nhaphanphoi_fk FROM NhanVien_Quyen_NhaPhanPhoi WHERE nhanvien_fk = '" + userId + "'  ";
        }

        public string getRole_Warehouse(string userId)
        {
            return " SELECT kho_fk FROM NhanVien_Quyen_Kho WHERE nhanvien_fk = '" + userId + "'  ";
        }

        public string getRole_Location(string userId)
        {
            return " SELECT location_fk FROM NhanVien_Quyen_Location WHERE nhanvien_fk = '" + userId + "'  ";
        }

        public string GetValue(string search, int pos)
        {
            try
            {
                string[] arr = Regex.Split(search, ";;");

                if (arr[pos].Contains("__"))
                {
                    string[] arr2 = Regex.Split(arr[pos], "__");
                    return arr2[1];
                }
            }
            catch (Exception e)
            {
                return "";
            }

            return "";

        }

        public static DataTable createTable(string tableName, string[] columnName)
        {
            DataTable dt = new DataTable(tableName);

            for (int i = 0; i < columnName.Length; i++)
            {
                dt.Columns.Add(columnName[i], Type.GetType("System.String"));
            }

            return dt;
        }
    }
}