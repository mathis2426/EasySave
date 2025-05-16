namespace LibrairieJsonHelper
{
    public class JsonHelperFactory
    {
        public static ILoggerWriter CreateLoggerDaily()
        {      
            return new JsonHelperClassLoggerDaily();   
        }
        public static ILoggerWriter CreateLoggerStatus()
        {
            return new JsonHelperClassLoggerStatus();
        }
        public static JsonHelperClassJsonUpdate CreateJsonUpdate()
        {     
            return new JsonHelperClassJsonUpdate();
        }
        public static JsonHelperClassJsonReadMultipleObj CreateJsonReadMultipleObj()
        {
            return new JsonHelperClassJsonReadMultipleObj();
        }
        public static JsonHelperClassJsonReadSingleObj CreateJsonReadSingleObj()
        {
            return new JsonHelperClassJsonReadSingleObj();
        }
    }
}
