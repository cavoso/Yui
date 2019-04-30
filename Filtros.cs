﻿namespace Yui
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
    public enum TipoFecha
    {
        /// <summary>
        /// dd/MM/yyyy
        /// </summary>
        ddMMyyyySlash = 0,
        /// <summary>
        /// dd/MM/yyyy HH:mm:ss
        /// </summary>
        ddMMyyyySlashHHmmss = 1,
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        yyyyMMddGuion = 2,
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        yyyyMMddGuionHHmmss = 3
    }
    public enum TipoCampo
    {
        General = 0,
        Numero = 1,
        Letras = 2
    }
    public enum Signo
    {
        Ninguno = 0,
        Peso = 1,
        Dolar = 2,
        Euro = 3
    }
    public enum RespuestaSimple
    {
        NO = 0,
        SI = 1
    }
}