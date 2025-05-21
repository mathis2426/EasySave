using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Runtime.InteropServices.Marshalling;
using System.Reflection;
using ControllerModel.JsonHelper;



namespace ControllerModel.Logs2
{
    /// <summary>
    /// Interface définissant les méthodes pour écrire des logs au format JSON.
    /// </summary>
    public interface ILoggerWriter
    {
        /// <summary>
        /// Écrit un objet de log dans un fichier spécifié.
        /// </summary>
        /// <typeparam name="T">Type de l'objet de log.</typeparam>
        /// <param name="path">Chemin du fichier où écrire le log.</param>
        /// <param name="obj">Objet de log à écrire.</param>
        void WriteLog<T>(string path, T obj);

        /// <summary>
        /// Écrit une liste d'objets de log dans un fichier spécifié.
        /// </summary>
        /// <typeparam name="T">Type des objets de log dans la liste.</typeparam>
        /// <param name="path">Chemin du fichier où écrire les logs.</param>
        /// <param name="list">Liste d'objets de log à écrire.</param>
        void WriteLogList<T>(string path, List<T> list);

    }
}
