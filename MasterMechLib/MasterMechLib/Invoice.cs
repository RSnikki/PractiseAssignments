using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMechLib
{
    public class Invoice
    {
        public string msInnvoiceNo;
        public int? mnInvoiceSNo;
        public DateTime mdInvoiceDate;
        public string msInvoiceStatus;
        public Customer InvoiceCustomer;
        public string msVehicleRegNo;
        public string msVehicleModel;
        public string msChassisNo;
        public string msEngineNo;
        public int? mnMileage;
        public string msServiceType;
        public string msServiceAssoName;
        public string msServiceAssoMobNo;
        public double? mnPartsTotal;
        public double? mnLabourTotal;
        public double? mnPartsCGSTTotal;
        public double? mnLabourCGSTTotal;
        public double? mnPartsSGSTTotal;
        public double? mnLabourSGSTTotal;
        public double? mnPartsIGSTTotal;
        public double? mnLabourIGSTTotal;
        public double? mnTotalCGST;
        public double? mnTotalSGST;
        public double? mnTotalIGST;
        public double? mnTotalTax;
        public double? mnTotalAmount;
        public double? mnGrandTotal;
        public double? mnDiscountAmount;
        public double? mnInvoiceTotal;
        public string msInvoiceRemarks;
        public DateTime? mdCreated;
        public string msCreatedBy;
        public DateTime? mdModified;
        public string msModifiedBy;
        public string msDeleted;
        public DateTime? mdDeletedOn;
        public string msDeletedBy;

        public List<InvoiceItem> InvoiceItems;
        public string msConString;
        public string msUserID;

        public Invoice()
        {
            this.InvoiceCustomer = new Customer();
            this.InvoiceItems = new List<InvoiceItem>();
        }

        public Invoice(string msConString, string msUserID)
        {
            this.msConString = msConString;
            this.msUserID = msUserID;
            this.InvoiceCustomer = new Customer();
            this.InvoiceItems = new List<InvoiceItem>();
        }

        public bool Save()
        {
           using(SqlConnection lObjSqlCon = new SqlConnection(msConString))
           {
                lObjSqlCon.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = lObjSqlCon;

                //Set the Transaction
                SqlTransaction lObjInvoiceTrans;
                lObjInvoiceTrans = lObjSqlCon.BeginTransaction("Invoice Transaction");
                cmd.Transaction = lObjInvoiceTrans;

                //To Insert New Invoice
                if(mnInvoiceSNo is null)//If New Invoice i.e mnInvoiceSNo is null : Insert New Invoice details into DB
                {
                    //New Customer. Save/Insert Customer details in DB and also update his last visit
                    if (InvoiceCustomer.mnCustomerNo is null)
                    {
                        //If unsuccessful in insert operation, then rollback transaction and return to the form
                        InvoiceCustomer.msCreatedBy = msUserID;
                        if (!InvoiceCustomer.Save(cmd))
                        {
                            lObjInvoiceTrans.Rollback();
                            lObjSqlCon.Close();
                            return false;
                        }
                    }
                    else//If Customer already exists, update his Last visit
                    {
                        if (!InvoiceCustomer.UpdateLastVisit(cmd))//If unsuccessful in updating last visit, then rollback and return to the form
                        {
                            lObjInvoiceTrans.Rollback();
                            lObjSqlCon.Close();
                            return false;
                        }
                    }

                    try
                    {
                        string lsQuery = "Insert Into [Invoice" + MasterMechUtil.sFY + "]" +
                            " (InvoiceDate, InvoiceSts, CustNo, CustFName, CustLName, CustMobNo, CustEmail, CustSts, CustType, CustStAddr, " +
                            "CustArAddr, CustCity, CustState, CustPinCode, CustCountry, CustGSTNo, CustlLastVisit, CustRemarks, VehicleRegNo, VehicleModel, ChassisNo, EngineNo, Mileage, " +
                            "ServiceType, ServiceAssoName, ServiceAssoMobNo, PartsTotal, LabourTotal, PartsCGSTTotal, LabourCGSTTotal, PartsSGSTTotal, LabourSGSTTotal, PartsIGSTTotal, " +
                            "LabourIGSTTotal, TotalSGST, TotalCGST, TotalIGST, TotalTax, TotalAmount, GrandTotal, DiscountAmount, InvoiceTotal, InvoiceRemarks, " +
                            "Created, CreatedBy, Deleted) " +
                            "OUTPUT INSERTED.InvoiceSNo " +
                            "Values (@InvoiceDate, @InvoiceSts, @CustNo, @CustFName, @CustLName, " +
                            "@CustMobNo, @CustEmail, @CustSts, @CustType, @CustStAddr, @CustArAddr, @CustCity, @CustState, @CustPinCode, @CustCountry, @CustGSTNo, @CustlLastVisit, @CustRemarks, " +
                            "@VehicleRegNo, @VehicleModel, @ChassisNo, @EngineNo, @Mileage, @ServiceType, @ServiceAssoName, @ServiceAssoMobNo, @PartsTotal, @LabourTotal, @PartsCGSTTotal, @LabourCGSTTotal, " +
                            "@PartsSGSTTotal, @LabourSGSTTotal, @PartsIGSTTotal, @LabourIGSTTotal, @TotalSGST, @TotalCGST, @TotalIGST, @TotalTax, @TotalAmount, @GrandTotal, @DiscountAmount, @InvoiceTotal, " +
                            "@InvoiceRemarks, @Created, @CreatedBy, @Deleted)";

                        cmd.CommandText = lsQuery;
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("@InvoiceDate", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.AddWithValue("@InvoiceSts", SqlDbType.VarChar).Value = "Saved";
                        cmd.Parameters.AddWithValue("@CustNo", SqlDbType.Int).Value = InvoiceCustomer.mnCustomerNo;
                        cmd.Parameters.AddWithValue("@CustFName", SqlDbType.VarChar).Value = InvoiceCustomer.msCustomerFName;
                        cmd.Parameters.AddWithValue("@CustLName", SqlDbType.VarChar).Value = InvoiceCustomer.msCustomerLName;
                        cmd.Parameters.AddWithValue("@CustMobNo", SqlDbType.VarChar).Value = InvoiceCustomer.msCustomerMNo;
                        cmd.Parameters.AddWithValue("@CustEmail", SqlDbType.VarChar).Value = InvoiceCustomer.msCustomerEmail;
                        cmd.Parameters.AddWithValue("@CustSts", SqlDbType.VarChar).Value = InvoiceCustomer.msCustomerStatus;
                        cmd.Parameters.AddWithValue("@CustType", SqlDbType.VarChar).Value = InvoiceCustomer.msCustomerType;
                        cmd.Parameters.AddWithValue("@CustStAddr", SqlDbType.VarChar).Value = (object)InvoiceCustomer.msCustomerStAddr ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@CustArAddr", SqlDbType.VarChar).Value = (object)InvoiceCustomer.msCustomerArAddr ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@CustCity", SqlDbType.VarChar).Value = (object)InvoiceCustomer.msCustomerCity ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@CustState", SqlDbType.VarChar).Value = (object)InvoiceCustomer.msCustomerState ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@CustPinCode", SqlDbType.VarChar).Value = (object)InvoiceCustomer.msCustomerPinCode ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@CustCountry", SqlDbType.VarChar).Value = (object)InvoiceCustomer.msCustomerCountry ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@CustGSTNo", SqlDbType.VarChar).Value = (object)InvoiceCustomer.msCustomerGSTNo ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@CustlLastVisit", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.AddWithValue("@CustRemarks", SqlDbType.VarChar).Value = (object)InvoiceCustomer.msCustomerRemarks ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@VehicleRegNo", SqlDbType.VarChar).Value = (object)msVehicleRegNo ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@VehicleModel", SqlDbType.VarChar).Value = (object)msVehicleModel ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@ChassisNo", SqlDbType.VarChar).Value = (object)msChassisNo ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@EngineNo", SqlDbType.VarChar).Value = (object)msEngineNo ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@Mileage", SqlDbType.Int).Value = (object)mnMileage ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@ServiceType", SqlDbType.VarChar).Value = (object)msServiceType ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@ServiceAssoName", SqlDbType.VarChar).Value = (object)msServiceAssoName ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@ServiceAssoMobNo", SqlDbType.VarChar).Value = (object)msServiceAssoMobNo ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@PartsTotal", SqlDbType.Float).Value = (object)mnPartsTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@LabourTotal", SqlDbType.Float).Value = (object)mnLabourTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@PartsCGSTTotal", SqlDbType.Float).Value = (object)mnPartsCGSTTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@LabourCGSTTotal", SqlDbType.Float).Value = (object)mnLabourCGSTTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@PartsSGSTTotal", SqlDbType.Float).Value = (object)mnPartsSGSTTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@LabourSGSTTotal", SqlDbType.Float).Value = (object)mnLabourSGSTTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@PartsIGSTTotal", SqlDbType.Float).Value = (object)mnPartsIGSTTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@LabourIGSTTotal", SqlDbType.Float).Value = (object)mnLabourIGSTTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@TotalSGST", SqlDbType.Float).Value = (object)mnTotalSGST ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@TotalCGST", SqlDbType.Float).Value = (object)mnTotalCGST ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@TotalIGST", SqlDbType.Float).Value = (object)mnTotalIGST ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@TotalTax", SqlDbType.Float).Value = (object)mnTotalTax ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@TotalAmount", SqlDbType.Float).Value = (object)mnTotalAmount ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@GrandTotal", SqlDbType.Float).Value = (object)mnGrandTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@DiscountAmount", SqlDbType.Float).Value = (object)mnDiscountAmount ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@InvoiceTotal", SqlDbType.Float).Value = (object)mnInvoiceTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@InvoiceRemarks", SqlDbType.VarChar).Value = (object)msInvoiceRemarks ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@Created", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.VarChar).Value = msUserID;
                        cmd.Parameters.AddWithValue("@Deleted", SqlDbType.VarChar).Value = "N";

                        mnInvoiceSNo = (int)cmd.ExecuteScalar();//Getting InvoiceSNo to update in Line Items
                        cmd.Parameters.Clear();

                        foreach(InvoiceItem LineItem in InvoiceItems)//Getting individual InvoiceItem or Line Item in InvoiceItems List and saving the details ib DB InvoiceItem Table
                        {
                            LineItem.mnInvoiceSNo = Convert.ToInt32(mnInvoiceSNo);
                            LineItem.msCreatedBy = msUserID;
                            if (!LineItem.Save(cmd))//If unsuccessful in inserting InvoiceItems or Line Item, Transaction is Rollback and return to form  
                            {
                                lObjInvoiceTrans.Rollback();
                                lObjSqlCon.Close();
                                return false;
                            }
                        }

                        //Ater successful update of customer last visit and inserting Invoice details and InvoiceItem details in respective table, transaction is commited and saved to DB respective table
                        lObjInvoiceTrans.Commit();
                        lObjSqlCon.Close();
                        
                    }
                    catch (SqlException ex)
                    {
                        //MasterMechUtil.ShowError(ex.Message);
                        lObjInvoiceTrans.Rollback();
                        lObjSqlCon.Close();
                        return false;
                    }
                }
                else// Invoice exists, update Invoice values
                {
                    try
                    {
                        string lsQuery = "Update [Invoice"+ MasterMechUtil.sFY + "]" +
                            " Set InvoiceSts = @InvoiceSts, VehicleRegNo = @VehicleRegNo, VehicleModel = @VehicleModel, " +
                        "ChassisNo = @ChassisNo, EngineNo = @EngineNo, Mileage = @Mileage, ServiceType = @ServiceType , ServiceAssoName = @ServiceAssoName, ServiceAssoMobNo = @ServiceAssoMobNo, PartsTotal = @PartsTotal, LabourTotal = @LabourTotal, PartsCGSTTotal = @PartsCGSTTotal, LabourCGSTTotal = @LabourCGSTTotal, " +
                        "PartsSGSTTotal = @PartsSGSTTotal, LabourSGSTTotal = @LabourSGSTTotal, PartsIGSTTotal = @PartsIGSTTotal, LabourIGSTTotal = @LabourIGSTTotal, TotalSGST = @TotalSGST, TotalCGST = @TotalCGST, TotalIGST = @TotalIGST, TotalTax = @TotalTax, " +
                        "TotalAmount = @TotalAmount, GrandTotal = @GrandTotal, DiscountAmount = @DiscountAmount, InvoiceTotal = @InvoiceTotal, InvoiceRemarks = @InvoiceRemarks, Modified = @Modified, ModifiedBy = @ModifiedBy " +
                        "Where InvoiceSNo = @InvoiceSNo";

                        cmd.CommandText = lsQuery;
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("@InvoiceSNo", SqlDbType.Int).Value = mnInvoiceSNo;
                        cmd.Parameters.AddWithValue("@InvoiceSts", SqlDbType.VarChar).Value = msInvoiceStatus;
                        cmd.Parameters.AddWithValue("@VehicleRegNo", SqlDbType.VarChar).Value = (object)msVehicleRegNo ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@VehicleModel", SqlDbType.VarChar).Value = (object)msVehicleModel ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@ChassisNo", SqlDbType.VarChar).Value = (object)msChassisNo ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@EngineNo", SqlDbType.VarChar).Value = (object)msEngineNo ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@Mileage", SqlDbType.Int).Value = (object)mnMileage ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@ServiceType", SqlDbType.VarChar).Value = (object)msServiceType ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@ServiceAssoName", SqlDbType.VarChar).Value = (object)msServiceAssoName ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@ServiceAssoMobNo", SqlDbType.VarChar).Value = (object)msServiceAssoMobNo ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@PartsTotal", SqlDbType.Float).Value = (object)mnPartsTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@LabourTotal", SqlDbType.Float).Value = (object)mnLabourTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@PartsCGSTTotal", SqlDbType.Float).Value = (object)mnPartsCGSTTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@LabourCGSTTotal", SqlDbType.Float).Value = (object)mnLabourCGSTTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@PartsSGSTTotal", SqlDbType.Float).Value = (object)mnPartsSGSTTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@LabourSGSTTotal", SqlDbType.Float).Value = (object)mnLabourSGSTTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@PartsIGSTTotal", SqlDbType.Float).Value = (object)mnPartsIGSTTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@LabourIGSTTotal", SqlDbType.Float).Value = (object)mnLabourIGSTTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@TotalSGST", SqlDbType.Float).Value = (object)mnTotalSGST ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@TotalCGST", SqlDbType.Float).Value = (object)mnTotalCGST ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@TotalIGST", SqlDbType.Float).Value = (object)mnTotalIGST ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@TotalTax", SqlDbType.Float).Value = (object)mnTotalTax ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@TotalAmount", SqlDbType.Float).Value = (object)mnTotalAmount ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@GrandTotal", SqlDbType.Float).Value = (object)mnGrandTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@DiscountAmount", SqlDbType.Float).Value = (object)mnDiscountAmount ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@InvoiceTotal", SqlDbType.Float).Value = (object)mnInvoiceTotal ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@InvoiceRemarks", SqlDbType.VarChar).Value = (object)msInvoiceRemarks ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.VarChar).Value = msUserID;
                        cmd.Parameters.AddWithValue("@Modified", SqlDbType.DateTime).Value = DateTime.Now;

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        foreach (InvoiceItem LineItem in InvoiceItems)
                        {
                            LineItem.mnInvoiceSNo = Convert.ToInt32(mnInvoiceSNo);
                            LineItem.msModifiedBy = msUserID;
                            LineItem.msCreatedBy = msUserID;
                            if (!LineItem.Save(cmd))
                            {
                                lObjInvoiceTrans.Rollback();
                                lObjSqlCon.Close();
                                return false;
                            }
                        }
                        lObjInvoiceTrans.Commit();
                        lObjSqlCon.Close();

                    }
                    catch (SqlException ex)
                    {
                        //MasterMechUtil.ShowError(ex.Message);
                        lObjInvoiceTrans.Rollback();
                        lObjSqlCon.Close();
                        return false;
                    }
                }
           }
            return true;
        }

        public bool Delete()
        {
            using(SqlConnection lObjSqlCon = new SqlConnection(msConString))
            {
                lObjSqlCon.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = lObjSqlCon;

                //Set the Transaction
                SqlTransaction lObjInvoiceTrans;
                lObjInvoiceTrans = lObjSqlCon.BeginTransaction("InvoiceTransaction");
                cmd.Transaction = lObjInvoiceTrans;

                try
                {
                    string lsQuery = "Update [Invoice"+ MasterMechUtil.sFY + "]" +
                        " Set InvoiceSts = @InvoiceSts, Deleted = @Deleted, DeletedOn = @DeletedOn, DeletedBy = @DeletedBy Where InvoiceSNo = @InvoiceSNo AND Deleted = 'N'";

                    cmd.CommandText = lsQuery;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@InvoiceSNo", SqlDbType.Int).Value = mnInvoiceSNo;
                    cmd.Parameters.AddWithValue("@InvoiceSts", SqlDbType.VarChar).Value = "Deleted";
                    cmd.Parameters.AddWithValue("@Deleted", SqlDbType.VarChar).Value = "Y";
                    cmd.Parameters.AddWithValue("@DeletedOn", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.AddWithValue("@DeletedBy", SqlDbType.VarChar).Value = msUserID;

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    InvoiceItem lObjDummyInvoiceItem = new InvoiceItem();
                    lObjDummyInvoiceItem.mnInvoiceSNo = Convert.ToInt32(mnInvoiceSNo);
                    lObjDummyInvoiceItem.msDeletedBy = msUserID;
                        if (!lObjDummyInvoiceItem.Delete(cmd))//Delete operation all InvoiceItem with given InvoiceNo which is to be deleted, if unsuccessful transaction is rollback and control returns to form, no changes made in DB
                        {
                            lObjInvoiceTrans.Rollback();
                            lObjSqlCon.Close();
                            return false;
                        }
                    

                    lObjInvoiceTrans.Commit();
                    lObjSqlCon.Close();
                }
                catch(SqlException ex)//Incase of unsuccessful delete operation of Invoice, the transaction is rollback and returns to form, no changes made in DB
                {
                    //MasterMechUtil.ShowError(ex.Message);
                    lObjInvoiceTrans.Rollback();
                    lObjSqlCon.Close();
                    return false;
                }
            }
            return true;
        }

        public bool Load(int inInvoiceSNo)//Search individual Invoice on basis of invoice SNo and equate each field of invoice object with value from DB, including Customer Obj and InvoiceItems List for this InvoiceSNo
        {
            using (SqlConnection lObjSqlCon = new SqlConnection(msConString))
            {
                lObjSqlCon.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = lObjSqlCon;
                try
                {
                    string lsQuery = "Select InnvoiceNo, InvoiceSNo, InvoiceDate, InvoiceSts, CustNo, CustFName, CustLName, CustMobNo, CustEmail, CustSts, CustType, CustStAddr, CustArAddr, CustCity, CustState, CustPinCode, CustCountry, CustGSTNo, CustlLastVisit, CustRemarks, VehicleRegNo, VehicleModel,	ChassisNo, EngineNo, Mileage, ServiceType, ServiceAssoName,	ServiceAssoMobNo, PartsTotal, LabourTotal, PartsCGSTTotal, LabourCGSTTotal, PartsSGSTTotal, LabourSGSTTotal, PartsIGSTTotal, LabourIGSTTotal, TotalSGST, TotalCGST, TotalIGST, TotalTax, TotalAmount, GrandTotal, DiscountAmount, InvoiceTotal, InvoiceRemarks, Created, CreatedBy, Modified, ModifiedBy From [Invoice" + MasterMechUtil.sFY + "]" + " Where InvoiceSNo = @InvoiceSNo AND Deleted = 'N'";

                    cmd.CommandText = lsQuery;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@InvoiceSNo", SqlDbType.Int).Value = inInvoiceSNo;

                    using (SqlDataReader lObjSDR = cmd.ExecuteReader())
                    {
                        if (lObjSDR.HasRows)
                        {
                            while (lObjSDR.Read())
                            {
                                msInnvoiceNo = Convert.ToString(lObjSDR["InnvoiceNo"]);
                                mnInvoiceSNo = Convert.ToInt32(lObjSDR["InvoiceSNo"]);
                                mdInvoiceDate = Convert.ToDateTime(lObjSDR["InvoiceDate"]);
                                msInvoiceStatus = Convert.ToString(lObjSDR["InvoiceSts"]);
                                InvoiceCustomer.mnCustomerNo = Convert.ToInt32(lObjSDR["CustNo"]);
                                InvoiceCustomer.msCustomerFName = Convert.ToString(lObjSDR["CustFName"]);
                                InvoiceCustomer.msCustomerLName = Convert.ToString(lObjSDR["CustLName"]);
                                InvoiceCustomer.msCustomerMNo = Convert.ToString(lObjSDR["CustMobNo"]);
                                InvoiceCustomer.msCustomerEmail = Convert.ToString(lObjSDR["CustEmail"]);
                                InvoiceCustomer.msCustomerStatus = Convert.ToString(lObjSDR["CustSts"]);
                                InvoiceCustomer.msCustomerType = Convert.ToString(lObjSDR["CustType"]);
                                InvoiceCustomer.msCustomerStAddr = (lObjSDR["CustStAddr"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["CustStAddr"]);
                                InvoiceCustomer.msCustomerArAddr = (lObjSDR["CustArAddr"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["CustArAddr"]);
                                InvoiceCustomer.msCustomerCity = (lObjSDR["CustCity"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["CustCity"]);
                                InvoiceCustomer.msCustomerState = (lObjSDR["CustState"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["CustState"]);
                                InvoiceCustomer.msCustomerPinCode = (lObjSDR["CustPinCode"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["CustPinCode"]);
                                InvoiceCustomer.msCustomerCountry = (lObjSDR["CustCountry"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["CustCountry"]);
                                InvoiceCustomer.msCustomerGSTNo = (lObjSDR["CustGSTNo"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["CustGSTNo"]);
                                if (lObjSDR["CustlLastVisit"] == DBNull.Value)
                                {
                                    InvoiceCustomer.mdCustomerLastVisit = null;
                                }
                                else
                                {
                                    InvoiceCustomer.mdCustomerLastVisit = Convert.ToDateTime(lObjSDR["CustlLastVisit"]);
                                }
                                InvoiceCustomer.msCustomerRemarks = (lObjSDR["CustRemarks"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["CustRemarks"]);
                                msVehicleRegNo = (lObjSDR["VehicleRegNo"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["VehicleRegNo"]);
                                msVehicleModel = (lObjSDR["VehicleModel"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["VehicleModel"]);
                                msChassisNo = (lObjSDR["ChassisNo"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["ChassisNo"]);
                                msEngineNo = (lObjSDR["EngineNo"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["EngineNo"]);
                                if (lObjSDR["Mileage"].Equals(DBNull.Value))
                                {
                                    mnMileage = null;
                                }
                                else
                                {
                                    mnMileage = Convert.ToInt32(lObjSDR["Mileage"]);
                                }
                                msServiceType = (lObjSDR["ServiceType"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["ServiceType"]);
                                msServiceAssoName = (lObjSDR["ServiceAssoName"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["ServiceAssoName"]);
                                msServiceAssoMobNo = (lObjSDR["ServiceAssoMobNo"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["ServiceAssoMobNo"]);
                                if (lObjSDR["PartsTotal"].Equals(DBNull.Value))
                                {
                                    mnPartsTotal = null;
                                }
                                else
                                {
                                    mnPartsTotal = Convert.ToDouble(lObjSDR["PartsTotal"]);
                                }
                                if (lObjSDR["LabourTotal"].Equals(DBNull.Value))
                                {
                                    mnLabourTotal = null;
                                }
                                else
                                {
                                    mnLabourTotal = Convert.ToDouble(lObjSDR["LabourTotal"]);
                                }
                                if (lObjSDR["PartsCGSTTotal"].Equals(DBNull.Value))
                                {
                                    mnPartsCGSTTotal = null;
                                }
                                else
                                {
                                    mnPartsCGSTTotal = Convert.ToDouble(lObjSDR["PartsCGSTTotal"]); 
                                }
                                if (lObjSDR["LabourCGSTTotal"].Equals(DBNull.Value))
                                {
                                    mnLabourCGSTTotal = null;
                                }
                                else
                                {
                                    mnLabourCGSTTotal = Convert.ToDouble(lObjSDR["LabourCGSTTotal"]);
                                }
                                if (lObjSDR["PartsSGSTTotal"].Equals(DBNull.Value))
                                {
                                    mnPartsSGSTTotal = null;
                                }
                                else
                                {
                                    mnPartsSGSTTotal = Convert.ToDouble(lObjSDR["PartsSGSTTotal"]);
                                }
                                if (lObjSDR["LabourSGSTTotal"].Equals(DBNull.Value))
                                {
                                    mnLabourSGSTTotal = null;
                                }
                                else
                                {
                                    mnLabourSGSTTotal = Convert.ToDouble(lObjSDR["LabourSGSTTotal"]);
                                }
                                if (lObjSDR["PartsIGSTTotal"].Equals(DBNull.Value))
                                {
                                    mnPartsIGSTTotal = null;
                                }
                                else
                                {
                                    mnPartsIGSTTotal = Convert.ToDouble(lObjSDR["PartsIGSTTotal"]);
                                }
                                if (lObjSDR["LabourIGSTTotal"].Equals(DBNull.Value))
                                {
                                    mnLabourIGSTTotal = null;
                                }
                                else
                                {
                                    mnLabourIGSTTotal = Convert.ToDouble(lObjSDR["LabourIGSTTotal"]);
                                }
                                if (lObjSDR["TotalSGST"].Equals(DBNull.Value))
                                {
                                    mnTotalSGST = null;
                                }
                                else
                                {
                                    mnTotalSGST = Convert.ToDouble(lObjSDR["TotalSGST"]);
                                }
                                if (lObjSDR["TotalCGST"].Equals(DBNull.Value))
                                {
                                    mnTotalCGST = null;
                                }
                                else
                                {
                                    mnTotalCGST = Convert.ToDouble(lObjSDR["TotalCGST"]);
                                }
                                if (lObjSDR["TotalIGST"].Equals(DBNull.Value))
                                {
                                    mnTotalIGST = null;
                                }
                                else
                                {
                                    mnTotalIGST = Convert.ToDouble(lObjSDR["TotalIGST"]);
                                }
                                if (lObjSDR["TotalTax"].Equals(DBNull.Value))
                                {
                                    mnTotalTax = null;
                                }
                                else
                                {
                                    mnTotalTax = Convert.ToDouble(lObjSDR["TotalTax"]);
                                }
                                if (lObjSDR["TotalAmount"].Equals(DBNull.Value))
                                {
                                    mnTotalAmount = null;
                                }
                                else
                                {
                                    mnTotalAmount = Convert.ToDouble(lObjSDR["TotalAmount"]);
                                }
                                if (lObjSDR["GrandTotal"].Equals(DBNull.Value))
                                {
                                    mnGrandTotal = null;
                                }
                                else
                                {
                                    mnGrandTotal = Convert.ToDouble(lObjSDR["GrandTotal"]);
                                }
                                if (lObjSDR["DiscountAmount"].Equals(DBNull.Value))
                                {
                                    mnDiscountAmount = null;
                                }
                                else
                                {
                                    mnDiscountAmount = Convert.ToDouble(lObjSDR["DiscountAmount"]);
                                }
                                if (lObjSDR["InvoiceTotal"].Equals(DBNull.Value))
                                {
                                    mnInvoiceTotal = null;
                                }
                                else
                                {
                                    mnInvoiceTotal = Convert.ToDouble(lObjSDR["InvoiceTotal"]);
                                }
                                msInvoiceRemarks = (lObjSDR["InvoiceRemarks"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["InvoiceRemarks"]);
                                if (lObjSDR["Created"].Equals(DBNull.Value))
                                {
                                    mdCreated = null;
                                }
                                else
                                {
                                    mdCreated = Convert.ToDateTime(lObjSDR["Created"]);
                                }
                                msCreatedBy = (lObjSDR["CreatedBy"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["CreatedBy"]);
                                if (lObjSDR["Modified"].Equals(DBNull.Value))
                                {
                                    mdModified = null;
                                }
                                else
                                {
                                    mdModified = Convert.ToDateTime(lObjSDR["Modified"]);
                                }
                                msModifiedBy = (lObjSDR["ModifiedBy"].Equals(DBNull.Value)) ? null : Convert.ToString(lObjSDR["ModifiedBy"]);
                            }  
                        }
                    }

                    InvoiceItem lObjDummyInvoiceItem = new InvoiceItem();
                    lObjDummyInvoiceItem.SearchInvoiceItems(msConString, Convert.ToInt32(mnInvoiceSNo), InvoiceItems);
                    
                    lObjSqlCon.Close();
                }
                catch(SqlException ex)
                {
                    //MasterMechUtil.ShowError(ex.Message);
                    
                    lObjSqlCon.Close();
                    return false;
                }
            }
            return true;
        }

        //Searches a list of invoices on basis of key and fills the list passed as parameter
        public bool SearchInvoices(string issearchKey, List<Invoice> iObjListInvoices)
        {
            try
            {
                using(SqlConnection lObjSqlCon = new SqlConnection(msConString))
                {
                    string lsQuery = "Select InvoiceSNo, InvoiceDate, InvoiceSts, CustNo, CustFName, CustLName, CustMobNo, CustEmail, " +
                        "CustSts, CustType, CustCity, CustState, CustGSTNo, CustlLastVisit, VehicleRegNo, VehicleModel, ServiceType, " +
                        "ServiceAssoName, PartsTotal, LabourTotal, TotalTax, TotalAmount, GrandTotal, DiscountAmount, " +
                        "InvoiceTotal From [Invoice" + MasterMechUtil.sFY + "]" + " Where CustMobNo Like @SearchKey And Deleted = 'N'";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;
                    lObjCmd.Parameters.AddWithValue("@SearchKey", SqlDbType.VarChar).Value = "%" + issearchKey + "%";

                    lObjSqlCon.Open();
                    using(SqlDataReader lObjSDR = lObjCmd.ExecuteReader())
                    {
                        if (lObjSDR.HasRows)
                        {
                            while (lObjSDR.Read())
                            {
                                Invoice lObjInvoice = new Invoice();
                                lObjInvoice.mnInvoiceSNo = Convert.ToInt32(lObjSDR["InvoiceSNo"]);
                                lObjInvoice.mdInvoiceDate = Convert.ToDateTime(lObjSDR["InvoiceDate"]);
                                lObjInvoice.msInvoiceStatus = Convert.ToString(lObjSDR["InvoiceSts"]);
                                lObjInvoice.InvoiceCustomer.mnCustomerNo = Convert.ToInt32(lObjSDR["CustNo"]);
                                lObjInvoice.InvoiceCustomer.msCustomerFName = Convert.ToString(lObjSDR["CustFName"]);
                                lObjInvoice.InvoiceCustomer.msCustomerLName = Convert.ToString(lObjSDR["CustLName"]);
                                lObjInvoice.InvoiceCustomer.msCustomerMNo = Convert.ToString(lObjSDR["CustMobNo"]);
                                lObjInvoice.InvoiceCustomer.msCustomerEmail = Convert.ToString(lObjSDR["CustEmail"]);
                                lObjInvoice.InvoiceCustomer.msCustomerStatus = Convert.ToString(lObjSDR["CustSts"]);
                                lObjInvoice.InvoiceCustomer.msCustomerType = Convert.ToString(lObjSDR["CustType"]);
                                lObjInvoice.InvoiceCustomer.msCustomerCity = (lObjSDR["CustCity"] == DBNull.Value) ? null : Convert.ToString(lObjSDR["CustCity"]);
                                lObjInvoice.InvoiceCustomer.msCustomerState = (lObjSDR["CustState"] == DBNull.Value) ? null : Convert.ToString(lObjSDR["CustState"]);
                                lObjInvoice.InvoiceCustomer.msCustomerGSTNo = (lObjSDR["CustGSTNo"] == DBNull.Value) ? null : Convert.ToString(lObjSDR["CustGSTNo"]);
                                if(lObjSDR["CustlLastVisit"] == DBNull.Value)
                                {
                                    lObjInvoice.InvoiceCustomer.mdCustomerLastVisit = null;
                                }
                                else
                                {
                                    lObjInvoice.InvoiceCustomer.mdCustomerLastVisit = Convert.ToDateTime(lObjSDR["CustlLastVisit"]);
                                }
                                lObjInvoice.msVehicleRegNo = (lObjSDR["VehicleRegNo"] == DBNull.Value) ? null : Convert.ToString(lObjSDR["VehicleRegNo"]);
                                lObjInvoice.msVehicleModel = (lObjSDR["VehicleModel"] == DBNull.Value) ? null : Convert.ToString(lObjSDR["VehicleModel"]);
                                lObjInvoice.msServiceType = (lObjSDR["ServiceType"] == DBNull.Value) ? null : Convert.ToString(lObjSDR["ServiceType"]);
                                lObjInvoice.msServiceAssoName = (lObjSDR["ServiceAssoName"] == DBNull.Value) ? null : Convert.ToString(lObjSDR["ServiceAssoName"]);
                                if (lObjSDR["PartsTotal"] == DBNull.Value)
                                {
                                    lObjInvoice.mnPartsTotal = null;
                                }
                                else
                                {
                                    lObjInvoice.mnPartsTotal = Convert.ToDouble(lObjSDR["PartsTotal"]);
                                }

                                if (lObjSDR["LabourTotal"] == DBNull.Value)
                                {
                                    lObjInvoice.mnLabourTotal = null;
                                }
                                else
                                {
                                    lObjInvoice.mnLabourTotal = Convert.ToDouble(lObjSDR["LabourTotal"]);
                                }

                                if (lObjSDR["TotalTax"] == DBNull.Value)
                                {
                                    lObjInvoice.mnTotalTax = null;
                                }
                                else
                                {
                                    lObjInvoice.mnTotalTax = Convert.ToDouble(lObjSDR["TotalTax"]);
                                }
                               
                                if (lObjSDR["TotalAmount"] == DBNull.Value)
                                {
                                    lObjInvoice.mnTotalAmount = null;
                                }
                                else
                                {
                                    lObjInvoice.mnTotalAmount = Convert.ToDouble(lObjSDR["TotalAmount"]);
                                }

                                if (lObjSDR["GrandTotal"] == DBNull.Value)
                                {
                                    lObjInvoice.mnGrandTotal = null;
                                }
                                else
                                {
                                    lObjInvoice.mnGrandTotal = Convert.ToDouble(lObjSDR["GrandTotal"]);
                                }

                                if (lObjSDR["DiscountAmount"] == DBNull.Value)
                                {
                                    lObjInvoice.mnDiscountAmount = null;
                                }
                                else
                                {
                                    lObjInvoice.mnDiscountAmount = Convert.ToDouble(lObjSDR["DiscountAmount"]);
                                }

                                if (lObjSDR["InvoiceTotal"] == DBNull.Value)
                                {
                                    lObjInvoice.mnInvoiceTotal = null;
                                }
                                else
                                {
                                    lObjInvoice.mnInvoiceTotal = Convert.ToDouble(lObjSDR["InvoiceTotal"]);
                                }

                                iObjListInvoices.Add(lObjInvoice);
                            }
                        }
                    }
                        
                }
                return true;
            }
            catch (SqlException ex)
            {
                //MasterMechUtil.ShowError(ex.Message);
                return false;
            }
            
        }

        public bool AdvancedSearch(string isFNameKey, string isVRegNo, string isCustCity, string isServiceAssoName, List<Invoice> iObjListInvoices)
        {
            try
            {
                using (SqlConnection lObjSqlCon = new SqlConnection(msConString))
                {
                    string lsQuery = "Select InvoiceSNo, InvoiceDate, InvoiceSts, CustNo, CustFName, CustLName, CustMobNo, CustEmail, " +
                        "CustSts, CustType, CustCity, CustState, CustGSTNo, CustlLastVisit, VehicleRegNo, VehicleModel, ServiceType, " +
                        "ServiceAssoName, PartsTotal, LabourTotal, TotalTax, TotalAmount, GrandTotal, DiscountAmount, " +
                        "InvoiceTotal From [Invoice" + MasterMechUtil.sFY + "]" + " Where CustFName Like @FNameKey And VehicleRegNo Like @VRegNo And CustCity Like @CustCity And ServiceAssoName Like @ServiceAssoName And Deleted = 'N'";

                    SqlCommand lObjCmd = new SqlCommand(lsQuery, lObjSqlCon);
                    lObjCmd.CommandType = CommandType.Text;

                    lObjCmd.Parameters.AddWithValue("@FNameKey", SqlDbType.VarChar).Value = "%" + isFNameKey + "%";
                    lObjCmd.Parameters.AddWithValue("@VRegNo", SqlDbType.VarChar).Value = "%" + isVRegNo + "%";
                    lObjCmd.Parameters.AddWithValue("@CustCity", SqlDbType.VarChar).Value = "%" + isCustCity + "%";
                    lObjCmd.Parameters.AddWithValue("@ServiceAssoName", SqlDbType.VarChar).Value = "%" + isServiceAssoName + "%";

                    lObjSqlCon.Open();
                    using (SqlDataReader lObjSDR = lObjCmd.ExecuteReader())
                    {
                        if (lObjSDR.HasRows)
                        {
                            while (lObjSDR.Read())
                            {
                                Invoice lObjInvoice = new Invoice();
                                lObjInvoice.mnInvoiceSNo = Convert.ToInt32(lObjSDR["InvoiceSNo"]);
                                lObjInvoice.mdInvoiceDate = Convert.ToDateTime(lObjSDR["InvoiceDate"]);
                                lObjInvoice.msInvoiceStatus = Convert.ToString(lObjSDR["InvoiceSts"]);
                                lObjInvoice.InvoiceCustomer.mnCustomerNo = Convert.ToInt32(lObjSDR["CustNo"]);
                                lObjInvoice.InvoiceCustomer.msCustomerFName = Convert.ToString(lObjSDR["CustFName"]);
                                lObjInvoice.InvoiceCustomer.msCustomerLName = Convert.ToString(lObjSDR["CustLName"]);
                                lObjInvoice.InvoiceCustomer.msCustomerMNo = Convert.ToString(lObjSDR["CustMobNo"]);
                                lObjInvoice.InvoiceCustomer.msCustomerEmail = Convert.ToString(lObjSDR["CustEmail"]);
                                lObjInvoice.InvoiceCustomer.msCustomerStatus = Convert.ToString(lObjSDR["CustSts"]);
                                lObjInvoice.InvoiceCustomer.msCustomerType = Convert.ToString(lObjSDR["CustType"]);
                                lObjInvoice.InvoiceCustomer.msCustomerCity = (lObjSDR["CustCity"] == DBNull.Value) ? null : Convert.ToString(lObjSDR["CustCity"]);
                                lObjInvoice.InvoiceCustomer.msCustomerState = (lObjSDR["CustState"] == DBNull.Value) ? null : Convert.ToString(lObjSDR["CustState"]);
                                lObjInvoice.InvoiceCustomer.msCustomerGSTNo = (lObjSDR["CustGSTNo"] == DBNull.Value) ? null : Convert.ToString(lObjSDR["CustGSTNo"]);
                                if (lObjSDR["CustlLastVisit"] == DBNull.Value)
                                {
                                    lObjInvoice.InvoiceCustomer.mdCustomerLastVisit = null;
                                }
                                else
                                {
                                    lObjInvoice.InvoiceCustomer.mdCustomerLastVisit = Convert.ToDateTime(lObjSDR["CustlLastVisit"]);
                                }
                                lObjInvoice.msVehicleRegNo = (lObjSDR["VehicleRegNo"] == DBNull.Value) ? null : Convert.ToString(lObjSDR["VehicleRegNo"]);
                                lObjInvoice.msVehicleModel = (lObjSDR["VehicleModel"] == DBNull.Value) ? null : Convert.ToString(lObjSDR["VehicleModel"]);
                                lObjInvoice.msServiceType = (lObjSDR["ServiceType"] == DBNull.Value) ? null : Convert.ToString(lObjSDR["ServiceType"]);
                                lObjInvoice.msServiceAssoName = (lObjSDR["ServiceAssoName"] == DBNull.Value) ? null : Convert.ToString(lObjSDR["ServiceAssoName"]);
                                if (lObjSDR["PartsTotal"] == DBNull.Value)
                                {
                                    lObjInvoice.mnPartsTotal = null;
                                }
                                else
                                {
                                    lObjInvoice.mnPartsTotal = Convert.ToDouble(lObjSDR["PartsTotal"]);
                                }

                                if (lObjSDR["LabourTotal"] == DBNull.Value)
                                {
                                    lObjInvoice.mnLabourTotal = null;
                                }
                                else
                                {
                                    lObjInvoice.mnLabourTotal = Convert.ToDouble(lObjSDR["LabourTotal"]);
                                }

                                if (lObjSDR["TotalTax"] == DBNull.Value)
                                {
                                    lObjInvoice.mnTotalTax = null;
                                }
                                else
                                {
                                    lObjInvoice.mnTotalTax = Convert.ToDouble(lObjSDR["TotalTax"]);
                                }

                                if (lObjSDR["TotalAmount"] == DBNull.Value)
                                {
                                    lObjInvoice.mnTotalAmount = null;
                                }
                                else
                                {
                                    lObjInvoice.mnTotalAmount = Convert.ToDouble(lObjSDR["TotalAmount"]);
                                }

                                if (lObjSDR["GrandTotal"] == DBNull.Value)
                                {
                                    lObjInvoice.mnGrandTotal = null;
                                }
                                else
                                {
                                    lObjInvoice.mnGrandTotal = Convert.ToDouble(lObjSDR["GrandTotal"]);
                                }

                                if (lObjSDR["DiscountAmount"] == DBNull.Value)
                                {
                                    lObjInvoice.mnDiscountAmount = null;
                                }
                                else
                                {
                                    lObjInvoice.mnDiscountAmount = Convert.ToDouble(lObjSDR["DiscountAmount"]);
                                }

                                if (lObjSDR["InvoiceTotal"] == DBNull.Value)
                                {
                                    lObjInvoice.mnInvoiceTotal = null;
                                }
                                else
                                {
                                    lObjInvoice.mnInvoiceTotal = Convert.ToDouble(lObjSDR["InvoiceTotal"]);
                                }

                                iObjListInvoices.Add(lObjInvoice);
                            }
                        }
                    }

                }
                return true;
            }
            catch (SqlException ex)
            {
                //MasterMechUtil.ShowError(ex.Message);
                return false;
            }
        }

        public static void GetCustStatus(List<string> iObjListStatus)
        {
            iObjListStatus.Add("A");
            iObjListStatus.Add("P");
            iObjListStatus.Add("B");
        }

        public static void GetCustType(List<string> iObjListType)
        {
            iObjListType.Add("IND");
            iObjListType.Add("BUS");  
        }

        public static void GetServiceType(List<string> iObjListServiceType)
        {
            iObjListServiceType.Add("Annual Service");
            iObjListServiceType.Add("Accidental Repair");
            iObjListServiceType.Add("Normal Repair");
        }
    }
}
//Customer Fields: ------
//public int mnCustNo;
//public string msCustFName;
//public string msCustLName;
//public string msCustMobNo;
//public string msCustEmail;
//public string msCustSts;
//public string msCustType;
//public string msCustStAddr;
//public string msCustArAddr;
//public string msCustCity;
//public string msCustState;
//public string msCustPinCode;
//public string msCustCountry;
//public string msCustGSTNo;
//public DateTime mdCustlLastVisit;
//public string msCustRemarks;
