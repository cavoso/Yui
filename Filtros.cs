namespace Yui
{
    public enum TipoConexion
    {
        MSSQL = 0,
        MYSQL = 1
    }
    public enum TipoQuery
    {
        SELECT = 0,
        UPDATE = 1,
        INSERT = 2,
        DELETE = 3
    }
    public enum TipoLike
    {
        /// <summary>
        /// Se establecera como %valor%
        /// </summary>
        Contiene = 0,
        /// <summary>
        /// Se establecera como valor%
        /// </summary>
        Comienza = 1,
        /// <summary>
        /// Se establecera como %valor
        /// </summary>
        Termina = 2
    }
}