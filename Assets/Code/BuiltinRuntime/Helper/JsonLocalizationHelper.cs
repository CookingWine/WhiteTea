using GameFramework.Localization;
using System;
using UnityGameFramework.Runtime;

namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// Json本地化辅助器
    /// </summary>
    public class JsonLocalizationHelper:DefaultLocalizationHelper
    {
        public override bool ParseData(ILocalizationManager localizationManager , string dictionaryString , object userData)
        {
            try
            {
                string currentLanguage = WTGame.Localization.Language.ToString( );

            }
            catch(Exception exception)
            {
                Log.Error("Can not parse dictionary data with exception '{0}'." , exception.Message);
                return false;
            }


            return false;
        }
    }
}
