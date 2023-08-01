using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMechLib
{
    public class Customer
    {
        public int? mnCustomerNo;
        public string msCustomerFName;
        public string msCustomerLName;
        public string msCustomerMNo;
        public string msCustomerEmail;
        public string msCustomerStatus;
        public string msCustomerType;
        public string msCustomerStAddr;
        public string msCustomerArAddr;
        public string msCustomerCity;
        public string msCustomerState;
        public string msCustomerPinCode;
        public string msCustomerCountry;
        public string msCustomerGSTNo;
        public DateTime? mdCustomerLastVisit;
        public string msCustomerRemarks;
        public DateTime? mdCreated;
        public string msCreatedBy;
        public DateTime? mdModified;
        public string msModifiedBy;
        public string msDeleted;
        public DateTime? mdDeletedOn;
        public string msDeletedBy;

       

        public Customer()
        {

        }


        public Customer(string isFName, string isLName, string isMNo, string isEID, string isStatus, string isType)
        {
            msCustomerFName = isFName;
            msCustomerLName = isLName;
            msCustomerMNo = isMNo;
            msCustomerEmail = isEID;
            msCustomerStatus = isStatus;
            msCustomerType = isType;
        }

        public bool Save(string isConString, string isUserId, bool ibMode)
        {
            bool result;
            try
            {
                using(SqlConnection lObjSqlCon = new SqlConnection(isConString))
                {
                    string lsQuery;
                    if (ibMode)
                    {
                        lsQuery = "Insert Into Customer (CustFName, CustLName, CustMobNo, CustEmail, CustSts, CustType, CustStAddr, CustArAddr, CustCity, CustState, CustPinCode, CustCountry, CustGSTNo, CustRemarks, Created, CreatedBy, Deleted) Values (@FName, @LName, @MNo, @Eid, @Status, @Type, @StAddr, @ArAddr, @City, @State, @PinCode, @Country, @GSTNo, @Remarks, @Created, @CreatedBy, 'N')";

                    }
                    else
                    {
                        lsQuery = "Update Customer Set CustFName = @FName, CustLName = @LName, CustMobNo = @MNo, CustEmail = @Eid, CustSts = @Status, CustType = @Type, CustStAddr = @StAddr, CustArAddr = @ArAddr, CustCity = @City, CustState = @State, CustPinCode = @PinCode, CustCountry = @Country, CustGSTNo = @GSTNo, CustRemarks = @Remarks, Modified = @Modified, ModifiedBy = @ModifiedBy Where CustNo = @CustNo";
                    }

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@FName", SqlDbType.VarChar).Value = msCustomerFName;
                    lObjCmd.Parameters.AddWithValue("@LName", SqlDbType.VarChar).Value = msCustomerLName;
                    lObjCmd.Parameters.AddWithValue("@MNo", SqlDbType.VarChar).Value = msCustomerMNo;
                    lObjCmd.Parameters.AddWithValue("@Eid", SqlDbType.VarChar).Value = msCustomerEmail;
                    lObjCmd.Parameters.AddWithValue("@Status", SqlDbType.VarChar).Value = msCustomerStatus;
                    lObjCmd.Parameters.AddWithValue("@Type", SqlDbType.VarChar).Value = msCustomerType;
                    lObjCmd.Parameters.AddWithValue("@StAddr", SqlDbType.VarChar).Value = (object)msCustomerStAddr ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@ArAddr", SqlDbType.VarChar).Value = (object)msCustomerArAddr ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@City", SqlDbType.VarChar).Value = (object)msCustomerCity ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@State", SqlDbType.VarChar).Value = (object)msCustomerState ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@PinCode", SqlDbType.VarChar).Value = (object)msCustomerPinCode ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@Country", SqlDbType.VarChar).Value = (object)msCustomerCountry ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@GSTNo", SqlDbType.VarChar).Value = (object)msCustomerGSTNo ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@Remarks", SqlDbType.VarChar).Value = (object)msCustomerRemarks ?? DBNull.Value;
                    if (ibMode)
                    {
                        lObjCmd.Parameters.AddWithValue("@Created", SqlDbType.DateTime).Value = DateTime.Now; 
                        lObjCmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.VarChar).Value = isUserId;
                    }
                    else
                    {
                        lObjCmd.Parameters.AddWithValue("@CustNo", SqlDbType.Int).Value = mnCustomerNo;
                        lObjCmd.Parameters.AddWithValue("@Modified", SqlDbType.DateTime).Value = DateTime.Now;
                        lObjCmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.VarChar).Value = isUserId;
                    }

                    lObjSqlCon.Open();
                    lObjCmd.ExecuteNonQuery();
                    lObjSqlCon.Close();
                }
                result = true;
            }
            catch(SqlException ex)
            {
                result = false;
            }
            
            return result;
        }

        public bool Save(SqlCommand cmd)
        {
            try
            {
                string lsQuery = "Insert Into Customer(CustFName, CustLName, CustMobNo, CustEmail, CustSts, CustType, CustStAddr, CustArAddr, " +
                    "CustCity, CustState, CustPinCode, CustlLastVisit, CustCountry, CustGSTNo, CustRemarks, Created, CreatedBy, Deleted) OUTPUT INSERTED.CustNo Values" +
                    "(@FName, @LName, @MNo, @Eid, @Status, @Type, @StAddr, @ArAddr, @City, @State, @PinCode, @CLastVisit, @Country, @GSTNo, @Remarks, @Created, @CreatedBy, 'N')";

                cmd.CommandText = lsQuery;

                cmd.Parameters.AddWithValue("@FName", SqlDbType.VarChar).Value = msCustomerFName;
                cmd.Parameters.AddWithValue("@LName", SqlDbType.VarChar).Value = msCustomerLName;
                cmd.Parameters.AddWithValue("@MNo", SqlDbType.VarChar).Value = msCustomerMNo;
                cmd.Parameters.AddWithValue("@Eid", SqlDbType.VarChar).Value = msCustomerEmail;
                cmd.Parameters.AddWithValue("@Status", SqlDbType.VarChar).Value = msCustomerStatus;
                cmd.Parameters.AddWithValue("@Type", SqlDbType.VarChar).Value = msCustomerType;
                cmd.Parameters.AddWithValue("@StAddr", SqlDbType.VarChar).Value = (object)msCustomerStAddr ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@ArAddr", SqlDbType.VarChar).Value = (object)msCustomerArAddr ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@City", SqlDbType.VarChar).Value = (object)msCustomerCity ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@State", SqlDbType.VarChar).Value = (object)msCustomerState ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@PinCode", SqlDbType.VarChar).Value = (object)msCustomerPinCode ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@CLastVisit", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.AddWithValue("@Country", SqlDbType.VarChar).Value = (object)msCustomerCountry ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@GSTNo", SqlDbType.VarChar).Value = (object)msCustomerGSTNo ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@Remarks", SqlDbType.VarChar).Value = (object)msCustomerRemarks ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@Created", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.VarChar).Value = msCreatedBy;
                mnCustomerNo = (int)cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
            
        }

        public bool Delete(string isConString, string isUserId)
        {
            bool result;
            try
            {
                using(SqlConnection lObjSqlCon = new SqlConnection(isConString))
                {
                    string lsQuery = "Update Customer Set Deleted = 'Y', DeletedOn = @DeletedOn, DeletedBy = @DeletedBy Where CustNo = @CNo";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@CNo", SqlDbType.Int).Value = mnCustomerNo;
                    lObjCmd.Parameters.AddWithValue("@DeletedOn", SqlDbType.DateTime).Value = DateTime.Now;
                    lObjCmd.Parameters.AddWithValue("@DeletedBy", SqlDbType.VarChar).Value = isUserId;

                    lObjSqlCon.Open();
                    lObjCmd.ExecuteNonQuery();
                    lObjSqlCon.Close();
                }
                result = true;
            }
            catch (SqlException ex)
            {
                result = false;
            }
            return result;
        }

        public bool UpdateLastVisit(string isConString, int inCNo)
        {
            bool result;
            try
            {
                using (SqlConnection lObjSqlCon = new SqlConnection(isConString))
                {
                    string lsQuery = "Update Customer Set CustlLastVisit = @CLastVisit Where CustNo = @CNo";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@CLastVisit", SqlDbType.DateTime).Value = DateTime.Now;
                    lObjCmd.Parameters.AddWithValue("@CNo", SqlDbType.Int).Value = inCNo;

                    lObjSqlCon.Open();
                    lObjCmd.ExecuteNonQuery();
                    lObjSqlCon.Close();

                }
                result = true;
            }
            catch (SqlException ex)
            {
                result = false;
            }
            return result;
        }

        public bool UpdateLastVisit(SqlCommand cmd)
        {
            try
            {
                string lsQuery = "Update Customer Set CustlLastVisit = @CLastVisit Where CustNo = @CNo";
                cmd.CommandText = lsQuery;
                cmd.Parameters.AddWithValue("@CLastVisit", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.AddWithValue("@CNo", SqlDbType.Int).Value = mnCustomerNo;

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool Load(string isConString, int inCNo)
        {
            bool result;
            try
            {
                using(SqlConnection lObjSqlCon = new SqlConnection(isConString))
                {
                    string lsQuery = "Select CustNo, CustFName, CustLName, CustMobNo, CustEmail, CustSts, CustType, CustStAddr, CustArAddr, CustCity, CustState, CustPinCode, CustCountry, CustGSTNo, CustlLastVisit, CustRemarks, Created, CreatedBy, Modified, ModifiedBy From Customer Where CustNo = @CNo";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@CNo", SqlDbType.Int).Value = inCNo;

                    lObjSqlCon.Open();
                    using (SqlDataReader lObjSDR = lObjCmd.ExecuteReader())
                    {
                        if (lObjSDR.HasRows)
                        {
                            while (lObjSDR.Read())
                            {
                                mnCustomerNo = Convert.ToInt32(lObjSDR["CustNo"]);
                                msCustomerFName = Convert.ToString(lObjSDR["CustFName"]);
                                msCustomerLName = Convert.ToString(lObjSDR["CustLName"]);
                                msCustomerMNo = Convert.ToString(lObjSDR["CustMobNo"]);
                                msCustomerEmail = Convert.ToString(lObjSDR["CustEmail"]);
                                msCustomerStatus = Convert.ToString(lObjSDR["CustSts"]);
                                msCustomerType = Convert.ToString(lObjSDR["CustType"]);
                                msCustomerStAddr =  lObjSDR["CustStAddr"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustStAddr"]);
                                msCustomerArAddr = lObjSDR["CustArAddr"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustArAddr"]);
                                msCustomerCity = lObjSDR["CustCity"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustCity"]);
                                msCustomerState = lObjSDR["CustState"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustState"]);
                                msCustomerPinCode = lObjSDR["CustPinCode"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustPinCode"]);
                                msCustomerCountry = lObjSDR["CustCountry"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustCountry"]);
                                msCustomerGSTNo = lObjSDR["CustGSTNo"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustGSTNo"]);

                                if(lObjSDR["CustlLastVisit"].Equals(DBNull.Value))
                                {
                                    mdCustomerLastVisit = null;
                                }
                                else
                                {
                                    mdCustomerLastVisit = Convert.ToDateTime(lObjSDR["CustlLastVisit"]);
                                }
                                msCustomerRemarks = lObjSDR["CustRemarks"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustRemarks"]);
                                if (lObjSDR["Created"].Equals(DBNull.Value))
                                {
                                    mdCreated = null;
                                }
                                else
                                {
                                    mdCreated = Convert.ToDateTime(lObjSDR["Created"]);
                                }
                                msCreatedBy = lObjSDR["CreatedBy"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CreatedBy"]);
                                if (lObjSDR["Modified"].Equals(DBNull.Value))
                                {
                                    mdModified = null;
                                }
                                else
                                {
                                    mdModified = Convert.ToDateTime(lObjSDR["Modified"]);
                                }
                                msModifiedBy = lObjSDR["ModifiedBy"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["ModifiedBy"]);

                            }
                        }
                    }
                }
                result = true;
            }
            catch (SqlException ex)
            {
                result = false;
            }
            return result;
        }

        public bool Search(string isConString, string isSearchKey, List<Customer> iObjCustsList)
        {
            bool result;
            try
            {
                using(SqlConnection lObjSqlCon = new SqlConnection(isConString))
                {
                    string lsQuery = "Select CustNo, CustFName, CustLName, CustMobNo, CustEmail, CustSts, CustType, CustStAddr, CustArAddr, CustCity, CustState, CustPinCode, CustCountry, CustGSTNo, CustlLastVisit, CustRemarks From Customer Where CustMobNo Like @MobNo And Deleted = 'N'";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);

                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@MobNo", SqlDbType.VarChar).Value = "%" + isSearchKey + "%";

                    lObjSqlCon.Open();

                    using(SqlDataReader lObjSDR = lObjCmd.ExecuteReader())
                    {
                        if (lObjSDR.HasRows)
                        {
                            while (lObjSDR.Read())
                            {
                                int lnCustomerNo = Convert.ToInt32(lObjSDR["CustNo"]);
                                string lsCustomerFName = Convert.ToString(lObjSDR["CustFName"]);
                                string lsCustomerLName = Convert.ToString(lObjSDR["CustLName"]);
                                string lsCustomerMNo = Convert.ToString(lObjSDR["CustMobNo"]);
                                string lsCustomerEmail = Convert.ToString(lObjSDR["CustEmail"]);
                                string lsCustomerStatus = Convert.ToString(lObjSDR["CustSts"]);
                                string lsCustomerType = Convert.ToString(lObjSDR["CustType"]);
                                string lsCustomerStAddr = lObjSDR["CustStAddr"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustStAddr"]);
                                string lsCustomerArAddr = lObjSDR["CustArAddr"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustArAddr"]);
                                string lsCustomerCity = lObjSDR["CustCity"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustCity"]);
                                string lsCustomerState = lObjSDR["CustState"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustState"]);
                                string lsCustomerPinCode = lObjSDR["CustPinCode"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustPinCode"]);
                                string lsCustomerCountry = lObjSDR["CustCountry"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustCountry"]);
                                string lsCustomerGSTNo = lObjSDR["CustGSTNo"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustGSTNo"]);
                                DateTime? ldCustomerLastVisit;
                                if (lObjSDR["CustlLastVisit"].Equals(DBNull.Value))
                                {
                                    ldCustomerLastVisit = null;
                                }
                                else
                                {
                                    ldCustomerLastVisit = Convert.ToDateTime(lObjSDR["CustlLastVisit"]);
                                }
                                string lsCustomerRemarks = lObjSDR["CustRemarks"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustRemarks"]);

                                Customer lObjDummyCust = new Customer(lsCustomerFName, lsCustomerLName, lsCustomerMNo, lsCustomerEmail, lsCustomerStatus, lsCustomerType);
                                lObjDummyCust.msCustomerStAddr = lsCustomerStAddr;
                                lObjDummyCust.msCustomerArAddr = lsCustomerArAddr;
                                lObjDummyCust.msCustomerCity = lsCustomerCity;
                                lObjDummyCust.msCustomerState = lsCustomerState;
                                lObjDummyCust.msCustomerPinCode = lsCustomerPinCode;
                                lObjDummyCust.msCustomerCountry = lsCustomerCountry;
                                lObjDummyCust.msCustomerGSTNo = lsCustomerGSTNo;
                                lObjDummyCust.mdCustomerLastVisit = ldCustomerLastVisit;
                                lObjDummyCust.msCustomerRemarks = lsCustomerRemarks;
                                lObjDummyCust.mnCustomerNo = lnCustomerNo;

                                iObjCustsList.Add(lObjDummyCust);
                            }
                        }
                    }
                }
                result = true;
            }
            catch (SqlException ex)
            {
                result = false;
            }
            return result;
        }

        public bool ShowAll(string isConString, List<Customer> iObjCustsList)
        {
            bool result;
            try
            {
                using (SqlConnection lObjSqlCon = new SqlConnection(isConString))
                {
                    string lsQuery = "Select CustNo, CustFName, CustLName, CustMobNo, CustEmail, CustSts, CustType, CustStAddr, CustArAddr, CustCity, CustState, CustPinCode, CustCountry, CustGSTNo, CustlLastVisit, CustRemarks From Customer Where Deleted = 'N'";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);

                    lObjCmd.CommandType = CommandType.Text;

                    lObjSqlCon.Open();

                    using (SqlDataReader lObjSDR = lObjCmd.ExecuteReader())
                    {
                        if (lObjSDR.HasRows)
                        {
                            while (lObjSDR.Read())
                            {
                                int lnCustomerNo = Convert.ToInt32(lObjSDR["CustNo"]);
                                string lsCustomerFName = Convert.ToString(lObjSDR["CustFName"]);
                                string lsCustomerLName = Convert.ToString(lObjSDR["CustLName"]);
                                string lsCustomerMNo = Convert.ToString(lObjSDR["CustMobNo"]);
                                string lsCustomerEmail = Convert.ToString(lObjSDR["CustEmail"]);
                                string lsCustomerStatus = Convert.ToString(lObjSDR["CustSts"]);
                                string lsCustomerType = Convert.ToString(lObjSDR["CustType"]);
                                string lsCustomerStAddr = lObjSDR["CustStAddr"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustStAddr"]);
                                string lsCustomerArAddr = lObjSDR["CustArAddr"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustArAddr"]);
                                string lsCustomerCity = lObjSDR["CustCity"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustCity"]);
                                string lsCustomerState = lObjSDR["CustState"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustState"]);
                                string lsCustomerPinCode = lObjSDR["CustPinCode"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustPinCode"]);
                                string lsCustomerCountry = lObjSDR["CustCountry"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustCountry"]);
                                string lsCustomerGSTNo = lObjSDR["CustGSTNo"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustGSTNo"]);
                                DateTime? ldCustomerLastVisit;
                                if (lObjSDR["CustlLastVisit"].Equals(DBNull.Value))
                                {
                                    ldCustomerLastVisit = null;
                                }
                                else
                                {
                                    ldCustomerLastVisit = Convert.ToDateTime(lObjSDR["CustlLastVisit"]);
                                }
                                string lsCustomerRemarks = lObjSDR["CustRemarks"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustRemarks"]);

                                Customer lObjDummyCust = new Customer(lsCustomerFName, lsCustomerLName, lsCustomerMNo, lsCustomerEmail, lsCustomerStatus, lsCustomerType);
                                lObjDummyCust.msCustomerStAddr = lsCustomerStAddr;
                                lObjDummyCust.msCustomerArAddr = lsCustomerArAddr;
                                lObjDummyCust.msCustomerCity = lsCustomerCity;
                                lObjDummyCust.msCustomerState = lsCustomerState;
                                lObjDummyCust.msCustomerPinCode = lsCustomerPinCode;
                                lObjDummyCust.msCustomerCountry = lsCustomerCountry;
                                lObjDummyCust.msCustomerGSTNo = lsCustomerGSTNo;
                                lObjDummyCust.mdCustomerLastVisit = ldCustomerLastVisit;
                                lObjDummyCust.msCustomerRemarks = lsCustomerRemarks;
                                lObjDummyCust.mnCustomerNo = lnCustomerNo;
                                iObjCustsList.Add(lObjDummyCust);
                            }
                        }
                    }
                }
                result = true;
            }
            catch (SqlException ex)
            {
                result = false;
            }
            return result;
        }

        public static void  GetStatus(List<string> lObjSts)
        {
            lObjSts.Add("A");
            lObjSts.Add("P");
            lObjSts.Add("B");
        }

        public static void GetTypes(List<String> lObjType)
        {
            lObjType.Add("IND");
            lObjType.Add("BUS");
        }

        public bool AdvancedSearch(string isConString, string isFNameKey, string isLNameKey, string isCity, List<Customer> iObjCustsList)
        {
            bool result;
            try
            {
                using (SqlConnection lObjSqlCon = new SqlConnection(isConString))
                {
                    string lsQuery = "Select CustNo, CustFName, CustLName, CustMobNo, CustEmail, CustSts, CustType, CustStAddr, CustArAddr, CustCity, CustState, CustPinCode, CustCountry, CustGSTNo, CustlLastVisit, CustRemarks From Customer Where CustFName Like @FName And CustLName Like @LName And CustCity Like @City And Deleted = 'N'";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@FName", SqlDbType.VarChar).Value = "%" + isFNameKey + "%";
                    lObjCmd.Parameters.AddWithValue("@LName", SqlDbType.VarChar).Value = "%" + isLNameKey + "%";
                    lObjCmd.Parameters.AddWithValue("@City", SqlDbType.VarChar).Value = "%" + isCity + "%";

                    lObjSqlCon.Open();

                    using (SqlDataReader lObjSDR = lObjCmd.ExecuteReader())
                    {
                        if (lObjSDR.HasRows)
                        {
                            while (lObjSDR.Read())
                            {
                                int lnCustomerNo = Convert.ToInt32(lObjSDR["CustNo"]);
                                string lsCustomerFName = Convert.ToString(lObjSDR["CustFName"]);
                                string lsCustomerLName = Convert.ToString(lObjSDR["CustLName"]);
                                string lsCustomerMNo = Convert.ToString(lObjSDR["CustMobNo"]);
                                string lsCustomerEmail = Convert.ToString(lObjSDR["CustEmail"]);
                                string lsCustomerStatus = Convert.ToString(lObjSDR["CustSts"]);
                                string lsCustomerType = Convert.ToString(lObjSDR["CustType"]);
                                string lsCustomerStAddr = lObjSDR["CustStAddr"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustStAddr"]);
                                string lsCustomerArAddr = lObjSDR["CustArAddr"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustArAddr"]);
                                string lsCustomerCity = lObjSDR["CustCity"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustCity"]);
                                string lsCustomerState = lObjSDR["CustState"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustState"]);
                                string lsCustomerPinCode = lObjSDR["CustPinCode"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustPinCode"]);
                                string lsCustomerCountry = lObjSDR["CustCountry"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustCountry"]);
                                string lsCustomerGSTNo = lObjSDR["CustGSTNo"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustGSTNo"]);
                                DateTime? ldCustomerLastVisit;
                                if (lObjSDR["CustlLastVisit"] == (DBNull.Value))
                                {
                                    ldCustomerLastVisit = null;
                                }
                                else
                                {
                                    ldCustomerLastVisit = Convert.ToDateTime(lObjSDR["CustlLastVisit"]);
                                }
                                string lsCustomerRemarks = lObjSDR["CustRemarks"].Equals(DBNull.Value) ? null : Convert.ToString(lObjSDR["CustRemarks"]);

                                Customer lObjDummyCust = new Customer(lsCustomerFName, lsCustomerLName, lsCustomerMNo, lsCustomerEmail, lsCustomerStatus, lsCustomerType);
                                lObjDummyCust.msCustomerStAddr = lsCustomerStAddr;
                                lObjDummyCust.msCustomerArAddr = lsCustomerArAddr;
                                lObjDummyCust.msCustomerCity = lsCustomerCity;
                                lObjDummyCust.msCustomerState = lsCustomerState;
                                lObjDummyCust.msCustomerPinCode = lsCustomerPinCode;
                                lObjDummyCust.msCustomerCountry = lsCustomerCountry;
                                lObjDummyCust.msCustomerGSTNo = lsCustomerGSTNo;
                                lObjDummyCust.mdCustomerLastVisit = ldCustomerLastVisit;
                                lObjDummyCust.msCustomerRemarks = lsCustomerRemarks;
                                lObjDummyCust.mnCustomerNo = lnCustomerNo;

                                iObjCustsList.Add(lObjDummyCust);
                            }
                        }
                    }
                }

                result = true;
            }
            catch (SqlException ex)
            {
                result = false;
            }
            return result;
        }
    }
}
