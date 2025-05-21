using ControllerModel.Logs2;

namespace ControllerModel.JsonHelper
{
    public class JsonHelperFactory
    {
        /// <summary>
        /// Crée une instance de <see cref="JsonHelperClassLoggerDaily"/> pour gérer les logs quotidiens.
        /// </summary>
        /// <returns>Instance de <see cref="ILoggerWriter"/>.</returns>
        public static ILoggerWriter CreateLoggerDaily()
        {      
            return new JsonHelperClassLoggerDaily();   
        }

        /// <summary>
        /// Crée une instance de <see cref="JsonHelperClassLoggerStatus"/> pour gérer les logs d'état.
        /// </summary>
        /// <returns>Instance de <see cref="JsonHelperClassLoggerStatus"/>.</returns>
        public static JsonHelperClassLoggerStatus CreateLoggerStatus()
        {
            return new JsonHelperClassLoggerStatus();
        }

        /// <summary>
        /// Crée une instance de <see cref="JsonHelperClassJsonUpdate"/> pour mettre à jour les fichiers JSON.
        /// </summary>
        /// <returns>Instance de <see cref="JsonHelperClassJsonUpdate"/>.</returns>
        public static JsonHelperClassJsonUpdate CreateJsonUpdate()
        {     
            return new JsonHelperClassJsonUpdate();
        }

        /// <summary>
        /// Crée une instance de <see cref="JsonHelperClassJsonReadMultipleObj"/> pour lire plusieurs objets depuis un fichier JSON.
        /// </summary>
        /// <returns>Instance de <see cref="JsonHelperClassJsonReadMultipleObj"/>.</returns>
        public static JsonHelperClassJsonReadMultipleObj CreateJsonReadMultipleObj()
        {
            return new JsonHelperClassJsonReadMultipleObj();
        }

        /// <summary>
        /// Crée une instance de <see cref="JsonHelperClassJsonReadSingleObj"/> pour lire un objet unique depuis un fichier JSON.
        /// </summary>
        /// <returns>Instance de <see cref="JsonHelperClassJsonReadSingleObj"/>.</returns>
        public static JsonHelperClassJsonReadSingleObj CreateJsonReadSingleObj()
        {
            return new JsonHelperClassJsonReadSingleObj();
        }
    }
}
