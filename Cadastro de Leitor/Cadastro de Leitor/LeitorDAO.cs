using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastro_de_Leitor
{
    internal class LeitorDAO
    {
        private SqlConnection Connection { get; }
        public LeitorDAO(SqlConnection connection)
        {
            Connection = connection;
        }
        public void Salvar(LeitorModel leitor)
        {
            using (SqlCommand command = Connection.CreateCommand())
            {
                SqlTransaction t = Connection.BeginTransaction();
                try
                {
                    //Excluir(codAutor, t);

                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine($"INSERT INTO mvtBibLeitor(Leitor,sexo,dataNascimento,cpf,rg,email,telefone,telefoneCelular,enderecoNome,enderecoBairro,enderecoCidade,enderecoCEP,enderecoUF,enderecoNumero) VALUES(@nome,@sexo,@data,@cpf,@rg,@email,@telefone,@celular,@rua,@bairro,@cidade,@cep,@uf,@enderecoNumero)");
                    command.CommandText = sql.ToString();
                    command.Parameters.Add(new SqlParameter("@nome", leitor.nomeLeitor));
                    command.Parameters.Add(new SqlParameter("@sexo", leitor.sexoLeitor));
                    command.Parameters.Add(new SqlParameter("@data", leitor.dataNasc));
                    command.Parameters.Add(new SqlParameter("@cpf", leitor.cpfLeitor));
                    command.Parameters.Add(new SqlParameter("@rg", leitor.rgLeitor));
                    command.Parameters.Add(new SqlParameter("@email", leitor.emailLeitor));
                    command.Parameters.Add(new SqlParameter("@telefone", leitor.telefoneLeitor));
                    command.Parameters.Add(new SqlParameter("@celular", leitor.celularLeitor));
                    command.Parameters.Add(new SqlParameter("@rua", leitor.ruaLeitor));
                    command.Parameters.Add(new SqlParameter("@bairro", leitor.bairroLeitor));
                    command.Parameters.Add(new SqlParameter("@cidade", leitor.cidadeLeitor));
                    command.Parameters.Add(new SqlParameter("@cep", leitor.cepResidencial));
                    command.Parameters.Add(new SqlParameter("@uf", leitor.ufLeitor));
                    command.Parameters.Add(new SqlParameter("@enderecoNumero", leitor.numeroResidencial));
                   
                    command.Transaction = t;
                    command.ExecuteNonQuery();
                    t.Commit();
                }

                catch (Exception ex)
                {
                    t.Rollback();
                    throw ex;
                }

            }
        }
        public void Alterar(LeitorModel leitor)
        {
            using (SqlCommand command = Connection.CreateCommand())
            {
                SqlTransaction t = Connection.BeginTransaction();
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine($"UPDATE mvtBibLeitor SET Leitor = @nome, sexo = @sexo, dataNascimento = @data, cpf = @cpf, rg = @rg, email = @email, telefone = @telefone, telefoneCelular = @telefoneCelular, enderecoNome = @enderecoNome, enderecoBairro = @enderecoBairro, enderecoCidade = @enderecoCidade, enderecoCEP = @enderecoCEP, enderecoUF = @enderecoUF, enderecoNumero = @enderecoNumero WHERE codLeitor = @codLeitor");
                    command.CommandText = sql.ToString();
                    command.Parameters.AddWithValue("@codLeitor", leitor.codLeitor);
                    command.Parameters.Add(new SqlParameter("@nome", leitor.nomeLeitor));
                    command.Parameters.Add(new SqlParameter("@sexo", leitor.sexoLeitor));
                    command.Parameters.Add(new SqlParameter("@data", leitor.dataNasc));
                    command.Parameters.Add(new SqlParameter("@cpf", leitor.cpfLeitor));
                    command.Parameters.Add(new SqlParameter("@rg", leitor.rgLeitor));
                    command.Parameters.Add(new SqlParameter("@email", leitor.emailLeitor));
                    command.Parameters.Add(new SqlParameter("@telefone", leitor.telefoneLeitor));
                    command.Parameters.Add(new SqlParameter("@telefoneCelular", leitor.celularLeitor));
                    command.Parameters.Add(new SqlParameter("@enderecoNome", leitor.ruaLeitor));
                    command.Parameters.Add(new SqlParameter("@enderecoBairro", leitor.bairroLeitor));
                    command.Parameters.Add(new SqlParameter("@enderecoCidade", leitor.cidadeLeitor));
                    command.Parameters.Add(new SqlParameter("@enderecoCEP", leitor.cepResidencial));
                    command.Parameters.Add(new SqlParameter("@enderecoUF", leitor.ufLeitor));
                    command.Parameters.Add(new SqlParameter("@enderecoNumero ", leitor.numeroResidencial));

                    command.Transaction = t;
                    command.ExecuteNonQuery();
                    t.Commit();
                }
                catch (Exception ex)
                {
                    t.Rollback();
                    throw ex;
                }
            }
        }
        public void Excluir(LeitorModel leitor, SqlTransaction t = null)
        {
            using (SqlCommand command = Connection.CreateCommand())
            {
                if (t != null)
                {
                    command.Transaction = t;
                }
                StringBuilder sql = new StringBuilder();
                sql.AppendLine($"DELETE FROM mvtBibLeitor WHERE codLeitor = @codLeitor");
                command.CommandText = sql.ToString();
                command.Parameters.Add(new SqlParameter("@codLeitor", leitor.codLeitor));
                command.ExecuteNonQuery();
            }
        }
        public List<LeitorModel> GetEditoras()
        {
            List<LeitorModel> leitores = new List<LeitorModel>();
            using (SqlCommand command = Connection.CreateCommand())
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT LEI.codLeitor, LEI.Leitor, LEI.sexo, LEI.dataNascimento, LEI.cpf, LEI.rg, LEI.email, LEI.telefone, LEI.telefoneCelular, LEI.enderecoNome, LEI.enderecoBairro, LEI.enderecoCidade, LEI.enderecoCEP, LEI.enderecoUF, LEI.enderecoNumero FROM mvtBibLeitor LEI");
                command.CommandText = sql.ToString();
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        leitores.Add(PopulateDr(dr));
                    }
                }
            }
            return leitores;
        }
        private LeitorModel PopulateDr(SqlDataReader dr)
        {
            string codLeitor = "";
            string nomeLeitor = "";
            string cpfLeitor = "";
            string emailLeitor = "";
            string rgLeitor = "";
            string celularLeitor = "";
            string telefoneLeitor = "";
            string dataNasc = "";
            string sexoLeitor = "";
            string ruaLeitor = "";
            string bairroLeitor = "";
            string cidadeLeitor = "";
            string ufLeitor = "";
            string numeroResidencial = "";
            string cepResidencial = "";
            if (DBNull.Value != dr["codLeitor"])
            {
                codLeitor = dr["codLeitor"] + "";
            }

            if (DBNull.Value != dr["Leitor"])
            {
                nomeLeitor = dr["Leitor"] + "";
            }

            if (DBNull.Value != dr["cpf"])
            {
                cpfLeitor = dr["cpf"] + "";
            }

            if (DBNull.Value != dr["email"])
            {
                emailLeitor = dr["email"] + "";
            }

            if (DBNull.Value != dr["rg"])
            {
                rgLeitor = dr["rg"] + "";
            }

            if (DBNull.Value != dr["telefoneCelular"])
            {
                celularLeitor = dr["telefoneCelular"] + "";
            }

            if (DBNull.Value != dr["telefone"])
            {
                telefoneLeitor = dr["telefone"] + "";
            }

            if (DBNull.Value != dr["dataNascimento"])
            {
                dataNasc = dr["dataNascimento"] + "";
            }

            if (DBNull.Value != dr["sexo"])
            {
                sexoLeitor = dr["sexo"] + "";
            }

            if (DBNull.Value != dr["enderecoNome"])
            {
                ruaLeitor = dr["enderecoNome"] + "";
            }

            if (DBNull.Value != dr["enderecoBairro"])
            {
                bairroLeitor = dr["enderecoBairro"] + "";
            }

            if (DBNull.Value != dr["enderecoCidade"])
            {
                cidadeLeitor = dr["enderecoCidade"] + "";
            }

            if (DBNull.Value != dr["enderecoUF"])
            {
                ufLeitor = dr["enderecoUF"] + "";
            }

            if (DBNull.Value != dr["enderecoNumero"])
            {
                numeroResidencial = dr["enderecoNumero"] + "";
            }

            if (DBNull.Value != dr["enderecoCEP"])
            {
                cepResidencial = dr["enderecoCEP"] + "";
            }

            return new LeitorModel()
            {
                codLeitor = codLeitor,
                nomeLeitor = nomeLeitor,
                cpfLeitor = cpfLeitor,
                emailLeitor = emailLeitor,
                rgLeitor = rgLeitor,
                celularLeitor = celularLeitor,
                telefoneLeitor = telefoneLeitor,
                dataNasc = dataNasc,
                sexoLeitor = sexoLeitor,
                ruaLeitor = ruaLeitor,
                bairroLeitor = bairroLeitor,
                cidadeLeitor = cidadeLeitor,
                ufLeitor = ufLeitor,
                numeroResidencial = numeroResidencial,
                cepResidencial = cepResidencial

            };
        }

    }   
}
