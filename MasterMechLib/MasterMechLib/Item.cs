using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace MasterMechLib
{
    public class Item
    {
        public int mnItemNo;
        public string msItemDesc;
        public string msItemType;
        public string msItemCategory;
        public double mnItemPrice;
        public string msUOM;
        public string msStatus;
        public double? mnCGST;
        public double? mnSGST;
        public double? mnIGST;
        public string msUPC;
        public string msHSN;
        public string msSAC;
        public double? mnQuantityInHand;
        public int? mnNoOfParts;
        public double? mnReOrderQty;
        public double? mnReOrderLevel;
        public string msRemarks;
        public DateTime? mdCreated;
        public string msCreatedBy;
        public DateTime? mdModified;
        public string msModifiedBy;
        public string msDeleted;
        public DateTime? mdDeletedOn;
        public string msDeletedBy;

        public static List<string> itemType = new List<string>();
        public static List<string> itemCategories = new List<string>();
        public static List<string> itemStatus = new List<string>();

        public Item()
        {


        }

        public Item(string isItemDesc, string isItemType, string isItemCategory, double inItemPrice, string isUOM, string isStatus)
        {
            msItemDesc = isItemDesc;
            msItemType = isItemType;
            msItemCategory = isItemCategory;
            mnItemPrice = inItemPrice;
            msUOM = isUOM;
            msStatus = isStatus;
        }

        public bool Save(string isConStr, string isUserId, bool ibNewMode)
        {
            bool result;
            string lsQuery;
            try
            {
                using(SqlConnection lObjSqlCon = new SqlConnection(isConStr))
                {
                    if(ibNewMode == true)
                    {
                        lsQuery = "Insert into Item (ItemDesc, ItemType, ItemCategory, ItemPrice, ItemUOM, ItemStatus, CGSTRate, SGSTRate, IGSTRate, UPCCode, HSNCode, SACCode, QuantityInHand, NoOfParts, ReOrderQty, ReOrderLevel, ItemRemarks, Created, CreatedBy, Deleted) values (@Desc, @Type, @Category, @Price, @UOM, @Status, @CGST, @SGST, @IGST, @UPC, @HSN, @SAC, @QtyHand, @NoOfParts, @ReorderQty, @ReorderLevel, @Remarks, @Created, @CreatedBy, 'N')";
                    }
                    else
                    {
                        lsQuery = "Update Item Set ItemDesc = @Desc, ItemType = @Type, ItemCategory = @Category, ItemPrice = @Price, ItemUOM = @UOM, ItemStatus = @Status, CGSTRate = @CGST, SGSTRate = @SGST, IGSTRate = @IGST, UPCCode = @UPC, HSNCode = @HSN, SACCode = @SAC, QuantityInHand = @QtyHand, NoOfParts = @NoOfParts, ReOrderQty = @ReorderQty, ReOrderLevel = @ReorderLevel, ItemRemarks = @Remarks, Modified = @Modified, ModifiedBy = @ModifiedBy Where ItemNo = @No";
                    }
                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@Desc", SqlDbType.VarChar).Value = msItemDesc;
                    lObjCmd.Parameters.AddWithValue("@Type", SqlDbType.VarChar).Value = msItemType;
                    lObjCmd.Parameters.AddWithValue("@Category", SqlDbType.VarChar).Value = msItemCategory;
                    lObjCmd.Parameters.AddWithValue("@Price", SqlDbType.Float).Value = mnItemPrice;
                    lObjCmd.Parameters.AddWithValue("@UOM", SqlDbType.VarChar).Value = msUOM;
                    lObjCmd.Parameters.AddWithValue("@Status", SqlDbType.VarChar).Value = msStatus;
                    lObjCmd.Parameters.AddWithValue("@CGST", SqlDbType.Float).Value = (object)mnCGST ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@SGST", SqlDbType.Float).Value = (object)mnSGST ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@IGST", SqlDbType.Float).Value = (object)mnIGST ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@UPC", SqlDbType.VarChar).Value = (object)msUPC ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@HSN", SqlDbType.VarChar).Value = (object)msHSN ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@SAC", SqlDbType.VarChar).Value = (object)msSAC ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@QtyHand", SqlDbType.Float).Value = (object)mnQuantityInHand ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@NoOfParts", SqlDbType.Int).Value = (object)mnNoOfParts ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@ReorderQty", SqlDbType.Float).Value = (object)mnReOrderQty ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@ReorderLevel", SqlDbType.Float).Value = (object)mnReOrderLevel ?? DBNull.Value;
                    lObjCmd.Parameters.AddWithValue("@Remarks", SqlDbType.VarChar).Value = (object)msRemarks ?? DBNull.Value;

                    if (ibNewMode == true)
                    {
                        lObjCmd.Parameters.AddWithValue("@Created", SqlDbType.DateTime).Value = DateTime.Now;
                        lObjCmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.VarChar).Value = isUserId;
                    }
                    else
                    {
                        lObjCmd.Parameters.AddWithValue("@No", SqlDbType.Int).Value = mnItemNo;
                        lObjCmd.Parameters.AddWithValue("@Modified", SqlDbType.DateTime).Value = DateTime.Now;
                        lObjCmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.VarChar).Value = isUserId;
                    }

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

        public bool Delete(string isConStr, string isUserId)
        {
            bool result;
            try
            {
                using(SqlConnection lObjSqlCon = new SqlConnection(isConStr))
                {
                    string lsQuery = "Update Item Set Deleted = 'Y', DeletedOn = @DeletedOn, DeletedBy = @DeletedBy Where ItemNo = @No";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@No", SqlDbType.Int).Value = mnItemNo;
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

        public bool SearchItems(string isConStr, string isSearchKey, List<Item> ilItemList)
        {
            bool result;
            try
            {
                using (SqlConnection lObjSqlCon = new SqlConnection(isConStr))
                {
                    string lsQuery = "Select ItemNo, ItemDesc, ItemType, ItemCategory, ItemPrice, ItemUOM, ItemStatus, CGSTRate, SGSTRate, IGSTRate, UPCCode, HSNCode, SACCode, QuantityInHand, NoOfParts, ReOrderQty, ReOrderLevel, ItemRemarks From Item Where ItemDesc Like '"+ "%" + isSearchKey + "%' And Deleted = 'N'";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;

                    lObjSqlCon.Open();
                    using(SqlDataReader lObjSDR = lObjCmd.ExecuteReader())
                    {
                        if (lObjSDR.HasRows)
                        {
                            while (lObjSDR.Read())
                            {
                                int lnItemNo = Convert.ToInt32(lObjSDR["ItemNo"]);
                                string lsItemDesc = Convert.ToString(lObjSDR["ItemDesc"]);
                                string lsItemType = Convert.ToString(lObjSDR["ItemType"].ToString());
                                string lsItemCategory = Convert.ToString(lObjSDR["ItemCategory"].ToString());
                                double lnItemPrice = Convert.ToDouble(lObjSDR["ItemPrice"]);
                                string lsUOM = Convert.ToString(lObjSDR["ItemUOM"]);
                                string lsStatus = Convert.ToString(lObjSDR["ItemStatus"]);
                                double lnCGST = Convert.ToDouble(lObjSDR["CGSTRate"].Equals(DBNull.Value) ? null : lObjSDR["CGSTRate"]);
                                double lnSGST = Convert.ToDouble(lObjSDR["SGSTRate"].Equals(DBNull.Value) ? null : lObjSDR["SGSTRate"]);
                                double lnIGST = Convert.ToDouble(lObjSDR["IGSTRate"].Equals(DBNull.Value) ? null : lObjSDR["IGSTRate"]);
                                string lsUPC = Convert.ToString(lObjSDR["UPCCode"].Equals(DBNull.Value) ? null : lObjSDR["UPCCode"]);
                                string lsHSN = Convert.ToString(lObjSDR["HSNCode"].Equals(DBNull.Value) ? null : lObjSDR["HSNCode"]);
                                string lsSAC = Convert.ToString(lObjSDR["SACCode"].Equals(DBNull.Value) ? null : lObjSDR["SACCode"]);
                                double lnQuantityInHand = Convert.ToDouble(lObjSDR["QuantityInHand"].Equals(DBNull.Value) ? null : lObjSDR["QuantityInHand"]);
                                int? lnNoOfParts;
                                if (lObjSDR["NoOfParts"].Equals(DBNull.Value))
                                {
                                    lnNoOfParts = null;
                                }
                                else
                                {
                                    lnNoOfParts = Convert.ToInt32(lObjSDR["NoOfParts"]);
                                }
                                //int lnNoOfParts = Convert.ToInt32(lObjSDR["NoOfParts"].Equals(DBNull.Value) ? null : lObjSDR["NoOfParts"]);
                                double lnReOrderQty = Convert.ToDouble(lObjSDR["ReOrderQty"].Equals(DBNull.Value) ? null : lObjSDR["ReOrderQty"]);
                                double lnReOrderLevel = Convert.ToDouble(lObjSDR["ReOrderLevel"].Equals(DBNull.Value) ? null : lObjSDR["ReOrderLevel"]);
                                string lsRemarks = Convert.ToString(lObjSDR["ItemRemarks"].Equals(DBNull.Value) ? null : lObjSDR["ItemRemarks"]);

                                Item lObjDummyItem = new Item(lsItemDesc, lsItemType, lsItemCategory, lnItemPrice, lsUOM, lsStatus);

                                lObjDummyItem.mnItemNo = lnItemNo;
                                lObjDummyItem.mnCGST = lnCGST;
                                lObjDummyItem.mnSGST = lnSGST;
                                lObjDummyItem.mnIGST = lnIGST;
                                lObjDummyItem.msUPC = lsUPC;
                                lObjDummyItem.msHSN = lsHSN;
                                lObjDummyItem.msSAC = lsSAC;
                                lObjDummyItem.mnQuantityInHand = lnQuantityInHand;
                                lObjDummyItem.mnNoOfParts = lnNoOfParts;
                                lObjDummyItem.mnReOrderQty = lnReOrderQty;
                                lObjDummyItem.mnReOrderLevel = lnReOrderLevel;
                                lObjDummyItem.msRemarks = lsRemarks;

                                ilItemList.Add(lObjDummyItem);

                            }
                        }
                    }
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
        
        public bool Load(string isConStr, int inItemNo)
        {
            bool result;
            try
            {
                using(SqlConnection lObjSqlCon = new SqlConnection(isConStr))
                {
                    string lsQuery = "Select ItemNo, ItemDesc, ItemType, ItemCategory, ItemPrice, ItemUOM, ItemStatus, CGSTRate, SGSTRate, IGSTRate, UPCCode, HSNCode, SACCode, QuantityInHand, NoOfParts, ReOrderQty, ReOrderLevel, ItemRemarks, Created, CreatedBy, Modified, ModifiedBy From Item Where ItemNo = @No And Deleted = 'N'";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@No", SqlDbType.Int).Value = inItemNo;

                    lObjSqlCon.Open();

                    using (SqlDataReader lObjSDR = lObjCmd.ExecuteReader())
                    {
                        if (lObjSDR.HasRows)
                        {
                            while (lObjSDR.Read())
                            {
                                mnItemNo = Convert.ToInt32(lObjSDR["ItemNo"]);
                                msItemDesc = Convert.ToString(lObjSDR["ItemDesc"]);
                                msItemType = Convert.ToString(lObjSDR["ItemType"].ToString());
                                msItemCategory = Convert.ToString(lObjSDR["ItemCategory"].ToString());
                                mnItemPrice = Convert.ToDouble(lObjSDR["ItemPrice"]);
                                msUOM = Convert.ToString(lObjSDR["ItemUOM"]);
                                msStatus = Convert.ToString(lObjSDR["ItemStatus"]);
                                mnCGST = Convert.ToDouble(lObjSDR["CGSTRate"].Equals(DBNull.Value) ? null : lObjSDR["CGSTRate"]);
                                mnSGST = Convert.ToDouble(lObjSDR["SGSTRate"].Equals(DBNull.Value) ? null : lObjSDR["SGSTRate"]);
                                mnIGST = Convert.ToDouble(lObjSDR["IGSTRate"].Equals(DBNull.Value) ? null : lObjSDR["IGSTRate"]);
                                msUPC = Convert.ToString(lObjSDR["UPCCode"].Equals(DBNull.Value) ? null : lObjSDR["UPCCode"]);
                                msHSN = Convert.ToString(lObjSDR["HSNCode"].Equals(DBNull.Value) ? null : lObjSDR["HSNCode"]);
                                msSAC = Convert.ToString(lObjSDR["SACCode"].Equals(DBNull.Value) ? null : lObjSDR["SACCode"]);
                                mnQuantityInHand = Convert.ToDouble(lObjSDR["QuantityInHand"].Equals(DBNull.Value) ? null : lObjSDR["QuantityInHand"]);
                                if (lObjSDR["NoOfParts"].Equals(DBNull.Value))
                                {
                                    mnNoOfParts = null;
                                }
                                else
                                {
                                    mnNoOfParts = Convert.ToInt32(lObjSDR["NoOfParts"]);
                                }   
                                mnReOrderQty = Convert.ToDouble(lObjSDR["ReOrderQty"].Equals(DBNull.Value) ? null : lObjSDR["ReOrderQty"]);
                                mnReOrderLevel = Convert.ToDouble(lObjSDR["ReOrderLevel"].Equals(DBNull.Value) ? null : lObjSDR["ReOrderLevel"]);
                                msRemarks = Convert.ToString(lObjSDR["ItemRemarks"].Equals(DBNull.Value) ? null : lObjSDR["ItemRemarks"]);
                                
                                if (lObjSDR["Created"].Equals(DBNull.Value))
                                {
                                    mdCreated = null;
                                }
                                else
                                {
                                    mdCreated = Convert.ToDateTime(lObjSDR["Created"]);
                                }
                                msCreatedBy = Convert.ToString(lObjSDR["CreatedBy"].Equals(DBNull.Value) ? null : lObjSDR["CreatedBy"]);
                                
                                if (lObjSDR["Modified"].Equals(DBNull.Value))
                                {
                                    mdModified = null;
                                }
                                else
                                {
                                    mdModified = Convert.ToDateTime(lObjSDR["Modified"]);
                                }
                                msModifiedBy = Convert.ToString(lObjSDR["ModifiedBy"].Equals(DBNull.Value) ? null : lObjSDR["ModifiedBy"]);
                            }
                        }
                    }
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

        public static void GetItemCategory()
        {
            itemCategories.Clear();
            string[] categories = new string[]
            {
                "ENGN",
                "BODY",
                "DRIVE",
                "ELECT",
                "FRAME", 
                "SUSPN",
                "BRAKE"
            };
            foreach(string category in categories)
            {
                itemCategories.Add(category);
            }
        }

        public static void GetItemType()
        {
            itemType.Clear();
            string[] types = new string[]
            {
                "PARTS",
                "SERVICES"
            };
            foreach (string type in types)
            {
                itemType.Add(type);
            }
        }

        public static void GetItemStatus()
        {
            itemStatus.Clear();
            string[] status = new string[]
            {
                "ACT",
                "SUSP"
            };
            foreach (string x in status)
            {
                itemStatus.Add(x);
            }
        }

        public bool ShowAll(string isConStr, List<Item> ilItemList)
        {
            bool result;
            try
            {
                using (SqlConnection lObjSqlCon = new SqlConnection(isConStr))
                {
                    string lsQuery = "Select ItemNo, ItemDesc, ItemType, ItemCategory, ItemPrice, ItemUOM, ItemStatus, CGSTRate, SGSTRate, IGSTRate, UPCCode, HSNCode, SACCode, QuantityInHand, NoOfParts, ReOrderQty, ReOrderLevel, ItemRemarks From Item Where Deleted = 'N'";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;

                    lObjSqlCon.Open();
                    using (SqlDataReader lObjSDR = lObjCmd.ExecuteReader())
                    {
                        if (lObjSDR.HasRows)
                        {
                            while (lObjSDR.Read())
                            {
                                int lnItemNo = Convert.ToInt32(lObjSDR["ItemNo"]);
                                string lsItemDesc = Convert.ToString(lObjSDR["ItemDesc"]);
                                string lsItemType = Convert.ToString(lObjSDR["ItemType"].ToString());
                                string lsItemCategory = Convert.ToString(lObjSDR["ItemCategory"].ToString());
                                double lnItemPrice = Convert.ToDouble(lObjSDR["ItemPrice"]);
                                string lsUOM = Convert.ToString(lObjSDR["ItemUOM"]);
                                string lsStatus = Convert.ToString(lObjSDR["ItemStatus"]);
                                double lnCGST = Convert.ToDouble(lObjSDR["CGSTRate"].Equals(DBNull.Value) ? null : lObjSDR["CGSTRate"]);
                                double lnSGST = Convert.ToDouble(lObjSDR["SGSTRate"].Equals(DBNull.Value) ? null : lObjSDR["SGSTRate"]);
                                double lnIGST = Convert.ToDouble(lObjSDR["IGSTRate"].Equals(DBNull.Value) ? null : lObjSDR["IGSTRate"]);
                                string lsUPC = Convert.ToString(lObjSDR["UPCCode"].Equals(DBNull.Value) ? null : lObjSDR["UPCCode"]);
                                string lsHSN = Convert.ToString(lObjSDR["HSNCode"].Equals(DBNull.Value) ? null : lObjSDR["HSNCode"]);
                                string lsSAC = Convert.ToString(lObjSDR["SACCode"].Equals(DBNull.Value) ? null : lObjSDR["SACCode"]);
                                double lnQuantityInHand = Convert.ToDouble(lObjSDR["QuantityInHand"].Equals(DBNull.Value) ? null : lObjSDR["QuantityInHand"]);
                                int? lnNoOfParts;
                                if (lObjSDR["NoOfParts"].Equals(DBNull.Value))
                                {
                                    lnNoOfParts = null;
                                }
                                else
                                {
                                    lnNoOfParts = Convert.ToInt32(lObjSDR["NoOfParts"]);
                                }
                                //int lnNoOfParts = Convert.ToInt32(lObjSDR["NoOfParts"].Equals(DBNull.Value) ? null : lObjSDR["NoOfParts"]);
                                double lnReOrderQty = Convert.ToDouble(lObjSDR["ReOrderQty"].Equals(DBNull.Value) ? null : lObjSDR["ReOrderQty"]);
                                double lnReOrderLevel = Convert.ToDouble(lObjSDR["ReOrderLevel"].Equals(DBNull.Value) ? null : lObjSDR["ReOrderLevel"]);
                                string lsRemarks = Convert.ToString(lObjSDR["ItemRemarks"].Equals(DBNull.Value) ? null : lObjSDR["ItemRemarks"]);

                                Item lObjDummyItem = new Item(lsItemDesc, lsItemType, lsItemCategory, lnItemPrice, lsUOM, lsStatus);

                                lObjDummyItem.mnItemNo = lnItemNo;
                                lObjDummyItem.mnCGST = lnCGST;
                                lObjDummyItem.mnSGST = lnSGST;
                                lObjDummyItem.mnIGST = lnIGST;
                                lObjDummyItem.msUPC = lsUPC;
                                lObjDummyItem.msHSN = lsHSN;
                                lObjDummyItem.msSAC = lsSAC;
                                lObjDummyItem.mnQuantityInHand = lnQuantityInHand;
                                lObjDummyItem.mnNoOfParts = lnNoOfParts;
                                lObjDummyItem.mnReOrderQty = lnReOrderQty;
                                lObjDummyItem.mnReOrderLevel = lnReOrderLevel;
                                lObjDummyItem.msRemarks = lsRemarks;

                                ilItemList.Add(lObjDummyItem);

                            }
                        }
                    }
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
