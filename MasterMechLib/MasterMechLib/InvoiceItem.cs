using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMechLib
{
    public class InvoiceItem
    {
        public int? mnInvoiceItemSNo;
        public int mnInvoiceSNo;
        public int mnItemNo;
        public string msItemDesc;
        public string msItemType;
        public string msItemCatg;
        public double mnItemPrice;
        public string msItemUOM;
        public string msItemSts;
        public double? mnCGSTRate;
        public double? mnSGSTRate;
        public double? mnIGSTRate;
        public string msUPCCode;
        public string msHSNCode;
        public string msSACCode;
        public double? mnQty;
        public double? mnCGSTAmount;
        public double? mnSGSTAmount;
        public double? mnIGSTAmount;
        public double? mnDiscountAmount;
        public double? mnTotalAmount;
        public DateTime? mdCreated;
        public string msCreatedBy;
        public DateTime? mdModified;
        public string msModifiedBy;
        public string msDeleted;
        public DateTime? mdDeletedOn;
        public string msDeletedBy;

        MasterMechUtil.OPMode LineMode;

        public InvoiceItem()
        {

        }

        public bool Save(SqlCommand cmd)
        {
            string lsQuery;
            try
            {
                if(mnInvoiceItemSNo is null)
                {
                    lsQuery = "Insert Into [InvoiceItem" + MasterMechUtil.sFY + "]" +
                        " (InvoiceSNo, ItemNo, ItemDesc, ItemType, ItemCatg, ItemPrice, ItemUOM, ItemSts, CGSTRate, SGSTRate, IGSTRate," +
                        " UPCCode, HSNCode, SACCode, Qty, CGSTAmount, SGSTAmount, IGSTAmount, DiscountAmount, TotalAmount, Created," +
                        " CreatedBy, Deleted) Values " +
                        "(@InvoiceSNo, @ItemNo, @ItemDesc, @ItemType, @ItemCatg, @ItemPrice, @ItemUOM, @ItemSts, @CGSTRate, @SGSTRate," +
                        " @IGSTRate, @UPCCode, @HSNCode, @SACCode, @Qty, @CGSTAmount, @SGSTAmount, @IGSTAmount, @DiscountAmount," +
                        " @TotalAmount, @Created, @CreatedBy, @Deleted)";
                }
                else //if((mnInvoiceItemSNo != null && LineMode == MasterMechUtil.OPMode.Open)
                {
                    lsQuery = "Update [InvoiceItem" + MasterMechUtil.sFY + "]" +
                        " Set ItemNo = @ItemNo, ItemDesc = @ItemDesc, ItemType = @ItemType, ItemCatg = @ItemCatg," +
                        " ItemPrice = @ItemPrice, ItemUOM = @ItemUOM, ItemSts = @ItemSts, CGSTRate = @CGSTRate," +
                        " SGSTRate = @SGSTRate, IGSTRate = @IGSTRate, UPCCode = @UPCCode, HSNCode = @HSNCode," +
                        " SACCode = @SACCode, Qty = @Qty, CGSTAmount = @CGSTAmount, SGSTAmount = @SGSTAmount," +
                        " IGSTAmount = @IGSTAmount, DiscountAmount = @DiscountAmount, TotalAmount = @TotalAmount," +
                        " Modified = @Modified, ModifiedBy = @ModifiedBy Where InvoiceSNo = @InvoiceSNo And InvoiceItemSNo = @InvoiceItemSNo";
                }

                cmd.CommandText = lsQuery;
                cmd.CommandType = CommandType.Text;

                
                cmd.Parameters.AddWithValue("@ItemNo", SqlDbType.Int).Value = mnItemNo;
                cmd.Parameters.AddWithValue("@ItemDesc", SqlDbType.VarChar).Value = msItemDesc;
                cmd.Parameters.AddWithValue("@ItemType", SqlDbType.VarChar).Value = msItemType;
                cmd.Parameters.AddWithValue("@ItemCatg", SqlDbType.VarChar).Value = msItemCatg;
                cmd.Parameters.AddWithValue("@ItemPrice", SqlDbType.Float).Value = (object)mnItemPrice ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@ItemUOM", SqlDbType.VarChar).Value = (object)msItemUOM ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@ItemSts", SqlDbType.VarChar).Value = (object)msItemSts ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@CGSTRate", SqlDbType.Float).Value = (object)mnCGSTRate ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@SGSTRate", SqlDbType.Float).Value = (object)mnSGSTRate ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@IGSTRate", SqlDbType.Float).Value = (object)mnIGSTRate ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@UPCCode", SqlDbType.VarChar).Value = (object)msUPCCode ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@HSNCode", SqlDbType.VarChar).Value = (object)msHSNCode ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@SACCode", SqlDbType.VarChar).Value = (object)msSACCode ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@Qty", SqlDbType.Float).Value = (object)mnQty ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@CGSTAmount", SqlDbType.Float).Value = (object)mnCGSTAmount ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@SGSTAmount", SqlDbType.Float).Value = (object)mnSGSTAmount ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@IGSTAmount", SqlDbType.Float).Value = (object)mnIGSTAmount ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@DiscountAmount", SqlDbType.Float).Value = (object)mnDiscountAmount ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@TotalAmount", SqlDbType.Float).Value = (object)mnTotalAmount ?? DBNull.Value;
                
                if (mnInvoiceItemSNo is null)
                {
                    cmd.Parameters.AddWithValue("@InvoiceSNo", SqlDbType.Int).Value = mnInvoiceSNo;
                    cmd.Parameters.AddWithValue("@Created", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.AddWithValue("@Deleted", SqlDbType.VarChar).Value = "N";
                    cmd.Parameters.AddWithValue("CreatedBy", SqlDbType.VarChar).Value = msCreatedBy;
                }
                else
                {
                    cmd.Parameters.AddWithValue("@InvoiceSNo", SqlDbType.Int).Value = mnInvoiceSNo;
                    cmd.Parameters.AddWithValue("@InvoiceItemSNo", SqlDbType.Int).Value = mnInvoiceItemSNo;
                    cmd.Parameters.AddWithValue("Modified", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.AddWithValue("ModifiedBy", SqlDbType.VarChar).Value = msModifiedBy;
                }

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return true;
            }
            catch (SqlException ex)
            {
                //MasterMechUtil.ShowError(ex.Message);
                return false;
            }
        }

        public bool Delete(SqlCommand cmd)
        {
            string lsQuery;
            try
            {
                lsQuery = "Update [InvoiceItem" + MasterMechUtil.sFY + "]" +
                    " Set Deleted = 'Y', DeletedOn = @DeletedOn, DeletedBy = @DeletedBy Where InvoiceSNo = @InvoiceSNo AND Deleted ='N'";
                cmd.CommandText = lsQuery;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@InvoiceSNo", SqlDbType.Int).Value = mnInvoiceSNo;
                cmd.Parameters.AddWithValue("@DeletedOn", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.AddWithValue("@DeletedBy", SqlDbType.VarChar).Value = msDeletedBy;

                cmd.ExecuteNonQuery();
                
                cmd.Parameters.Clear();
                return true;
            }
            catch (SqlException ex)
            {
                //MasterMechUtil.ShowError(ex.Message);
                return false;
            }
        }

        public bool Delete(string isConString, string isUserId)
        {
            string lsQuery;
            try
            {
                using (SqlConnection lObjSqlCon = new SqlConnection(isConString))
                {
                    lsQuery = "Update [InvoiceItem" + MasterMechUtil.sFY + "]" +
                        " Set Deleted = 'Y', DeletedOn = @DeletedOn, DeletedBy = @DeletedBy Where InvoiceItemSNo = @InvoiceItemSNo AND Deleted = 'N'";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@DeletedOn", SqlDbType.DateTime).Value = DateTime.Now;
                    lObjCmd.Parameters.AddWithValue("@DeletedBy", SqlDbType.VarChar).Value = isUserId;
                    lObjCmd.Parameters.AddWithValue("@InvoiceItemSNo", SqlDbType.Int).Value = mnInvoiceItemSNo;
                    lObjSqlCon.Open();
                    lObjCmd.ExecuteNonQuery();
                    lObjSqlCon.Close();
                }
                return true;
            }
            catch (SqlException ex)
            {
                //MasterMechUtil.ShowError(ex.Message);
                return false;
            }
        }

        public bool Load(string isConString, string isUserId, int inInvoiceItemSNo)
        {
            string lsQuery;
            try
            {
                using (SqlConnection lObjSqlCon = new SqlConnection(isConString))
                {
                    lsQuery = "Select InvoiceItemSNo, InvoiceSNo, ItemNo, ItemDesc, ItemType, ItemCatg, ItemPrice, ItemUOM, ItemSts, CGSTRate, SGSTRate, IGSTRate, UPCCode, HSNCode, SACCode, Qty, SGSTAmount, CGSTAmount, IGSTAmount, DiscountAmount, TotalAmount, Created, CreatedBy, Modified, ModifiedBy From [InvoiceItem" + MasterMechUtil.sFY + "]" +
                        " Where InvoiceItemSNo = @InvoiceItemSNo AND Deleted = 'N'";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@InvoiceItemSNo", SqlDbType.Int).Value = inInvoiceItemSNo;

                    lObjSqlCon.Open();
                    using(SqlDataReader lObjSDR = lObjCmd.ExecuteReader())
                    {
                        if (lObjSDR.HasRows)
                        {
                            while (lObjSDR.Read())
                            {
                                mnInvoiceSNo = Convert.ToInt32(lObjSDR["InvoiceSNo"]);
                                mnItemNo = Convert.ToInt32(lObjSDR["ItemNo"]);
                                msItemDesc = Convert.ToString(lObjSDR["ItemDesc"]);
                                msItemType = Convert.ToString(lObjSDR["ItemType"]);
                                msItemCatg = Convert.ToString(lObjSDR["ItemCatg"]);
                                mnItemPrice = Convert.ToDouble(lObjSDR["ItemPrice"]);
                                if (lObjSDR["ItemUOM"].Equals(DBNull.Value))
                                {
                                    msItemUOM = null;
                                }
                                else
                                {
                                    msItemUOM = Convert.ToString(lObjSDR["ItemUOM"]);
                                }
                                if (lObjSDR["ItemSts"] == DBNull.Value)
                                {
                                    msItemSts = null;
                                }
                                else
                                {
                                    msItemSts = Convert.ToString(lObjSDR["ItemSts"]);
                                }
                                if (lObjSDR["CGSTRate"] == DBNull.Value)
                                {
                                    mnCGSTRate = null;
                                }
                                else
                                {
                                    mnCGSTRate = Convert.ToDouble(lObjSDR["CGSTRate"]);
                                }
                                if (lObjSDR["SGSTRate"] == DBNull.Value)
                                {
                                    mnSGSTRate = null;
                                }
                                else
                                {
                                    mnSGSTRate = Convert.ToDouble(lObjSDR["SGSTRate"]);
                                }
                                if (lObjSDR["IGSTRate"] == DBNull.Value)
                                {
                                    mnIGSTRate = null;
                                }
                                else
                                {
                                    mnIGSTRate = Convert.ToDouble(lObjSDR["IGSTRate"]);
                                }
                                if (lObjSDR["UPCCode"] == DBNull.Value)
                                {
                                    msUPCCode = null;
                                }
                                else
                                {
                                    msUPCCode = Convert.ToString(lObjSDR["UPCCode"]);
                                }
                                if (lObjSDR["HSNCode"] == DBNull.Value)
                                {
                                    msHSNCode = null;
                                }
                                else
                                {
                                    msHSNCode = Convert.ToString(lObjSDR["HSNCode"]);
                                }
                                if (lObjSDR["SACCode"] == DBNull.Value)
                                {
                                    msSACCode = null;
                                }
                                else
                                {
                                    msSACCode = Convert.ToString(lObjSDR["SACCode"]);
                                }
                                if (lObjSDR["Qty"] == DBNull.Value)
                                {
                                    mnQty = null;
                                }
                                else
                                {
                                    mnQty = Convert.ToDouble(lObjSDR["Qty"]);
                                }
                                if (lObjSDR["SGSTAmount"] == DBNull.Value)
                                {
                                    mnSGSTAmount = null;
                                }
                                else
                                {
                                    mnSGSTAmount = Convert.ToDouble(lObjSDR["SGSTAmount"]);
                                }
                                if (lObjSDR["CGSTAmount"] == DBNull.Value)
                                {
                                    mnCGSTAmount = null;
                                }
                                else
                                {
                                    mnCGSTAmount = Convert.ToDouble(lObjSDR["CGSTAmount"]);
                                }
                                if (lObjSDR["IGSTAmount"] == DBNull.Value)
                                {
                                    mnIGSTAmount = null;
                                }
                                else
                                {
                                    mnIGSTAmount = Convert.ToDouble(lObjSDR["IGSTAmount"]);
                                }
                                if (lObjSDR["DiscountAmount"] == DBNull.Value)
                                {
                                    mnDiscountAmount = null;
                                }
                                else
                                {
                                    mnDiscountAmount = Convert.ToDouble(lObjSDR["DiscountAmount"]);
                                }
                                if (lObjSDR["TotalAmount"] == DBNull.Value)
                                {
                                    mnTotalAmount = null;
                                }
                                else
                                {
                                    mnTotalAmount = Convert.ToDouble(lObjSDR["TotalAmount"]);
                                }
                                if (lObjSDR["Created"] == DBNull.Value)
                                {
                                    mdCreated = null;
                                }
                                else
                                {
                                    mdCreated = Convert.ToDateTime(lObjSDR["Created"]);
                                }
                                if (lObjSDR["CreatedBy"] == DBNull.Value)
                                {
                                    msCreatedBy = null;
                                }
                                else
                                {
                                    msCreatedBy = Convert.ToString(lObjSDR["ModifiedBy"]);
                                }
                                if (lObjSDR["Modified"] == DBNull.Value)
                                {
                                    mdModified = null;
                                }
                                else
                                {
                                    mdModified = Convert.ToDateTime(lObjSDR["Modified"]);
                                }
                                if (lObjSDR["ModifiedBy"] == DBNull.Value)
                                {
                                    msModifiedBy = null;
                                }
                                else
                                {
                                    msModifiedBy = Convert.ToString(lObjSDR["ModifiedBy"]);
                                }
                            }
                        }
                        

                    }
                    lObjSqlCon.Close(); 
                }
                return true;
            }
            catch (SqlException ex)
            {
                //MasterMechUtil.ShowError(ex.Message);
                return false;
            }
        }

        public bool SearchInvoiceItems(string isConString, int inInvoiceSNo, List<InvoiceItem> iListInvoiceItems)
        {
            string lsQuery;
            try
            {
                using(SqlConnection lObjSqlCon = new SqlConnection(isConString))
                {
                    lsQuery = "Select InvoiceSNo, InvoiceItemSNo, ItemDesc, ItemType, ItemCatg, ItemPrice, Qty, SGSTAmount, CGSTAmount, IGSTAmount, DiscountAmount, TotalAmount	 From [InvoiceItem" + MasterMechUtil.sFY + "]" + 
                        " Where InvoiceSNo = @InvoiceSNo And Deleted = 'N'";

                    SqlCommand lObjCmd = new SqlCommand(isConString);
                    lObjCmd.Connection = lObjSqlCon;
                    lObjCmd.CommandText = lsQuery;
                    lObjCmd.Parameters.AddWithValue("@InvoiceSNo", SqlDbType.Int).Value = inInvoiceSNo;
                    lObjSqlCon.Open();
                    using(SqlDataReader lObjSDR = lObjCmd.ExecuteReader())
                    {
                        if (lObjSDR.HasRows)
                        {
                            while (lObjSDR.Read())
                            {
                                InvoiceItem lObjInvoiceItem = new InvoiceItem();
                                lObjInvoiceItem.mnInvoiceSNo = Convert.ToInt32(lObjSDR["InvoiceSNo"]);
                                lObjInvoiceItem.mnInvoiceItemSNo = Convert.ToInt32(lObjSDR["InvoiceItemSNo"]);
                                lObjInvoiceItem.msItemDesc = Convert.ToString(lObjSDR["ItemDesc"]);
                                lObjInvoiceItem.msItemType = Convert.ToString(lObjSDR["ItemType"]);
                                lObjInvoiceItem.msItemCatg = Convert.ToString(lObjSDR["ItemCatg"]);
                                lObjInvoiceItem.mnItemPrice = Convert.ToDouble(lObjSDR["ItemPrice"]);
                                if(lObjSDR["Qty"] == DBNull.Value)
                                {
                                    lObjInvoiceItem.mnQty = null;
                                }
                                else
                                {
                                    lObjInvoiceItem.mnQty = Convert.ToDouble(lObjSDR["Qty"]);
                                }
                                if (lObjSDR["SGSTAmount"] == DBNull.Value)
                                {
                                    lObjInvoiceItem.mnSGSTAmount = null;
                                }
                                else
                                {
                                    lObjInvoiceItem.mnSGSTAmount = Convert.ToDouble(lObjSDR["SGSTAmount"]);
                                }
                                if (lObjSDR["CGSTAmount"] == DBNull.Value)
                                {
                                    lObjInvoiceItem.mnCGSTAmount = null;
                                }
                                else
                                {
                                    lObjInvoiceItem.mnCGSTAmount = Convert.ToDouble(lObjSDR["CGSTAmount"]);
                                }
                                if (lObjSDR["IGSTAmount"] == DBNull.Value)
                                {
                                    lObjInvoiceItem.mnIGSTAmount = null;
                                }
                                else
                                {
                                    lObjInvoiceItem.mnIGSTAmount = Convert.ToDouble(lObjSDR["IGSTAmount"]);
                                }
                                if (lObjSDR["DiscountAmount"] == DBNull.Value)
                                {
                                    lObjInvoiceItem.mnDiscountAmount = null;
                                }
                                else
                                {
                                    lObjInvoiceItem.mnDiscountAmount = Convert.ToDouble(lObjSDR["DiscountAmount"]);
                                }
                                if (lObjSDR["TotalAmount"] == DBNull.Value)
                                {
                                    lObjInvoiceItem.mnTotalAmount = null;
                                }
                                else
                                {
                                    lObjInvoiceItem.mnTotalAmount = Convert.ToDouble(lObjSDR["TotalAmount"]);
                                }

                                iListInvoiceItems.Add(lObjInvoiceItem);
                            }
                        }
                    }
                    lObjSqlCon.Close();
                }
                
                return true;
            }
            catch (SqlException ex)
            {
                //MasterMechUtil.ShowError(ex.Message);
                return false;
            }
        }
    }
}
