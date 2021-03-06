﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using RMS;
using RMS.DataSets;
using NewPayrollSystem.Model;
using NewPayrollSystem.AnotherReport;



namespace PayRoll.ReportForms
{
    public partial class WarningLetter : Form
    {
        CCommonConstants oTempConstant = ConfigManager.GetConfig<CCommonConstants>();
        private String connectionString;
        public WarningLetter()
        {
            connectionString = oTempConstant.DBConnection;
            InitializeComponent();
        }

        private void WarningLetter_Load(object sender, EventArgs e)
        {
            PopulateComboboxEmpID();
        }
        private void PopulateComboboxEmpID()
        {
            comboBox1.Items.Clear();

            SqlConnection conn = new SqlConnection(this.connectionString);
            SqlCommand comm = new SqlCommand("SELECT DISTINCT EmployeeID FROM tblWarningLetter ORDER BY EmployeeID", conn);

            try
            {
                conn.Open();
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["EmployeeID"].ToString() != "")
                        comboBox1.Items.Add(reader["EmployeeID"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Read(comboBox1.Text);  
        }
        public void Read(string srr)
        {
            SqlConnection conn = new SqlConnection(this.connectionString);
            SqlCommand comm = new SqlCommand("SELECT *FROM Vw_Warning WHERE EmployeeID = @EmployeeID", conn);

            comm.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 50);
            comm.Parameters["@EmployeeID"].Value = comboBox1.Text;

            SqlDataAdapter adap = new SqlDataAdapter();
            WarningLetterDataSet dataset = new WarningLetterDataSet();
            WarningLetterCrystalReport crp = new WarningLetterCrystalReport();

            try
            {
                conn.Open();
                adap.SelectCommand = comm;
                adap.Fill(dataset, "DataTable1");
                crp.SetDataSource(dataset);
                crystalReportViewer2.ReportSource = crp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
