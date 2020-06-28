using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DataGrid
{
    public partial class FrmExemplo : MetroFramework.Forms.MetroForm
    {
        SqlConnection con = new SqlConnection("Data Source=aula2020.database.windows.net;Initial Catalog=DataGrid;User ID=tds02;Password=@nuvem2020;Connect Timeout=60;Encrypt=True;MultipleActiveResultSets=true;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        List<Exemplo> ex { get; set; }
        public class Exemplo
        {
            public string nome { get; set; }
            public string idade { get; set; }
            public string cidade { get; set; }
        }
        public FrmExemplo()
        {
            InitializeComponent();
            ex = new List<Exemplo>();
        }

        public void CarregaDgvBanco()
        {
            String query = "SELECT * FROM Pessoa";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable pessoa = new DataTable();
            da.Fill(pessoa);
            MdgvPessoaBanco.DataSource = pessoa;
            con.Close();
        }

        private void MbtnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmExemplo_Load(object sender, EventArgs e)
        {
            MdgvExemplo.DataSource = ex;
            MdgvExemplo.Refresh();
            CarregaDgvBanco();
        }

        private void MbtnInserir_Click(object sender, EventArgs e)
        {
            MdgvExemplo.DataSource = null;
            ex.Add(new Exemplo() { nome = MtxtNome.Text.Trim(), idade = MtxtIdade.Text.Trim(), cidade = MtxtCidade.Text.Trim() });
            MdgvExemplo.DataSource = ex;
            MdgvExemplo.Refresh();
            MtxtNome.Text = "";
            MtxtIdade.Text = "";
            MtxtCidade.Text = "";
        }

        private void MdgvExemplo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void MbtnGravar_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ex.Count; i++)
            {
                SqlCommand cmd = new SqlCommand("Inserir", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nome", SqlDbType.NVarChar).Value = ex[i].nome;
                cmd.Parameters.Add("@idade", SqlDbType.NVarChar).Value = ex[i].idade;
                cmd.Parameters.Add("@cidade", SqlDbType.NVarChar).Value = ex[i].cidade;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            MessageBox.Show("Dados gravados com sucesso!", "Gravação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CarregaDgvBanco();
            MdgvExemplo.DataSource = null;
            MdgvExemplo.Refresh();
        }
    }
}
