using BriddaBhatta.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BriddaBhatta
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            BriddaBhattaBLL b1 = new BriddaBhattaBLL();
            SqlDataReader dr = b1.getColumnNames();
            while(dr.Read())
            {
                cboSearchBox.Items.Add(dr.GetValue(0).ToString());
                cboview.Items.Add(dr.GetValue(0).ToString());
            }
            
            dr.Close();
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {            
            BriddaBhattaBLL b1 = new BriddaBhattaBLL(txtFname.Text,txtLname.Text,dtPicker.Value,int.Parse(txtAge.Text),rdoMale.Checked?"Male":"Female",txtAdd.Text,txtStr.Text);
            if(b1.CheckUser())
            {
                if(MessageBox.Show("Same Name exists with same date of birth? Do you wish to continue ?", "Duplicate Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning).ToString()=="Yes")
                {
                    MessageBox.Show(b1.addPeople(), "user registration");
                }
            }
            else
            {
                MessageBox.Show(b1.addPeople(), "user registration");
            }
            
        }

        private void btnSearchBy_Click(object sender, EventArgs e)
        {
            
        }
        
        private void btnReset1_Click(object sender, EventArgs e)
        {
            txtFname.Clear();
            txtLname.Clear();
            txtAdd.Clear();
            txtStr.Clear();
            txtAge.Clear();
        }

        private void dtPicker_ValueChanged(object sender, EventArgs e)
        {
            BriddaBhattaBLL b1 = new BriddaBhattaBLL();
            txtAge.Text= b1.CalculateAge(dtPicker.Value).ToString();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            
        }

        private void btnReset2_Click(object sender, EventArgs e)
        {
            gbPersonDetails.Visible = false;
            txtFnameUD.Text="";
            txtLnameUD.Text="";
            dtPickerUD.Value = DateTime.Today;
            txtAddUD.Text = "";
            txtStrUD.Text = "";
            txtAgeUD.Text = "";
            txtAmtUD.Text = "";
            rdoMaleUD.Checked = true;
            txtFnameUD.Enabled = false;
            txtLnameUD.Enabled = false;
            txtAddUD.Enabled = false;
            txtStrUD.Enabled = false;
            rdoMaleUD.Enabled = false;
            rdoFemaleUD.Enabled = false;
            dtPickerUD.Enabled = false;
            btnDelete.Visible = true;
        }

        private void dtPickerUD_ValueChanged(object sender, EventArgs e)
        {
            BriddaBhattaBLL b1 = new BriddaBhattaBLL();
            txtAgeUD.Text = b1.CalculateAge(dtPickerUD.Value).ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(!txtFnameUD.Enabled)
            {
                btnDelete.Visible = false;
                txtFnameUD.Enabled = true;
                txtLnameUD.Enabled = true;
                txtAddUD.Enabled = true;
                txtStrUD.Enabled = true;
                rdoMaleUD.Enabled = true;
                rdoFemaleUD.Enabled = true;
                dtPickerUD.Enabled = true;
            }
            else
            {
                BriddaBhattaBLL b1 = new BriddaBhattaBLL(txtFnameUD.Text, txtLnameUD.Text, dtPickerUD.Value, int.Parse(txtAgeUD.Text), rdoMaleUD.Checked ? "Male" : "Female", txtAddUD.Text, txtStrUD.Text);
                if (MessageBox.Show("Are you sure you want to Update?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question).ToString() == "Yes")
                {
                    int update = b1.UpdatePeople(int.Parse(cboList.SelectedValue.ToString()));
                    if (update==1)
                        MessageBox.Show("Update Successful.", "Update People", MessageBoxButtons.OK);
                    else
                        MessageBox.Show("Error while Updating", "Update People", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                txtFnameUD.Enabled = false;
                txtLnameUD.Enabled = false;
                txtAddUD.Enabled = false;
                txtStrUD.Enabled = false;
                rdoMaleUD.Enabled = false;
                rdoFemaleUD.Enabled = false;
                dtPickerUD.Enabled = false;
                btnDelete.Visible = true;
            }
            btnSearchBy_Click(sender, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            BriddaBhattaBLL b1 = new BriddaBhattaBLL();
            if (MessageBox.Show("Are you sure you want to Delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question).ToString() == "Yes")
            {
                int delete = b1.DeletePeople(int.Parse(cboList.SelectedValue.ToString()));
                if (Convert.ToBoolean(delete))
                    MessageBox.Show("Delete Successful.", "Delete People", MessageBoxButtons.OK);
                else
                    MessageBox.Show("Error while Deleting", "Delete People", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnReset2_Click(sender, e);
            btnSearchBy_Click(sender, e);
        }

        

        private void cboSearchBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BriddaBhattaBLL b1 = new BriddaBhattaBLL();
                
                cboList.ValueMember = "ID";
                cboList.DisplayMember = "value";
                cboList.DataSource = b1.getValues(cboSearchBox.SelectedItem.ToString());
        }

        private void cboList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnReset2_Click(sender, e);
            gbPersonDetails.Visible = true;
            BriddaBhattaBLL b1 = new BriddaBhattaBLL();

            SqlDataReader dr = b1.getPerson(int.Parse(cboList.SelectedValue.ToString()));

            while (dr.Read())
            {
                txtFnameUD.Text = dr[1].ToString();
                txtLnameUD.Text = dr[2].ToString();
                dtPickerUD.Value = DateTime.Parse(dr[3].ToString());
                txtAddUD.Text = dr[4].ToString();
                txtStrUD.Text = dr[5].ToString();
                txtAgeUD.Text = dr[6].ToString();
                txtAmtUD.Text = dr[7].ToString();
                rdoMaleUD.Checked = (dr[8].ToString() == "Male" ? true : false);
                rdoFemaleUD.Checked = !rdoMaleUD.Checked;
            }
            dr.Close();
        }

        private void cboview_SelectedIndexChanged(object sender, EventArgs e)
        {
            BriddaBhattaBLL b1 = new BriddaBhattaBLL();
            SqlDataReader dr= b1.getDistinctValues(cboview.SelectedItem.ToString());
            cboValue.Items.Clear();
            while (dr.Read())
                cboValue.Items.Add(dr[0].ToString());
            dr.Close();
        }

        private void cboValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            BriddaBhattaBLL b1 = new BriddaBhattaBLL();
            SqlDataReader dr = b1.getPersons(cboview.SelectedItem.ToString(),checkString(cboValue.SelectedItem.ToString()));
            DataTable dt = new DataTable();
            dt.Clear();
            GV.DataSource = null;
            dt.Load(dr);
            
            GV.DataSource = dt;
            //GV.Refresh();
            //GV.Update();
            dr.Close();
        }

     
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                lblSearchBy.Enabled = false;
                lblValue.Enabled = false;
                cboview.Enabled = false;
                cboValue.Enabled = false;
                GV.DataSource = null;
                GV.DataSource = this.forumsDataSet.AdultPerson;
                this.adultPersonTableAdapter.Fill(this.forumsDataSet.AdultPerson);
            }
            else
            {
                lblSearchBy.Enabled = true;
                lblValue.Enabled = true;
                cboview.Enabled = true;
                cboValue.Enabled = true;
            }
        }

        private string checkString(string s)
        {
            if (s is String)
                return "'" + s + "'";
            else
                return s;
        }
        
    }
}
