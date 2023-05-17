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

namespace Cadastro_de_Leitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void LoadId()
        {
            using (SqlConnection connection = DaoConnection.GetConexao())
            {
                SqlCommand cmd = new SqlCommand("SELECT IDENT_CURRENT('mvtBibLeitor') + 1", connection);
                int proximoID = Convert.ToInt32(cmd.ExecuteScalar());


                txtCodLeitor.Text = proximoID.ToString();
            }
        }
        private void CarregarUsuariosGrid()
        {
            dadosGrid.Rows.Clear();
            using (SqlConnection connection = DaoConnection.GetConexao())
            {
                LeitorDAO dao = new LeitorDAO(connection);
                List<LeitorModel> leitores = dao.GetLeitores();
                foreach (LeitorModel leitor in leitores)
                {
                    DataGridViewRow row = dadosGrid.Rows[dadosGrid.Rows.Add()];
                    row.Cells[colCodigoLeitor.Index].Value = leitor.codLeitor;
                    row.Cells[colNomeDoLeitor.Index].Value = leitor.nomeLeitor;
                    row.Cells[colCpf.Index].Value = leitor.cpfLeitor;
                    row.Cells[colEmail.Index].Value = leitor.emailLeitor;
                    row.Cells[colRG.Index].Value = leitor.rgLeitor;
                    row.Cells[colCelular.Index].Value = leitor.celularLeitor;
                    row.Cells[colTelefone.Index].Value = leitor.telefoneLeitor;
                    row.Cells[colDataNasc.Index].Value = leitor.dataNasc;
                    row.Cells[colSexoDoLeitor.Index].Value = leitor.sexoLeitor;
                    row.Cells[colRua.Index].Value = leitor.ruaLeitor;
                    row.Cells[colBairro.Index].Value = leitor.bairroLeitor;
                    row.Cells[colCidade.Index].Value = leitor.cidadeLeitor;
                    row.Cells[colUf.Index].Value = leitor.ufLeitor;
                    row.Cells[colNumero.Index].Value = leitor.numeroResidencial;
                    row.Cells[colCep.Index].Value = leitor.cepResidencial;
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CarregarUsuariosGrid();
            LoadId();
            btnExcluir.Enabled = false;
        }
        private void ApagarCampos()
        {
            txtNomeLeitor.Text = "";
            txtCpf.Text = "";
            txtEmail.Text = "";
            txtRg.Text = "";
            txtCelular.Text = "";
            txtTelefone.Text = "";
            txtDataNasc.Text = "";
            txtsexoLeitor.Text = "";
            txtRua.Text = "";
            txtBairro.Text = "";
            txtCidade.Text = "";
            txtUf.Text = "";
            txtNumero.Text = "";
            txtCep.Text = "";

        }
        private void btnSalvar_Click(object sender, EventArgs e)
        {

            try
            {
                using (SqlConnection connection = DaoConnection.GetConexao())
                {
                    LeitorDAO dao = new LeitorDAO(connection);

                    bool val = dao.Validacoes(new LeitorModel()
                    {
                        nomeLeitor = txtNomeLeitor.Text,
                        dataNasc = txtDataNasc.Text,
                        sexoLeitor = txtsexoLeitor.Text,
                        ruaLeitor = txtRua.Text,
                        bairroLeitor = txtBairro.Text,
                        cidadeLeitor = txtCidade.Text,
                        ufLeitor = txtUf.Text,


                    });
                    if (val)
                    {


                        string sql2 = "SELECT COUNT(codLeitor) FROM mvtBibLeitor WHERE codLeitor = @codLeitor";
                        SqlCommand cmdSelect = new SqlCommand(sql2, connection);
                        cmdSelect.Parameters.AddWithValue("@codLeitor", txtCodLeitor.Text);
                        int count = Convert.ToInt32(cmdSelect.ExecuteScalar());



                        if (count > 0)
                        {
                            dao.Alterar(new LeitorModel()
                            {   
                                codLeitor = txtCodLeitor.Text,
                                nomeLeitor = txtNomeLeitor.Text,
                                cpfLeitor = txtCpf.Text.Replace(".", "").Replace("-", ""),
                                emailLeitor = txtEmail.Text,
                                rgLeitor = txtRg.Text.Replace(".", "").Replace("-", ""),
                                celularLeitor = txtCelular.Text.Replace("(", "").Replace(")", "").Replace("-", ""),
                                telefoneLeitor = txtTelefone.Text.Replace("(", "").Replace(")", "").Replace("-", ""),
                                dataNasc = txtDataNasc.Text,
                                sexoLeitor = txtsexoLeitor.Text,
                                ruaLeitor = txtRua.Text,
                                bairroLeitor = txtBairro.Text,
                                cidadeLeitor = txtCidade.Text,
                                ufLeitor = txtUf.Text,
                                numeroResidencial = txtNumero.Text,
                                cepResidencial = txtCep.Text.Replace(".", "").Replace("-", ""),
                            }); MessageBox.Show("Leitor atualizado com sucesso!");
                        }
                        else
                        {
                            dao.Salvar(new LeitorModel()
                            {
                                nomeLeitor = txtNomeLeitor.Text,
                                cpfLeitor = txtCpf.Text.Replace(".","").Replace("-",""),
                                emailLeitor = txtEmail.Text,
                                rgLeitor = txtRg.Text.Replace(".", "").Replace("-", ""),
                                celularLeitor = txtCelular.Text.Replace("(","").Replace(")","").Replace("-", ""),
                                telefoneLeitor = txtTelefone.Text.Replace("(", "").Replace(")", "").Replace("-", ""),
                                dataNasc = txtDataNasc.Text,
                                sexoLeitor = txtsexoLeitor.Text,
                                ruaLeitor = txtRua.Text,
                                bairroLeitor = txtBairro.Text,
                                cidadeLeitor = txtCidade.Text,
                                ufLeitor = txtUf.Text,
                                numeroResidencial = txtNumero.Text,
                                cepResidencial = txtCep.Text.Replace(".", "").Replace("-", ""),
                            });
                            MessageBox.Show("Leitor salvo com sucesso!");
                        }
                    }


                    
                    // se salvar deu certo carrega os usuários
                    CarregarUsuariosGrid();
                    LoadId();
                    ApagarCampos();
                    btnExcluir.Enabled = false;

                }
            }
            catch (Exception ex)
            {
                // se salvar deu errado mostra mensagem
                MessageBox.Show($"Houve um problema ao salvar o Leitor!\n{ex.Message}");
            }
        }

        private void dadosGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                txtCodLeitor.Text = dadosGrid.Rows[e.RowIndex].Cells[colCodigoLeitor.Index].Value + "";
                txtNomeLeitor.Text = dadosGrid.Rows[e.RowIndex].Cells[colNomeDoLeitor.Index].Value + "";
                txtCpf.Text = dadosGrid.Rows[e.RowIndex].Cells[colCpf.Index].Value + "";
                txtEmail.Text = dadosGrid.Rows[e.RowIndex].Cells[colEmail.Index].Value + "";
                txtRg.Text = dadosGrid.Rows[e.RowIndex].Cells[colRG.Index].Value + "";
                txtCelular.Text = dadosGrid.Rows[e.RowIndex].Cells[colCelular.Index].Value + "";
                txtTelefone.Text = dadosGrid.Rows[e.RowIndex].Cells[colTelefone.Index].Value + "";
                txtDataNasc.Text = dadosGrid.Rows[e.RowIndex].Cells[colDataNasc.Index].Value + "";
                txtsexoLeitor.Text = dadosGrid.Rows[e.RowIndex].Cells[colSexoDoLeitor.Index].Value + "";
                txtRua.Text = dadosGrid.Rows[e.RowIndex].Cells[colRua.Index].Value + "";
                txtBairro.Text = dadosGrid.Rows[e.RowIndex].Cells[colBairro.Index].Value + "";
                txtCidade.Text = dadosGrid.Rows[e.RowIndex].Cells[colCidade.Index].Value + "";
                txtUf.Text = dadosGrid.Rows[e.RowIndex].Cells[colUf.Index].Value + "";
                txtNumero.Text = dadosGrid.Rows[e.RowIndex].Cells[colNumero.Index].Value + "";
                txtCep.Text = dadosGrid.Rows[e.RowIndex].Cells[colCep.Index].Value + "";

                using (SqlConnection connection = DaoConnection.GetConexao())
                {
                    LeitorDAO dao = new LeitorDAO(connection);

                    bool val = dao.Validacoes(new LeitorModel()
                    {
                        nomeLeitor = txtNomeLeitor.Text,
                        dataNasc = txtDataNasc.Text,
                        sexoLeitor = txtsexoLeitor.Text,
                        ruaLeitor = txtRua.Text,
                        bairroLeitor = txtBairro.Text,
                        cidadeLeitor = txtCidade.Text,
                        ufLeitor = txtUf.Text,


                    });
                    if (val)
                    {
                        btnExcluir.Enabled = true;
                    }





                }
            }

            }
            private void btnExcluir_Click(object sender, EventArgs e)
            {
                DialogResult confirmacao = MessageBox.Show("Deseja excluir o leitor?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                try
                {
                    if (confirmacao == DialogResult.Yes)
                    {
                        using (SqlConnection connection = DaoConnection.GetConexao())
                        {
                            LeitorDAO dao = new LeitorDAO(connection);

                            bool val = dao.Validacoes(new LeitorModel()
                            {
                                nomeLeitor = txtNomeLeitor.Text,
                                dataNasc = txtDataNasc.Text,
                                sexoLeitor = txtsexoLeitor.Text,
                                ruaLeitor = txtRua.Text,
                                bairroLeitor = txtBairro.Text,
                                cidadeLeitor = txtCidade.Text,
                                ufLeitor = txtUf.Text,


                            });
                            if (val)
                            {
                                dao.Excluir(new LeitorModel()
                                {
                                    codLeitor = txtCodLeitor.Text
                                });
                            }
                        }
                        MessageBox.Show("Leitor excluída com sucesso!");
                        CarregarUsuariosGrid();
                        LoadId();
                        btnExcluir.Enabled = true;
                        ApagarCampos();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Houve um problema ao excluir o leitor!\n{ex.Message}");
                }
            }

        private void label20_MouseHover(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            string mensagem = "Este Campo é obrigatório!";
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(label, mensagem);
        }

        private void label19_MouseHover(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            string mensagem = "Este Campo é obrigatório!";
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(label, mensagem);
        }

        private void label21_MouseHover(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            string mensagem = "Este Campo é obrigatório!";
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(label, mensagem);
        }

        private void label9_MouseHover(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            string mensagem = "Este Campo é obrigatório!";
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(label, mensagem);
        }

        private void label17_MouseHover(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            string mensagem = "Este Campo é obrigatório!";
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(label, mensagem);
        }

        private void label15_MouseHover(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            string mensagem = "Este Campo é obrigatório!";
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(label, mensagem);
        }

        private void label16_MouseHover(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            string mensagem = "Este Campo é obrigatório!";
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(label, mensagem);
        }

        private void label22_MouseHover(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            string mensagem = "Este Campo é obrigatório!";
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(label, mensagem);
        }
        private void InfoAsterisco(object sender, EventArgs e) 
        {
            Label label = (Label)sender;
            string mensagem = "Este Campo é obrigatório!";
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(label, mensagem);
        } 
    }   
}  

