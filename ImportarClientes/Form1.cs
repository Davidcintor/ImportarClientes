using System.Data;
using Microsoft.Data.SqlClient;

namespace ImportarClientes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt",
                    Title = "Select a Text File"
                };

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFile.FileName;
                    string[] lines = File.ReadAllLines(filePath);


                    foreach (string line in lines)
                    {
                        if (line.IndexOf("first_name") > -1)
                        {
                            continue;
                        }
                        string[] fields = line.Split('|');
                        Cliente cliente = new Cliente
                        {
                            Nombre = fields[0],
                            Apellido = fields[1],
                            Email = fields[2],
                            Telefono = fields[3],
                        };

                        GuardarCliente(cliente);
                    };
                }
                MessageBox.Show("Clientes importados correctamente");
            }
            catch (Exception ex )
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void GuardarCliente(Cliente cliente)
        {
            try
            {
                string query = "INSERT INTO Clientes" +
        " (Nombre, Apellidos, Correo, Telefono)" +
        " VALUES" +
        " (@Nombre, @Apellidos, @Correo, @Telefono)";

                string connectionString = "Server=10.7.3.53;Database=ClientesDB;User ID=sa;Password=123456789;TrustServerCertificate=True;";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;


                        cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                        cmd.Parameters.AddWithValue("@Apellidos", cliente.Apellido);
                        cmd.Parameters.AddWithValue("@Correo", cliente.Email);
                        cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);

                        con.Open();
                        int registrosAfectados = cmd.ExecuteNonQuery();

                        if (registrosAfectados == 0)
                        {
                            throw new Exception("No se pudo agregar cliente");
                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
