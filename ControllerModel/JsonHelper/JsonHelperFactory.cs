using ControllerModel.Logs2;

namespace ControllerModel.JsonHelper
{
    public class JsonHelperFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="JsonHelperClassLoggerDaily"/> to manage daily logs.
        /// </summary>
        /// <returns>Instance of <see cref="ILoggerWriter"/>.</returns>
        public static ILoggerWriter CreateLoggerDaily()
        {      
            return new JsonHelperClassLoggerDaily();   
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonHelperClassLoggerStatus"/> to manage status logs.
        /// </summary>
        /// <returns>Instance of <see cref="JsonHelperClassLoggerStatus"/>.</returns>
        public static JsonHelperClassLoggerStatus CreateLoggerStatus()
        {
            return new JsonHelperClassLoggerStatus();
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonHelperClassJsonUpdate"/> to update JSON files.
        /// </summary>
        /// <returns>Instance of <see cref="JsonHelperClassJsonUpdate"/>.</returns>
        public static JsonHelperClassJsonUpdate CreateJsonUpdate()
        {     
            return new JsonHelperClassJsonUpdate();
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonHelperClassJsonReadMultipleObj"/> to read multiple objects from a JSON file.
        /// </summary>
        /// <returns>Instance of <see cref="JsonHelperClassJsonReadMultipleObj"/>.</returns>
        public static JsonHelperClassJsonReadMultipleObj CreateJsonReadMultipleObj()
        {
            return new JsonHelperClassJsonReadMultipleObj();
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonHelperClassJsonReadSingleObj"/> to read a single object from a JSON file.
        /// </summary>
        /// <returns>Instance of <see cref="JsonHelperClassJsonReadSingleObj"/>.</returns>
        public static JsonHelperClassJsonReadSingleObj CreateJsonReadSingleObj()
        {
            return new JsonHelperClassJsonReadSingleObj();
        }
    }
}
