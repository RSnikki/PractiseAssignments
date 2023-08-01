using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace MasterMechLib
{
    public class UserDtl
    {
        public string msUserID;
        public string msPwd;
        public string msUserName;
        public string msMobNo;
        public string msEmailID;
        public string msUserType;

        public DateTime? mdLastLoginTime { get; set; }
        public DateTime? mdLastPwdChangeTime;
        public string msRemarks;
        public DateTime? mdCreated;
        public string msCreatedBy;
        public DateTime? mdModified;
        public string msModifiedBy;
        public string msDeleted;
        public DateTime? mdDeletedOn;
        public string msDeletedBy;

        public UserDtl()
        {

        }

        public UserDtl(string isUserID, string isUserName, string isMobNo, string isEmailID, string isUserType)
        {
            msUserID = isUserID;
            msUserName = isUserName;
            msMobNo = isMobNo;
            msEmailID = isEmailID;
            msUserType = isUserType;
        }

        public bool Load(string isConStr, string isUserID)
        {
            
            try
            {
                using (SqlConnection lObjSqlCon = new SqlConnection(isConStr))
                {
                    string lsQuery = "Select * From UserDtl Where UserID = @ID AND Deleted = 'N'";
                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@ID", SqlDbType.VarChar).Value = isUserID;
                    lObjSqlCon.Open();
                    using (SqlDataReader lObjSDR = lObjCmd.ExecuteReader())
                    {
                        if (lObjSDR.HasRows)
                        {
                            while (lObjSDR.Read())
                            {
                                msUserID = lObjSDR["UserID"].ToString();
                                msPwd = lObjSDR["Pwd"].ToString();
                                msUserName = lObjSDR["UserName"].ToString();
                                msMobNo = DBNull.Value.Equals(lObjSDR["MobNo"])? null : lObjSDR["MobNo"].ToString();
                                msEmailID = DBNull.Value.Equals(lObjSDR["EmailID"]) ? null : lObjSDR["EmailID"].ToString();
                                msUserType = DBNull.Value.Equals(lObjSDR["UserType"]) ? null : lObjSDR["UserType"].ToString();
                                mdLastLoginTime = Convert.ToDateTime(DBNull.Value.Equals(lObjSDR["LastLoginTime"]) ? null : lObjSDR["LastLoginTime"]);
                                mdLastPwdChangeTime = Convert.ToDateTime(DBNull.Value.Equals(lObjSDR["LastPwdChangeTime"]) ? null : lObjSDR["LastPwdChangeTime"]);
                                msRemarks = DBNull.Value.Equals(lObjSDR["Remarks"]) ? null : lObjSDR["Remarks"].ToString();
                                
                                if (DBNull.Value.Equals(lObjSDR["Created"]))
                                {
                                    mdCreated = null;
                                }
                                else
                                {
                                    mdCreated = Convert.ToDateTime(lObjSDR["Created"]);
                                }

                                msCreatedBy = DBNull.Value.Equals(lObjSDR["CreatedBy"]) ? null : lObjSDR["CreatedBy"].ToString();
                                 
                                if (DBNull.Value.Equals(lObjSDR["Modified"]))
                                {
                                    mdModified = null;
                                }
                                else
                                {
                                    mdModified = Convert.ToDateTime(lObjSDR["Modified"]);
                                }
                                msModifiedBy = DBNull.Value.Equals(lObjSDR["ModifiedBy"]) ? null : lObjSDR["ModifiedBy"].ToString();
                                msDeleted = DBNull.Value.Equals(lObjSDR["Deleted"]) ? null : lObjSDR["Deleted"].ToString();
                                
                                if (DBNull.Value.Equals(lObjSDR["DeletedOn"]))
                                {
                                    mdDeletedOn = null;
                                }
                                else
                                {
                                    mdDeletedOn = Convert.ToDateTime(lObjSDR["DeleteOn"]);
                                }

                                msDeletedBy = DBNull.Value.Equals(lObjSDR["DeletedBy"]) ? null : lObjSDR["DeletedBy"].ToString();

                            }
                            return  true;
                        }
                        else
                        {
                            return  false;
                        }
                    }
                }
                
            }
            catch(SqlException ex)
            {
                return  false;
            }
        }

        public bool ValidUserID(string isConStr)
        {
            int lnUserCount = 0;
            try
            {
                using(SqlConnection lObjSqlCon = new SqlConnection(isConStr))
                {
                    string lsQuery = "SELECT Count(*) AS 'UserCount' From UserDtl Where UserId = @Id And Deleted = 'N'";
                    SqlCommand lsObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lsObjCmd.CommandType = CommandType.Text;
                    lsObjCmd.Parameters.AddWithValue("@Id", SqlDbType.VarChar).Value = msUserID;
                    lObjSqlCon.Open();
                    using(SqlDataReader lObjSdr = lsObjCmd.ExecuteReader())
                    {
                        if (lObjSdr.HasRows)
                        {
                            lObjSdr.Read();
                            lnUserCount = Convert.ToInt32(lObjSdr["UserCount"]);
                        }
                        else
                        {
                            lnUserCount = 0;
                        }
                    }

                    lObjSqlCon.Close();
                }
            }
            catch (SqlException ex)
            {
               
            }
            
            if(lnUserCount == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidLogin(string isConStr)
        {
            bool lbValidUser = false;

            try
            {
                using (SqlConnection lObjCon = new SqlConnection(isConStr))
                {
                    string lsQuery = "Select UserType From UserDtl Where UserID = @ID AND Pwd = @Pwd AND DELETED = 'N'";
                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@ID", SqlDbType.VarChar).Value = msUserID;
                    lObjCmd.Parameters.AddWithValue("@Pwd", SqlDbType.VarChar).Value = msPwd;
                    lObjCon.Open();

                    using(SqlDataReader lObjSdr = lObjCmd.ExecuteReader())
                    {
                        if (lObjSdr.HasRows)
                        {
                            while (lObjSdr.Read())
                            {
                                msUserType = DBNull.Value.Equals(lObjSdr["UserType"]) ? null : lObjSdr["UserType"].ToString();
                            }
                            lbValidUser = true;
                        }
                        else
                        {
                            lbValidUser = false;
                        }
                    }

                }
            }
            catch (SqlException ex)
            {
                lbValidUser = false;
            }
            return lbValidUser;
        }

        public bool UpdateLoginTime(string isConStr, string isUserID)
        {
            bool result = false;
            try
            {
                using(SqlConnection lObjSqlCon = new SqlConnection(isConStr))
                {
                    string lsQuery = "Update UserDtl Set LastLoginTime = @LastLogin Where UserID = @ID";
                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@ID", SqlDbType.DateTime).Value = isUserID;
                    lObjCmd.Parameters.AddWithValue("@LastLogin", SqlDbType.DateTime).Value = DateTime.Now;

                    lObjSqlCon.Open();
                    lObjCmd.ExecuteNonQuery();
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool UpdateUserGeneralInfo(string isConStr, string UserID)
        {
            bool result;
            try
            {
                using(SqlConnection lObjSqlCon = new SqlConnection(isConStr))
                {
                    string lsQuery = "Update UserDtl Set UserName = @name, MobNo = @mno, EmailID = @eid, Modified = @modified, ModifiedBy = @modifiedBy Where UserId = @uid And Deleted = 'N'";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@uid", SqlDbType.VarChar).Value = msUserID;
                    lObjCmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = msUserName;
                    lObjCmd.Parameters.AddWithValue("@mno", SqlDbType.VarChar).Value = msMobNo;
                    lObjCmd.Parameters.AddWithValue("@eid", SqlDbType.VarChar).Value = msEmailID;
                    lObjCmd.Parameters.AddWithValue("@modified", SqlDbType.DateTime).Value = DateTime.Now;
                    lObjCmd.Parameters.AddWithValue("@modifiedBy", SqlDbType.VarChar).Value = UserID;
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

        public bool UpdatePassword(string isConStr, string isUserID, string isPWD)
        {
            bool result;

            if (ValidLogin(isConStr))
            {
                try
                {
                    using(SqlConnection lObjSqlCon = new SqlConnection(isConStr))
                    {
                        string lsQuery = "Update UserDtl Set Pwd = @pwd, LastPwdChangeTime = @lastPwdTime Where UserID = @id And Deleted = 'N'";
                        SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                        lObjCmd.CommandType = CommandType.Text;
                        lObjCmd.Parameters.AddWithValue("@id", SqlDbType.VarChar).Value = isUserID;
                        lObjCmd.Parameters.AddWithValue("@pwd", SqlDbType.VarChar).Value = isPWD;
                        lObjCmd.Parameters.AddWithValue("@lastPwdTime", SqlDbType.DateTime).Value = DateTime.Now;

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
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool Save(string isConStr, string isUserID, bool ibNewMode)
        {
            bool result;
            try
            {
                string lsQuery;
                SqlConnection lObjSqlCon = new SqlConnection(isConStr);
                if (ibNewMode == true)
                {
                    lsQuery = "Insert Into UserDtl (UserID, PWD, UserName, MobNo, EmailID, UserType, Remarks,Created, CreatedBy, Deleted) Values (@id, @pwd, @name, @mno, @eid, @utype, @remarks, @createTime, @createBy, 'N')";
                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@id", SqlDbType.VarChar).Value = msUserID;
                    lObjCmd.Parameters.AddWithValue("@pwd", SqlDbType.VarChar).Value = msPwd;
                    lObjCmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = msUserName;
                    lObjCmd.Parameters.AddWithValue("@mno", SqlDbType.VarChar).Value = (object)msMobNo ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@eid", SqlDbType.VarChar).Value = (Object)msEmailID ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@utype", SqlDbType.VarChar).Value = (Object)msUserType ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@remarks", SqlDbType.VarChar).Value = (Object)msRemarks ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@createTime", SqlDbType.DateTime).Value = (Object)DateTime.Now ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@createBy", SqlDbType.VarChar).Value = (Object)isUserID ?? DBNull.Value;

                    lObjSqlCon.Open();
                    lObjCmd.ExecuteNonQuery();
                    lObjSqlCon.Close();
                }
                else
                {
                    lsQuery = "Update UserDtl Set UserName = @name, MobNo = @mno, EmailID = @eid, UserType= @utype, Remarks = @remarks, Modified = @modifyTime, ModifiedBy = @modifyBy  Where UserId = @uid And Deleted = 'N'";
                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@uid", SqlDbType.VarChar).Value = msUserID;
                    lObjCmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = msUserName;
                    lObjCmd.Parameters.AddWithValue("@mno", SqlDbType.VarChar).Value = (object)msMobNo ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@eid", SqlDbType.VarChar).Value = (Object)msEmailID ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@utype", SqlDbType.VarChar).Value = (Object)msUserType ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@remarks", SqlDbType.VarChar).Value = (Object)msRemarks ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@modifyTime", SqlDbType.DateTime).Value = (Object)DateTime.Now ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@modifyBy", SqlDbType.VarChar).Value = (Object)isUserID ?? DBNull.Value;
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

        public bool SearchUser(string isConStr, string isSearchUserID, List<UserDtl> oObjUserDtls)
        {
            bool result;
            try
            {
                using(SqlConnection lObjSqlCon = new SqlConnection(isConStr))
                {
                    string lsQuery = "Select * From UserDtl Where UserId like '" + "%" + isSearchUserID + "%' And Deleted = 'N'";
                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjSqlCon.Open();
                    using (SqlDataReader lObjSDR = lObjCmd.ExecuteReader())
                    {
                        if (lObjSDR.HasRows)
                        {
                            while (lObjSDR.Read())
                            {
                                string lsUserID = lObjSDR["UserId"].ToString();
                                string lsUserName = lObjSDR["UserName"].ToString();
                                string lsMobNo = DBNull.Value.Equals(lObjSDR["MobNo"]) ? null: lObjSDR["MobNo"].ToString();
                                string lsEmailID = DBNull.Value.Equals(lObjSDR["EmailID"]) ? null : lObjSDR["EmailID"].ToString();
                                string lsUserType = DBNull.Value.Equals(lObjSDR["UserType"]) ? null : lObjSDR["UserType"].ToString();
                                
                                DateTime? ldLL = Convert.ToDateTime(DBNull.Value.Equals(lObjSDR["LastLoginTime"]) ? null : lObjSDR["LastLoginTime"]);
                                DateTime? ldLPC = Convert.ToDateTime(DBNull.Value.Equals(lObjSDR["LastPwdChangeTime"]) ? null : lObjSDR["LastPwdChangeTime"]);
                                String lsRemarks = DBNull.Value.Equals(lObjSDR["Remarks"]) ? null : lObjSDR["Remarks"].ToString();
                                DateTime? ldCreated;
                                if (DBNull.Value.Equals(lObjSDR["Created"]))
                                {
                                    ldCreated = null;
                                }
                                else
                                {
                                    ldCreated = Convert.ToDateTime(lObjSDR["Created"]);
                                }
                                String lsCreatedBy = DBNull.Value.Equals(lObjSDR["CreatedBy"]) ? null : lObjSDR["CreatedBy"].ToString();
                                DateTime? ldModified;
                                if (DBNull.Value.Equals(lObjSDR["Modified"]))
                                {
                                    ldModified = null;
                                }
                                else
                                {
                                    ldModified = Convert.ToDateTime(lObjSDR["Modified"]);
                                }
                                String lsModifiedBy = DBNull.Value.Equals(lObjSDR["ModifiedBy"]) ? null : lObjSDR["ModifiedBy"].ToString();

                                UserDtl lObjUser = new UserDtl(lsUserID, lsUserName, lsMobNo, lsEmailID, lsUserType);
                                lObjUser.mdLastLoginTime = ldLL;
                                lObjUser.msRemarks = lsRemarks;
                                lObjUser.mdLastPwdChangeTime = ldLPC;
                                lObjUser.msRemarks = lsRemarks;
                                lObjUser.mdCreated = ldCreated;
                                lObjUser.msCreatedBy = lsCreatedBy;
                                lObjUser.mdModified = ldModified;
                                lObjUser.msModifiedBy = lsModifiedBy;
                                oObjUserDtls.Add(lObjUser);
                            }
                        }
                    }

                    result = true;
                }
            }
            catch (SqlException ex)
            {
                result = false;
            }
            return result;
        }

        public bool Delete(string isConStr, string isUserId)
        {
            bool result;
            try
            {
                using(SqlConnection lObjSqlCon = new SqlConnection(isConStr))
                {
                    string lsQuery = "Update UserDtl Set Deleted = 'Y', DeletedOn = @deleteTime, DeletedBy = @deletedBy Where UserId = @UserId";
                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@deleteTime", SqlDbType.VarChar).Value = DateTime.Now;
                    lObjCmd.Parameters.AddWithValue("@deletedBy", SqlDbType.VarChar).Value = isUserId;
                    lObjCmd.Parameters.AddWithValue("@UserId", SqlDbType.VarChar).Value = msUserID;

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
    }
}
