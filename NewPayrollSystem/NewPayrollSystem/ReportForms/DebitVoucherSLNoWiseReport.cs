using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using RMS;
using NewPayrollSystem.Model;
using NewPayrollSystem.AnotherReport;

namespace PayRoll.ReportForms
{
    public partial class DebitVoucherSLNoWiseReport : Form
    {
        CCommonConstants oTempConstant = ConfigManager.GetConfig<CCommonConstants>();
        private String connectionString;
        public DebitVoucherSLNoWiseReport()
        {
            connectionString = oTempConstant.DBConnection;
            InitializeComponent();
        }

        private void DebitVoucherSLNoWiseReport_Load(object sender, EventArgs e)
        {
            PopulateComboboxFactoryName();
        }
        private void PopulateComboboxFactoryName()
        {
            comboBox1.Items.Clear();

            SqlConnection conn = new SqlConnection(this.connectionString);
            SqlCommand comm = new SqlCommand("SELECT DISTINCT VoucherNo FROM TbDailyIncome ORDER BY VoucherNo", conn);

            try
            {
                conn.Open();
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["VoucherNo"].ToString() != "")
                        comboBox1.Items.Add(reader["VoucherNo"].ToString());
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Read(comboBox1.Text);  
        }
        public void Read(string srr)
        {
            SqlConnection conn = new SqlConnection(this.connectionString);
            SqlCommand comm = new SqlCommand("SELECT *FROM TbDailyIncome WHERE VoucherNo = @VoucherNo ORDER BY VoucherNo", conn);

            comm.Parameters.Add("@VoucherNo", SqlDbType.VarChar, 50);
            comm.Parameters["@VoucherNo"].Value = comboBox1.Text;

            SqlDataAdapter adap = new SqlDataAdapter();
            DataSets.DebitVoucherDataSet dataset = new DataSets.DebitVoucherDataSet();
            DebitVoucherBySLNoCrystalReport crp = new DebitVoucherBySLNoCrystalReport();

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