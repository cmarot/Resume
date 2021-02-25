using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CM_Assignment8
{
    public partial class MainForm : Form
    {
        //Define array to hold hours.
        double[,] Hours = new double[4, 7];
        double MonTotal, TueTotal, WedTotal, ThuTotal, FriTotal, SatTotal, SunTotal;
        double Proj1Total, Proj2Total, Proj3Total, Proj4Total;
        double WeekTotal;
        public const double REG_RATE = 15;
        public const double OVER_RATE = 22.5;


        private void btnReset_Click(object sender, EventArgs e)
        {
            //Clears Hours[,] array.
            ResetHoursArray();

            //Clears Hour Totals.
            ResetHoursTotals();

            //Unlocks form to be re-edited.
            grpDataEntry.Enabled = true;
            btnSubmit.Enabled = true;

            //Hides Pay Stub output.
            grpPayStub.Visible = false;
            lblFortyHours.Visible = false;
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //Calculates hour totals.
            CalculateHours();
            //Determines whether information is correct.
            if (ValidateData())
            {
                //Unhides Pay Stub output.
                grpPayStub.Visible = true;

                //Locks user from editing.
                grpDataEntry.Enabled = false;
                btnSubmit.Enabled = false;

                //Prints pay stub.
                PrintPayStub();
            }  
        }

        private void CalculateHours()
        {
            //Calculates all information needed to create pay stub.
            FillHoursArray();
            CalculateProjectTotals();
            FillProjectTotals();
            CalculateDailyTotals();
            FillDailyTotals();
            CalculateWeekTotal();

        }
        private void PrintPayStub()
        {
            //Declare variables used.
            decimal wk = nudReportingPeriod.Value;
            string Week = wk.ToString();
            string Name = txtEmployeeName.Text;
            double RegPay;
            double OverTime = 0;
            double GrossPay;
            int Vacation = 0;
            

            //Print payroll.
            string message = "Payroll information for " + Name + " for the week ending Week " + Week + ".";
            lblPayStubHeader.Text = message;
            lblHoursWorkedNum.Text = txtWeekTotal.Text;
            lblRatePerRegNum.Text = REG_RATE.ToString();
            lblRatePerOverNum.Text = OVER_RATE.ToString();

            if (WeekTotal >= 40)
            {
                //Calcuate overtime.
                double OverHours = WeekTotal - 40;
                lblOverHoursNum.Text = OverHours.ToString();
                OverTime = OverHours * OVER_RATE;
                lblOverPayNum.Text = OverTime.ToString("C2");
                lblRegHoursNum.Text = "40";
                RegPay = 40 * REG_RATE;
                lblRegPayNum.Text = RegPay.ToString("C2");
                
            }
            else
            {
                //Without overtime.
                lblFortyHours.Visible = true;
                lblRegHoursNum.Text = txtWeekTotal.Text;
                RegPay = WeekTotal * REG_RATE;
                lblRegPayNum.Text = RegPay.ToString("C2");
                
            }
            //Calcuate total gross pay.
            GrossPay = RegPay + OverTime;
            lblGrossPayNum.Text = GrossPay.ToString("C2");
            
            //Calculate vacation days taken.
            if(chkMon.Checked == true)
            {
                Vacation += 1;
            }
            if (chkTue.Checked == true)
            {
                Vacation += 1;
            }
            if (chkWed.Checked == true)
            {
                Vacation += 1;
            }
            if (chkThu.Checked == true)
            {
                Vacation += 1;
            }
            if (chkFri.Checked == true)
            {
                Vacation += 1;
            }
            if (chkSat.Checked == true)
            {
                Vacation += 1;
            }
            if (chkSun.Checked == true)
            {
                Vacation += 1;
            }
            lblHolidayClaimNum.Text = Vacation.ToString();

        }
        private bool ValidateData()
        {
            //Checks all sources of error.
            if (CheckDaysOff() & CheckHoursPerDay() & CheckDescriptionEntry())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CheckDaysOff()
        {
            //Handle hours entered when vacation selected or reverse.
            try
            {
                if ((MonTotal == 0 & chkMon.Checked == false) || (MonTotal != 0 & chkMon.Checked == true))
                {
                    NoHoursRecordedException nhre = new NoHoursRecordedException();
                    throw (nhre);
                }
                if ((TueTotal == 0 & chkTue.Checked == false) || (TueTotal != 0 & chkTue.Checked == true))
                {
                    NoHoursRecordedException nhre = new NoHoursRecordedException();
                    throw (nhre);
                }
                if ((WedTotal == 0 & chkWed.Checked == false) || (WedTotal != 0 & chkWed.Checked == true))
                {
                    NoHoursRecordedException nhre = new NoHoursRecordedException();
                    throw (nhre);
                }
                if ((ThuTotal == 0 & chkThu.Checked == false) || (ThuTotal != 0 & chkThu.Checked == true))
                {
                    NoHoursRecordedException nhre = new NoHoursRecordedException();
                    throw (nhre);
                }
                if ((FriTotal == 0 & chkFri.Checked == false) || (FriTotal != 0 & chkFri.Checked == true))
                {
                    NoHoursRecordedException nhre = new NoHoursRecordedException();
                    throw (nhre);
                }
                if ((SatTotal == 0 & chkSat.Checked == false) || (SatTotal != 0 & chkSat.Checked == true))
                {
                    NoHoursRecordedException nhre = new NoHoursRecordedException();
                    throw (nhre);
                }
                if ((SunTotal == 0 & chkSun.Checked == false) || (SunTotal != 0 & chkSun.Checked == true))
                {
                    NoHoursRecordedException nhre = new NoHoursRecordedException();
                    throw (nhre);
                }
                return true;
            }
            catch(NoHoursRecordedException e)
            {
                MessageBox.Show(e.Message, "Hours Not Entered", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetHoursTotals();
                return false;
            }
        }
        private bool CheckHoursPerDay()
        {
            try
            {
                if(MonTotal > 24)
                {
                    TimeOverloadException toe = new TimeOverloadException();
                    throw (toe);
                }
                if (TueTotal > 24)
                {
                    TimeOverloadException toe = new TimeOverloadException();
                    throw (toe);
                }
                if (WedTotal > 24)
                {
                    TimeOverloadException toe = new TimeOverloadException();
                    throw (toe);
                }
                if (ThuTotal > 24)
                {
                    TimeOverloadException toe = new TimeOverloadException();
                    throw (toe);
                }
                if (FriTotal > 24)
                {
                    TimeOverloadException toe = new TimeOverloadException();
                    throw (toe);
                }
                if (SatTotal > 24)
                {
                    TimeOverloadException toe = new TimeOverloadException();
                    throw (toe);
                }
                if (SunTotal > 24)
                {
                    TimeOverloadException toe = new TimeOverloadException();
                    throw (toe);
                }
                return true;
            }
            catch(TimeOverloadException e)
            {
                MessageBox.Show(e.Message, "Hours Per Day",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetHoursTotals();
                return false;
            }
        }

        private bool CheckDescriptionEntry()
        {
            try
            {
                //Ensures Employee and Supervisor names are entered.
                if(txtEmployeeName.Text == "" | txtSupervisorName.Text == "")
                {
                    NoDescriptionException nde = new NoDescriptionException();
                    throw (nde);
                }
                //Ensures descriptions are added for Project records that accrued hours.
                if (Proj1Total != 0 && (txtClient1.Text == "" | txtContract1.Text == "" |
                txtProject1.Text == "" | txtBillingLevel1.Text == ""))
                {
                    NoDescriptionException nde = new NoDescriptionException();
                    throw (nde);
                }
                if (Proj2Total != 0 && (txtClient2.Text == "" | txtContract2.Text == "" |
                txtProject2.Text == "" | txtBillingLevel2.Text == ""))
                {
                    NoDescriptionException nde = new NoDescriptionException();
                    throw (nde);
                }
                if (Proj3Total != 0 && (txtClient3.Text == "" | txtContract3.Text == "" |
                txtProject3.Text == "" | txtBillingLevel3.Text == ""))
                {
                    NoDescriptionException nde = new NoDescriptionException();
                    throw (nde);
                }
                if (Proj4Total != 0 && (txtClient4.Text == "" | txtContract4.Text == "" |
                txtProject4.Text == "" | txtBillingLevel4.Text == ""))
                {
                    NoDescriptionException nde = new NoDescriptionException();
                    throw (nde);
                }
                return true;
            }
            catch(NoDescriptionException e)
            {
                MessageBox.Show(e.Message, "Descriptions Needed",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetHoursTotals();
                return false;
            }
            
        }
        private void CalculateProjectTotals()
        {
            //Iterate project totals from array.
            for(int i = 0; i < 7; i++)
            {
                Proj1Total += Hours[0, i];
                Proj2Total += Hours[1, i];
                Proj3Total += Hours[2, i];
                Proj4Total += Hours[3, i];
            }
        }
        private void FillProjectTotals()
        {
            //Fill project total controls on form.
            txtProjTotal1.Text = Proj1Total.ToString();
            txtProjTotal2.Text = Proj2Total.ToString();
            txtProjTotal3.Text = Proj3Total.ToString();
            txtProjTotal4.Text = Proj4Total.ToString();
        }

        private void CalculateDailyTotals()
        {
            //Interate daily totals from array.
            for(int i = 0; i < 4; i++)
            {
                MonTotal += Hours[i, 0];
                TueTotal += Hours[i, 1];
                WedTotal += Hours[i, 2];
                ThuTotal += Hours[i, 3];
                FriTotal += Hours[i, 4];
                SatTotal += Hours[i, 5];
                SunTotal += Hours[i, 6];
            }
        }
        private void FillDailyTotals()
        {
            //Fill daily total controls on form.
            txtMonTotal.Text = MonTotal.ToString();
            txtTueTotal.Text = TueTotal.ToString();
            txtWedTotal.Text = WedTotal.ToString();
            txtThuTotal.Text = ThuTotal.ToString();
            txtFriTotal.Text = FriTotal.ToString();
            txtSatTotal.Text = SatTotal.ToString();
            txtSunTotal.Text = SunTotal.ToString();
        }
        private void CalculateWeekTotal()
        {
            //Calculate the total hours worked in the week.
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 7; j++)
                {
                    WeekTotal += Hours[i, j];
                }
            }
            txtWeekTotal.Text = WeekTotal.ToString();
        }
        private void ResetHoursArray()
        {
            //Empty Hours[,] of any values.
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Hours[i, j] = 0;
                }
            }
        }
        private void ResetHoursTotals()
        {
            //Empty Project and Daily Totals.
            Proj1Total = 0;
            Proj2Total = 0;
            Proj3Total = 0;
            Proj4Total = 0;
            MonTotal = 0;
            TueTotal = 0;
            WedTotal = 0;
            ThuTotal = 0;
            FriTotal = 0;
            SatTotal = 0;
            SunTotal = 0;
            WeekTotal = 0;

            //Reset total controls on form.
            FillDailyTotals();
            FillProjectTotals();
            txtWeekTotal.Text = WeekTotal.ToString();
        }

        private void FillHoursArray()
        {
            //Handle blank entries.
            //Fill Hours[,] array with all values.

            try
            {
                //Monday blank entries.
                if (txtMon1.Text == "")
                {
                    Hours[0, 0] = 0;
                }
                else
                {
                    Hours[0, 0] = Convert.ToDouble(txtMon1.Text);
                }
                if (txtMon2.Text == "")
                {
                    Hours[1, 0] = 0;
                }
                else
                {
                    Hours[1, 0] = Convert.ToDouble(txtMon2.Text);
                }
                if (txtMon3.Text == "")
                {
                    Hours[2, 0] = 0;
                }
                else
                {
                    Hours[2, 0] = Convert.ToDouble(txtMon3.Text);
                }
                if (txtMon4.Text == "")
                {
                    Hours[3, 0] = 0;
                }
                else
                {
                    Hours[3, 0] = Convert.ToDouble(txtMon4.Text);
                }

                //Tuesday blank entries.
                if (txtTue1.Text == "")
                {
                    Hours[0, 1] = 0;
                }
                else
                {
                    Hours[0, 1] = Convert.ToDouble(txtTue1.Text);
                }
                if (txtTue2.Text == "")
                {
                    Hours[1, 1] = 0;
                }
                else
                {
                    Hours[1, 1] = Convert.ToDouble(txtTue2.Text);
                }
                if (txtTue3.Text == "")
                {
                    Hours[2, 1] = 0;
                }
                else
                {
                    Hours[2, 1] = Convert.ToDouble(txtTue3.Text);
                }
                if (txtTue4.Text == "")
                {
                    Hours[3, 1] = 0;
                }
                else
                {
                    Hours[3, 1] = Convert.ToDouble(txtTue4.Text);
                }

                //Wednesday blank entries.
                if (txtWed1.Text == "")
                {
                    Hours[0, 2] = 0;
                }
                else
                {
                    Hours[0, 2] = Convert.ToDouble(txtWed1.Text);
                }
                if (txtWed2.Text == "")
                {
                    Hours[1, 2] = 0;
                }
                else
                {
                    Hours[1, 2] = Convert.ToDouble(txtWed2.Text);
                }
                if (txtWed3.Text == "")
                {
                    Hours[2, 2] = 0;
                }
                else
                {
                    Hours[2, 2] = Convert.ToDouble(txtWed3.Text);
                }
                if (txtWed4.Text == "")
                {
                    Hours[3, 2] = 0;
                }
                else
                {
                    Hours[3, 2] = Convert.ToDouble(txtWed4.Text);
                }

                //Thursday blank entries.
                if (txtThu1.Text == "")
                {
                    Hours[0, 3] = 0;
                }
                else
                {
                    Hours[0, 3] = Convert.ToDouble(txtThu1.Text);
                }
                if (txtThu2.Text == "")
                {
                    Hours[1, 3] = 0;
                }
                else
                {
                    Hours[1, 3] = Convert.ToDouble(txtThu2.Text);
                }
                if (txtThu3.Text == "")
                {
                    Hours[2, 3] = 0;
                }
                else
                {
                    Hours[2, 3] = Convert.ToDouble(txtThu3.Text);
                }
                if (txtThu4.Text == "")
                {
                    Hours[3, 3] = 0;
                }
                else
                {
                    Hours[3, 3] = Convert.ToDouble(txtThu4.Text);
                }

                //Friday blank values.
                if (txtFri1.Text == "")
                {
                    Hours[0, 4] = 0;
                }
                else
                {
                    Hours[0, 4] = Convert.ToDouble(txtFri1.Text);
                }
                if (txtFri2.Text == "")
                {
                    Hours[1, 4] = 0;
                }
                else
                {
                    Hours[1, 4] = Convert.ToDouble(txtFri2.Text);
                }
                if (txtFri3.Text == "")
                {
                    Hours[2, 4] = 0;
                }
                else
                {
                    Hours[2, 4] = Convert.ToDouble(txtFri3.Text);
                }
                if (txtFri4.Text == "")
                {
                    Hours[3, 4] = 0;
                }
                else
                {
                    Hours[3, 4] = Convert.ToDouble(txtFri4.Text);
                }

                //Saturday blank values.
                if (txtSat1.Text == "")
                {
                    Hours[0, 5] = 0;
                }
                else
                {
                    Hours[0, 5] = Convert.ToDouble(txtSat1.Text);
                }
                if (txtSat2.Text == "")
                {
                    Hours[1, 5] = 0;
                }
                else
                {
                    Hours[1, 5] = Convert.ToDouble(txtSat2.Text);
                }
                if (txtSat3.Text == "")
                {
                    Hours[2, 5] = 0;
                }
                else
                {
                    Hours[2, 5] = Convert.ToDouble(txtSat3.Text);
                }
                if (txtSat4.Text == "")
                {
                    Hours[3, 5] = 0;
                }
                else
                {
                    Hours[3, 5] = Convert.ToDouble(txtSat4.Text);
                }

                //Sunday blank values.
                if (txtSun1.Text == "")
                {
                    Hours[0, 6] = 0;
                }
                else
                {
                    Hours[0, 6] = Convert.ToDouble(txtSun1.Text);
                }
                if (txtSun2.Text == "")
                {
                    Hours[1, 6] = 0;
                }
                else
                {
                    Hours[1, 6] = Convert.ToDouble(txtSun2.Text);
                }
                if (txtSun3.Text == "")
                {
                    Hours[2, 6] = 0;
                }
                else
                {
                    Hours[2, 6] = Convert.ToDouble(txtSun3.Text);
                }
                if (txtSun4.Text == "")
                {
                    Hours[3, 6] = 0;
                }
                else
                {
                    Hours[3, 6] = Convert.ToDouble(txtSun4.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter only values for hours worked.", "Enter Values", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information); 
                return;
            }
        }
    }
}
