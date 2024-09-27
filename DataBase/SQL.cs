using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using Yui.Extensiones;
using Yui.DataBase.Atributos;
using System.Web.UI.WebControls;

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
        public long LastId { get; set; }
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
            SQLConfig c = new SQLConfig(Host, User, Pass, DB, tipo, "");
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
                        con1 = new SqlConnection(string.Format("data source ={0}; initial catalog ={1}; user id ={2}; password ={3}", c.Host, c.DB, c.User, c.Pass));
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
                        string stringconexion = string.Format("server={0};User Id={1};password={2};Persist Security Info=True;database={3};Convert Zero Datetime=True", c.Host, c.User, c.Pass, c.DB);
                        con2 = new MySqlConnection(stringconexion);
                        _Status = true;
                    }
                    catch (MySqlException ex)
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
        /// Devuelve el listado de sintaxis SQL preservadas para Commit
        /// </summary>
        /// <returns></returns>
        public List<String> PreserveSQL()
        {
            return esql;
        }
        #region Metodos Manuales
        /// <summary>
        /// Permite ejecutar una consulta SQL indicando todo el sisntaxis SQL
        /// </summary>
        /// <typeparam name="T">Class con el formato que se devolvera la informacion</typeparam>
        /// <param name="sql">Sintaxis SQL a ejecutar</param>
        /// <returns>Devuelve lista de objetos basados en la class indicada</returns>
        public List<T> Query<T>(String sql)
        {
            if (Preserve)
            {
                esql.Add(sql);
                return new List<T>();
            }
            else
            {
                return ExecuteQuery<T>(sql);
            }
        }
        public List<Dictionary<string, object>> QuerybyDictionary(String sql)
        {
            if (Preserve)
            {
                esql.Add(sql);
                return new List<Dictionary<string, object>>();
            }
            else
            {
                return ExecuteQueryDictionary(sql);
            }
        }
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
            if (Preserve)
            {
                esql.Add(sql);
                return new Estructura.ObjSQL();
            }
            else
            {
                return ExecuteQuery(sql);
            }
            
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
        #region Select For Class
        /// <summary>
        /// Ejecuta la consulta SQL, los parametros de configuracion deben estar establecidos previamente
        /// </summary>
        /// <returns></returns>
        public List<T> Get<T>()
        {
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
                return new List<T>();
            }
            else
            {
                return ExecuteQuery<T>(sql);
            }
        }
        /// <summary>
        /// Ejecuta la consulta SQL con la tabla asignada si no hay otros parametros configurados realizara la consulta SELECT * FROM TABLA 
        /// </summary>
        /// <param name="tabla"></param>
        /// <returns></returns>
        public List<T> Get<T>(String tabla)
        {
            base.Tabla(tabla);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
                return new List<T>();
            }
            else
            {
                return ExecuteQuery<T>(sql);
            }
        }
        /// <summary>
        /// Ejecuta la consulta SQL con la tabla y las condiciones asignadas
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<T> Get<T>(String tabla, Dictionary<String, Object> where)
        {
            base.Tabla(tabla);
            base.Where(where);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
                return new List<T>();
            }
            else
            {
                return ExecuteQuery<T>(sql);
            }
        }
        /// <summary>
        /// Ejecuta la consulta SQL solicitando los campos con la tabla y las condiciones asignadas
        /// </summary>
        /// <param name="campos"></param>
        /// <param name="tabla"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<T> Get<T>(String campos, String tabla, Dictionary<String, Object> where)
        {
            base.SetCampos(campos);
            base.Tabla(tabla);
            base.Where(where);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
                return new List<T>();
            }
            else
            {
                return ExecuteQuery<T>(sql);
            }
        }
        #endregion
        #region Select For Class Single
        /// <summary>
        /// Ejecuta la consulta SQL, los parametros de configuracion deben estar establecidos previamente
        /// </summary>
        /// <returns></returns>
        public T GetOne<T>()
        {
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
                return Activator.CreateInstance<T>();
            }
            else
            {
                List<T> list = ExecuteQuery<T>(sql);
                return list.First();
            }
        }
        /// <summary>
        /// Ejecuta la consulta SQL con la tabla asignada si no hay otros parametros configurados realizara la consulta SELECT * FROM TABLA 
        /// </summary>
        /// <param name="tabla"></param>
        /// <returns></returns>
        public T GetOne<T>(String tabla)
        {
            base.Tabla(tabla);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
                return Activator.CreateInstance<T>();
            }
            else
            {
                List<T> list = ExecuteQuery<T>(sql);
                return list.First();
            }
        }
        /// <summary>
        /// Ejecuta la consulta SQL con la tabla y las condiciones asignadas
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public T GetOne<T>(String tabla, Dictionary<String, Object> where)
        {
            base.Tabla(tabla);
            base.Where(where);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
                return Activator.CreateInstance<T>();
            }
            else
            {
                List<T> list = ExecuteQuery<T>(sql);
                return list.First();
            }
        }
        /// <summary>
        /// Ejecuta la consulta SQL solicitando los campos con la tabla y las condiciones asignadas
        /// </summary>
        /// <param name="campos"></param>
        /// <param name="tabla"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public T GetOne<T>(String campos, String tabla, Dictionary<String, Object> where)
        {
            base.SetCampos(campos);
            base.Tabla(tabla);
            base.Where(where);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
                return Activator.CreateInstance<T>();
            }
            else
            {
                List<T> list = ExecuteQuery<T>(sql);
                return list.First();
            }
        }
        #endregion
        #region Select for Dictionary
        public List<Dictionary<string, object>> GetByDictionary()
        {
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
                return new List<Dictionary<string, object>>();
            }
            else
            {
                return ExecuteQueryDictionary(sql);
            }
        }
        public List<Dictionary<string, object>> GetByDictionary(String tabla)
        {
            base.Tabla(tabla);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
                return new List<Dictionary<string, object>>();
            }
            else
            {
                return ExecuteQueryDictionary(sql);
            }
        }
        public List<Dictionary<string, object>> GetByDictionary(String tabla, Dictionary<String, Object> where)
        {
            base.Tabla(tabla);
            base.Where(where);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
                return new List<Dictionary<string, object>>();
            }
            else
            {
                return ExecuteQueryDictionary(sql);
            }
        }
        public List<Dictionary<string, object>> GetByDictionary(String campos, String tabla, Dictionary<String, Object> where)
        {
            base.SetCampos(campos);
            base.Tabla(tabla);
            base.Where(where);
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
                return new List<Dictionary<string, object>>();
            }
            else
            {
                return ExecuteQueryDictionary(sql);
            }
        }
        #endregion
        #region Select
        /// <summary>
        /// Ejecuta la consulta SQL, los parametros de configuracion deben estar establecidos previamente
        /// </summary>
        /// <returns></returns>
        public Estructura.ObjSQL Get()
        {
            String sql = base.Generar();
            if (Preserve)
            {
                esql.Add(sql);
                return new Estructura.ObjSQL();
            }
            else
            {
                return ExecuteQuery(sql);
            }
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
            if (Preserve)
            {
                esql.Add(sql);
                return new Estructura.ObjSQL();
            }
            else
            {
                return ExecuteQuery(sql);
            }
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
            if (Preserve)
            {
                esql.Add(sql);
                return new Estructura.ObjSQL();
            }
            else
            {
                return ExecuteQuery(sql);
            }
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
            if (Preserve)
            {
                esql.Add(sql);
                return new Estructura.ObjSQL();
            }
            else
            {
                return ExecuteQuery(sql);
            }
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
        private List<T> ExecuteQuery<T>(String sql)
        {
            LastQuery = sql;
            List<T> s = new List<T>();
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
                        s = ConvertDataTable<T>(ds.Tables[0]);                        
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
                        MySqlCommand cmd = new MySqlCommand()
                        {
                            Connection = con2,
                            CommandText = sql,
                            CommandType = CommandType.Text,
                            CommandTimeout = 1000

                        };
                        
                        MySqlDataAdapter ADP = new MySqlDataAdapter();
                        DataSet DS = new DataSet();
                        ADP = new MySqlDataAdapter(cmd);
                        
                        ADP.Fill(DS);
                        s = ConvertDataTable<T>(DS.Tables[0]);
                    }
                    catch (SqlException ex)
                    {
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Imposible ejecutar la consulta: " + sql + "\n" +
                                "Error: " + ex.StackTrace,
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
                                "Error: " + ex.StackTrace,
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
                            CommandType = CommandType.Text,
                            CommandTimeout = 1000
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
                                "Error: " + ex.StackTrace,
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
        private List<Dictionary<string, object>> ExecuteQueryDictionary(String sql)
        {
            LastQuery = sql;
            List<Dictionary<string, object>> s = new List<Dictionary<string, object>>();
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
                        s = new List<Dictionary<string, object>>();
                        foreach (DataRow fila in ds.Tables[0].Rows)
                        {
                            Dictionary<string, object> diccionario = new Dictionary<string, object>();
                            foreach (DataColumn columna in ds.Tables[0].Columns)
                            {
                                diccionario[columna.ColumnName] = fila[columna];
                            }
                            s.Add(diccionario);
                        }
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
                            CommandType = CommandType.Text,
                            CommandTimeout = 1000
                        };
                        con2.Open();
                        MySqlDataAdapter ADP = new MySqlDataAdapter();
                        DataSet DS = new DataSet();
                        ADP = new MySqlDataAdapter(cmd);
                        ADP.Fill(DS);
                        s = new List<Dictionary<string, object>>();
                        foreach (DataRow fila in DS.Tables[0].Rows)
                        {
                            Dictionary<string, object> diccionario = new Dictionary<string, object>();
                            foreach (DataColumn columna in DS.Tables[0].Columns)
                            {
                                diccionario[columna.ColumnName] = fila[columna];
                            }
                            s.Add(diccionario);
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Imposible ejecutar la consulta: " + sql + "\n" +
                                "Error: " + ex.StackTrace,
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
                        cmd.CommandTimeout = 1200;
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
                        LastId = cmd.LastInsertedId;
                    }
                    catch (SqlException ex)
                    {
                        if (DebugMode)
                        {
                            MessageBox.Show(
                                "Imposible ejecutar la consulta: " + sql + "\n" +
                                "Error: " + ex.StackTrace,
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
                                    "Error: " + ex.StackTrace,
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
        private List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    T item = GetItem<T>(row);
                    data.Add(item);
                }
            }
            catch (Exception ex)
            {
                if (DebugMode)
                {
                    MessageBox.Show(
                        "Imposible Cargar la lista \n" +
                        "Error: " + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            return data;
        }
        private T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            
                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        try
                        {
                            var atributos = pro.GetCustomAttributes();
                            if(atributos.Where(x => x.GetType().Name == "ColumnaAttribute" || x.GetType().Name == "IgnoreAttribute").ToList().Count() > 0){
                                object valor = dr[column.ColumnName];
                                if (valor.GetType() != typeof(DBNull))
                                {
                                    bool ignore = false;
                                    string columna = "";
                                    foreach (var attr in atributos)
                                    {
                                        if (attr.GetType().Name == "ColumnaAttribute")
                                        {
                                            columna = ((ColumnaAttribute)attr).ColumName;
                                        }
                                        if (attr.GetType().Name == "IgnoreAttribute")
                                        {
                                            ignore = true;
                                        }
                                    }
                                    if (!ignore) {
                                        if (columna == column.ColumnName) {
                                        pro.SetValue(obj, valor, null);
                                        }
                                    }                                    
                                }
                            }
                            else {
                                SQLAttribute[] attribute = (SQLAttribute[])pro.GetCustomAttributes(typeof(SQLAttribute), true);
                                if (attribute.Length > 0)
                                {
                                    if (!attribute[0].Ignore)
                                    {
                                        if (attribute[0].ColumnSQLName == column.ColumnName)
                                        {
                                            if (dr[column.ColumnName].GetType() != typeof(DBNull))
                                            {
                                                pro.SetValue(obj, dr[column.ColumnName], null);
                                            }
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (pro.Name == column.ColumnName)
                                    {
                                        if (dr[column.ColumnName].GetType() != typeof(DBNull))
                                        {
                                            pro.SetValue(obj, dr[column.ColumnName], null);
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }                           
                            
                        }
                        catch (Exception ex)
                        {
                            if (DebugMode){
                                MessageBox.Show(
                                    "Imposible procesar registro " + pro.Name + " \n" +
                                    "Error: " + ex.Message,
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error
                                );
                            }
                        }                   
                    }
                }
            return obj;
        }
        #endregion

    }
}
