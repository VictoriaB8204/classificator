﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Установка_типа_признака : Form
    {
        SqlConnection sqlConnection;
        string type = "";

        public Установка_типа_признака()
        {
            InitializeComponent();
        }
    
        private async void Установка_типа_признака_Load(object sender, EventArgs e)
        {
            //Центрирование формы
            if (Owner != null)
                Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2,
                Owner.Location.Y + Owner.Height / 2 - Height / 2);

            //заполнение значениями из БД
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\учеба\Смагин\Реализация классификатора\WindowsFormsApp1\WindowsFormsApp1\Database1.mdf;Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.OpenAsync();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT [Feature] FROM [Feature] ORDER BY Feature", sqlConnection);
            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    comboBox1.Items.Add(Convert.ToString(sqlReader["Feature"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        private void Установка_типа_признака_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
            // вызываем главную форму приложения, которая открыла текущую форму Form2, главная форма всегда = 0
            Form ifrm = Application.OpenForms[1];
            ifrm.Show();
        }

        private void SetTypeOnForm(string type)
        {
            if (type == "Скалярный")
                radioButton1.Checked = true;
            else
                radioButton1.Checked = false;

            if (type == "Размерный")
                radioButton2.Checked = true;
            else
                radioButton2.Checked = false;

            if (type == "Логический")
                radioButton3.Checked = true;
            else
                radioButton3.Checked = false;
        }
        
        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //заполнение значениями из БД
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT [Type], [Feature] FROM [Feature] WHERE [Feature]=@feature", sqlConnection);
            command.Parameters.AddWithValue("feature", comboBox1.SelectedItem);
            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                await sqlReader.ReadAsync();

                type = Convert.ToString(sqlReader["Type"]);
                SetTypeOnForm(type);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            string selectedType = "";

            if (radioButton1.Checked) selectedType = "Скалярный";
            if (radioButton2.Checked) selectedType = "Размерный";
            if (radioButton3.Checked) selectedType = "Логический";

            if (type != "" && type != selectedType)
            {
                var result = MessageBox.Show("Вы уверены, что хотите изменить тип признака?\nВозможные значения признака и значения признака для классов будут утеряны.", "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                { 
                    if(type == "Скалярный")
                    {
                        SqlCommand command1 = new SqlCommand("DELETE ScalarValues " +
                            "FROM Feature INNER JOIN ScalarValues ON Feature.Id=ScalarValues.Feature " +
                            "WHERE [Feature]=@feature", sqlConnection);

                        command1.Parameters.AddWithValue("feature", comboBox1.SelectedItem);
                        await command1.ExecuteNonQueryAsync();

                        SqlCommand command2 = new SqlCommand("DELETE ClassScalarValues " +
                            "FROM (Feature INNER JOIN [Feature description] ON Feature.Id=[Feature description].Feature) " +
                            "INNER JOIN ClassScalarValues ON ClassScalarValues.Feature=[Feature description].Id " +
                            "WHERE [Feature]=@feature", sqlConnection);

                        command2.Parameters.AddWithValue("feature", comboBox1.SelectedItem);
                        await command2.ExecuteNonQueryAsync();
                    }

                    if (type == "Размерный")
                    {
                        SqlCommand command1 = new SqlCommand("DELETE DimentionValue " +
                            "FROM Feature INNER JOIN DimentionValue ON Feature.Id=DimentionValue.Feature " +
                            "WHERE [Feature]=@feature", sqlConnection);

                        command1.Parameters.AddWithValue("feature", comboBox1.SelectedItem);
                        await command1.ExecuteNonQueryAsync();

                        SqlCommand command2 = new SqlCommand("DELETE ClassDimentionValue " +
                            "FROM (Feature INNER JOIN [Feature description] ON Feature.Id=[Feature description].Feature) " +
                            "INNER JOIN ClassDimentionValue ON ClassDimentionValue.Feature=[Feature description].Id " +
                            "WHERE [Feature]=@feature", sqlConnection);

                        command2.Parameters.AddWithValue("feature", comboBox1.SelectedItem);
                        await command2.ExecuteNonQueryAsync();
                    }

                    if (type == "Логический")
                    {
                        SqlCommand command1 = new SqlCommand("DELETE LogicalValues " +
                            "FROM Feature INNER JOIN LogicalValues ON Feature.Id=LogicalValues.Feature " +
                            "WHERE [Feature]=@feature", sqlConnection);

                        command1.Parameters.AddWithValue("feature", comboBox1.SelectedItem);
                        await command1.ExecuteNonQueryAsync();

                        SqlCommand command2 = new SqlCommand("DELETE ClassLogicalValues " +
                            "FROM (Feature INNER JOIN [Feature description] ON Feature.Id=[Feature description].Feature) " +
                            "INNER JOIN ClassLogicalValues ON ClassLogicalValues.Feature=[Feature description].Id " +
                            "WHERE [Feature]=@feature", sqlConnection);

                        command2.Parameters.AddWithValue("feature", comboBox1.SelectedItem);
                        await command2.ExecuteNonQueryAsync();
                    }

                    SqlCommand command = new SqlCommand("UPDATE [Feature] SET [Type]=@type WHERE [Feature]=@feature", sqlConnection);
                    command.Parameters.AddWithValue("feature", comboBox1.SelectedItem);
                    command.Parameters.AddWithValue("type", selectedType);
                    await command.ExecuteNonQueryAsync();
                }
                else
                {
                    SetTypeOnForm(type);
                }
            }

            if (radioButton1.Checked)
            {
                //открыть форму для скаляр знач
            }

            if (radioButton2.Checked)
            {
                //открыть форму для размер знач
            }

            if (radioButton3.Checked)
            {
                //открыть форму для логич знач
            }
        }
    }
}
