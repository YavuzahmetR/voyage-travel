using DataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace Sefer_Seyahat
{
    public partial class FrmGiris : Form
    {
        private DataAccess db;
        public FrmGiris()
        {
            InitializeComponent();
            db = new DataAccess(@"Data Source=LAPTOP-1PLLIFRV\SQLEXPRESS;Initial Catalog=TestYolcuBilet;Integrated Security=True");
        }
        SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-1PLLIFRV\SQLEXPRESS;Initial Catalog=TestYolcuBilet;Integrated Security=True");
        
        private void comboKalkis()
        {

            
            DataTable Dt = db.DataTableResult("select distinct KALKIS from TBLSEFERBILGI");
            comboBox1.ValueMember = "SEFERNO";
            comboBox1.DisplayMember = "KALKIS";
            comboBox1.DataSource = Dt;           
            if (comboBox1.Items.Count > 1)
            {
                comboBox1.SelectedIndex = -1;
            }
        }
        private void comboVaris()
        {
            DataTable dt = db.DataTableResult("Select distinct VARIS from TBLSEFERBILGI");
            comboBox2.ValueMember = "SEFERNO";
            comboBox2.DisplayMember = "VARIS";
            comboBox2.DataSource = dt;
            if (comboBox2.Items.Count > 1)
            {
                comboBox2.SelectedIndex = -1;
            }
        }
        private void Secim()
        {           
          dataGridView1.DataSource = db.DataTableResult("Select SEFERNO,KALKIS,VARIS,ADSOYAD AS 'KAPTAN',TARIH,SAAT,FIYAT FROM TBLSEFERBILGI INNER JOIN TBLKAPTAN ON  TBLSEFERBILGI.KAPTAN = TBLKAPTAN.KAPTANNO where KALKIS LIKE '%" + comboBox1.Text + "'and VARIS LIKE '%" + comboBox2.Text + "'");           
        }   
        private void CtrlTemizle(Control control)
        {
            foreach (Control ctr in control.Controls)// groupBox4.Controls)
            {

                if (ctr is TextBox)
                {
                    ctr.Text = string.Empty;
                }
                else if (ctr is ComboBox)
                {
                    (ctr as ComboBox).SelectedIndex = -1;
                }
                else if (ctr is MaskedTextBox)
                {
                    (ctr as MaskedTextBox).Text = string.Empty;
                }
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("insert into TBLYOLCUBILGI (AD,SOYAD,TELEFON,TC,CINSIYET,MAIL) values (@p1,@p2,@p3,@p4,@p5,@p6)");
            cmd.Parameters.AddWithValue("@p1", tbxName.Text);
            cmd.Parameters.AddWithValue("@p2", tbxLastName.Text);
            cmd.Parameters.AddWithValue("@p3", mtbPhoneNum.Text);
            cmd.Parameters.AddWithValue("@p4", mtbIdNum.Text);
            cmd.Parameters.AddWithValue("@p5", cbxGender.Text);
            cmd.Parameters.AddWithValue("@p6", tbxMail.Text);
            int result = db.InsertUpdate(cmd);

         if (result == 1)
            {
                MessageBox.Show(Mesajlar.KayıtBasarili, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
            }
            else
            {
                MessageBox.Show(Mesajlar.KayitHatali, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            CtrlTemizle(groupBox4);
        }
        private void btnCapReg_Click(object sender, EventArgs e)
        {            
            SqlCommand cmd = new SqlCommand("insert into TBLKAPTAN (KAPTANNO,ADSOYAD,TELEFON) VALUES (@P1,@P2,@P3)");
            cmd.Parameters.AddWithValue("@P1", mtbCapNo.Text);
            cmd.Parameters.AddWithValue("@P2", tbxCapName.Text);
            cmd.Parameters.AddWithValue("@P3", mtbCapPhone.Text);           
            db.InsertUpdate(cmd);
            MessageBox.Show(Mesajlar.KaptanKaydedildi, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            CtrlTemizle(groupBox2);
        
        }
        private void btnSeferOlustur_Click(object sender, EventArgs e)
        {
            
            SqlCommand cmd = new SqlCommand("insert into TBLSEFERBILGI (KALKIS,VARIS,TARIH,SAAT,KAPTAN,FIYAT) values (@p1,@p2,@p3,@p4,@p5,@p6)");
            cmd.Parameters.AddWithValue("@p1", tbxDeparture.Text);
            cmd.Parameters.AddWithValue("@p2", tbxArrival.Text);
            cmd.Parameters.AddWithValue("@p3", mtbDate.Text);
            cmd.Parameters.AddWithValue("@p4", mtbTime.Text);
            cmd.Parameters.AddWithValue("@p5", mtbCaptain.Text);
            cmd.Parameters.AddWithValue("@p6", tbxPrice.Text);           
            db.InsertUpdate(cmd);          
            MessageBox.Show("Sefer Başarıyla Oluşturuldu!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);            
            CtrlTemizle(groupBox5);
           
            string qry = string.Format("select count(*) from tblseferdetay");
            string result = db.StringResult(qry);
            lblsefercount.Text = result;
        }

        private void FrmGiris_Load(object sender, EventArgs e)
        {
            string qry = string.Format("select count(*) from tblseferdetay");
            string result = db.StringResult(qry);
            lblsefercount.Text = result;

            comboKalkis();
            comboVaris();          
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            tbxTripPassanger.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            string query = string.Format("select count(yolcutc) from tblseferdetay where seferno='"+tbxTripPassanger.Text+"'");
            string result = db.StringResult(query);
            lblpassagercount.Text = result;
        }


        private void SeatNum_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            tbxSeatNum.Text = btn.Tag.ToString();
        }

        

        private void btnRez_Click(object sender, EventArgs e)
        {
            string tarih = string.Format("{0}.{1}.{2} 00:00", dtpSefer.Value.Year.ToString(), dtpSefer.Value.Month.ToString(), dtpSefer.Value.Day.ToString());
            
            

            string query = string.Format("select count(*)  from TBLSEFERDETAY where YOLCUTC='{0}' and SEFERTARIH>='{1}' and KOLTUK='{2}' and  seferno = '{3}'"
                , mtbPassangerIdNum.Text
                , tarih
                , tbxSeatNum.Text, tbxTripPassanger.Text);

            string querynd = string.Format("select koltuk from TBLSEFERDETAY where seferno= '{0}'", tbxTripPassanger.Text);
            
            string queryrd = string.Format("select yolcutc from TBLSEFERDETAY where seferno= '{0}'", tbxTripPassanger.Text);

            

            if (string.IsNullOrEmpty(tbxTripPassanger.Text) ||  string.IsNullOrEmpty(tbxSeatNum.Text))
            {
                MessageBox.Show(Mesajlar.AlanlarBosGecilemez);
                CtrlTemizle(groupBox3);
                return;
            }
            else if(mtbPassangerIdNum.Text.Length < 11)
            {
                MessageBox.Show(Mesajlar.TCkimlik);
                CtrlTemizle(groupBox3);
                return;
            }

            

            string result = db.StringResult(query);
            string resultnd = db.StringResult(querynd);
            string resultrd = db.StringResult(queryrd);
            

            

            if (int.Parse(result) == 0)
            {
                if(resultnd == tbxSeatNum.Text || resultrd == mtbPassangerIdNum.Text) { MessageBox.Show(Mesajlar.ZatemKayıtlı);   }
                else
                {
                    DateTime dt = dtpSefer.Value;
                    SqlCommand command = new SqlCommand("INSERT INTO TBLSEFERDETAY (SEFERNO,YOLCUTC,KOLTUK,SEFERTARIH) values (@P1,@P2,@P3,@p4)");
                    command.Parameters.AddWithValue("@P1", tbxTripPassanger.Text);
                    command.Parameters.AddWithValue("@P2", mtbPassangerIdNum.Text);
                    command.Parameters.AddWithValue("@P3", tbxSeatNum.Text);
                    command.Parameters.AddWithValue("@P4", dt.ToString("yyyy-MM-dd HH:mm:ss"));

                    if (db.InsertUpdate(command) == 1) { MessageBox.Show(Mesajlar.Rezervasyon); }

                    CtrlTemizle(groupBox3);
                }
                
            }
            else MessageBox.Show(Mesajlar.ZatemKayıtlı);

        }
       
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Secim();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Secim();
        }
        
    }
}


