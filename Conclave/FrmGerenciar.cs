using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conclave
{
    public partial class FrmGerenciar : Form
    {
        string[][] dados;
        Conclave anterior;
        public FrmGerenciar(Conclave anterior, string[][] dados)
        {
            InitializeComponent();
            this.anterior = anterior;
            this.dados = dados;
            Atualizar();
        }

        int Length(string[][] v)
        {
            int q = 0;
            for (int i = 0; i < v.Length; i++)
            {
                if (v[i] != null)
                    q++;
            }
            return q;
        }

        int Buscar(string nome)
        {
            int indice;
            for (indice = 0; indice < Length(dados) && dados[indice][0] != nome; indice++) ;
            if (indice < Length(dados))
                return indice;
            return -1;
        }

        private void FrmGerenciar_FormClosing(object sender, FormClosingEventArgs e)
        {
            anterior.ReceberDados(dados);
            anterior.Show();
        }

        void Atualizar()
        {
            dgvCardeais.Rows.Clear();
            for (int i = 0; i < Length(dados); i++)
            {
                DataGridViewRow linha = new DataGridViewRow();
                linha.CreateCells(dgvCardeais);
                linha.Cells[0].Value = dados[i][0];
                dgvCardeais.Rows.Add(linha);
            }
        }

        private void btAdicionar_Click(object sender, EventArgs e)
        {
            if (Length(dados) == dados.Length)
            {
                MessageBox.Show("Lista cheia!");
            }

            string nome = txtNome.Text.Trim();

            if (String.IsNullOrEmpty(nome))
            {
                MessageBox.Show("Insira um nome válido!");
                return;
            }

            if (Funcoes.Buscar(nome, dados) > -1)
            {
                MessageBox.Show($"{nome} já cadastrado");
                return;
            }
            dados[Funcoes.Length(dados)] = new string[] { nome, "0" };

            Atualizar();
            txtNome.Text = "";
        }

        private void btRemover_Click(object sender, EventArgs e)
        {
            if (dgvCardeais.SelectedCells.Count == 0)
            {
                MessageBox.Show("Não há linhas selecionadas!");
                return;
            }

            DataGridViewCell cell = dgvCardeais.SelectedCells[0];
            int linha = cell.RowIndex;
            string id = dgvCardeais.Rows[linha].Cells[0].Value?.ToString();

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Valor inválido selecionado.");
                return;
            }

            int indice = -1;
            for (int i = 0; i < Length(dados); i++)
            {
                if (dados[i] != null && dados[i][0] == id)
                {
                    indice = i;
                    break;
                }
            }

            if (indice == -1)
            {
                MessageBox.Show("Item não encontrado na lista de dados.");
                return;
            }

            DialogResult r = MessageBox.Show($"Deseja mesmo remover o {dados[indice][0]}?",
                                             "Confirmação",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

            if (r == DialogResult.Yes)
            {
                for (int i = indice; i < Length(dados) - 1; i++)
                {
                    dados[i] = dados[i + 1];
                }

                dados[Length(dados) - 1] = null;

                Atualizar();
            }
            else
            {
                int linhaRemover = dgvCardeais.SelectedCells[0].RowIndex;
                if (linhaRemover >= 0 && linhaRemover < dgvCardeais.Rows.Count)
                {
                    dgvCardeais.Rows.RemoveAt(linhaRemover);
                }
            }
        }
    }

}

