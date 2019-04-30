using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yui.Estructuras
{
    class ObjDataToGrid
    {
        private DataTable Datos = new DataTable("Consultar");
        private DataSet mydataset = new DataSet();
        private BindingSource datosds;

        public ObjDataToGrid()
        {

        }
        public void Limpiar()
        {
            try
            {
                Datos.Rows.Clear();
                Datos.Columns.Clear();
                Datos.Clear();
                mydataset.Reset();
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    "Imposible limpiar los datos.\n" +
                    "Error: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        public void AddColumn(string nombre)
        {
            try
            {
                Datos.Columns.Add(nombre);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Imposible agregar columna\n" +
                    "Error: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        public void AddRow(DataRow d)
        {
            try
            {
                Datos.Rows.Add(d);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Imposible agregar row\n" +
                    "Error: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        public DataRow NewRow()
        {
            return Datos.NewRow();
        }
        public BindingSource MostrarGrid(string orden = "")
        {
            mydataset.Tables.Add(Datos);
            datosds = new BindingSource
            {
                DataMember = "Consultar",
                DataSource = mydataset
            };
            if (orden != "")
            {
                datosds.Sort = orden;
            }
            return datosds;
        }
        public DataView Filtrar(String condicion, String orden)
        {
            return new DataView(Datos.DataSet.Tables[0], condicion, orden, DataViewRowState.CurrentRows);
        }
        public void EditInfo(int row, int col, String  info)
        {
            mydataset.Tables[0].Rows[row].ItemArray[col] = info;
        }
        public String ConsultInfo(int row, int col)
        {
            return mydataset.Tables[0].Rows[row].ItemArray[col].ToString();
        }
        public int CountReg()
        {
            return mydataset.Tables[0].Rows.Count;
        }
        public void PausarDatos()
        {
            datosds.SuspendBinding();
        }
        public void ReanudarDatos()
        {
            datosds.ResumeBinding();
        }

    }
}
