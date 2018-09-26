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
        protected new TipoConexion Tipo;        
        protected Boolean _Status = false;
        protected List<String> esql;
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
        /// <summary>
        /// Devuelve o Establece el estado de Debug, lo que hace que el sistema informe de cualquier error
        /// </summary>
        public Boolean DebugMode { get; set; } = false;
        /// <summary>
        /// Devuelve o Establece la ultima consultas SQL
        /// </summary>
        public String LastQuery { get; set; } = "";
        /// <summary>
        /// Devuelve o Establece el estado de mantener la consulta, esta propiedad se usa solo para hacer Commit
        /// </summary>
        public Boolean Preserve { get; set; } = false;
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
            base.TipoDB = c.Tipo;
            esql = new List<string>();
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
        #region Metodos Manuales
        /// <summary>
        /// Permite ejecutar una consulta SQL indicando todo el sintaxis SQL
        /// </summary>
        /// <param name="sql">
        /// Sintaxis SQL a ejecutar
        /// </param>
        /// <returns>
        /// Devuelve Objeto SQL para procesar los datos que retorna
        /// </returns>
        public Estructura.ObjSQL Query(String sql)
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
            if (Preserve)
            {
                esql.Add(sql);
            }
            else
            {
                ExecuteNonQuery(sql);
            }
            return Affected_rows;
        }
        /// <summary>
        /// Permite ejecutar un Update especificando la sintexis SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int UpdateRaw(String sql)
        {
            if (Preserve)
            {
                esql.Add(sql);
            }
            else
            {
                ExecuteNonQuery(sql);
            }
            return Affected_rows;
        }
        /// <summary>
        /// Permite ejecutar un Delete especificando la sintexis SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int DeleteRaw(String sql)
        {
            if (Preserve)
            {
                esql.Add(sql);
            }
            else
            {
                ExecuteNonQuery(sql);
            }
            return Affected_rows;
        }
        #endregion
        #region Metodos Autogenerados
        #region Select
        /// <summary>
        /// Ejecuta la consulta SQL, los parametros de configuracion deben estar establecidos previamente
        /// </summary>
        /// <returns></returns>
        public Estructura.ObjSQL Get()
        {
            String sql = base.Generar();
            return ExecuteQuery(sql);
        }
        /// <summary>
        /// Ejecuta la consulta SQL con la tabla asignada si no hay otros parametros configurados realizara la consulta SELECT * FROM TABLA 
        /// </summary>
        /// <param name="tabla"></param>
        /// <returns></returns>
        public Estructura.ObjSQL Get(String tabla)
        {
            base.Tabla(tabla);
            String sql = base.Generar();
            return ExecuteQuery(sql);
        }
        /// <summary>
        /// Ejecuta la consulta SQL con la tabla y las condiciones asignadas
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public Estructura.ObjSQL Get(String tabla, Dictionary<String, Object> where)
        {
            base.Tabla(tabla);
            base.Where(where);
            String sql = base.Generar();
            return ExecuteQuery(sql);
        }
        /// <summary>
        /// Ejecuta la consulta SQL solicitando los campos con la tabla y las condiciones asignadas
        /// </summary>
        /// <param name="campos"></param>
        /// <param name="tabla"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public Estructura.ObjSQL Get(String campos, String tabla, Dictionary<String, Object> where)
        {
            base.SetCampos(campos);
            base.Tabla(tabla);
            base.Where(where);
            String sql = base.Generar();
            return ExecuteQuery(sql);
        }
        #endregion
        #region Insert
        /// <summary>
        /// Realiza un insert, requiere que se hayan establecido campos, valores y la tabla previamente
        /// </summary>
        /// <returns></returns>
        public int Insert()
        {
            base.Tipo = TipoQuery.INSERT;
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
            }
            else
            {
                ExecuteNonQuery(sql);
            }
            return Affected_rows;
        }
        /// <summary>
        /// Permite realizar un insert, requiere que se hayan establecido campos y valores previamente
        /// </summary>
        /// <param name="tabla"></param>
        /// <returns></returns>
        public int Insert(String tabla)
        {
            base.Tipo = TipoQuery.INSERT;
            base.Tabla(tabla);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
            }
            else
            {
                ExecuteNonQuery(sql);
            }
            return Affected_rows;
        }
        /// <summary>
        /// Permite realizar un insert, con la tabla y los campos indicados
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <returns></returns>
        public int Insert(String tabla, Dictionary<String, Object> campos)
        {
            base.Tipo = TipoQuery.INSERT;
            base.Tabla(tabla);
            base.SetCampos(campos);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
            }
            else
            {
                ExecuteNonQuery(sql);
            }
            return Affected_rows;
        }
        #endregion
        #region Update
        /// <summary>
        /// Ejecuta un update, los parametros deben estar establecidos previamente
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
            base.Tipo = TipoQuery.UPDATE;
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
            }
            else
            {
                ExecuteNonQuery(sql);
            }
            return Affected_rows;
        }
        public int Update(String tabla)
        {
            base.Tipo = TipoQuery.UPDATE;
            base.Tabla(tabla);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
            }
            else
            {
                ExecuteNonQuery(sql);
            }
            return Affected_rows;
        }
        public int Update(String tabla, Dictionary<String, Object> campos)
        {
            base.Tipo = TipoQuery.UPDATE;
            base.Tabla(tabla);
            base.SetCampos(campos);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
            }
            else
            {
                ExecuteNonQuery(sql);
            }
            return Affected_rows;
        }
        public int Update(String tabla, Dictionary<String, Object> campos, Dictionary<String, Object> where)
        {
            base.Tipo = TipoQuery.UPDATE;
            base.Tabla(tabla);
            base.SetCampos(campos);
            base.Where(where);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
            }
            else
            {
                ExecuteNonQuery(sql);
            }
            return Affected_rows;
        }
        #endregion
        #region Delete
        public int Delete()
        {
            base.Tipo = TipoQuery.DELETE;
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
            }
            else
            {
                ExecuteNonQuery(sql);
            }
            
            return Affected_rows;
        }
        public int Delete(String tabla)
        {
            base.Tipo = TipoQuery.DELETE;
            base.Tabla(tabla);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
            }
            else
            {
                ExecuteNonQuery(sql);
            }
            return Affected_rows;
        }
        public int Delete(String tabla, Dictionary<String, Object> where)
        {
            base.Tipo = TipoQuery.DELETE;
            base.Tabla(tabla);
            base.Where(where);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
            }
            else
            {
                ExecuteNonQuery(sql);
            }
            return Affected_rows;
        }
        #endregion
        public Boolean Commit()
        {
            ExecuteNonQueryCommit();
            if (Affected_rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #endregion

        #region Metodos Privados
        private Estructura.ObjSQL ExecuteQuery(String sql)
        {
            LastQuery = sql;
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
            base.NewQuery();
            return s;
        }
        private void ExecuteNonQuery(String sql)
        {
            LastQuery = sql;
            Affected_rows = 0;
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
            base.NewQuery();
        }
        private void ExecuteNonQueryCommit()
        {
            LastQuery = "";
            Affected_rows = 0;
            switch (Tipo)
            {
                case TipoConexion.MSSQL:
                    con1.Open();

                    SqlCommand cmd1 = con1.CreateCommand();
                    SqlTransaction transaction1;

                    transaction1 = con1.BeginTransaction();
                    cmd1.Connection = con1;
                    cmd1.Transaction = transaction1;
                    try
                    {
                        foreach (String item in esql)
                        {
                            cmd1.CommandText = item;
                            Affected_rows += cmd1.ExecuteNonQuery();
                        }
                        transaction1.Commit();
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            transaction1.Rollback();
                        }
                        catch (SqlException ex)
                        {
                            if (DebugMode)
                            {
                                MessageBox.Show(
                                    "Imposible ejecutar el Rollback \n" +
                                    "Error: " + ex.Message,
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error
                                );
                            }
                        }
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Imposible ejecutar el commit \n" +
                                "Error: " + e.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                    finally
                    {
                        con1.Close();
                        esql = new List<string>();
                        Preserve = false;
                    }
                    break;
                case TipoConexion.MYSQL:
                    con2.Open();
                    MySqlCommand cmd = con2.CreateCommand();
                    MySqlTransaction transaction;
                    transaction = con2.BeginTransaction();
                    cmd.Connection = con2;
                    cmd.Transaction = transaction;
                    try
                    {
                        foreach (String item in esql)
                        {
                            cmd.CommandText = item;
                            Affected_rows += cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (SqlException ex)
                        {
                            if (DebugMode)
                            {
                                MessageBox.Show(
                                    "Imposible ejecutar el Rollback \n" +
                                    "Error: " + ex.Message,
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error
                                );
                            }
                        }
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Imposible ejecutar el commit \n" +
                                "Error: " + e.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                    finally
                    {
                        con2.Close();
                        esql = new List<string>();
                        Preserve = false;
                    }
                    break;
            }
        }
        #endregion

    }
}
