using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;

namespace Yui.DataBase
{
    public class SQL:Builder
    {
        #region Propiedades Privadas
        protected SqlConnection con1;
        protected MySqlConnection con2;
        protected TipoConexion Tipo;
        protected Boolean DebugMode = false;
        protected Boolean _Status = false;
        #endregion
        #region Propiedades Publicas
        /// <summary>
        /// Devuelve el Status de la conexion
        /// </summary>
        public Boolean Status
        {
            get
            {
                return _Status;
            }
        }
        /// <summary>
        /// Devuelve la cantidad de filas afectadas al ejecutar Insert, Update o Delete
        /// </summary>
        public int Affected_rows { get; set; }
        #endregion

        #region Metodos Inicializadores
        /// <summary>
        /// Inicializa el objeto SQL vacio, requiere llamar Inicializar
        /// </summary>
        public SQL()
            : base()
        {
            Tipo = TipoConexion.MYSQL;
        }
        /// <summary>
        /// Inicializa el objeto SQL, NO requiere llamar Inicializar
        /// </summary>
        /// <param name="c">
        /// Configuracion SQL
        /// </param>
        public SQL(SQLConfig c)
            : base()
        {
            Inicializar(c);
        }
        /// <summary>
        /// Inicializa el objeto SQL, NO requiere llamar Inicializar
        /// </summary>
        /// <param name="Host">
        /// Direccion del servidor
        /// </param>
        /// <param name="User">
        /// Usuario con el que se conectara al servidor
        /// </param>
        /// <param name="Pass">
        /// Password con el que se conectara al servidor
        /// </param>
        /// <param name="DB">
        /// Base de datos a la que se conectara
        /// </param>
        /// <param name="tipo">
        /// Tipo de servidor al que se conectara
        /// </param>
        public SQL(String Host, String User, String Pass, String DB, TipoConexion tipo)
            :base()
        {
            SQLConfig c = new SQLConfig(Host, User, Pass, DB, tipo, "", false);
            Inicializar(c);
        }
        /// <summary>
        /// Inicializa la conexion con el servidor
        /// </summary>
        /// <param name="c">
        /// configuracion para la conexion
        /// </param>
        public void Inicializar(SQLConfig c)
        {
            Tipo = c.Tipo;
            Conexion(c);
        }
        private void Conexion(SQLConfig c)
        {
            switch (Tipo)
            {
                case TipoConexion.MSSQL:
                    try
                    {
                        con1 = new SqlConnection("data source =" + c.Host + "; initial catalog =" + c.DB + "; user id =" + c.User + "; password =" + c.Pass + "");
                        _Status = true;
                    }
                    catch (Exception ex)
                    {
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Problema al intentar inicializar la conexion: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                    break;
                case TipoConexion.MYSQL:
                    try
                    {
                        con2 = new MySqlConnection("server=" + c.Host + ";User Id=" + c.User + ";password=" + c.Pass + ";Persist Security Info=True;database=" + c.DB + ";Convert Zero Datetime=True");
                        _Status = true;
                    }
                    catch (Exception ex)
                    {
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Problema al intentar inicializar la conexion: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                    break;
                default:
                    _Status = false;
                    break;
            }
        }
        #endregion

        #region Metodos Publicos
        /// <summary>
        /// Permite ejecutar una consulta SQL indicando todo el sintaxis SQL
        /// </summary>
        /// <param name="sql">
        /// Sintaxis SQL a ejecutar
        /// </param>
        /// <returns>
        /// Devuelve Objeto SQL para procesar los datos que retorna
        /// </returns>
        public Estructura.ObjSQL SelectRaw(String sql)
        {
            return ExecuteQuery(sql);
        }
        /// <summary>
        /// Permite ejecutar un Insert especificando la sintaxis SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int InsertRaw(String sql)
        {
            ExecuteNonQuery(sql);
            return Affected_rows;
        }       
        public int UpdateRaw(String sql)
        {
            ExecuteNonQuery(sql);
            return Affected_rows;
        }
        public int DeleteRaw(String sql)
        {
            ExecuteNonQuery(sql);
            return Affected_rows;
        }
        #endregion

        #region Metodos Privados
        private Estructura.ObjSQL ExecuteQuery(String sql)
        {
            Estructura.ObjSQL s = new Estructura.ObjSQL();
            switch (Tipo)
            {
                case TipoConexion.MSSQL:
                    try
                    {
                        con1.Open();
                        SqlDataAdapter ADP = new SqlDataAdapter();
                        DataSet ds = new DataSet();
                        ADP.SelectCommand = new SqlCommand(sql, con1)
                        {
                            CommandTimeout = 240
                        };
                        ADP.Fill(ds);
                        s = new Estructura.ObjSQL(ds);
                    }
                    catch (SqlException ex)
                    {
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Imposible ejecutar la consulta: " + sql + "\n" +
                                "Error: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Imposible ejecutar la consulta: " + sql + "\n" +
                                "Error: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                    finally
                    {
                        con1.Close();
                        SqlConnection.ClearAllPools();
                    }
                    break;
                case TipoConexion.MYSQL:
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand()
                        {
                            Connection = con2,
                            CommandText = sql,
                            CommandType = CommandType.Text
                        };
                        con2.Open();
                        MySqlDataAdapter ADP = new MySqlDataAdapter();
                        DataSet DS = new DataSet();
                        ADP = new MySqlDataAdapter(cmd);
                        ADP.Fill(DS);
                        s = new Estructura.ObjSQL(DS);
                    }
                    catch (SqlException ex)
                    {
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Imposible ejecutar la consulta: " + sql + "\n" +
                                "Error: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Imposible ejecutar la consulta: " + sql + "\n" +
                                "Error: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                    finally
                    {
                        con2.Close();
                        MySqlConnection.ClearAllPools();
                    }
                    break;
            }
            return s;
        }
        private void ExecuteNonQuery(String sql)
        {
            switch (Tipo)
            {
                case TipoConexion.MSSQL:
                    try
                    {
                        con1.Open();
                        SqlCommand cmd = new SqlCommand(sql, con1);
                        Affected_rows = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Imposible ejecutar la consulta: " + sql + "\n" +
                                "Error: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Imposible ejecutar la consulta: " + sql + "\n" +
                                "Error: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                    finally
                    {
                        con1.Close();
                        SqlConnection.ClearAllPools();
                    }
                    break;
                case TipoConexion.MYSQL:
                    try
                    {
                        con2.Open();
                        MySqlCommand cmd = new MySqlCommand(sql, con2);
                        Affected_rows = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Imposible ejecutar la consulta: " + sql + "\n" +
                                "Error: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Imposible ejecutar la consulta: " + sql + "\n" +
                                "Error: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                    finally
                    {
                        con2.Close();
                        MySqlConnection.ClearAllPools();
                    }
                    break;
            }
        }
        #endregion

    }
}
